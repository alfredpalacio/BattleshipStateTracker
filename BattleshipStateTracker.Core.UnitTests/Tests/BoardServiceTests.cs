using Autofac.Extras.Moq;
using BattleshipStateTracker.Core.Services.Implementations;
using BattleshipStateTracker.Core.Services.Interfaces;
using BattleshipStateTracker.Shared.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BattleshipStateTracker.Core.UnitTests.Tests
{
    public class BoardServiceTests
    {
        #region Fields

        private readonly Battleship _battleship;

        #endregion Fields

        #region Constructor

        public BoardServiceTests()
        {
            _battleship = new Battleship
            {
                XCoordinate = 1,
                YCoordinate = 2,
                Length = 3,
                IsHorizontal = true,
            };
        }

        #endregion Constructor

        #region AddBattleship Tests

        [Theory]
        [InlineData(1, 2, 3, true)]
        [InlineData(4, 3, 2, true)]
        public void AddBattleshipTest(
            int xCoordinate,
            int yCoordinate,
            int length,
            bool isHorizontal)
        {
            // Arrange
            using var mock = AutoMock.GetLoose();
            var service = mock.Create<BoardService>();

            var battleship = new Battleship
            {
                XCoordinate = xCoordinate,
                YCoordinate = yCoordinate,
                Length = length,
                IsHorizontal = isHorizontal,
            };

            var board = new Board();
            board.TryAdd(battleship);

            // Mock _memoryCacheWrapper.GetCache<Board>(nameof(Board));
            mock.Mock<IMemoryCacheWrapper>()
                .Setup(x => x.GetCache<Board>(nameof(Board)))
                .Returns(() => (true, board));

            // Act
            service.AddBattleship(battleship);

            // Assert
        }

        #endregion AddBattleship Tests

        #region Attack Tests

        [Theory]
        [InlineData(1, 1, false)]
        [InlineData(1, 2, true)]
        public void AttackTest(
            int xCoordinate,
            int yCoordinate,
            bool result)
        {
            // Arrange
            using var mock = AutoMock.GetLoose();
            var service = mock.Create<BoardService>();

            var board = new Board();
            board.TryAdd(_battleship);

            // Mock _memoryCacheWrapper.GetCache<Board>(nameof(Board));
            mock.Mock<IMemoryCacheWrapper>()
                .Setup(x => x.GetCache<Board>(nameof(Board)))
                .Returns(() => (true, board));

            // Act
            service.AddBattleship(_battleship);
            var response = service.Attack(xCoordinate, yCoordinate);

            // Assert
            Assert.Equal(response, result);
        }

        [Fact]
        public async Task AttackNegativeTest()
        {
            // Arrange
            using var mock = AutoMock.GetLoose();
            var service = mock.Create<BoardService>();

            // Act
            Task act() => Task.Run(() => service.Attack(_battleship.XCoordinate, _battleship.YCoordinate));

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        #endregion Attack Tests

        #region IsGameOver Tests

        [Fact]
        public void IsGameOverTest()
        {
            // Arrange
            using var mock = AutoMock.GetLoose();
            var service = mock.Create<BoardService>();

            var board = new Board();
            board.TryAdd(_battleship);

            // Mock _memoryCacheWrapper.GetCache<Board>(nameof(Board));
            mock.Mock<IMemoryCacheWrapper>()
                .Setup(x => x.GetCache<Board>(nameof(Board)))
                .Returns(() => (true, board));

            // Mock _memoryCacheWrapper.SetCache(nameof(Board), board);
            mock.Mock<IMemoryCacheWrapper>()
                .Setup(x => x.SetCache(nameof(Board), board));

            // Act
            service.AddBattleship(_battleship);
            var response = service.IsGameOver();

            // Assert
            Assert.False(response);
        }

        [Fact]
        public async Task IsGameOverNegativeTest()
        {
            // Arrange
            using var mock = AutoMock.GetLoose();
            var service = mock.Create<BoardService>();

            // Act
            Task act() => Task.Run(() => service.IsGameOver());

            // Assert
            await Assert.ThrowsAsync<Exception>(act);
        }

        #endregion IsGameOver Tests

        #region CreateBattleship Tests

        [Theory]
        [InlineData(5, true)]
        [InlineData(10, true)]
        [InlineData(1, true)]
        [InlineData(0, false)]
        public void CreateBattleshipTest(
            int dimension,
            bool result)
        {
            // Arrange
            using var mock = AutoMock.GetLoose();
            var service = mock.Create<BoardService>();

            // Act
            var response = service.CreateBattleship(dimension);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(result, response.XCoordinate > 0 && response.XCoordinate <= dimension);
            Assert.Equal(result, response.YCoordinate > 0 && response.YCoordinate <= dimension);
            Assert.Equal(result, response.Length >= 1 && response.Length <= dimension);
        }

        #endregion CreateBattleship Tests
    }
}