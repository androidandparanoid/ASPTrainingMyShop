using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching; //1. Add References to caching
using MyShop.Core; //1. Add References to Product Class
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        //2. Add your Object Cache
        ObjectCache cache = MemoryCache.Default;

        List<Product> products;

        //3. Validate and initialize your cache
        public ProductRepository()
        {
            products = cache["products"] as List<Product>; //if there's not a cache it will initialize cache for Products

            if (products == null)
            {
                products = new List<Product>();
            }
        }

        //4. Create a new method to commit your CRUD operations, we want to explicitly save a product

        public void Commit()
        {
            cache["products"] = products;
        }

        //5. Create CRUD Operations
        //5.1 Create
        public void Insert(Product p)
        {
            products.Add(p);
        }

        //5.2 Update
        public void Update(Product product)
        {
            Product productToUpdate = products.Find(p => p.Id == product.Id); // find the product that you want to update

            if (productToUpdate != null)
            {
                productToUpdate = product; //if a product is retrieve it will be updated by the product to Update
            }
            else
            {
                throw new Exception("Product not Found");
            }

        }

        //5.3 Find
        public Product Find(string Id)
        {
            Product product = products.Find(p => p.Id == Id); // find the product by ID

            if (product != null)
            {
                return product; //if product is found, it will be return
            }
            else
            {
                throw new Exception("Product not Found");
            }

        }

        //5.4 Return a list of products to query

        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }

        //5.5 Delete command
        public void Delete(string Id)
        {

            Product productToDelete = products.Find(p => p.Id == Id); // find the product that you want to update

            if (productToDelete != null)
            {
               products.Remove(productToDelete); //if a product is retrieve it will be updated by the product to Update
            }
            else
            {
                throw new Exception("Product not Found");
            }

        }



    }
}
