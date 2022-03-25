using System;
using System.Collections.Generic;

namespace Czytnik.Models
{
    public class UserRooms
    {

        public int UserId { get; set; }

        public int RoomId { get; set; }

        public User User { get; set; }

        public Room Room { get; set; }


    }

}

