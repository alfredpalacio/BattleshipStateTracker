namespace BattleshipStateTracker.Shared.Models
{
    public class Battleship
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public int Length { get; set; }
        public bool IsHorizontal { get; set; }

        public int XIndex => XCoordinate - 1;
        public int YIndex => YCoordinate - 1;
    }
}