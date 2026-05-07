
using LearnStore.Infrastructure.Auth;
using LearnStore.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace LearnStore.Tests.Unit.Infrastructure.Auth
{
    public class JwtTokenGeneratorTests
    {
        private readonly Faker _faker = new();
        [Fact]
        public void GenerateToken_WhenCalled_ReturnsValidJwtToken()
        {
            var opitionsProvider = Substitute.For<IJwtOptionsProvider>();
            opitionsProvider.GetOptions().Returns(new JwtOptions
            {
                Issuer= "LearnStore",
                Audience= "LearnStoreAudience",
                Key= "ttlErQkX9a4w1Yh9p3jW4c4Zk3s0o2b5h1n0v9t1y4p3r8w5d7f3g2h9m0k1l2t3",
                ExpireHours= 2
            });
            var tokenGenerator = new JwtTokenGenerator(opitionsProvider);
            var user = new UserDto() 
            { 
                Id=Guid.NewGuid().ToString(),
                Email= _faker.Internet.Email(),
                Name = "Test",
                Roles = ["Admin", "User"]
            };
            var token = tokenGenerator.GenerateToken(user);
            var handler = new JwtSecurityTokenHandler();
            var jwt= handler.ReadJwtToken(token);

            jwt.Should().NotBeNull();
            jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id);
            jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
            jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.UniqueName && c.Value == user.Name);
            jwt.Claims.Should().Contain(c => c.Type == "roles" && c.Value == "Admin,User");
            jwt.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddHours(2), TimeSpan.FromSeconds(5));
        }
        [Fact]
        public void GenerateToken_WhenUserIsNull_ThrowsArgumentNullException()
        {
                var opitionsProvider = Substitute.For<IJwtOptionsProvider>();
                var tokenGenerator = new JwtTokenGenerator(opitionsProvider);
                
                Action act = () => tokenGenerator.GenerateToken(null!);
                
                act.Should().Throw<ArgumentNullException>().WithParameterName("user");
        }
        [Fact]
        public void GenerateToken_WhenUserHasNoRoles_ReturnsTokenWithEmptyRolesClaim()
        {
            var opitionsProvider = Substitute.For<IJwtOptionsProvider>();
            opitionsProvider.GetOptions().Returns(new JwtOptions
            {
                Issuer = "LearnStore",
                Audience = "LearnStoreAudience",
                Key = "ttlErQkX9a4w1Yh9p3jW4c4Zk3s0o2b5h1n0v9t1y4p3r8w5d7f3g2h9m0k1l2t3",
                ExpireHours = 2
            });
            var tokenGenerator = new JwtTokenGenerator(opitionsProvider);
            var user = new UserDto()
            {
                Id = Guid.NewGuid().ToString(),
                Email = _faker.Internet.Email(),
                Name = "Test",
                Roles = []
            };
            var token = tokenGenerator.GenerateToken(user);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            jwt.Claims.Should().Contain(c => c.Type == "roles" && c.Value == "");
        }
    }
}
