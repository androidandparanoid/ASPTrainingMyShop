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
    public class ProductCategoryRepository
    {
        //2. Add your Object Cache
        ObjectCache cache = MemoryCache.Default;

        List<ProductCategory> productCategories;

        //3. Validate and initialize your cache
        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>; //if there's not a cache it will initialize cache for Products

            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }

        //4. Create a new method to commit your CRUD operations, we want to explicitly save a product

        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }

        //5. Create CRUD Operations
        //5.1 Create
        public void Insert(ProductCategory p)
        {
            productCategories.Add(p);
        }

        //5.2 Update

        public void Update(ProductCategory productCategory)
        {
            ProductCategory productCategoryToUpdate = productCategories.Find(p => p.Id == productCategory.Id);

            if(productCategoryToUpdate != null)
            {
                productCategoryToUpdate = productCategory;
            }
            else
            {
                throw new Exception("Product Category not found");
            }
        }

        //5.3 Find
        public ProductCategory Find(string Id)
        {
            ProductCategory productCategory = productCategories.Find(p => p.Id == Id); // find the product by ID

            if (productCategory != null)
            {
                return productCategory; //if product is found, it will be return
            }
            else
            {
                throw new Exception("Product Category not Found");
            }

        }

        //5.4 Return a list of products to query

        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        //5.5 Delete command
        public void Delete(string Id)
        {

            ProductCategory productCategoryToDelete = productCategories.Find(p => p.Id == Id); // find the product that you want to update

            if (productCategoryToDelete != null)
            {
                productCategories.Remove(productCategoryToDelete); //if a product is retrieve it will be updated by the product to Update
            }
            else
            {
                throw new Exception("Product Category not Found");
            }

        }

    }
}
