using Microsoft.EntityFrameworkCore;
using Shop.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data.Entities
{
    public class AppDataContext:DbContext
    {
        public AppDataContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
    }
}
