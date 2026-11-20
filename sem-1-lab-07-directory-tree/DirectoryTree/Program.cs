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

        Console.WriteLine(PrintTree(tree));
    }

    public static TreeElement[] CollectTree(string[] lines)
    {
        List<TreeElement> tree = new List<TreeElement>();
        int currentOffset = -1;

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
    public static string[] PrintTree(TreeElement[] tree)
    {
        string[] result = new string[tree.Length];
        for (int i = 0; i < tree.Length; i++)
        {
            result[i] = "";
            int offset = tree[i].Offset;

            for (int j = 0; j < offset; j++)
            {
                result[i] += "│ ";
            }

            if (offset == -1)
            {
                result[i] += $" {tree[i].Var}";
                continue;
            }

            if (i == tree.Length - 1)
            {
                result[i] += "└─";
            }
            else
            {
                if (offset > tree[i + 1].Offset)
                {
                    result[i] += "└─";
                }
                else
                {
                    result[i] += "├─";
                }
            }
            result[i] += $" {tree[i].Var}";
        }
        return result;
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