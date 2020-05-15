using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    //3. Create your class out of your baseentity
   public class Basket : BaseEntity
    {
        //4. make a virtual collection, important for entity framework, it will tell the program to load all items from the basket "lazy loading"

        public virtual ICollection<BasketItem> BasketItems { get; set; }

        public Basket()
        {
            this.BasketItems = new List<BasketItem>();
        }

    }
}
