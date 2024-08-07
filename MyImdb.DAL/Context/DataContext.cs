using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer;
using MyImdb.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyImdb.DAL.Context
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DbConStr"));
            }
        }

        public DbSet<Actor> Actor { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Movie_Tag> Movie_Tag { get; set; }
        public DbSet<Movie_Actor> Movie_Actor { get; set; }
        public DbSet<Movie_Comment> Movie_Comment { get; set; }
    }
}
