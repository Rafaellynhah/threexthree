using threexthree.Models;
using Microsoft.EntityFrameworkCore;

namespace threexthree.Data
{
    public class DataContext : DbContext
    {
        
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Championship> Championships { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<TypeKey> TypeKeys { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<Game> Games { get; set; }
    }
}

