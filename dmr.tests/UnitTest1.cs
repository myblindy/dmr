using System;
using System.IO;
using dmr.Loaders;
using dmr.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dmr.tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RoomLoadingTest()
        {
            var roomtemplate = RoomLoader.Load(@"
++d+
+..++++
d.....+
+.....d
+++++++".ToMemoryStream());

            // test the loaded room
            Assert.AreEqual(TileTemplate.WallTile, roomtemplate.Tiles[0, 0]);
            Assert.AreEqual(TileTemplate.WallTile, roomtemplate.Tiles[0, 1]);
            Assert.AreEqual(TileTemplate.DoorTile, roomtemplate.Tiles[0, 2]);
            Assert.AreEqual(TileTemplate.WallTile, roomtemplate.Tiles[0, 3]);
            Assert.AreEqual(null, roomtemplate.Tiles[0, 4]);
            Assert.AreEqual(null, roomtemplate.Tiles[0, 5]);
            Assert.AreEqual(TileTemplate.EmptyTile, roomtemplate.Tiles[3, 3]);
            Assert.AreEqual(TileTemplate.DoorTile, roomtemplate.Tiles[3, 6]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => roomtemplate.Tiles[0, 7]);
        }

        [TestMethod]
        public void MapPlacementTest()
        {
            var room1 = RoomLoader.Load(@"
+d++
ds.+
++++".ToMemoryStream());
            var room2 = RoomLoader.Load(@"
++++
+..d
++++".ToMemoryStream());
            var map = new Map(10, 4);

            // place the rooms
            room2.Place(map, 0, 0);
            room1.Place(map, 4, 0);

            // test the placed rooms
            Assert.AreEqual(TileTemplate.WallTile, map.Tiles[0, 3]);
            Assert.AreEqual(TileTemplate.WallTile, map.Tiles[0, 4]);
            Assert.AreEqual(TileTemplate.DoorTile, map.Tiles[0, 5]);
            Assert.AreEqual(TileTemplate.DoorTile, map.Tiles[1, 3]);
            Assert.AreEqual(TileTemplate.DoorTile, map.Tiles[1, 4]);
            Assert.AreEqual(TileTemplate.EmptyTile, map.Tiles[1, 2]);
            Assert.AreEqual(TileTemplate.EmptyTile, map.Tiles[1, 1]);
            Assert.AreEqual(TileTemplate.EmptyTile, map.Tiles[1, 6]);
            Assert.AreEqual(TileTemplate.StartTile, map.Tiles[1, 5]);
        }
    }
}
