﻿namespace UniversityProject.Model
{
    public class TimeSheet
    {
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan BreakTime { get; set; }
        public TimeSpan TotalHoursWorked { get => EndTime - StartTime - BreakTime; }

    }
}
