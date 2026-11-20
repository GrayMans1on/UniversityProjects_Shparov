namespace MapEditor;

public struct Player
{
    private Position _position;

    public Position Position => _position;

    public void SetPosition(Position pos)
    {
        _position = pos;
    }

    private CellType[] _blockedCellTypes; // типы ячеек, на которые игрок не может наступать
    public CellType[] BlockedCellTypes => _blockedCellTypes;

    public Player(Position pos, CellType[] blockedTypes)
    {
        _position = pos;
        _blockedCellTypes = blockedTypes;
    }
    public Player(Position pos)
    {
        _position = pos;
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