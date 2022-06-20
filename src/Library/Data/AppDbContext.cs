using Library.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<ArticleEntity> Articles { get; set; }
        public DbSet<LaunchEntity> Launches { get; set; }
        public DbSet<EventEntity> Events { get; set; }
    }
}
