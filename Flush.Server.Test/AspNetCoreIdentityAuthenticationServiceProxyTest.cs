using System.Security.Claims;
using System.Threading.Tasks;
using Flush.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Flush.Server.Test
{
    public class AspNetCoreIdentityAuthenticationServiceProxyTest
    {
        [Fact]
        public async void GetExistingUserWithValidCredentials()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AspNetCoreIdentityAuthenticationServiceProxy>>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>();
            var expected = new IdentityUser();
            mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(expected));

            // Act
            var sut = new AspNetCoreIdentityAuthenticationServiceProxy(
                mockLogger.Object,
                mockUserManager.Object);
            var actual = await sut.GetExistingUser();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void RegisterNewUserWithValidCredentials()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AspNetCoreIdentityAuthenticationServiceProxy>>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>();
            var expected = new OkResult();

            // Act
            var sut = new AspNetCoreIdentityAuthenticationServiceProxy(
                mockLogger.Object,
                mockUserManager.Object);
            var actual = await sut.RegisterNewUser();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void RegisterGuestUserWithValidCredentials()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AspNetCoreIdentityAuthenticationServiceProxy>>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>();
            var expected = new OkResult();

            // Act
            var sut = new AspNetCoreIdentityAuthenticationServiceProxy(
                mockLogger.Object,
                mockUserManager.Object);
            var actual = await sut.RegisterGuestUser();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void SignInWithValidCredentials()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AspNetCoreIdentityAuthenticationServiceProxy>>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>();
            var expected = new OkResult();

            // Act
            var sut = new AspNetCoreIdentityAuthenticationServiceProxy(
                mockLogger.Object,
                mockUserManager.Object);
            var actual = await sut.SignIn();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void SignOutWithValidCredentials()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AspNetCoreIdentityAuthenticationServiceProxy>>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>();
            var expected = new OkResult();

            // Act
            var sut = new AspNetCoreIdentityAuthenticationServiceProxy(
                mockLogger.Object,
                mockUserManager.Object);
            var actual = await sut.SignOut();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void RemoveUserWithValidCredentials()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AspNetCoreIdentityAuthenticationServiceProxy>>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>();
            var expected = new OkResult();

            // Act
            var sut = new AspNetCoreIdentityAuthenticationServiceProxy(
                mockLogger.Object,
                mockUserManager.Object);
            var actual = await sut.RemoveUser();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
