using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        //6. Create an instance of the productcategory repository and initialize it

        ProductCategoryRepository context;

        //Constructor initializes our repository
        public ProductCategoryManagerController()
        {
            context = new ProductCategoryRepository();
        }

        // GET: ProductCategoryManager
        public ActionResult Index()
        {
            //5. on Your index return a list of the products that you get out of a collection

            List<ProductCategory> productCategories = context.Collection().ToList();

            //7. Add View

            return View(productCategories);
        }

        //8. Add Create Action

        //The first will display the Product

        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();

            return View(productCategory);
        }

        //the second will Create the product

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                //if its valid return it
                return View(productCategory);
            }
            else
            {
                //else CREATE the product
                context.Insert(productCategory);
                //Call the Commit Method
                context.Commit();
                //Once Created redirect the user back to index.
                return RedirectToAction("Index");

                //9. Add your create View

            }
        }

        //10. Add your Edit Code

        //The first will display the Product
        public ActionResult Edit(string Id)
        {
            //call off the product by using the find method
            ProductCategory productCategory = context.Find(Id);

            //if product does not exist
            if (productCategory == null)
            {
                //Will return HTTP Not found
                return HttpNotFound();
            }
            else
            {
                //If found it will display the product
                return View(productCategory);
            }
        }

        //The Second will Edit the product out, you need to pass a product object
        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string Id)
        {
            //Load the product from the DB
            ProductCategory productCategoryToEdit = context.Find(Id);

            //If the product to edit is not found it will return error
            if (productCategoryToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                //if found product will be checked for validity
                if (!ModelState.IsValid)
                {
                    return View(productCategory);
                }

                //Edit product properties
                productCategoryToEdit.Category = productCategory.Category;
                productCategoryToEdit.Description = productCategory.Description;           
                
                //Commit Changes

                context.Commit();

                //Once Edited redirect the user back to index.
                return RedirectToAction("Index");

                //11. Add your Edit View


            }
        }

        //12. Create DELETE Method
        //The first will display the Product      
        public ActionResult Delete(string Id)
        {
            ProductCategory productCategoryToDelete = context.Find(Id);

            //If the product to edit is not found it will return error
            if (productCategoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategoryToDelete);
            }
        }
        //The Second will confirm the deletion
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCategory productCategoryToDelete = context.Find(Id);

            //If the product to edit is not found it will return error
            if (productCategoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);

                //Commit Changes

                context.Commit();
                //Once deleted redirect the user back to index.
                return RedirectToAction("Index");

                //13. Create Delete view
            }
        }



    }
}