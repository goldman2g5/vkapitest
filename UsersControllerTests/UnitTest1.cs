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
                _client = factory.CreateClient();
                
                var username = "root";
                var password = "root";
                var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
            }

            [Fact]
            public async Task GetUsers_ReturnsOkResult()
            {

                var response = await _client.GetAsync("/api/Users");

                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            [Fact]
            public async Task GetUser_WithValidId_ReturnsOkResult()
            {
                var validUserId = 45;

                var response = await _client.GetAsync($"/api/Users/{validUserId}");

                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            [Fact]
            public async Task GetUser_WithInvalidId_ReturnsNotFoundResult()
            {
                var invalidUserId = 1000;

                var response = await _client.GetAsync($"/api/Users/{invalidUserId}");

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Fact]
            public async Task GetUsersByIds_WithValidIds_ReturnsOkResult()
            {
                var validUserIds = "1,2,3";

                var response = await _client.GetAsync($"/api/Users/ids?userIds={validUserIds}");

                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            [Fact]
            public async Task GetUsersByIds_WithEmptyIds_ReturnsBadRequestResult()
            {
                var emptyUserIds = string.Empty;

                var response = await _client.GetAsync($"/api/Users/ids?userIds={emptyUserIds}");

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task PutUser_WithValidIdAndUser_ReturnsNoContentResult()
            {
                var validUserId = 45;
                var user = new UserPutModel
                {
                    Id = validUserId,
                    Login = "John"
                };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                
                var response = await _client.PutAsync($"/api/Users/{validUserId}", jsonContent);
                
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

            [Fact]
            public async Task PutUser_WithInvalidIdAndUser_ReturnsBadRequestResult()
            {
                var invalidUserId = 1000;
                var user = new UserPutModel
                {
                    Id = invalidUserId,
                    Login = "John"
                };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"/api/Users/{invalidUserId}", jsonContent);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task PostUser_WithValidUser_ReturnsCreatedResult()
            {
                var user = new UserPostModel { Login = "John" };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("/api/Users", jsonContent);

                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }

            [Fact]
            public async Task PostUser_WithAdminUser_ReturnsProblemResult()
            {
                var adminUser = new UserPostModel { Id = 0, UserGroupId = 1 };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(adminUser), Encoding.UTF8, "application/json");

                await _client.PostAsync("/api/Users", jsonContent);
                var response = await _client.PostAsync("/api/Users", jsonContent);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            [Fact]
            public async Task PostUser_WithDuplicateLogin_ReturnsProblemResult()
            {
                var user = new UserPostModel { Login = "John" };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                var response1 = await _client.PostAsync("/api/Users", jsonContent);
                var response2 = await _client.PostAsync("/api/Users", jsonContent);

                Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
            }
        }
    }
}