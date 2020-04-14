using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class DataContext : DbContext
    {
        //5. Create a constructor to use the connection string
            //DefaultConnection will force webconfig checkup
        public DataContext()
            : base("DefaultConnection")
        {

        }

        //6. Create a DB Set and pass on your models
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}
