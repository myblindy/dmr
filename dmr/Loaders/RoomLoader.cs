using dmr.Models;
using dmr.Utilities;
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
            const string separator = "===";
            var def = new List<string>();

            {
                string line;
                using (var streamreader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
                    while ((line = streamreader.ReadLine()) != null)
                        def.Add(line);
            }

            var res = new RoomTemplate { Tiles = new TileTemplate[def.SkipWhile(w => w.Length == 0).Count(), def.Max(w => w.Length)], Name = roomname };

            // room layout, read only until the separator
            def.SkipWhile(w => w.Length == 0)
                .TakeWhile(w => !w.Trim().StartsWith(separator))
                .SkipLastWhere(w => w.Length == 0)
                .ForEach((l, colidx) =>
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

            // room properties, if any
            foreach (var line in def.SkipUntil(w => w.StartsWith(separator)).Where(w => !string.IsNullOrWhiteSpace(w)))
            {
                var eqidx = line.IndexOf('=');
                switch (line.Substring(0, eqidx).Trim())
                {
                    case "choose_weight":
                        res.ChooseWeight = float.Parse(line.Substring(eqidx + 1));
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

            return res;
        }
    }
}
