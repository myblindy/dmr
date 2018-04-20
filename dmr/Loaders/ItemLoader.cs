using dmr.Models.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Loaders
{
    public static class ItemLoader
    {
        public static List<(byte rank, ItemTemplate itemtemplate)> Load(string path)
        {
            using (var stream = File.OpenRead(path))
                return Load(stream);
        }

        public static List<(byte rank, ItemTemplate itemtemplate)> Load(Stream stream)
        {
            var res = new List<(byte rank, ItemTemplate itemtemplate)>();
            var item = new ItemTemplate();
            int ranks = 1;

            string line;
            using (var streamreader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
                for (var rank = 1; rank <= ranks; ++rank)
                {
                    streamreader.BaseStream.Seek(0, SeekOrigin.Begin);

                    while ((line = streamreader.ReadLine()) != null)
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var eqidx = line.IndexOf('=');
                            var valarr = line.Substring(eqidx + 1).Split('|');
                            var val = valarr[Math.Max(valarr.Length - 1, rank)];
                            var keyname = line.Substring(0, eqidx).Trim();

                            if (keyname.StartsWith("stats."))
                                item.Stats.UpdateFromConfigurationPair(keyname.Substring(6), val);
                            else
                                switch (keyname)
                                {
                                    case "ranks":
                                        ranks = int.Parse(val);
                                        break;

                                    case "name":
                                        item.Name = val.Trim();
                                        break;

                                    case "item_type":
                                        item.Type = val.Trim();
                                        break;

                                    case "item_level":
                                        item.Level = byte.Parse(val);
                                        break;

                                    default:
                                        throw new InvalidOperationException();
                                }
                        }

                    res.Add(((byte)rank, item));
                }

            return res;
        }
    }
}
