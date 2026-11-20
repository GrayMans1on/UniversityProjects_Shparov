namespace MapEditor;

public struct Position
{
    private int _x;
    private int _y;

    public int X => _x;
    public int Y => _y;

    public bool SetPosition(int x, int y)
    {
        if (x <= 0 || y <= 0) { return false; }
        _x = x;
        _y = y;
        return true;
    }

    public Position(int x, int y)
    {
        _x = Math.Max(x, 1);
        _y = Math.Max(y, 1);
    }
}