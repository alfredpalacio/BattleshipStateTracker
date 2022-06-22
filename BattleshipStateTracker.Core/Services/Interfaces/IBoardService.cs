using BattleshipStateTracker.Shared.Models;

namespace BattleshipStateTracker.Core.Services.Interfaces
{
    public interface IBoardService
    {
        /// <summary>
        /// Add battleship on the board
        /// </summary>
        /// <param name="battleship">
        /// Battleship to add on the board.
        /// If no battleship object is given, a random one will be created
        /// </param>
        /// <returns>True, if successfully added. Otherwise, false</returns>
        bool AddBattleship(Battleship battleship = null);

        /// <summary>
        /// Attack the specified coordinates on the board
        /// </summary>
        /// <param name="xCoordinate">X coordinate to attack</param>
        /// <param name="yCoordinate">Y coordinate to attack</param>
        /// <returns>True, if it hits a part of a battleship. Otherwise, false</returns>
        bool Attack(int xCoordinate, int yCoordinate);

        /// <summary>
        /// Check whether all battleships are sunk
        /// </summary>
        /// <returns>True, if all battleships are sunk. Otherwise, false</returns>
        bool IsGameOver();

        /// <summary>
        /// Create a battleship with random properties
        /// </summary>
        /// <param name="dimension">Dimension of the board</param>
        /// <returns>New battleship instance</returns>
        Battleship CreateBattleship(int dimension);
    }
}