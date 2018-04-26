using dmr.Models.Items;
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
        private bool SlotsAllocated;

        public StatsCollection(bool allocateslots)
        {
            if (SlotsAllocated = allocateslots)
                for (int idx = 0; idx < (int)ItemSlot.MaxItemSlot; ++idx)
                    StatsList.Add(new Stats());
        }

        private void RefreshAggregateStats() =>
            AggregateStats = Stats.Aggregate(StatsList);

        public int Count => StatsList.Count;

        public bool IsReadOnly => false;

        public Stats this[int index]
        {
            get => StatsList[SlotsAllocated ? index + (int)ItemSlot.MaxItemSlot : index];
            set
            {
                StatsList[SlotsAllocated ? index + (int)ItemSlot.MaxItemSlot : index] = value;
                RefreshAggregateStats();
            }
        }

        public Stats this[ItemSlot slot]
        {
            get => StatsList[(int)slot];
            set
            {
                StatsList[(int)slot] = value;
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

        public void CopyTo(Stats[] array, int arrayIndex) => throw new NotImplementedException();

        public IEnumerator<Stats> GetEnumerator()
        {
            foreach (var item in SlotsAllocated ? StatsList.Skip((int)ItemSlot.MaxItemSlot - 1) : StatsList)
                yield return item;
        }

        public bool Remove(Stats item)
        {
            var res = StatsList.Remove(item);
            RefreshAggregateStats();
            return res;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(Stats item) => StatsList.IndexOf(item) - (SlotsAllocated ? (int)ItemSlot.MaxItemSlot : 0);

        public void Insert(int index, Stats item)
        {
            StatsList.Insert(index + (SlotsAllocated ? (int)ItemSlot.MaxItemSlot : 0), item);
            RefreshAggregateStats();
        }

        public void RemoveAt(int index)
        {
            StatsList.RemoveAt(index + (SlotsAllocated ? (int)ItemSlot.MaxItemSlot : 0));
            RefreshAggregateStats();
        }
    }
}
