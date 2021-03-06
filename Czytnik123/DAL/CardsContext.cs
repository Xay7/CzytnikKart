using Czytnik.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Czytnik123.DAL
{
    public class CardsContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }


        public CardsContext(DbContextOptions<CardsContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>().HasData(
              new Card { Id = 1, CardSerialNumber = "4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22 4b b9 ca 22" }
              );






            //ICollection<Room> list1 = new List<Room>();
            //list1.Add(room1);

            //List<Room> list2 = new List<Room>();
            //list2.Add(room2);

            //List<Room> list3 = new List<Room>();
            //list3.Add(room1);
            //list3.Add(room2);

            modelBuilder.Entity<Room>().HasData(
              new Room { Id = 1, Name = "123", Card = 1 }
              );


            modelBuilder.Entity<User>().HasData(
               new User { Id = 1, Name = "Jan", Surname = "Kowalski", },
               new User { Id = 2, Name = "Kowalski", Surname = "Jan" },
               new User { Id = 3, Name = "Oliwia", Surname = "Nowak", },
               new User { Id = 4, Name = "Janusz", Surname = "Kartka" },
               new User { Id = 5, Name = "Oliwier", Surname = "Zeszyt" }
               );



            modelBuilder.Entity<UserRooms>()
           .HasKey(bc => new { bc.UserId, bc.RoomId });
            modelBuilder.Entity<UserRooms>()
                .HasOne(bc => bc.Room)
                .WithMany(b => b.UserRooms)
                .HasForeignKey(bc => bc.RoomId);
            modelBuilder.Entity<UserRooms>()
                .HasOne(bc => bc.User)
                .WithMany(c => c.UserRooms)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<UserRooms>().HasData(
               new UserRooms { UserId = 1, RoomId = 1 },
               new UserRooms { UserId = 1, RoomId = 2 },
               new UserRooms { UserId = 2, RoomId = 2 },
               new UserRooms { UserId = 2, RoomId = 3 },
               new UserRooms { UserId = 3, RoomId = 1 },
               new UserRooms { UserId = 3, RoomId = 2 },
               new UserRooms { UserId = 3, RoomId = 3 },
               new UserRooms { UserId = 3, RoomId = 4 },
               new UserRooms { UserId = 2, RoomId = 4 }
               );
            //modelBuilder.Entity<User>()
            //  .HasMany<Room>(s => s.Rooms)
            //  .WithMany(c => c.Users)
            //  .Map(cs =>
            //  {
            //      cs.MapLeftKey("StudentRefId");
            //      cs.MapRightKey("CourseRefId");
            //      cs.ToTable("StudentCourse");
            //  });

            //modelBuilder.Entity<UserRooms>().HasData(
            //   new UserRooms { UserId = 1, RoomId = 1 },
            //   new UserRooms { UserId = 1, RoomId = 2 },
            //   new UserRooms { UserId = 2, RoomId = 1 });




        }
    }
}
