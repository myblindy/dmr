using dmr.Loaders;
using dmr.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var rooms = Directory.GetFiles(@"Content\Rooms", "*.txt", SearchOption.AllDirectories)
                .Select(path => RoomLoader.Load(path))
                .ToList();

            const int mapw = 75, maph = 25;
            var map = new Map(mapw, maph, rooms, new Random());

            for (var row = 0; row < maph; ++row)
            {
                Console.Write('/');
                for (var col = 0; col < mapw; ++col)
                {
                    var tile = map.Tiles[row, col];
                    if (tile == TileTemplate.DoorTile) Console.Write('d');
                    else if (tile == TileTemplate.WallTile) Console.Write('+');
                    else if (tile == TileTemplate.StartTile) Console.Write('s');
                    else if (tile == TileTemplate.EmptyTile) Console.Write('.');
                    else if (tile == null) Console.Write(' ');
                    else throw new InvalidOperationException();
                }
                Console.WriteLine('/');
            }

            Console.ReadKey();
        }
    }
}
