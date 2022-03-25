using System;
using System.Collections.Generic;

namespace Czytnik.Models
{
    public class User
    {


        public int Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        //public virtual ICollection<Room> Rooms{ get; set; }

        public ICollection<UserRooms>? UserRooms { get; set; }

    }

}

