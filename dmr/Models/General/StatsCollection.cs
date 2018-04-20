using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.Models.General
{
    public class StatsCollection : ICollection<Stats>, IList<Stats>
    {
        private List<Stats> StatsList = new List<Stats>();
        private Stats AggregateStats;

        private void RefreshAggregateStats() =>
            AggregateStats = Stats.Aggregate(StatsList);

        public int Count => StatsList.Count;

        public bool IsReadOnly => false;

        public Stats this[int index]
        {
            get => StatsList[index];
            set
            {
                StatsList[index] = value;
                RefreshAggregateStats();
            }
        }

        public void Add(Stats item)
        {
            StatsList.Add(item);
            RefreshAggregateStats();
        }

        public void Clear()
        {
            StatsList.Clear();
            RefreshAggregateStats();
        }

        public bool Contains(Stats item) => StatsList.Contains(item);

        public void CopyTo(Stats[] array, int arrayIndex) => StatsList.CopyTo(array, arrayIndex);

        public IEnumerator<Stats> GetEnumerator() => StatsList.GetEnumerator();

        public bool Remove(Stats item)
        {
            var res = StatsList.Remove(item);
            RefreshAggregateStats();
            return res;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(Stats item) => StatsList.IndexOf(item);

        public void Insert(int index, Stats item)
        {
            StatsList.Insert(index, item);
            RefreshAggregateStats();
        }

        public void RemoveAt(int index)
        {
            StatsList.RemoveAt(index);
            RefreshAggregateStats();
        }
    }
}
