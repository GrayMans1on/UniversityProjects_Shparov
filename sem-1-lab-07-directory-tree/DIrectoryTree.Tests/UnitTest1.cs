using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using DirectoryTree;

namespace DirectoryTree.Tests;

[TestFixture]
[NonParallelizable]
public class DirectoryTreeTests
{
    [Test]
    public void CollectTree_Example_PreservesNamesAndDepths()
    {
        TreeElement[] tree = Program.CollectTree(new[]
        {
            "A", "", "BEGIN B", "BEGIN C", "END", "",
            "BEGIN D", "END", "END", "", "BEGIN E", "END"
        });

        Assert.Multiple(() =>
        {
            Assert.That(tree.Select(element => element.Var).ToArray(), Is.EqualTo(new[] { 'A', 'B', 'C', 'D', 'E' }));
            Assert.That(tree.Select(element => element.Offset).ToArray(), Is.EqualTo(new[] { -1, 0, 1, 1, 0 }));
        });
    }

    [Test]
    public void CollectTree_EmptyInput_ReturnsEmptyTree()
    {
        TreeElement[] tree = Program.CollectTree(Array.Empty<string>());

        Assert.That(tree, Is.Empty);
    }

    [Test]
    public void CollectTree_OnlyRoot_ReturnsRootAtCorrectDepth()
    {
        TreeElement[] tree = Program.CollectTree(new[] { "A" });

        Assert.Multiple(() =>
        {
            Assert.That(tree, Has.Length.EqualTo(1));
            Assert.That(tree[0].Var, Is.EqualTo('A'));
            Assert.That(tree[0].Offset, Is.EqualTo(-1));
        });
    }

    [Test]
    public void CollectTree_BlankLines_DoNotSkipDirectories()
    {
        TreeElement[] tree = Program.CollectTree(new[] { "A", "", "BEGIN B", "", "BEGIN C", "END", "END" });

        Assert.That(tree.Select(element => element.Var).ToArray(), Is.EqualTo(new[] { 'A', 'B', 'C' }));
    }

    [Test]
    public void CollectTree_NestedDirectories_IncreaseDepth()
    {
        TreeElement[] tree = Program.CollectTree(new[]
        {
            "A", "BEGIN B", "BEGIN C", "BEGIN D", "END", "END", "END"
        });

        Assert.That(tree.Select(element => element.Offset).ToArray(), Is.EqualTo(new[] { -1, 0, 1, 2 }));
    }

    [Test]
    public void CollectTree_SiblingDirectories_HaveSameDepth()
    {
        TreeElement[] tree = Program.CollectTree(new[] { "A", "BEGIN B", "END", "BEGIN C", "END" });

        Assert.That(tree.Select(element => element.Offset).ToArray(), Is.EqualTo(new[] { -1, 0, 0 }));
    }

    [Test]
    public void PrintTree_Example_ProducesExpectedBranches()
    {
        string[] lines = Program.PrintTree(Program.CollectTree(new[]
        {
            "A", "", "BEGIN B", "BEGIN C", "END", "",
            "BEGIN D", "END", "END", "", "BEGIN E", "END"
        }));

        Assert.That(lines.Select(line => line.TrimStart(' ')).ToArray(), Is.EqualTo(new[]
        {
            "A", "├─ B", "│ ├─ C", "│ └─ D", "└─ E"
        }));
    }

    [Test]
    public void PrintTree_OnlyRoot_PrintsItsName()
    {
        string[] lines = Program.PrintTree(Program.CollectTree(new[] { "A" }));

        Assert.That(lines, Has.Length.EqualTo(1));
        Assert.That(lines[0].Trim(), Is.EqualTo("A"));
    }

    [Test]
    public void PrintTree_SiblingDirectories_EndsLastBranch()
    {
        string[] lines = Program.PrintTree(Program.CollectTree(new[] { "A", "BEGIN B", "END", "BEGIN C", "END" }));

        Assert.Multiple(() =>
        {
            Assert.That(lines[1].TrimStart(), Does.StartWith("├─"));
            Assert.That(lines[2].TrimStart(), Does.StartWith("└─"));
        });
    }

    [Test]
    public void PrintTree_LastSiblingWithChildren_EndsItsBranch()
    {
        string[] lines = Program.PrintTree(Program.CollectTree(new[]
        {
            "A", "BEGIN B", "END", "BEGIN C", "END", "BEGIN D"
        }));

        Assert.That(lines[3].TrimStart(), Does.StartWith("└─"));
    }

    [Test]
    public void PrintTree_EmptyTree_PrintsNothing()
    {
        string[] lines = Program.PrintTree(Array.Empty<TreeElement>());
    }
}