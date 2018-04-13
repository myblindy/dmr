using dmr.Models;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Loaders
{
    public static class RoomLoader
    {
        public static RoomTemplate Load(string path)
        {
            using (var stream = File.OpenRead(path))
                return Load(stream);
        }

        public static RoomTemplate Load(Stream stream)
        {
            var def = new List<string>();

            string line;
            using (var streamreader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
                while ((line = streamreader.ReadLine()) != null)
                    def.Add(line);

            var res = new RoomTemplate { Tiles = new TileTemplate[def.SkipWhile(w => w.Length == 0).Count(), def.Max(w => w.Length)] };
            def.SkipWhile(w => w.Length == 0).ForEach((l, colidx) =>
                l.ForEach((c, rowidx) =>
                    res.Tiles[colidx, rowidx] = c == '.' ? TileTemplate.EmptyTile : c == '+' ? TileTemplate.WallTile : c == 'd' ? TileTemplate.DoorTile : c == ' ' ? (TileTemplate)null :
                        c == 's' ? TileTemplate.StartTile : throw new InvalidOperationException()));

            return res;
        }
    }
}
