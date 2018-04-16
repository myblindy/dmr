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
                return Load(stream, Path.GetFileName(path));
        }

        public static RoomTemplate Load(Stream stream, string roomname = null)
        {
            var def = new List<string>();

            string line;
            using (var streamreader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
                while ((line = streamreader.ReadLine()) != null)
                    def.Add(line);

            var res = new RoomTemplate { Tiles = new TileTemplate[def.SkipWhile(w => w.Length == 0).Count(), def.Max(w => w.Length)], Name = roomname };
            def.SkipWhile(w => w.Length == 0).ForEach((l, colidx) =>
                l.ForEach((c, rowidx) =>
                {
                    void set(TileTemplate tile) => res.Tiles[colidx, rowidx] = tile;
                    switch (c)
                    {
                        case '.': set(TileTemplate.EmptyTile); break;
                        case '+': set(TileTemplate.WallTile); break;
                        case 's': set(TileTemplate.StartTile); break;
                        case ' ': set(null); break;
                        case 'd': set(TileTemplate.DoorTile); break;
                        default: throw new InvalidOperationException();
                    }
                }));
            res.UpdateDoorways();

            return res;
        }
    }
}
