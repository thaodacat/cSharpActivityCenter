using Microsoft.EntityFrameworkCore;
using activityCenter.Models;

namespace activityCenter.Contexts
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options){}

        public DbSet<User> Users { get; set; }

        public DbSet<Activityy> Activityys { get; set; }
        public DbSet<Attend> Attends { get; set; }


    }
}