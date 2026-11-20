using System;
using System.IO;

namespace DirectoryTree;
public class Program
{
    // File content
    private static string[] _lines = null!;

    private static void Main()
    {
        _lines = File.ReadAllLines("..\\..\\..\\tree.txt");

        TreeElement[] tree = CollectTree(_lines);

        PrintTree(tree);
    }

    private static TreeElement[] CollectTree(string[] lines)
    {
        List<TreeElement> tree = new List<TreeElement>();
        int currentOffset = 0;

        foreach (string line in lines)
        {
            if (line.Contains("BEGIN"))
            {
                char name = line[^1];
                currentOffset++;
                tree.Add(new TreeElement(name, currentOffset));
            }
            else if (line.Contains("END"))
            {
                currentOffset--;
            }
            else if (line.Trim() != "")
            {
                char name = line[^1];
                tree.Add(new TreeElement(name, currentOffset));
            }
        }

        return tree.ToArray();
    }
    private static void PrintTree(TreeElement[] tree)
    {
        for (int i = 0; i < tree.Length; i++)
        {
            int offset = tree[i].Offset;

            for (int j = 0; j < offset; j++)
            {
                Console.Write("│ ");
            }

            if (offset == 0)
            {
                Console.WriteLine($" {tree[i].Var}");
                continue;
            }

            if (i == tree.Length - 1)
            {
                Console.Write("└─");
            }
            else
            {
                if (offset > tree[i + 1].Offset)
                {
                    Console.Write("└─");
                }
                else
                {
                    Console.Write("├─");
                }
            }
            Console.WriteLine($" {tree[i].Var}");
        }
    }


    
}

public struct TreeElement
{
    public char Var;
    public int Offset;

    public TreeElement(char var, int offset)
    {
        Var = var;
        Offset = offset;
    }
}