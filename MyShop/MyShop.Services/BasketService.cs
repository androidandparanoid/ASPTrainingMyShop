using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService : IBasketService
    {
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;

        //use string to identify particular cookie you want
        public const string BasketSessionName = "eCommerceBasket";

        //Create a constructor that takes your repos ans hookes them internally

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext){
            this.basketContext = BasketContext;
            this.productContext = ProductContext;
        }

        //Write your methods private because they only be used within this service
            //Get Basket
        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null)
            {
                string basketId = cookie.Value;

                if (!string.IsNullOrEmpty(basketId))
                {
                    //if basketId is not null load basket from basket context
                    basket = basketContext.Find(basketId);
                }
                else
                {
                    //check if you want to create the basket
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }

            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;

        }

            //CreateNewBasket
        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            basketContext.Insert(basket);
            basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

            //AddToBasket
        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            //createIfNull must be true
            Basket basket = GetBasket(httpContext, true);
            //EntityFramework will load the basket items for us whenever we try to load the basket from the DB
            BasketItem item = basket.BasketItems.FirstOrDefault(i=>i.ProductId == productId);

            if (item==null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1

                };

                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;
            }

            basketContext.Commit();
        }

            //RemoveFromBasket
        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            //createIfNull must be true
            Basket basket = GetBasket(httpContext, true);
            //EntityFramework will load the basket items for us whenever we try to load the basket from the DB
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }            
            
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            //get our basket from the DB and if it doesnt exist create one and return an empty inmemory basket
            Basket basket = GetBasket(httpContext, false);

            //Create your linq join query
            if (basket != null)
            {
                var results = (from b in basket.BasketItems
                              join p in productContext.Collection() on b.ProductId equals p.Id
                              select new BasketItemViewModel()
                              {

                                  Id = b.Id,
                                  Quantity = b.Quantity,
                                  ProductName = p.Name,
                                  Image = p.Image,
                                  Price = p.Price

                              }).ToList();

                return results;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }

        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            //Call your class and use the empty declaration to pass default values
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);

            //Create your linq join query
            if (basket != null)
            {
                //calculations
                // int? / decimal? means that you can store a null value
                int? basketCount = (from item in basket.BasketItems
                                    select item.Quantity).Sum();

                decimal? basketTotal = (from item in basket.BasketItems
                                        join p in productContext.Collection() on item.ProductId equals p.Id
                                        select item.Quantity * p.Price).Sum();

                //Assign these values to model, you need to return 0's if empty basket

                model.BasketCount = basketCount ?? 0; // if basket count contains something return that, otherwise return 0
                model.BasketTotal = basketTotal ?? decimal.Zero;

                return model;

            }
            else
            {
                return model;
            }
        }
    }
}
