using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleshipStateTracker.Shared.Models
{
    public class Board
    {
        #region Fields

        private readonly ICollection<Battleship> _battleships;

        #endregion Fields

        #region Properties

        public int Dimension { get; set; }

        public Block[,] Blocks { get; set; }

        public bool HasBattleships => _battleships != null && _battleships.Any();

        #endregion Properties

        #region Constructor

        public Board()
        {
            _battleships = new List<Battleship>();

            Dimension = 10;
            Blocks = new Block[10, 10];
        }

        #endregion Properties

        #region Public methods

        /// <summary>
        /// Try to add a battleship on the board
        /// </summary>
        /// <param name="battleship">Battleship object</param>
        /// <returns>True, if successfully added. Otherwise, false</returns>
        public bool TryAdd(Battleship battleship)
        {
            if (!ValidateBattleship(battleship) || CheckOverlap(battleship))
            {
                return false;
            }

            _battleships.Add(battleship);
            CheckOverlap(battleship, true);
            return true;
        }

        /// <summary>
        /// Check if it will overlap with other battleships
        /// </summary>
        /// <param name="battleship">Battleship object</param>
        /// <param name="placeBattleship">
        /// True, if we will place the battleship on the board.
        /// Otherwise, just check if it will overlap with other battleships
        /// </param>
        /// <returns>True, if the current battleship overlaps with other battleships. Otherwise, false</returns>
        public bool CheckOverlap(
            Battleship battleship,
            bool placeBattleship = false)
        {
            var x = battleship.XCoordinate;
            var y = battleship.YCoordinate;

            for (var i = 0; i < battleship.Length; i++)
            {
                var xLenth = x + i;
                var yLenth = y + i;
                var exceedsDimension = battleship.IsHorizontal ? xLenth > Dimension : yLenth > Dimension;

                // Check if battleship's length is longer than the dimension of the board
                if (exceedsDimension)
                {
                    return true;
                }

                var block = battleship.IsHorizontal ? GetBlock(xLenth, y) : GetBlock(x, yLenth);

                // Check if it overlaps with other battleships
                if (block != null && block.IsPlaced)
                {
                    return true;
                }

                if (placeBattleship)
                {
                    block ??= new Block
                    {
                        IsPlaced = true,
                        IsHit = false,
                    };

                    if (battleship.IsHorizontal)
                    {
                        Blocks[xLenth - 1, battleship.YIndex] = block;
                    }
                    else
                    {
                        Blocks[battleship.XIndex, yLenth - 1] = block;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if properties of the battleship are valid
        /// </summary>
        /// <param name="battleship">Battleship object</param>
        /// <returns>True, if properties are valid. Otherwise, false</returns>
        public bool ValidateBattleship(Battleship battleship)
        {
            var x = battleship.XCoordinate;
            var y = battleship.YCoordinate;
            var length = battleship.Length;

            // X coordinate is within the dimension of the board
            var validX = x > 0 && x <= Dimension;

            // Y coordinate is within the dimension of the board
            var validY = y > 0 && y <= Dimension;

            // The length of the battleship is within the dimension of the board
            var validLength = length >= 1 && length <= Dimension;

            return validX && validY && validLength;
        }

        /// <summary>
        /// Attack a block using coordinates
        /// </summary>
        /// <param name="xCoordinate">X coordinate to attack</param>
        /// <param name="yCoordinate">Y coordinate to attack</param>
        /// <returns>True, if it hits a part of a battleship. Otherwise, false</returns>
        public bool Attack(int xCoordinate, int yCoordinate)
        {
            var block = GetBlock(xCoordinate, yCoordinate);

            if (block != null)
            {
                block.IsHit = true;
            }

            return block != null;
        }

        /// <summary>
        /// Check whether all battleships are sunk
        /// </summary>
        /// <returns>True, if all battleships are sunk. Otherwise, false</returns>
        public bool IsGameOver()
        {
            if (!HasBattleships)
            {
                return false;
            }

            for (var i = 1; i <= Dimension; i++)
            {
                for (var j = 1; j <= Dimension; j++)
                {
                    var block = GetBlock(i, j);

                    if (block != null && !block.IsHit)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Get a block using coordinates
        /// </summary>
        /// <param name="xCoordinate">X coordinate to attack</param>
        /// <param name="yCoordinate">Y coordinate to attack</param>
        /// <returns>Block object</returns>
        public Block GetBlock(int xCoordinate, int yCoordinate)
        {
            var xIndex = xCoordinate - 1;
            var yIndex = yCoordinate - 1;

            if (xIndex < 0 || xIndex >= Dimension
                || yIndex < 0 || yIndex >= Dimension)
            {
                throw new Exception("Invalid coordinates");
            }
            return Blocks[xIndex, yIndex];
        }

        #endregion Public methods
    }
}