namespace MapEditor;

public struct Cell
{
    public CellType Type;
    public Position Pos;

    public Cell(CellType type, Position position)
    {
        Type = type;
        Pos = position;
    }
}
public enum CellType
{
    Wall,
    Water
}