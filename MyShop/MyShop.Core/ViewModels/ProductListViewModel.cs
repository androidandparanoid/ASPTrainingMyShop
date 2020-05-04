using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; } //3. Add a Product Property

        public IEnumerable<ProductCategory> ProductCategories { get; set; } //4. Create a IENumerable list to hold product category
    }
}
