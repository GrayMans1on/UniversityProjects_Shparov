using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using MapEditor;
using Microsoft.VisualBasic;
using NUnit.Framework;

namespace MapEditor.Tests;

[TestFixture]
public class MapTests
{
    private Map _map = null!;
    private string _savePath = null!;

    [SetUp]
    public void SetUp()
    {
        Cell[] cells = new Cell[]
        {
            new Cell(CellType.Wall, new Position(4, 2)),
            new Cell(CellType.Water, new Position(1, 3))
        };

        _map = new Map(5, 4, cells, new Player(new Position(2, 2)));
        _savePath = Path.Combine(Path.GetTempPath(), "MapEditorTests-" + Guid.NewGuid() + ".json");
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_savePath))
        {
            File.Delete(_savePath);
        }
    }

    [Test]
    public void CreateMap_SetsSizeCellsAndPlayer()
    {
        Assert.That(_map.Width, Is.EqualTo(5));
        Assert.That(_map.Height, Is.EqualTo(4));
        Assert.That(_map.Cells, Has.Count.EqualTo(2));
        Assert.That(_map.Player.Pos, Is.EqualTo(new Position(2, 2)));
    }

    [Test]
    public void FindCellIndex_FindsCellByPosition()
    {
        int index = _map.FindCellIndex(new Position(4, 2));

        Assert.That(index, Is.GreaterThanOrEqualTo(0));
        Assert.That(_map.Cells[index].Type, Is.EqualTo(CellType.Wall));
    }

    [Test]
    public void FindCellIndex_ReturnsMinusOneForEmptyPosition()
    {
        Assert.That(_map.FindCellIndex(new Position(3, 2)), Is.EqualTo(-1));
    }

    [Test]
    public void AddCell_AddsCellToMap()
    {
        Cell cell = new Cell(CellType.Water, new Position(5, 4));

        Assert.That(_map.AddCell(cell), Is.True);
        Assert.That(_map.Cells, Does.Contain(cell));
    }[Test]
    public void RemoveCell_RemovesExistingCell()
    {
        _map.RemoveCell(_map.Cells[0]);

        Assert.That(_map.FindCellIndex(new Position(4, 2)), Is.EqualTo(-1));
    }

    [Test]
    public void ChangeCell_ReplacesExistingCell()
    {
        _map.ChangeCell(new Cell(CellType.Water, new Position(4, 2)));
        int index = _map.FindCellIndex(new Position(4, 2));

        Assert.That(index, Is.GreaterThanOrEqualTo(0));
        Assert.That(_map.Cells[index].Type, Is.EqualTo(CellType.Water));
        Assert.That(_map.Cells, Has.Count.EqualTo(2));
    }

    [Test]
    public void SetSize_RemovesCellsOutsideNewBounds()
    {
        bool removed = _map.SetSize(3, 2, out List<Cell> removedCells);

        Assert.That(removed, Is.True);
        Assert.That(removedCells, Has.Count.EqualTo(2));
        Assert.That(_map.Cells, Is.Empty);
        Assert.That(_map.Width, Is.EqualTo(3));
        Assert.That(_map.Height, Is.EqualTo(2));
    }

    [Test]
    public void ToString_DisplaysCellsAndPlayer()
    {
        string expected = ".....\n.P.#.\nW....\n.....\n";

        Assert.That(_map.ToString(), Is.EqualTo(expected));
    }

    [Test]
    public void TryMovePlayer_ChangesPlayerPosition()
    {
        bool moved = _map.TryMovePlayer(Direction.East);

        Assert.That(moved, Is.True);
        Assert.That(_map.Player.Pos, Is.EqualTo(new Position(3, 2)));
    }

    [TestCase(CellType.Wall)]
    [TestCase(CellType.Water)]
    public void TryMovePlayer_DoesNotEnterBlockedCell(CellType type)
    {
        _map.ChangeCell(new Cell(type, new Position(3, 2)));

        Assert.That(_map.TryMovePlayer(Direction.East), Is.False);
        Assert.That(_map.Player.Pos, Is.EqualTo(new Position(2, 2)));
    }

    [Test]
    public void TryMovePlayer_DoesNotLeaveMap()
    {
        Map map = new Map(3, 3, Array.Empty<Cell>(), new Player(new Position(1, 1)));

        Assert.That(map.TryMovePlayer(Direction.West), Is.False);
        Assert.That(map.Player.Pos, Is.EqualTo(new Position(1, 1)));
    }

    [Test]
    public void SaveAndLoad_RestoresMapSizeCellsAndPlayerPosition()
    {
        Program.SaveMap(_map, _savePath);
        Map loaded = Program.LoadMap(_savePath);

        Assert.That(loaded.Width, Is.EqualTo(_map.Width));
        Assert.That(loaded.Height, Is.EqualTo(_map.Height));
        Assert.That(loaded.Cells, Is.EqualTo(_map.Cells));
        Assert.That(loaded.Player.Pos, Is.EqualTo(_map.Player.Pos));
    }

    [Test]
    public void SaveAndLoad_RestoresBlockedCellTypes()
    {
        Program.SaveMap(_map, _savePath);
        Map loaded = Program.LoadMap(_savePath);

        Assert.That(loaded.Player.BlockedCellTypes, Is.EqualTo(_map.Player.BlockedCellTypes));
    }

    [Test]
    public void SaveAndLoad_RestoresStateAfterChanges()
    {
        string original = _map.ToString();
        Program.SaveMap(_map, _savePath);

        _map.ChangeCell(new Cell(CellType.Water, new Position(4, 2)));
        _map.TryMovePlayer(Direction.East);

        Map restored = Program.LoadMap(_savePath);

        Assert.That(restored.ToString(), Is.EqualTo(original));
        Assert.That(restored.Player.Pos, Is.EqualTo(new Position(2, 2)));
        Assert.That(restored.Cells, Is.Not.SameAs(_map.Cells));
    }
}