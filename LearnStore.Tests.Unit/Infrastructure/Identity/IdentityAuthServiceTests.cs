using LearnStore.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;


namespace LearnStore.Tests.Unit.Infrastructure.Identity
{
    public class IdentityAuthServiceTests
    {
        private UserManager<IdentityUser> CreateUserManager() => CreateUserManager(Substitute.For<IUserStore<IdentityUser>>());

        private UserManager<IdentityUser> CreateUserManager(IUserStore<IdentityUser> userStore)
        {
            var userManager = Substitute.ForPartsOf<UserManager<IdentityUser>>(
                userStore,
                Substitute.For<IOptions<IdentityOptions>>(),
                Substitute.For<IPasswordHasher<IdentityUser>>(),
                new List<IUserValidator<IdentityUser>>(),
                new List<IPasswordValidator<IdentityUser>>(),
                Substitute.For<ILookupNormalizer>(),
                Substitute.For<IdentityErrorDescriber>(),
                Substitute.For<IServiceProvider>(),
                Substitute.For<ILogger<UserManager<IdentityUser>>>()
            );
            return userManager;
        }

        private SignInManager<IdentityUser> CreateSignInManager()
        {
            var httpContext = new DefaultHttpContext();
            var authService = Substitute.For<IAuthenticationService>();
            httpContext.RequestServices = Substitute.For<IServiceProvider>();
            httpContext.RequestServices.GetService(typeof(IAuthenticationService))
                .Returns(authService);

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(httpContext);


            var claimsFactory = Substitute.For<IUserClaimsPrincipalFactory<IdentityUser>>();
            claimsFactory.CreateAsync(Arg.Any<IdentityUser>())
                .Returns(new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", "test-user")])));

            return Substitute.ForPartsOf<SignInManager<IdentityUser>>(
                CreateUserManager(),
                httpContextAccessor,
                claimsFactory,
                Substitute.For<IOptions<IdentityOptions>>(),
                Substitute.For<ILogger<SignInManager<IdentityUser>>>(),
                Substitute.For<IAuthenticationSchemeProvider>(),
                Substitute.For<IUserConfirmation<IdentityUser>>()
            );
        }

        private RoleManager<IdentityRole> CreateRoleManager() => CreateRoleManager(Substitute.For<IRoleStore<IdentityRole>>());

        private RoleManager<IdentityRole> CreateRoleManager(IRoleStore<IdentityRole> store)
        {
            var lookupNormalizer = Substitute.For<ILookupNormalizer>();
            var errors = Substitute.For<IdentityErrorDescriber>();
            var logger = Substitute.For<ILogger<RoleManager<IdentityRole>>>();

            var roleManager = new RoleManager<IdentityRole>(
                store,
                [],
                lookupNormalizer,
                errors,
                logger
            );

            return roleManager;
        }

        [Fact]
        public async Task LoginAsync_WhenUserNull_ThrowsArgumentNullException()
        {
            var userManager = CreateUserManager();
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            Func<Task> act = () => service.LoginAsync(null!, "password", CancellationToken.None);
            
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("user");
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordNullOrEmpty_ThrowsArgumentException()
        {
            var userManager = CreateUserManager();
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            Func<Task> actNull = () => service.LoginAsync(new UserDto { Email = "Email", Name = "Name" }, null!, CancellationToken.None);
            Func<Task> actEmpty = () => service.LoginAsync(new UserDto { Email = "Email", Name = "Name" }, string.Empty, CancellationToken.None);
            
            await actNull.Should().ThrowAsync<ArgumentException>().WithMessage("*Password*");
            await actEmpty.Should().ThrowAsync<ArgumentException>().WithMessage("*Password*");
        }

        [Fact]
        public async Task LoginAsync_WhenUserNameAndPasswordNullOrEmpty_ThrowsArgumentException()
        {
            var userManager = CreateUserManager();
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            Func<Task> actNull = () => service.LoginAsync(new UserDto { Email = null!, Name = null! }, "Password", CancellationToken.None);
            Func<Task> actEmpty = () => service.LoginAsync(new UserDto { Email = string.Empty, Name = string.Empty }, "Password", CancellationToken.None);
            
            await actNull.Should().ThrowAsync<ArgumentException>().WithMessage("*Username or email*");
            await actEmpty.Should().ThrowAsync<ArgumentException>().WithMessage("*Username or email*");
        }

        [Fact]
        public async Task LoginAsync_WhenUserNotFound_ReturnsFailedAuthResult()
        {
            var userManager = CreateUserManager();
            userManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(null));
            userManager.FindByNameAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(null));
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            var result = await service.LoginAsync(new UserDto { Email = "Email", Name = "Name" }, "Password", CancellationToken.None);
            
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Invalid username or email.");
            result.Token.Should().BeNull();
            result.UserId.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordInvalid_ReturnsFailedAuthResult()
        {

            var userManager = CreateUserManager();
            var user = new IdentityUser { UserName = "Name", Email = "Email" };
            userManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(user));
            userManager.FindByNameAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(user));
            var signInManager = CreateSignInManager();
            signInManager.PasswordSignInAsync(Arg.Any<IdentityUser>(), Arg.Any<string>(), false, false)
                .Returns(Task.FromResult(SignInResult.Failed));
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            var result = await service.LoginAsync(new UserDto { Email = "Email", Name = "Name" }, "Password", CancellationToken.None);
            
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("Invalid password");
            result.Token.Should().BeNull();
            result.UserId.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task LoginAsync_WhenValidCredentials_ReturnsSuccessAuthResult()
        {
            var userStore = Substitute.For<IUserRoleStore<IdentityUser>>();
            userStore.GetRolesAsync(Arg.Any<IdentityUser>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult((IList<string>)new List<string> { "Role1", "Role2" }));
            var userManager = CreateUserManager(userStore);
            var user = new IdentityUser { UserName = "Name", Email = "Email", Id = "user-id" };
            userManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(user));
            userManager.FindByNameAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(user));
            var signInManager = CreateSignInManager();
            signInManager.PasswordSignInAsync(Arg.Any<IdentityUser>(), Arg.Any<string>(), false, false)
                .Returns(Task.FromResult(SignInResult.Success));
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            var resultEmail = await service.LoginAsync(new UserDto { Email = "Email", Name = string.Empty }, "Password", CancellationToken.None);
            var resultName = await service.LoginAsync(new UserDto { Email = string.Empty, Name = "Name" }, "Password", CancellationToken.None);
            
            resultEmail.IsSuccess.Should().BeTrue();
            resultEmail.Error.Should().BeNull();
            resultEmail.UserId.Should().Be("user-id");

            resultName.IsSuccess.Should().BeTrue();
            resultName.Error.Should().BeNull();
            resultName.UserId.Should().Be("user-id");
        }

        [Fact]
        public async Task RegisterAsync_WhenUserNull_ThrowsArgumentNullException()
        {
            var userManager = CreateUserManager();
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            Func<Task> act = () => service.RegisterAsync(null!, "password", CancellationToken.None);
            
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("user");
        }

        [Fact]
        public async Task RegisterAsync_WhenPasswordNullOrEmpty_ThrowsArgumentException()
        {
            var userManager = CreateUserManager();
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            Func<Task> actNull = () => service.RegisterAsync(new UserDto { Email = "Email", Name = "Name" }, null!, CancellationToken.None);
            Func<Task> actEmpty = () => service.RegisterAsync(new UserDto { Email = "Email", Name = "Name" }, string.Empty, CancellationToken.None);
            
            await actNull.Should().ThrowAsync<ArgumentException>().WithMessage("*Password*");
            await actEmpty.Should().ThrowAsync<ArgumentException>().WithMessage("*Password*");
        }


        [Fact]
        public async Task RegisterAsync_WhenUserNameAndEmailNullOrEmpty_ThrowsArgumentException()
        {
            var userManager = CreateUserManager();
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            Func<Task> actNull = () => service.RegisterAsync(new UserDto { Email = null!, Name = null! }, "Password", CancellationToken.None);
            Func<Task> actEmpty = () => service.RegisterAsync(new UserDto { Email = string.Empty, Name = string.Empty }, "Password", CancellationToken.None);
            
            await actNull.Should().ThrowAsync<ArgumentException>().WithMessage("*Username or email*");
            await actEmpty.Should().ThrowAsync<ArgumentException>().WithMessage("*Username or email*");
        }

        [Fact]
        public async Task RegisterAsync_WhenCannotCreateUser_ReturnsFailedAuthResult()
        {
            var userManager = CreateUserManager();
            userManager.CreateAsync(Arg.Any<IdentityUser>(), Arg.Any<string>())
                .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "User creation failed." })));
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            var result = await service.RegisterAsync(new UserDto { Email = "Email", Name = "Name" }, "Password", CancellationToken.None);
            
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("User creation failed.");
            result.Token.Should().BeNull();
            result.UserId.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task RegisterAsync_WhenValidUser_ReturnsSuccessAuthResult()
        {
            var userManager = CreateUserManager();
            userManager.CreateAsync(Arg.Any<IdentityUser>(), Arg.Any<string>())
                .Returns(Task.FromResult(IdentityResult.Success));
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            var result = await service.RegisterAsync(new UserDto { Email = "Email", Name = "Name" }, "Password", CancellationToken.None);
            
            result.IsSuccess.Should().BeTrue();
            result.Error.Should().BeNull();
            result.UserId.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task AddRoleToUser_WhenUserIdNullOrEmpty_ThrowsArgumentException()
        {
            var userManager = CreateUserManager();
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            Func<Task> actNull = () => service.AddRoleToUserAsync(null!, "Role", CancellationToken.None);
            Func<Task> actEmpty = () => service.AddRoleToUserAsync(string.Empty, "Role", CancellationToken.None);
            
            await actNull.Should().ThrowAsync<ArgumentException>().WithParameterName("userId");
            await actEmpty.Should().ThrowAsync<ArgumentException>().WithParameterName("userId");
        }

        [Fact]
        public async Task AddRoleToUser_WhenRoleNullOrEmpty_ThrowsArgumentException()
        {
            var userManager = CreateUserManager();
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            Func<Task> actNull = () => service.AddRoleToUserAsync("user-id", null!, CancellationToken.None);
            Func<Task> actEmpty = () => service.AddRoleToUserAsync("user-id", string.Empty, CancellationToken.None);
            
            await actNull.Should().ThrowAsync<ArgumentException>().WithParameterName("role");
            await actEmpty.Should().ThrowAsync<ArgumentException>().WithParameterName("role");

        }

        [Fact]
        public async Task AddRoleToUser_WhenUserNotFound_ReturnsFailedResult()
        {
            var userManager = CreateUserManager();
            userManager.FindByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(null));
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            var result = await service.AddRoleToUserAsync("user-id", "Role", CancellationToken.None);
            
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be("User not found.");
        }

        [Fact]
        public async Task AddRoleToUser_WhenRoleNotFound_ReturnsFailedResult()
        {
            var userManager = CreateUserManager();
            userManager.FindByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(new IdentityUser { Id = "user-id" }));
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var store = Substitute.For<IRoleStore<IdentityRole>>();
            store.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult<IdentityRole?>(null));
            var roleManager = CreateRoleManager(store);
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            var result = await service.AddRoleToUserAsync("user-id", "Role", CancellationToken.None);
            
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be("Role not found.");
        }
        [Fact]
        public async Task AddRoleToUser_WhenUserAlreadyHasRole_ReturnsFailedResult()
        {
            var userStore = Substitute.For<IUserRoleStore<IdentityUser>>();
            userStore.IsInRoleAsync(Arg.Any<IdentityUser>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                    .Returns(Task.FromResult(true));
            var userManager = CreateUserManager(userStore);
            var user = new IdentityUser { Id = "user-id" };
            userManager.FindByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(user));
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var store = Substitute.For<IRoleStore<IdentityRole>>();
            var roleManager = CreateRoleManager(store);
            userManager.IsInRoleAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).Returns(Task.FromResult(true));
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            var result = await service.AddRoleToUserAsync("user-id", "Role", CancellationToken.None);
            
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be("User already has the role.");
        }

        [Fact]
        public async Task AddRoleToUser_WhenValid_ReturnsSuccessResult()
        {
            var userStore = Substitute.For<IUserRoleStore<IdentityUser>>();
            userStore.IsInRoleAsync(Arg.Any<IdentityUser>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                    .Returns(Task.FromResult(false));
            var userManager = CreateUserManager(userStore);
            var user = new IdentityUser { Id = "user-id" };
            userManager.FindByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(user));
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var store = Substitute.For<IRoleStore<IdentityRole>>();
            store.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult<IdentityRole?>(new IdentityRole() { Id = "1", Name = "Role" }));
            var roleManager = CreateRoleManager(store);
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            var result = await service.AddRoleToUserAsync("user-id", "Role", CancellationToken.None);
            
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
            result.RoleName.Should().Be("Role");
            result.UserId.Should().Be("user-id");
        }

        [Fact]
        public async Task RemoveRoleFromUser_WhenUserIdNullOrEmpty_ThrowsArgumentException()
        {
            var userManager = CreateUserManager();
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            Func<Task> actNull = () => service.RemoveRoleFromUserAsync(null!, "Role", CancellationToken.None);
            Func<Task> actEmpty = () => service.RemoveRoleFromUserAsync(string.Empty, "Role", CancellationToken.None);
            
            await actNull.Should().ThrowAsync<ArgumentException>().WithParameterName("userId");
            await actEmpty.Should().ThrowAsync<ArgumentException>().WithParameterName("userId");
        }

        [Fact]
        public async Task RemoveRoleFromUser_WhenRoleNullOrEmpty_ThrowsArgumentException()
        {
            var userManager = CreateUserManager();
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            Func<Task> actNull = () => service.RemoveRoleFromUserAsync("user-id", null!, CancellationToken.None);
            Func<Task> actEmpty = () => service.RemoveRoleFromUserAsync("user-id", string.Empty, CancellationToken.None);
            
            await actNull.Should().ThrowAsync<ArgumentException>().WithParameterName("role");
            await actEmpty.Should().ThrowAsync<ArgumentException>().WithParameterName("role");

        }

        [Fact]
        public async Task RemoveRoleFromUser_WhenUserNotFound_ReturnsFailedResult()
        {
            var userManager = CreateUserManager();
            userManager.FindByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(null));
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var roleManager = CreateRoleManager();
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            var result = await service.RemoveRoleFromUserAsync("user-id", "Role", CancellationToken.None);
            
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be("User not found.");
        }

        [Fact]
        public async Task RemoveRoleFromUser_WhenRoleNotFound_ReturnsFailedResult()
        {
            var userManager = CreateUserManager();
            userManager.FindByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(new IdentityUser { Id = "user-id" }));
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var store = Substitute.For<IRoleStore<IdentityRole>>();
            store.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult<IdentityRole?>(null));
            var roleManager = CreateRoleManager(store);
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            var result = await service.RemoveRoleFromUserAsync("user-id", "Role", CancellationToken.None);
            
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be("Role not found.");
        }
        [Fact]
        public async Task RemoveRoleFromUser_WhenUserDoesNotHaveRole_ReturnsFailedResult()
        {
            var userStore = Substitute.For<IUserRoleStore<IdentityUser>>();
            userStore.IsInRoleAsync(Arg.Any<IdentityUser>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                    .Returns(Task.FromResult(false));
            var userManager = CreateUserManager(userStore);
            var user = new IdentityUser { Id = "user-id" };
            userManager.FindByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(user));
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var store = Substitute.For<IRoleStore<IdentityRole>>();
            store.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult<IdentityRole?>(new IdentityRole() { Id = "1", Name = "Role" }));
            var roleManager = CreateRoleManager(store);
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            var result = await service.RemoveRoleFromUserAsync("user-id", "Role", CancellationToken.None);
            
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be("User does not have the role.");
        }

        [Fact]
        public async Task RemoveRoleFromUser_WhenValid_ReturnsSuccessResult()
        {
            var userStore = Substitute.For<IUserRoleStore<IdentityUser>>();
            userStore.IsInRoleAsync(Arg.Any<IdentityUser>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                    .Returns(Task.FromResult(true));
            var userManager = CreateUserManager(userStore);
            var user = new IdentityUser { Id = "user-id" };
            userManager.FindByIdAsync(Arg.Any<string>()).Returns(Task.FromResult<IdentityUser?>(user));
            var signInManager = CreateSignInManager();
            var generator = Substitute.For<IJwtTokenGenerator>();
            var store = Substitute.For<IRoleStore<IdentityRole>>();
            var roleManager = CreateRoleManager(store);
            userManager.IsInRoleAsync(Arg.Any<IdentityUser>(), Arg.Any<string>()).Returns(Task.FromResult(true));
            var service = new IdentityAuthService(userManager, signInManager, roleManager, generator);
            
            var result = await service.RemoveRoleFromUserAsync("user-id", "Role", CancellationToken.None);
            
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
            result.RoleName.Should().Be("Role");
            result.UserId.Should().Be("user-id");
        }
    }
}
