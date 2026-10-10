namespace Map;
internal class Program
{
    public enum Cell
    {
        Trap = -1,
        Empty = 0,
        Wall = 1,
        Treasure = 2,
    }
    static Cell CellFromChar(char c)
    {
        switch (c)
        {
            case '-':
                return Cell.Trap;
            case '0':
                return Cell.Empty;
            case '1':
                return Cell.Wall;
            case '2':
                return Cell.Treasure;
            default:
                return Cell.Empty;
        }
    }
    static char CharFromCell(Cell cell)
    {
        switch (cell)
        {
            case Cell.Trap:
                return '-';
            case Cell.Empty:
                return '0';
            case Cell.Wall:
                return '1';
            case Cell.Treasure:
                return '2';
            default:
                return '0';
        }
    }


    static void Main()
    {
        Cell[,] mapCells = GetFromFile("..\\..\\..\\map1.txt");

        Console.WriteLine("Всего сокровищ на карте: " + (CountByType(mapCells, Cell.Treasure) + "\nВсего ловушек на карте: " + CountByType(mapCells, Cell.Trap)));
        (string Row, int Rank) targetRow = RowByMaxType(mapCells, Cell.Treasure);
        Console.WriteLine($"В строке #{targetRow.Rank} \"{targetRow.Row}\" больше всего сокровищ");
    }

    public static Cell[,] GetFromFile(string path)
    {
        string[] lines = File.ReadAllLines(path);

        int sizeA = lines.Length;
        int sizeB = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Replace("-1", "-");
            sizeB = Math.Max(sizeB, lines[i].Length);
        }

        Cell[,] cells = new Cell[sizeB, sizeA];
        for (int i = 0; i < sizeA; i++)
        {
            string line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                cells[j, i] = CellFromChar(line[j]);
            }
        }
        return cells;
    }

    public static int CountByType(Cell[,] cells, Cell cellType)
    {
        int count = 0;

        int sizeA = cells.GetLength(0);
        int sizeB = cells.GetLength(1);

        for (int x = 0; x < sizeA; x++)
        {
            for (int y = 0; y < sizeB; y++)
            {
                if (cells[x, y] == cellType)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public static (string, int) RowByMaxType(Cell[,] cells, Cell cellType)
    {
        int maxCount = 0;
        (string Row, int Rank) targetRow = ("", 0);
        int sizeA = cells.GetLength(0);
        int sizeB = cells.GetLength(1);
        for (int y = 0; y < sizeB; y++)
        {
            int count = 0;
            string row = "";
            for (int x = 0; x < sizeA; x++)
            {
                row += CharFromCell(cells[x, y]);
                if (cells[x, y] == cellType)
                {
                    count++;
                }
            }
            if (count >= maxCount)
            {
                targetRow = (row, y + 1);
                maxCount = count;
            }
        }
        return targetRow;
    }
    public static void MapToConsole(Cell[,] cells)
    {
        int sizeA = cells.GetLength(0);
        int sizeB = cells.GetLength(1);
        for (int y = 0; y < sizeB; y++)
        {
            for (int x = 0; x < sizeA; x++)
            {
                Console.Write(CharFromCell(cells[x, y]));
            }
            Console.WriteLine();
        }
    }
}
