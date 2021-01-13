using Flush.Server.Controllers;
using Flush.Server.Hubs;
using Flush.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Flush.Server.Test
{
    public class ChatControllerTest
    {
        [Fact]
        public async void SendMessageWithValidMessage()
        {
            // Arrange
            var mockHubContext = new Mock<IHubContext<ChatHub, IChatClient>>();
            var mockLogger = new Mock<ILogger<ChatController>>();
            var expected = new ChatMessage() { User = "Test User", Message = "Test Message" };
            mockHubContext.Setup(hc => hc.Clients.All.ReceiveMessage(expected)).Verifiable();

            // Act
            var sut = new ChatController(mockLogger.Object, mockHubContext.Object);
            var actual = await sut.SendMessage(expected);

            // Assert
            Assert.IsType<OkResult>(actual);
            mockHubContext.Verify();
        }

        [Fact]
        public async void SendMessageWithNullArgumentFails()
        {
            // Arrange
            var mockHubContext = new Mock<IHubContext<ChatHub, IChatClient>>();
            var mockLogger = new Mock<ILogger<ChatController>>();

            // Act
            var sut = new ChatController(mockLogger.Object, mockHubContext.Object);
            var actual = await sut.SendMessage(null);

            // Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public async void SendMessageWithEmptyMessageTextFails()
        {
            // Arrange
            var mockHubContext = new Mock<IHubContext<ChatHub, IChatClient>>();
            var mockLogger = new Mock<ILogger<ChatController>>();

            // Act
            var sut = new ChatController(mockLogger.Object, mockHubContext.Object);
            var actual = await sut.SendMessage(new ChatMessage()
            {
                User = "Test User",
                Message = ""
            });

            // Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public async void SendMessageWithNullMessageTextFails()
        {
            // Arrange
            var mockHubContext = new Mock<IHubContext<ChatHub, IChatClient>>();
            var mockLogger = new Mock<ILogger<ChatController>>();

            // Act
            var sut = new ChatController(mockLogger.Object, mockHubContext.Object);
            var actual = await sut.SendMessage(new ChatMessage()
            {
                User = "Test User",
                Message = null
            });

            // Assert
            Assert.IsType<BadRequestResult>(actual);
        }

        [Fact]
        public async void SendMessageWithWhiteSpaceMessageTextFails()
        {
            // Arrange
            var mockHubContext = new Mock<IHubContext<ChatHub, IChatClient>>();
            var mockLogger = new Mock<ILogger<ChatController>>();

            // Act
            var sut = new ChatController(mockLogger.Object, mockHubContext.Object);
            var actual = await sut.SendMessage(new ChatMessage()
            {
                User = "Test User",
                Message = "       "
            });

            // Assert
            Assert.IsType<BadRequestResult>(actual);
        }
    }
}
