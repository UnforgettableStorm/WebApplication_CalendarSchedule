using System;

namespace WebApplication_CalendarSchedule.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsFullDay { get; set; }
        public EventType EventType { get; set; }
    }
}
