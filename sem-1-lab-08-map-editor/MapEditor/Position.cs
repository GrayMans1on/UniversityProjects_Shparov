namespace MapEditor;

public struct Position
{
    public int X;
    public int Y;

    public Position(int x, int y)
    {
        X = x;
        Y = y;
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