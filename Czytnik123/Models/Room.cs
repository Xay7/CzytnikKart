using System;
using System.Collections.Generic;

namespace Czytnik.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //public virtual ICollection<User> Users { get; set; }

        //public ICollection<User> Users { get; set; }
        public ICollection<UserRooms>? UserRooms { get; set; }

    }

}

