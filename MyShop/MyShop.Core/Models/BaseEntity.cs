using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    //13. Make it abstract so it cannot be called on their own, you can only create a class that implements it
    public abstract class BaseEntity
    {
        public string Id { get; set; }

        //Good practice, you can tell when it was created
        public DateTimeOffset CreatedAt { get; set; }

        //Add your construct
        public BaseEntity()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.Now;
        }

    }
}
