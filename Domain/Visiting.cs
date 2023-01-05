using System;

namespace Domain
{
    public class Visiting : BaseEntity
    {
        public DateTime Date { get; set; } = DateTime.Now;

        public long LessonId { get; set; }
        public Lesson? Lesson { get; set; }

        public long UserId { get; set; }
        public User? User { get; set; }
    }
}
