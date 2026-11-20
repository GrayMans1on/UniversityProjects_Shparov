using MapEditor;

public static class CellTypeExtension
{
    public static string ToSymbol(this CellType type)
    {
        switch (type)
        {
            case CellType.Wall:
                return "#";
            case CellType.Water:
                return "W";
            default:
                return "?";
        }
    }
}
public static class DirectionExtension
{
    public static Position ToPosition(this Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return new Position(0, 1);
            case Direction.South:
                return new Position(0, -1);
            case Direction.East:
                return new Position(1, 0);
            case Direction.West:
                return new Position(-1, 0);
            default:
                throw new Exception("Ошибка при задании направления");
        }
    }
}