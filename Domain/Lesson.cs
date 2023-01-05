using System;

namespace Domain
{
    public class Lesson : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsTaken { get; set; } = false;

        public User? User { get; set; }
    }
}
