using LearnStore.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LearnStore.Tests.Unit.Infrastructure.DataAccess.MsSql
{
    public class AppDbContextFactoryTests
    {
        [Fact]
        public void GetConnectionString_WhenConnectionStringMissing_ThrowsError()
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>()).Build();

            var initialaizer = Substitute.For<IDatabaseInitializer>();
            var passwordProvider = Substitute.For<IPasswordProvider>();

            var factory = new AppDbContextFactory(initialaizer, config, passwordProvider);

            Action act = () => factory.CreateDbContext([]);

            act.Should().Throw<InvalidOperationException>().WithMessage("Conection string 'LernStoreMigration' not found.");
        }

        [Fact]
        public void GetConnectionString_WhenMigrationUserNotMatch_ReturnsOriginalConnectionString()
        {
            var connectionString = "Server=Server;Database=LearnStore;User Id=other_user;";
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:LernStoreMigration"] = connectionString,
                ["MigrationUser:UserName"] = "migration_user"
            }).Build();

            var initialaizer = Substitute.For<IDatabaseInitializer>();
            var passwordProvider = Substitute.For<IPasswordProvider>();
            var factory = new AppDbContextFactory(initialaizer, config, passwordProvider);
            var context = factory.CreateDbContext([]);
            var builder = new SqlConnectionStringBuilder(context.Database.GetDbConnection().ConnectionString);
            builder.ConnectionString.Should().NotBeNullOrEmpty();
            builder.UserID.Should().Be("other_user");
            builder.Password.Should().BeNullOrEmpty();
            builder.DataSource.Should().Be("Server");
            builder.InitialCatalog.Should().Be("LearnStore");
        }

        [Fact]
        public void GetConnectionString_WhenMigrationUserInConnnectionStringInvalid_ReturnsOriginalConnectionString()
        {
            var connectionString = "Server=Server;Database=LearnStore;User Id=app_user;";
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:LernStoreMigration"] = connectionString,
                ["MigrationUser:UserName"] = "migration_user"
            }).Build();

            var initialaizer = Substitute.For<IDatabaseInitializer>();
            var passwordProvider = Substitute.For<IPasswordProvider>();
            var factory = new AppDbContextFactory(initialaizer, config, passwordProvider);
            var context = factory.CreateDbContext([]);
            var builder = new SqlConnectionStringBuilder(context.Database.GetDbConnection().ConnectionString);
            builder.ConnectionString.Should().NotBeNullOrEmpty();
            builder.UserID.Should().Be("app_user");
            builder.Password.Should().BeNullOrEmpty();
            builder.DataSource.Should().Be("Server");
            builder.InitialCatalog.Should().Be("LearnStore");
        }

        [Fact]
        public void GetConnectionString_WhenMigrationUserHavePasswordInConnnectionString_ReturnsOriginalConnectionString()
        {
            var connectionString = "Server=Server;Database=LearnStore;User Id=migration_user;Password=secret;";
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:LernStoreMigration"] = connectionString,
                ["MigrationUser:UserName"] = "migration_user"
            }).Build();
            var initialaizer = Substitute.For<IDatabaseInitializer>();
            var passwordProvider = Substitute.For<IPasswordProvider>();
            var factory = new AppDbContextFactory(initialaizer, config, passwordProvider);
            var context = factory.CreateDbContext([]);
            var builder = new SqlConnectionStringBuilder(context.Database.GetDbConnection().ConnectionString);
            builder.ConnectionString.Should().NotBeNullOrEmpty();
            builder.UserID.Should().Be("migration_user");
            builder.Password.Should().Be("secret");
            builder.DataSource.Should().Be("Server");
            builder.InitialCatalog.Should().Be("LearnStore");
            builder.Password.Should().Be("secret");
        }

        [Fact]
        public void GetConnectionString_WhenMigrationUserPasswordProvided_ReturnsConnectionStringWithPassword()
        {
            // Arrange
            var connectionString = "Server=Server;Database=LearnStore;User Id=migration_user;";

            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:LernStoreMigration"] = connectionString,
                ["MigrationUser:UserName"] = "migration_user"
            }).Build();

            var initializer = Substitute.For<IDatabaseInitializer>();

            var passwordProvider = Substitute.For<IPasswordProvider>();
            passwordProvider.GetPassword("migration_password").Returns("secret");

            var factory = new AppDbContextFactory(initializer, config, passwordProvider);

            // Act
            var context = factory.CreateDbContext([]);
            var builder = new SqlConnectionStringBuilder(context.Database.GetDbConnection().ConnectionString);

            // Assert
            builder.ConnectionString.Should().NotBeNullOrEmpty();
            builder.UserID.Should().Be("migration_user");
            builder.Password.Should().Be("secret");
            builder.DataSource.Should().Be("Server");
            builder.InitialCatalog.Should().Be("LearnStore");
        }

        [Fact]
        public void GetConnectionString_WhenMigrationUserCalled_EnsureDatabaseAndUserCalled()
        {
            // Arrange
            var connectionString = "Server=Server;Database=LearnStore;User Id=migration_user;";
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:LernStoreMigration"] = connectionString,
                ["MigrationUser:UserName"] = "migration_user"
            }).Build();
            var initializer = Substitute.For<IDatabaseInitializer>();
            var passwordProvider = Substitute.For<IPasswordProvider>();
            passwordProvider.GetPassword("migration_password").Returns("secret");
            var factory = new AppDbContextFactory(initializer, config, passwordProvider);
            // Act
            var context = factory.CreateDbContext([]);
            // Assert
            initializer.Received(1).EnsureDatabaseAndUser();
        }

        [Fact]
        public void GetConnectionString_WhenMigrationUserIsIntegratedSecurity_ReturnsConnectionStringWithIntegratedSecurity()
        {
            var connectionString = "Server=Server;Database=LearnStore;User Id=migration_user;Integrated Security=True;";
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:LernStoreMigration"] = connectionString,
                ["MigrationUser:UserName"] = "migration_user"
            }).Build();

            var initializer = Substitute.For<IDatabaseInitializer>();
            var passwordProvider = Substitute.For<IPasswordProvider>();
            var factory = new AppDbContextFactory(initializer, config, passwordProvider);

            // Act
            var context = factory.CreateDbContext([]);
            var builder = new SqlConnectionStringBuilder(context.Database.GetDbConnection().ConnectionString);

            // Assert
            builder.ConnectionString.Should().NotBeNullOrEmpty();
            builder.UserID.Should().Be("migration_user");
            builder.IntegratedSecurity.Should().BeTrue();
            builder.Password.Should().BeNullOrEmpty();
        }
    }
}
