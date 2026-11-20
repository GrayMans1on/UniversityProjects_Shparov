namespace MapEditor;
public class Program
{
    static void Main()
    {
        Map map = new Map(10, 3, new Cell[]{
        new Cell(CellType.Water, new Position(1, 2)),
        new Cell(CellType.Water, new Position(1, 3)),
        new Cell(CellType.Wall, new Position(5, 1)),
        new Cell(CellType.Wall, new Position(6, 1)),
        new Cell(CellType.Wall, new Position(7, 1)),
        new Cell(CellType.Wall, new Position(8, 1)),
        new Cell(CellType.Wall, new Position(9, 1)),
        new Cell(CellType.Wall, new Position(10, 1)),
        }, new Player(new Position(4, 2)));

        Console.WriteLine(map);

        while (true)
        {
            Console.Clear();
            Console.WriteLine(map);

            Console.WriteLine("\nПеремещаем персонажа: введите W/A/S/D для перемещения или что угодно для отмены: ");
            string move = Console.ReadLine();
            switch (move)
            {
                case "W":
                    map.TryMovePlayer(Direction.North);
                    break;
                case "S":
                    map.TryMovePlayer(Direction.South);
                    break;
                case "A":
                    map.TryMovePlayer(Direction.West);
                    break;
                case "D":
                    map.TryMovePlayer(Direction.East);
                    break;
                default:
                    return;
            }
        }
    }
}
