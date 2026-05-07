using LearnStore.Infrastructure.Auth;
using LearnStore.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LearnStore.Tests.Unit.Infrastructure.Auth
{
    public class JwtOptionsProviderTests
    {
        [Fact]
        public void GetOptions_WhenConfigurationEmpty_ReturnsDefaultOptions()
        { 
            var configuration = Substitute.For<IConfiguration>();
            configuration["Jwt:Issuer"].Returns((string)null!);
            configuration["Jwt:Audience"].Returns((string)null!);
            configuration["Jwt:ExpireHours"].Returns((string)null!);
            configuration["Jwt:Key"].Returns("Key");
            var secretProvider = Substitute.For<ISecretProvider>();
            var provider = new JwtOptionsProvider(configuration, secretProvider);

            var options = provider.GetOptions();

            options.Should().NotBeNull();
            options.Issuer.Should().Be("LearnStore");
            options.Audience.Should().Be("LearnStoreUser");
            options.ExpireHours.Should().Be(1);
        }

        [Fact]
        public void GetOptions_WhenconfigurationHasValues_ReturnsConfiguredOptions()
        {
            
            var configuration = Substitute.For<IConfiguration>();
            configuration["Jwt:Issuer"].Returns("TestIssuer");
            configuration["Jwt:Audience"].Returns("TestAudience");
            configuration["Jwt:ExpireHours"].Returns("2");
            configuration["Jwt:Key"].Returns("Key");
            var secretProvider = Substitute.For<ISecretProvider>();
            var provider = new JwtOptionsProvider(configuration, secretProvider);
           
            var options = provider.GetOptions();
            
            options.Should().NotBeNull();
            options.Issuer.Should().Be("TestIssuer");
            options.Audience.Should().Be("TestAudience");
            options.ExpireHours.Should().Be(2);
        }
        [Fact]
        public void GetOptions_WhenKeyExistsInBoth_ReturnsConfigValue()
        {
            
            var configuration = Substitute.For<IConfiguration>();
            configuration["Jwt:Key"].Returns("ConfigKey");
            configuration["Jwt:KeySecretName"].Returns("TestSecret");
            var secretProvider = Substitute.For<ISecretProvider>();
            secretProvider.GetSecret("TestSecret").Returns("SecretKey");
            var provider = new JwtOptionsProvider(configuration, secretProvider);
            
            var options = provider.GetOptions();
         
            options.Should().NotBeNull();
            options.Key.Should().Be("ConfigKey");
        }
        [Fact]
        public void GetOptions_WhenKeyMissingAndSecretNameMissing_ThrowsException()
        {
           
            var configuration = Substitute.For<IConfiguration>();
            configuration["Jwt:Key"].Returns((string)null!);
            configuration["Jwt:KeySecretName"].Returns((string)null!);
            var secretProvider = Substitute.For<ISecretProvider>();
            var provider = new JwtOptionsProvider(configuration, secretProvider);
            
            Action act = () => provider.GetOptions();
            
            act.Should().Throw<InvalidOperationException>().WithMessage("JWT key is not configured.");
        }
        [Fact]
        public void GetOptions_WhenKeyMissingButSecretNameExists_ReturnsSecret()
        {
            
            var configuration = Substitute.For<IConfiguration>();
            configuration["Jwt:Key"].Returns((string)null!);
            configuration["Jwt:KeySecretName"].Returns("TestSecret");
            var secretProvider = Substitute.For<ISecretProvider>();
            secretProvider.GetSecret("TestSecret").Returns("SecretKey");
            var provider = new JwtOptionsProvider(configuration, secretProvider);
           
            var options = provider.GetOptions();
            
            options.Should().NotBeNull();
            options.Key.Should().Be("SecretKey");

        }
    }
}
