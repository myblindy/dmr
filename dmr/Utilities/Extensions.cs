using dmr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Utilities
{
    public static class Extensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this T[,] source)
        {
            var imax = source.GetUpperBound(0);
            var jmin = source.GetLowerBound(1);
            var jmax = source.GetUpperBound(1);

            for (int i = source.GetLowerBound(0); i < imax; ++i)
                for (int j = jmin; j < jmax; ++j)
                    yield return source[i, j];
        }

        public static DoorwayDirection Opposite(this DoorwayDirection dir)
        {
            switch (dir)
            {
                case DoorwayDirection.East: return DoorwayDirection.West;
                case DoorwayDirection.West: return DoorwayDirection.East;
                case DoorwayDirection.North: return DoorwayDirection.South;
                case DoorwayDirection.South: return DoorwayDirection.North;
                default: throw new InvalidOperationException();
            }
        }

        public static T ChooseRandom<T>(this IList<T> source, Random random) =>
            source[random.Next(source.Count)];

        public static T ChooseRandom<T>(this IList<T> source, Random random, out int index) =>
            source[index = random.Next(source.Count)];

        public static T ChooseRandom<T>(this IEnumerable<T> source, Random random) =>
            source.ToList().ChooseRandom(random);
    }
}
