using BulkyBook.DataAccess.Repository.UnitofWork;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace BulkyBookWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitofWork _db;
        public ShoppinCartVM shoppinCartVM { get; set; }

        public CartController(IUnitofWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppinCartVM = new ShoppinCartVM()
            {
                listCart = _db.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value, IncludeProperties: "product"),
                orderHeader = new()
            };
            foreach(var item in shoppinCartVM.listCart)
            {
                item.Price = GetPricedBasedOnQuantity(item.Count, item.product.Price, item.product.Price50, item.product.Price100);
                shoppinCartVM.orderHeader.OrderTotal += item.Price * item.Count;
            }
            return View(shoppinCartVM);
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppinCartVM = new ShoppinCartVM()
            {
                listCart = _db.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value, IncludeProperties: "product")
                ,orderHeader = new()
            };

            shoppinCartVM.orderHeader.applicationUser = _db.ApplicationUser.GetFirstorDefault(u => u.Id == claims.Value);
            shoppinCartVM.orderHeader.Name = shoppinCartVM.orderHeader.applicationUser.Name;
            shoppinCartVM.orderHeader.PhoneNumber = shoppinCartVM.orderHeader.applicationUser.PhoneNumber;
            shoppinCartVM.orderHeader.PostalCode = shoppinCartVM.orderHeader.applicationUser.PostalCode;
            shoppinCartVM.orderHeader.StreetAddress = shoppinCartVM.orderHeader.applicationUser.StreetAddress;
            shoppinCartVM.orderHeader.State = shoppinCartVM.orderHeader.applicationUser.State;
            shoppinCartVM.orderHeader.City = shoppinCartVM.orderHeader.applicationUser.City;

            foreach (var item in shoppinCartVM.listCart)
            {
                item.Price = GetPricedBasedOnQuantity(item.Count, item.product.Price, item.product.Price50, item.product.Price100);
                shoppinCartVM.orderHeader.OrderTotal += item.Price * item.Count;
            }
            return View(shoppinCartVM);
        }
        [ActionName("Summary")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST(ShoppinCartVM shoppinCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppinCartVM.listCart = _db.ShoppingCart.GetAll
                (u => u.ApplicationUserId == claims.Value, IncludeProperties: "product");
            shoppinCartVM.orderHeader.PaymentStatus = SD.PaymentStatusPending;
            shoppinCartVM.orderHeader.OrderStatus = SD.StatusPending;
            shoppinCartVM.orderHeader.OrderDate = System.DateTime.Now;
            shoppinCartVM.orderHeader.ApplicationUserId = claims.Value;


            foreach (var item in shoppinCartVM.listCart)
            {
                item.Price = GetPricedBasedOnQuantity(item.Count, item.product.Price, item.product.Price50, item.product.Price100);
                shoppinCartVM.orderHeader.OrderTotal += item.Price * item.Count;
            }

            _db.orderHeader.Add(shoppinCartVM.orderHeader);
            _db.Save();

            foreach (var item in shoppinCartVM.listCart)
            {
                OrderDetails orderDetail = new()
                {
                    ProductId = item.ProductId,
                    OrderId = shoppinCartVM.orderHeader.Id,
                    Price = item.Price,
                    Count = item.Count
                };
                _db.OrderDetails.Add(orderDetail);
                _db.Save();
            }


            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new SessionCreateOptions
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                 new SessionLineItemOptions
                 {
                     PriceData = new SessionLineItemPriceDataOptions
                     {
                         UnitAmount = 2000,
                         Currency = "usd",
                         ProductData = new SessionLineItemDataProductDataOptions
                         {
                             Name = "T-shirt",
                         },
                     },
                     Quantity = 1,
                 }
                },
                Mode = "payment",
                SuccessUrl = domain + "/success.html",
                CancelUrl = domain + "/cancel.html",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);



            _db.ShoppingCart.RemoveRange(shoppinCartVM.listCart);
            _db.Save();

            return RedirectToAction("Index","Home");
        }

        public IActionResult plus(int cartid)
        {
            var cart = _db.ShoppingCart.GetFirstorDefault(u => u.Id == cartid);
            _db.ShoppingCart.IncrementCount(cart,1);
            _db.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult minus(int cartid)
        {
            var cart = _db.ShoppingCart.GetFirstorDefault(u => u.Id == cartid);
            if (cart.Count <= 1)
            {
                _db.ShoppingCart.Remove(cart);
            }
            else
            {
                _db.ShoppingCart.DecrementCount(cart, 1);
            }
            _db.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult delete(int cartid)
        {
            var cart = _db.ShoppingCart.GetFirstorDefault(u => u.Id == cartid);
            _db.ShoppingCart.Remove(cart);
            _db.Save();
            return RedirectToAction(nameof(Index));
        }

        private double GetPricedBasedOnQuantity( double quantity, double price, double price50, double price100)
        {
            if(quantity <=50)
            {
                return price;
            }
            else
            {
                if(quantity<=100)
                {
                    return price50;
                }
                else
                {
                    return price100;
                }
            }
        }
    }
}
