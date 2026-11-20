namespace MapEditor;

public struct Position
{
    private int _x;
    private int _y;

    public int X => _x;
    public int Y => _y;

    public void SetPosition(int x, int y)
    {
        //if (x <= 0 || y <= 0) { throw new Exception("Координаты должны быть положительными значениями"); }
        _x = x;
        _y = y;
    }

    public Position(int x, int y)
    {
        //if (x <= 0 || y <= 0) { throw new Exception("Координаты должны быть положительными значениями"); }
        _x = x;
        _y = y;
    }

    public static Position operator +(Position pos1, Position pos2)
    {
        return new Position(pos1.X + pos2.X, pos1.Y + pos2.Y);
    }
    public static bool operator ==(Position pos1, Position pos2)
    {
        return (pos1.X == pos2.X) && (pos1.Y == pos2.Y);
    }
    public static bool operator !=(Position pos1, Position pos2)
    {
        return (pos1.X != pos2.X) || (pos1.Y != pos2.Y);
    }
}