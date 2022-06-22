using BattleshipStateTracker.Core.Services.Interfaces;
using BattleshipStateTracker.Shared.Models;
using System;

namespace BattleshipStateTracker.Core.Services.Implementations
{
    public class BoardService : IBoardService
    {
        #region Local variables

        private readonly IMemoryCacheWrapper _memoryCacheWrapper;

        private readonly Random _random;

        #endregion Local variables

        #region Constructor

        public BoardService(IMemoryCacheWrapper memoryCacheWrapper)
        {
            _memoryCacheWrapper = memoryCacheWrapper;

            _random = new Random();
        }

        #endregion Constructor

        #region Public methods

        /// <inheritdoc />
        public bool AddBattleship(Battleship battleship = null)
        {
            try
            {
                var (isAvailable, board) = _memoryCacheWrapper.GetCache<Board>(nameof(Board));

                if (!isAvailable)
                {
                    board = new Board();
                }

                battleship ??= CreateBattleship(board.Dimension);
                var result = board.TryAdd(battleship);

                if (result)
                {
                    _memoryCacheWrapper.SetCache(nameof(Board), board);
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public bool Attack(
            int xCoordinate,
            int yCoordinate)
        {
            try
            {
                var (isAvailable, board) = _memoryCacheWrapper.GetCache<Board>(nameof(Board));

                if (!isAvailable || !board.HasBattleships)
                {
                    throw new Exception("There are no battleships on the board");
                }

                var xIndex = xCoordinate - 1;
                var yIndex = yCoordinate - 1;

                var result = board.Attack(xCoordinate, yCoordinate);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <inheritdoc />
        public bool IsGameOver()
        {
            try
            {
                var (isAvailable, board) = _memoryCacheWrapper.GetCache<Board>(nameof(Board));

                if (!isAvailable || !board.HasBattleships)
                {
                    throw new Exception("There are no battleships on the board");
                }

                var result = board.IsGameOver();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <inheritdoc />
        public Battleship CreateBattleship(int dimension)
        {
            dimension += 1;

            var battleship = new Battleship
            {
                XCoordinate = _random.Next(1, dimension),
                YCoordinate = _random.Next(1, dimension),
                Length = _random.Next(1, dimension),
                IsHorizontal = _random.Next(2) == 1,
            };

            return battleship;
        }

        #endregion Public methods
    }
}