using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LearnStore.Infrastructure.DataAccess.MsSql
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly IPasswordProvider _passwordProvider;
        private readonly IConfiguration _configuration;

        public DatabaseInitializer(IPasswordProvider passwordProvider, IConfiguration configuration)
        {
            _passwordProvider = passwordProvider ?? throw new ArgumentNullException(nameof(passwordProvider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void EnsureDatabaseAndUser()
        {
            var saPassword = _passwordProvider.GetPassword("mssql_sa_password");
            if(string.IsNullOrEmpty(saPassword))
                throw new InvalidOperationException("SA password is required to ensure midration user exists");
            var connectionString = _configuration.GetConnectionString("LernStoreMigration")
                ?? throw new InvalidOperationException("Connection string 'LernStoreMigration' not found ");
            var builder = new SqlConnectionStringBuilder(connectionString)
            {
                UserID = "SA",
                Password = saPassword,
                IntegratedSecurity = false
            };

            var builderForSa = new SqlConnectionStringBuilder(builder.ConnectionString)
            {
                InitialCatalog = "master"
            };
            var connection= new SqlConnection(builderForSa.ToString());

            EnsureDatabaseExists(builderForSa.ConnectionString, builder.InitialCatalog);
            EnsureMigrationUserExists(builderForSa.ConnectionString);
            EnsureAppUserExists(builderForSa.ConnectionString);
        }

        private void EnsureAppUserExists(string connectionString)
        {
            var appUserName = _configuration["AppUser:UserName"];
            if (string.IsNullOrEmpty(appUserName))
                throw new InvalidOperationException("App user name is not configured.");

            var password = _passwordProvider.GetPassword("app_password");
            var roles = "EXEC sp_addrolemember 'db_datareader', N'" + appUserName + "'; EXEC sp_addrolemember 'db_datawriter', N'" + appUserName + "';";
            EnsureUserExists(connectionString, appUserName, password, roles);
        }
        private void EnsureMigrationUserExists(string connectionString)
        {
            var migrationUserName = _configuration["MigrationUser:UserName"];
            if (string.IsNullOrEmpty(migrationUserName))
                throw new InvalidOperationException("Migration user name is not configured.");
            var password = _passwordProvider.GetPassword("migration_password");
            var roles = "EXEC sp_addrolemember 'db_owner', N'" + migrationUserName + "';";
            EnsureUserExists(connectionString, migrationUserName, password, roles);
        }
        private void EnsureUserExists(string connectionString, string userName, string password,string roles)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = $@"
                     IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'{userName}')
                     BEGIN
                         CREATE LOGIN [{userName}] WITH PASSWORD = '{password}';
                     END

                     IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'{userName}')
                     BEGIN
                         CREATE USER [{userName}] FOR LOGIN [{userName}];
                         {roles}
                     END";

            using var command = new SqlCommand(sql, connection);
            connection.Open();
            command.ExecuteNonQuery();
        }

        private void EnsureDatabaseExists(string connectionString, string databaseName)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = $@"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{databaseName}') BEGIN  CREATE DATABASE [{databaseName}] END";
           
            using var command = new SqlCommand(sql, connection);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
