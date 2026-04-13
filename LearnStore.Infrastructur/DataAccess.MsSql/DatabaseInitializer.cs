using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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
            var builderForSa = new SqlConnectionStringBuilder(connectionString)
            {
                UserID = "SA",
                Password = saPassword
            };
            var connection= new SqlConnection(builderForSa.ToString());

            EnsureDatabaseExists(builderForSa.ConnectionString,builderForSa.InitialCatalog);
            EnsureMigrationUserExists(builderForSa.ConnectionString);
            EnsureAppUserExists(builderForSa.ConnectionString);
        }

        private void EnsureAppUserExists(string connectionString)
        {
            var appUserName = _configuration["AppUser:UserName"];
            if (string.IsNullOrEmpty(appUserName))
                throw new InvalidOperationException("App user name is not configured.");

            var password = _passwordProvider.GetPassword("app_password");
            EnsureUserExists(connectionString, appUserName, password);
        }
        private void EnsureMigrationUserExists(string connectionString)
        {
            var migrationUserName = _configuration["MigrationUser:UserName"];
            if (string.IsNullOrEmpty(migrationUserName))
                throw new InvalidOperationException("Migration user name is not configured.");
            var password = _passwordProvider.GetPassword("migration_password");
            EnsureUserExists(connectionString, migrationUserName, password);
        }
        private void EnsureUserExists(string connectionString, string userName, string password)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@password", password);
            command.CommandText = "IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name=@userName) BEGIN CREATE LOGIN @userName WITH PASSWORD = @password;END"+
                "IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name=@userName) BEGIN CREATE USER @userName FOR LOGIN @userName; END";

            connection.Open();
            command.ExecuteNonQuery();
        }

        private void EnsureDatabaseExists(string connectionString, string databaseName)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "IF NOT EXISTS (SELECT * FROM sys.database WHERE name=@databaseName) BEGIN CREATE DATABASE [@databaseName] END";

            command.Parameters.AddWithValue("@databaseName", databaseName);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
