using System;
using System.Collections.Generic;

namespace Domain
{
    public class Lesson : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
