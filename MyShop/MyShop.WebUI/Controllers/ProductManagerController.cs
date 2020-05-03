using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {

        //4. Create an instance of the product repository and initialize it

        IRepository<Product> context;
        //before Interface: InMemoryRepository<Product> context;
        //Before Generic: ProductRepository context;

        // 6. Load your productCategoryRepo

        IRepository<ProductCategory> productCategories;
        //before Interface:InMemoryRepository<ProductCategory> productCategories;
        //Before Generic: ProductCategoryRepository productCategories;

        //Constructor initializes our repository
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            productCategories = productCategoryContext;
            
        }

        //before interfac: public ProductManagerController()
        //{

        //    context = new InMemoryRepository<Product>();
        //    //Before Generic: context = new ProductRepository();

        //    //Initialize your product categories repo

        //    //Before:productCategories = new ProductCategoryRepository();
        //    productCategories = new InMemoryRepository<ProductCategory>();
        //}

        // GET: ProductManager
        public ActionResult Index()
        {
            //5. on Your index return a list of the products that you get out of a collection

            List<Product> products = context.Collection().ToList();

            //7. Add View

            return View(products);
        }

        //8. Add Create Action

            //The first will display the Product

        public ActionResult Create()
        {

            //8. Make a reference to the product manager view model
            ProductManagerViewModel viewModel = new ProductManagerViewModel();

            //Before View Model: Product product = new Product();
            viewModel.Product = new Product();
            //Add your product Categories from the DB
            viewModel.ProductCategories = productCategories.Collection();

            //return a view model instead of the page
            return View(viewModel);
        }

            //the second will Create the product

        [HttpPost] //7.a add your file as en entry in create
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                //if its valid return it
                return View(product);
            }
            else
            {
                //Add your create file action
                if (file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }
                //else CREATE the product
                context.Insert(product);
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
            Product product = context.Find(Id);

            //if product does not exist
            if(product == null)
            {
                //Will return HTTP Not found
                return HttpNotFound();
            }
            else
            {
                //9. Update your Edit with View Model
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = productCategories.Collection();
                //If found it will display the product
                return View(viewModel);
            }
        }

            //The Second will Edit the product out, you need to pass a product object
        [HttpPost]
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
        {
            //Load the product from the DB
            Product productToEdit = context.Find(Id);

            //If the product to edit is not found it will return error
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
               
                //if found product will be checked for validity
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                //Add your create file action
                if (file != null)
                {
                    productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                }

                //Edit product properties
                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                //productToEdit.Image = product.Image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

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
            Product productToDelete = context.Find(Id);

            //If the product to edit is not found it will return error
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }
            //The Second will confirm the deletion
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);

            //If the product to edit is not found it will return error
            if (productToDelete == null)
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