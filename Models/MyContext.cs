using Microsoft.EntityFrameworkCore;

namespace BeltExam.Models
{
    public class MyContext : DbContext
    {
        public MyContext (DbContextOptions options) : base (options){}

        public DbSet<User> Users {get; set;}
        public DbSet<Banana> Bananas{get; set;}
        public DbSet<ToDo> ToDos{get; set;}
    }
}