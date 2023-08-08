using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MvcMessageLogger.DataAccess;
using System.Net;
using Microsoft.Extensions.Hosting;
using MvcMessageLogger.Models;
using System.Xml.Linq;


namespace MvcMessageLogger.FeatureTests
{
    [Collection("Controller Tests")]
    public class UsersTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UsersTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private MvcMessageLoggerContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MvcMessageLoggerContext>();
            optionsBuilder.UseInMemoryDatabase("TestDatabase");

            var context = new MvcMessageLoggerContext(optionsBuilder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task Test_Index_ReturnsViewWithUsers()
        {
            var context = GetDbContext();
            context.Users.Add(new User { Username = "JimComedy123", Name = "Jim" });
            context.Users.Add(new User { Username = "JamesRock98", Name = "James" });
            context.SaveChanges();

            var client = _factory.CreateClient();
            var response = await client.GetAsync("/users");
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("JimComedy123", html);
            Assert.Contains("Jim", html);

            Assert.Contains("JamesRock98", html);
            Assert.Contains("James", html);
        }

        [Fact]
        public async Task Test_AddUser_ReturnsRedirectToIndex()
        {
            // Context is only needed if you want to assert against the database
            var context = GetDbContext();

            // Arrange
            var client = _factory.CreateClient();
            var formData = new Dictionary<string, string>
            {
                { "Username", "Joe1011" },
                { "Name", "Joe" },
                { "Password", "Password123" },
                { "CoffeeOfChoice", "VLatte" }
            };

            // Act
            var response = await client.PostAsync("/Users", new FormUrlEncodedContent(formData));
            var html = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            Assert.Contains("UserName: Joe1011", html);
            Assert.Contains("Name: Joe", html);
        }

        [Fact]
        public async Task Test_UsersHaveLogInButton()
        {
            var context = GetDbContext();
            context.Users.Add(new User { Username = "JimComedy123", Name = "Jim" });
            context.SaveChanges();

            var client = _factory.CreateClient();
            var response = await client.GetAsync("/users");
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("UserName: JimComedy123", html);
            Assert.Contains("Log-In", html);
        }

        [Fact]
        public async Task Test_UsersCanSeeAllMessages()
        {
            var context = GetDbContext();
            var Jim = new User { Username = "JimComedy123", Name = "Jim" };
            Jim.Messages.Add(new Message { Content = "Hello world!", CreatedAt = DateTime.UtcNow});
            Jim.Messages.Add(new Message { Content = "Hello world two!", CreatedAt = DateTime.UtcNow });
            context.Users.Add(Jim);
            context.SaveChanges();

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/users/{Jim.Id}");
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("Hello world!", html);
            Assert.Contains("Hello world two!", html);
        }

        [Fact]
        public async Task Test_AUserCanAddAMessage_InShowPage()
        {
            // Context is only needed if you want to assert against the database
            var context = GetDbContext();

            // Arrange
            var client = _factory.CreateClient();
            var Jim = new User { Username = "JimComedy123", Name = "Jim" };
            context.Users.Add(Jim);
            context.SaveChanges();

            var formData = new Dictionary<string, string>
            {
                { "Content", "Hello world!" },
            };

            // Act
            var response = await client.PostAsync($"/Users/{Jim.Id}", new FormUrlEncodedContent(formData));
            var html = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Contains("Hello world!", html);
        }
    }
}