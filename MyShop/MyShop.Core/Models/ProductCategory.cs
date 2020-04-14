using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class ProductCategory : BaseEntity
    {
        //3. Add Class Properties
        //public string Id { get; set; }

        [StringLength(20)]
        [DisplayName("Category Name")]
        public string Category { get; set; }

        public string Description { get; set; }

        //4. Add your constructor

        /*public ProductCategory()
        {
            this.Id = Guid.NewGuid().ToString();//2. This constructor will create a guid automatically, this will provide control
        }*/


    }
}
