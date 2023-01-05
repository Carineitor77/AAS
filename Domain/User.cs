using System.Collections.Generic;

namespace Domain
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

        public long RoleId { get; set; }
        public Role? Role { get; set; }

        public ICollection<Lesson>? Lessons { get; set; }

        public ICollection<Visiting>? Visitings { get; set; }
    }
}
