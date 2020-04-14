using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    //3. Make this a generic class <YouCanPutWhatever>
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        //4. Create your object cache
        ObjectCache cache = MemoryCache.Default;
        //5. Create a generic list referencing our placeholder
        List<T> items;
        //6. create a handle to store our objects in the cache
        string className;
        //7. Create a constructor to initialize our repository
        public InMemoryRepository()
        {
            //8. Pass on the name of our class, eg. Product / ProductCategory
            className = typeof(T).Name;
            //9. Initialize our internal ItemsClass
            items = cache[className] as List<T>;

            if (items == null)
            {
                items = new List<T>();
            }
        }

        //10. Create a generic commit function
        public void Commit()
        {
            cache[className] = items;
        }

        //11. Create your CRUD Methods
        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);

            if (tToUpdate != null)
            {
                tToUpdate = t;
            }
            else
            {
                throw new Exception($"{className} Not found");
            }
        }

        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);

            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception($"{className} Not found");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {
            T tToDelete = items.Find(i => i.Id == Id);

            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }
            else
            {
                throw new Exception($"{className} Not found");
            }
        }

    }
}
