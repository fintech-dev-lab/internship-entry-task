using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TicTacToe.Data;
using TicTacToe.Tests.Extentions;
using TicTacToe.ViewModels.Request;
using TicTacToe.ViewModels.Response;

namespace TicTacToe.Tests.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Удаляем реальный контекст и заменяем InMemory
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationContext>));
                if (descriptor != null) services.Remove(descriptor);

        
                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Можно заполнить тестовыми данными
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                db.Database.EnsureCreated();
                db.SeedData();
            });
        }
    }

    public class GamesApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public GamesApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        #region Get API Tests

        [Fact]
        public async Task Get_Games_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/games");

            // Assert
            response.EnsureSuccessStatusCode();
            var games = JsonConvert.DeserializeObject<GameDto[]>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(games);
            Assert.NotEmpty(games);
        }

        [Fact]
        public async Task GetGame_ExistId_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/games/2");

            // Assert
            response.EnsureSuccessStatusCode();
            var game = JsonConvert.DeserializeObject<GameDto>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(game);
            Assert.Equal(2, game.Id);
        }


        [Fact]
        public async Task GetGame_NotExistId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/games/2");

            // Assert
            response.EnsureSuccessStatusCode();
            var game = JsonConvert.DeserializeObject<GameDto>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(game);
            Assert.Equal(2, game.Id);
        }

        #endregion

        #region Create Game API Tests

        [Fact]
        public async Task CreateGame_ValidModel_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var modelWithNullValues = new CreateGameDto();
            var modelWithValues = new CreateGameDto() { BoardSize = 5, WinLenght = 4 };

            var json1 = JsonConvert.SerializeObject(modelWithNullValues);
            var json2 = JsonConvert.SerializeObject(modelWithValues);

            var httpContent1 = new StringContent(json1, Encoding.UTF8, "application/json");
            var htppContent2 = new StringContent(json2, Encoding.UTF8, "application/json");
            var response1 = await client.PostAsync("/api/v1/games", httpContent1);
            var response2 = await client.PostAsync("/api/v1/games", htppContent2);

            // Assert
            response1.EnsureSuccessStatusCode();
            response2.EnsureSuccessStatusCode();
            var game1 = JsonConvert.DeserializeObject<GameDto>(await response1.Content.ReadAsStringAsync());
            var game2 = JsonConvert.DeserializeObject<GameDto>(await response2.Content.ReadAsStringAsync());

            Assert.NotNull(game1);
            Assert.Equal(3, game1.BoardSize);
            Assert.Equal(3, game1.WinLenght);

            Assert.NotNull(game2);
            Assert.Equal(modelWithValues.BoardSize, game2.BoardSize);
            Assert.Equal(modelWithValues.WinLenght, game2.WinLenght);
        }

        [Fact]
        public async Task CreateGame_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            var model1 = new CreateGameDto() { BoardSize = null, WinLenght = 78 };
            var model2 = new CreateGameDto() { BoardSize = 53, WinLenght = null };
            var model3 = new CreateGameDto() { BoardSize = 5, WinLenght = 100 };
            var model4 = new CreateGameDto() { BoardSize = 5, WinLenght = 1 };
            var model5 = new CreateGameDto() { BoardSize = 2, WinLenght = 3 };

            var json1 = JsonConvert.SerializeObject(model1);
            var json2 = JsonConvert.SerializeObject(model2);
            var json3 = JsonConvert.SerializeObject(model3);
            var json4 = JsonConvert.SerializeObject(model4);
            var json5 = JsonConvert.SerializeObject(model5);

            var httpContent1 = new StringContent(json1, Encoding.UTF8, "application/json");
            var htppContent2 = new StringContent(json2, Encoding.UTF8, "application/json");
            var httpContent3 = new StringContent(json3, Encoding.UTF8, "application/json");
            var htppContent4 = new StringContent(json4, Encoding.UTF8, "application/json");
            var httpContent5 = new StringContent(json5, Encoding.UTF8, "application/json");

            // Act
            var response1 = await client.PostAsync("/api/v1/games", httpContent1);
            var response2 = await client.PostAsync("/api/v1/games", htppContent2);
            var response3 = await client.PostAsync("/api/v1/games", httpContent3);
            var response4 = await client.PostAsync("/api/v1/games", htppContent4);
            var response5 = await client.PostAsync("/api/v1/games", httpContent5);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response1.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response3.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response4.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response5.StatusCode);
        }

        #endregion

        #region Move Game API Tests

        [Fact]
        public async Task MoveGame_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            var model1 = new CreateMoveDto() { Column = null, Row = 0, Symbol = Enums.TicTacToeSymbol.X };
            var model2 = new CreateMoveDto() { Column = 0, Row = null, Symbol = Enums.TicTacToeSymbol.X };
            var model3 = new CreateMoveDto() {Column = 0, Row = 0, Symbol = null};
            var model4 = new CreateMoveDto() {Column = 500, Row = 0, Symbol = Enums.TicTacToeSymbol.X };
            var model5 = new CreateMoveDto() {Column = 0, Row = 500, Symbol = Enums.TicTacToeSymbol.X };
            var model6 = new CreateMoveDto() { Column = 0, Row = 0, Symbol = Enums.TicTacToeSymbol.O };

            var json1 = JsonConvert.SerializeObject(model1);
            var json2 = JsonConvert.SerializeObject(model2);
            var json3 = JsonConvert.SerializeObject(model3);
            var json4 = JsonConvert.SerializeObject(model4);
            var json5 = JsonConvert.SerializeObject(model5);
            var json6 = JsonConvert.SerializeObject(model6);

            var httpContent1 = new StringContent(json1, Encoding.UTF8, "application/json");
            var htppContent2 = new StringContent(json2, Encoding.UTF8, "application/json");
            var httpContent3 = new StringContent(json3, Encoding.UTF8, "application/json");
            var htppContent4 = new StringContent(json4, Encoding.UTF8, "application/json");
            var httpContent5 = new StringContent(json5, Encoding.UTF8, "application/json");
            var httpContent6 = new StringContent(json6, Encoding.UTF8, "application/json");

            // Act
            var response1 = await client.PostAsync("/api/v1/games/3/move", httpContent1);
            var response2 = await client.PostAsync("/api/v1/games/3/move", htppContent2);
            var response3 = await client.PostAsync("/api/v1/games/3/move", httpContent3);
            var response4 = await client.PostAsync("/api/v1/games/3/move", htppContent4);
            var response5 = await client.PostAsync("/api/v1/games/3/move", httpContent5);
            var response6 = await client.PostAsync("/api/v1/games/3/move", httpContent6);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response1.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response3.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response4.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response5.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response6.StatusCode);
        }

        [Fact]
        public async Task MoveGame_NotExistGame_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            var model = new CreateMoveDto() { Column = 0, Row = 0, Symbol = Enums.TicTacToeSymbol.X };
            var json = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/v1/games/378/move", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task MoveGame_FinishedGame_ReturnsConflict()
        {
            // Arrange
            var client = _factory.CreateClient();

            var model = new CreateMoveDto() { Column = 0, Row = 0, Symbol = Enums.TicTacToeSymbol.X };
            var json = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/v1/games/2/move", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task MoveGame_ValidModel_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            var move1 = new CreateMoveDto() { Column = 0, Row = 0, Symbol = Enums.TicTacToeSymbol.X };
            var move2 = new CreateMoveDto() { Column = 0, Row = 2, Symbol = Enums.TicTacToeSymbol.O };
            var move3 = new CreateMoveDto() { Column = 1, Row = 0, Symbol = Enums.TicTacToeSymbol.X };
            var move4 = new CreateMoveDto() { Column = 1, Row = 2, Symbol = Enums.TicTacToeSymbol.O };
            var move5 = new CreateMoveDto() { Column = 2, Row = 0, Symbol = Enums.TicTacToeSymbol.X };

            var json1 = JsonConvert.SerializeObject(move1);
            var json2 = JsonConvert.SerializeObject(move2);
            var json3 = JsonConvert.SerializeObject(move3);
            var json4 = JsonConvert.SerializeObject(move4);
            var json5 = JsonConvert.SerializeObject(move5);

            var httpContent1 = new StringContent(json1, Encoding.UTF8, "application/json");
            var htppContent2 = new StringContent(json2, Encoding.UTF8, "application/json");
            var httpContent3 = new StringContent(json3, Encoding.UTF8, "application/json");
            var htppContent4 = new StringContent(json4, Encoding.UTF8, "application/json");
            var httpContent5 = new StringContent(json5, Encoding.UTF8, "application/json");

            // Act
            var response1 = await client.PostAsync("/api/v1/games/3/move", httpContent1);
            var response2 = await client.PostAsync("/api/v1/games/3/move", htppContent2);
            var response3 = await client.PostAsync("/api/v1/games/3/move", httpContent3);
            var response4 = await client.PostAsync("/api/v1/games/3/move", htppContent4);
            var response5 = await client.PostAsync("/api/v1/games/3/move", httpContent5);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response4.StatusCode);  
            Assert.Equal(HttpStatusCode.OK, response5.StatusCode);

            var game = JsonConvert.DeserializeObject<GameDto>(await response5.Content.ReadAsStringAsync());
            
            Assert.NotNull(game);
            Assert.Equal(Enums.GameResult.XWin, game.Result);
            Assert.Null(game.CurrentSymbol);
        }

        #endregion
    }
}
