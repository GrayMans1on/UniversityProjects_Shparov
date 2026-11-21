namespace MapEditor;

public struct Player
{
    public Position Pos;

    public void SetPosition(Position pos)
    {
        Pos = pos;
    }

    private CellType[] _blockedCellTypes; // типы ячеек, на которые игрок не может наступать
    public CellType[] BlockedCellTypes => _blockedCellTypes;

    public Player(Position pos, CellType[] blockedCellTypes)
    {
        Pos = pos;
        _blockedCellTypes = blockedCellTypes;
    }
    public Player(Position pos)
    {
        Pos = pos;
        _blockedCellTypes = new CellType[] { CellType.Wall, CellType.Water };
    }
}
public enum Direction
{
    North, // север (верх)
    South, // юг (низ)
    East, // восток (право)
    West // запад (лево)
}