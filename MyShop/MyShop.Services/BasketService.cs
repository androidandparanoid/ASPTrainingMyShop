using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService
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
    }
}
