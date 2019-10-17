using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;
 
namespace WeddingPlanner.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<LoginUser> LoginUsers {get;set;}
        public DbSet<Wedding> Weddings {get;set;}
        public DbSet<Attendee> Attendees {get;set;}
    }
}