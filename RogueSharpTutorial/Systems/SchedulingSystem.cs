using System.Collections.Generic;
using System.Linq;
using SadConsoleGame.Interfaces;

namespace SadConsoleGame.Systems
{
    public class SchedulingSystem
    {
        private int _time;

        private readonly SortedDictionary<int, List<ISchedulable>> _schedulables;

        public SchedulingSystem()
        {
            _time = 0;
            _schedulables = new SortedDictionary<int, List<ISchedulable>>();
        }

        //add a new object to the schedule
        //place it at the current time plus the object's time property
        public void Add(ISchedulable schedulable)
        {
            int key = _time + schedulable.Time;
            if (!_schedulables.ContainsKey(key))
            {
                _schedulables.Add(key, new List<ISchedulable>());
            }

            _schedulables[key].Add(schedulable);

        }

        public void Remove(ISchedulable schedulable)
        {
            KeyValuePair<int, List<ISchedulable>> schedulableListFound =
                new KeyValuePair<int, List<ISchedulable>>(-1, null);

            foreach (var schedulablesList in _schedulables)
            {
                if (schedulablesList.Value.Contains(schedulable))
                {
                    schedulableListFound = schedulablesList;
                    break;
                }
            }
            if (schedulableListFound.Value != null)
            {
                schedulableListFound.Value.Remove(schedulable);
                if (schedulableListFound.Value.Count <= 0)
                {
                    _schedulables.Remove(schedulableListFound.Key);
                }
            }
        }
        
        //get the next object whose turn it is from the schedule
        //advance time is ncessary
        public ISchedulable Get()
        {
            var firstScheduleGroup = _schedulables.First();
            var firstSchedulable = firstScheduleGroup.Value.First();
            Remove(firstSchedulable);
            _time = firstScheduleGroup.Key;
            return firstSchedulable;
        }

        public int GetTime()
        {
            return _time;
        }

        public void Clear()
        {
            _time = 0;
            _schedulables.Clear();
        }
    }
}