namespace MapEditor;

public struct Player
{
    private Position _position;

    public Position Position => _position;

    public void SetPosition(Position pos)
    {
        _position = pos;
    }

    public Player(Position pos)
    {
        _position = pos;
    }
}
public enum Direction
{
    North, // север (низ)
    South, // юг (верх)
    East, // восток (право)
    West // запад (лево)
}