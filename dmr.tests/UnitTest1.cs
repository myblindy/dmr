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
            Assert.AreEqual(TileTemplate.WallTile, roomtemplate.Tiles[1, 0]);
            Assert.AreEqual(TileTemplate.DoorTile, roomtemplate.Tiles[2, 0]);
            Assert.AreEqual(TileTemplate.WallTile, roomtemplate.Tiles[3, 0]);
            Assert.AreEqual(null, roomtemplate.Tiles[4, 0]);
            Assert.AreEqual(null, roomtemplate.Tiles[5, 0]);
            Assert.AreEqual(TileTemplate.EmptyTile, roomtemplate.Tiles[3, 3]);
            Assert.AreEqual(TileTemplate.DoorTile, roomtemplate.Tiles[6, 3]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => roomtemplate.Tiles[7, 0]);
        }
    }
}
