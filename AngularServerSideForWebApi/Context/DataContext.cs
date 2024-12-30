using AngularServerSideForWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AngularServerSideForWebApi.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
