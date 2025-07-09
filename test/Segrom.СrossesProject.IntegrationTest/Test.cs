using Microsoft.AspNetCore.Mvc.Testing;

namespace Segrom.СrossesProject.IntegrationTest;

public class Test(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
	[Fact]
	public async Task GamesNew_MustReturnGame()
	{
		// Arrange
		var client = factory.CreateClient();
		
		// Act
		HttpResponseMessage? response = null;
		var exception = await Record.ExceptionAsync(async () =>
		{
			response = await client.GetAsync("api/v1/games/new");
			response.EnsureSuccessStatusCode();
		});
		
		// Assert
		Assert.Null(exception);
		Assert.NotNull(response);
	}
}