namespace MapEditor;

public struct Player
{
    public Position Pos;

    public void SetPosition(Position pos)
    {
        Pos = pos;
    }

    public CellType[] BlockedCellTypes;

    public Player(Position pos, CellType[] blockedCellTypes)
    {
        Pos = pos;
        BlockedCellTypes = blockedCellTypes;
    }
    public Player(Position pos)
    {
        Pos = pos;
        BlockedCellTypes = new CellType[] { CellType.Wall, CellType.Water };
    }
}
public enum Direction
{
    North, // север (верх)
    South, // юг (низ)
    East, // восток (право)
    West // запад (лево)
}