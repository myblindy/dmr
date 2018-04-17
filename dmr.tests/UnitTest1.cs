using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            // test the doorways
            Assert.IsTrue(roomtemplate.Doorways.Any(d => d.X == 2 && d.Y == 0 && d.Direction == DoorwayDirection.North));
            Assert.IsTrue(roomtemplate.Doorways.Any(d => d.X == 0 && d.Y == 2 && d.Direction == DoorwayDirection.West));
            Assert.IsTrue(roomtemplate.Doorways.Any(d => d.X == 6 && d.Y == 3 && d.Direction == DoorwayDirection.East));
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

        [TestMethod]
        public void MapGenerationTest()
        {
            var room1 = RoomLoader.Load(@"
+d++
ds.+
++++".ToMemoryStream());
            var room2 = RoomLoader.Load(@"
++++
+..d
++++".ToMemoryStream());
            var room3 = RoomLoader.Load(@"
++++
+..+
+d++".ToMemoryStream());
            var room4 = RoomLoader.Load(@"
+++++
+...d
+++d+".ToMemoryStream());
            var map = new Map(30, 30, new List<RoomTemplate> { room1, room2, room3, room4 }, new Random());
        }

        [TestMethod]
        public void RoomPropertiesTest()
        {
            var room = RoomLoader.Load(@"
++
++

===

choose_weight = 0.1
".ToMemoryStream());

            Assert.AreEqual(0.1f, room.ChooseWeight);
        }
    }
}
