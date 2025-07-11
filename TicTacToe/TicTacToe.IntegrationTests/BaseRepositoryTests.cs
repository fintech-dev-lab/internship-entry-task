using Microsoft.EntityFrameworkCore;
using TicTacToe.Core.BaseEntities;
using TicTacToe.Core.Entities;
using TicTacToe.Services.Repository;

namespace TicTacToe.IntegrationTests;


public class BaseRepositoryTests : IAsyncLifetime
{
    private TicTacToeContext _context;
    private BaseRepository<User> _repository;
    private readonly Guid _testUserId = Guid.NewGuid();
    private DbContextOptions<TicTacToeContext> _options;

    public async Task InitializeAsync()
    {
        _options = new DbContextOptionsBuilder<TicTacToeContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TicTacToeContext(_options);
        await _context.Database.EnsureCreatedAsync();
        _repository = new BaseRepository<User>(_context);
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddEntity()
    {
        // Arrange
        var user = new User { Uuid = _testUserId, FullName = "Test User" };

        // Act
        await _repository.CreateAsync(user, CancellationToken.None);

        // Assert
        using (var assertContext = new TicTacToeContext(_options))
        {
            var result = await assertContext.Users.FindAsync(_testUserId);
            Assert.NotNull(result);
            Assert.Equal("Test User", result.FullName);
        }
    }

    [Fact]
    public async Task GetAsync_ShouldRetrieveEntity()
    {
        // Arrange
        using (var arrangeContext = new TicTacToeContext(_options))
        {
            await arrangeContext.Users.AddAsync(new User { Uuid = _testUserId, FullName = "Test User" });
            await arrangeContext.SaveChangesAsync();
        }

        // Act
        var result = await _repository.GetAsync(_testUserId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test User", result.FullName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyEntity()
    {
        // Arrange
        using (var arrangeContext = new TicTacToeContext(_options))
        {
            await arrangeContext.Users.AddAsync(new User { Uuid = _testUserId, FullName = "Original" });
            await arrangeContext.SaveChangesAsync();
        }

        var user = new User { Uuid = _testUserId, FullName = "Updated" };

        // Act
        await _repository.UpdateAsync(user, CancellationToken.None);

        // Assert
        using (var assertContext = new TicTacToeContext(_options))
        {
            var updated = await assertContext.Users.FindAsync(_testUserId);
            Assert.Equal("Updated", updated.FullName);
        }
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        using (var arrangeContext = new TicTacToeContext(_options))
        {
            await arrangeContext.Users.AddAsync(new User { Uuid = _testUserId, FullName = "To Delete" });
            await arrangeContext.SaveChangesAsync();
        }

        // Act
        await _repository.DeleteAsync(_testUserId, CancellationToken.None);

        // Assert
        using (var assertContext = new TicTacToeContext(_options))
        {
            var deleted = await assertContext.Users.FindAsync(_testUserId);
            Assert.Null(deleted);
        }
    }
}