using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasMasr.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<MainVideo> MainVideos { get; set; }
        public DbSet<News> News { get; set; }

        public DbSet<Contest> Contests { get; set; }

        public DbSet<Sponser> Sponsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
