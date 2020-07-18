using System;
using System.Collections.Generic;

namespace HabitTracker.Domain
{
    public class Habit
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string HabitDays { get; set; }
        public DateTime HabitTimeNotification { get; set; }

    }
}
