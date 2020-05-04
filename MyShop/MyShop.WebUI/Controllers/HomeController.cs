using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> context;      

        IRepository<ProductCategory> productCategories;
        
        //Constructor initializes our repository
        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            productCategories = productCategoryContext;

        }

        //string category = null will allow to pass on a category or a null value
        public ActionResult Index( string Category = null)
        {
            //Before category param:
            //List<Product> products = context.Collection().ToList();
            //return View(products);
            List<Product> products;
            List<ProductCategory> categories = productCategories.Collection().ToList();
            
            if(Category == null)
            {
                //if category is null return all
                products = context.Collection().ToList();
            }
            else
            {
                //if category is not null return a filtered list, since it is an Iqueryable you can filter
                products = context.Collection().Where(p => p.Category == Category).ToList();
                //This will convert to a sql query string that filters the information out

            }

            //7. Add your viewmodel

            ProductListViewModel model = new ProductListViewModel();

            model.Products = products;
            model.ProductCategories = categories;

            return View(model);
        }

        //3. Add your Details page
        //4. Add your Details view
        public ActionResult Details(string Id)
        {
            Product product = context.Find(Id);

            if(product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}