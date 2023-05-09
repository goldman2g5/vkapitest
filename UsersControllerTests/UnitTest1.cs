using Newtonsoft.Json;
using System.Net;
using System.Text;
using UsersControllerTests.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using vkapitest;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;

namespace UsersControllerTests
{
    public class UnitTest1
    {
        public class UsersControllerTests : IClassFixture<WebApplicationFactory<Program>>
        {
            private readonly HttpClient _client;

            public UsersControllerTests(WebApplicationFactory<Program> factory)
            {
                // Create the HttpClient with the configured base address of your application
                _client = factory.CreateClient();

                // Add the basic authorization header
                var username = "root";
                var password = "root";
                var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
            }

            [Fact]
            public async Task GetUsers_ReturnsOkResult()
            {
                // Arrange

                // Act
                var response = await _client.GetAsync("/api/Users");

                // Assert
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            [Fact]
            public async Task GetUser_WithValidId_ReturnsOkResult()
            {
                // Arrange
                var validUserId = 52;

                // Act
                var response = await _client.GetAsync($"/api/Users/{validUserId}");

                // Assert
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            [Fact]
            public async Task GetUser_WithInvalidId_ReturnsNotFoundResult()
            {
                // Arrange
                var invalidUserId = 1000;

                // Act
                var response = await _client.GetAsync($"/api/Users/{invalidUserId}");

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Fact]
            public async Task GetUsersByIds_WithValidIds_ReturnsOkResult()
            {
                // Arrange
                var validUserIds = "1,2,3";

                // Act
                var response = await _client.GetAsync($"/api/Users/ids?userIds={validUserIds}");

                // Assert
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            [Fact]
            public async Task GetUsersByIds_WithEmptyIds_ReturnsBadRequestResult()
            {
                // Arrange
                var emptyUserIds = string.Empty;

                // Act
                var response = await _client.GetAsync($"/api/Users/ids?userIds={emptyUserIds}");

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task PutUser_WithValidIdAndUser_ReturnsNoContentResult()
            {
                // Arrange
                var validUserId = 45;
                var user = new UserPostModel
                {
                    Id = validUserId,
                    Login = "John",
                    Password = "123456",
                    CreatedDate = DateTime.Now,
                    UserGroupId = 2,
                    UserStateId = 1
                };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                // Act
                var response = await _client.PutAsync($"/api/Users/{validUserId}", jsonContent);

                // Assert
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

            [Fact]
            public async Task PutUser_WithInvalidIdAndUser_ReturnsBadRequestResult()
            {
                // Arrange
                var invalidUserId = 1000;
                var user = new User
                {
                    Id = invalidUserId,
                    Login = "John",
                    Password = "123456",
                    CreatedDate = DateTime.Now,
                    UserGroupId = 2,
                    UserStateId = 1
                };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                // Act
                var response = await _client.PutAsync($"/api/Users/{invalidUserId}", jsonContent);

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task PostUser_WithValidUser_ReturnsCreatedResult()
            {
                // Arrange
                var user = new UserPostModel { Login = "John" };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                // Act
                var response = await _client.PostAsync("/api/Users", jsonContent);

                // Assert
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }

            [Fact]
            public async Task PostUser_WithAdminUser_ReturnsProblemResult()
            {
                // Arrange
                var adminUser = new UserPostModel { Login = "Admin",
                    Password = "123456",
                    UserStateId = 1, 
                    UserGroupId = 1 };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(adminUser), Encoding.UTF8, "application/json");

                // Act
                await _client.PostAsync("/api/Users", jsonContent);
                var response = await _client.PostAsync("/api/Users", jsonContent);

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task PostUser_WithDuplicateLogin_ReturnsProblemResult()
            {
                // Arrange
                var user = new UserPostModel
                {
                    Login = "Bebra",
                    Password = "Bebra",
                    CreatedDate = DateTime.Now,
                    UserGroupId = 1
                };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                // Act
                var response1 = await _client.PostAsync("/api/Users", jsonContent);
                var response2 = await _client.PostAsync("/api/Users", jsonContent);

                // Assert
                Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
            }
        }
    }
}