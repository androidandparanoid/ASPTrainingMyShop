using MyShop.Core.Models; //2. Add your Core Models
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ViewModels
{
    public class ProductManagerViewModel
    {
        public Product Product { get; set; } //3. Add a Product Property
        
        public IEnumerable<ProductCategory> ProductCategories { get; set; } //4. Create a IENumerable list to hold product category
    }
}
