using dmr.Utilities;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Models
{
    public class Map
    {
        public TileTemplate[,] Tiles;
        public int StartX, StartY;

        public Map(int w, int h, List<RoomTemplate> rooms, Random random)
        {
            Tiles = new TileTemplate[h, w];

            var bystart = rooms.GroupBy(r => r.Tiles.AsEnumerable().Any(tile => tile?.Start == true))
                .ToDictionary(ww => ww.Key, ww => (IEnumerable<RoomTemplate>)ww);

            var doors = new List<(Doorway doorway, int xoffset, int yoffset)>();

            bool place(RoomTemplate room, int _posx, int _posy)
            {
                if (room.CanPlace(this, _posx, _posy))
                {
                    room.Place(this, _posx, _posy);
                    doors.AddRange(room.Doorways.Select(d => (d, _posx, _posy)));
                    return true;
                }
                return false;
            }

            const int retries = 25;

            // place the start
            var startroom = bystart[true].ChooseRandom(random);
            place(startroom, w / 2 - startroom.Tiles.GetLength(1) / 2, h / 2 - startroom.Tiles.GetLength(0) / 2);

            // loop over the doorways queued and fill them if possible
            while (doors.Count > 0)
            {
                var (doorway, xoffset, yoffset) = doors.ChooseRandom(random, out var chosenindex);
                doors.RemoveAt(chosenindex);

                // possible rooms that match this doorway
                var matchingrooms = bystart[false].SelectMany(r => r.Doorways
                    .Where(d => d.Direction.Opposite() == doorway.Direction)
                    .Select(d => (room: r, door: d))).ToList();
                if (matchingrooms.Count == 0)
                    continue;

                // keep trying until we give up, then ignore the doorway
                for (int retry = 0; retry < retries; ++retry)
                {
                    // choose a random matching room and doorway
                    var (chosenroom, chosendoor) = matchingrooms.ChooseRandom(item => item.room.ChooseWeight, random);

                    // and place it at the calculated position
                    if (place(chosenroom,
                        doorway.X + xoffset +
                            (doorway.Direction == DoorwayDirection.West ? -chosendoor.X - 1
                            : doorway.Direction == DoorwayDirection.East ? chosendoor.X + 1
                            : -chosendoor.X),
                        doorway.Y + yoffset +
                            (doorway.Direction == DoorwayDirection.North ? -chosendoor.Y - 1
                            : doorway.Direction == DoorwayDirection.South ? chosendoor.Y + 1
                            : -chosendoor.Y)))
                    {
                        break;
                    }
                }
            }
        }

        public Map(int w, int h) => Tiles = new TileTemplate[h, w];
    }
}
