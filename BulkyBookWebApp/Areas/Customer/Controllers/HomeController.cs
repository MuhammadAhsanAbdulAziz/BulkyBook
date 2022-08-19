using BulkyBook.DataAccess.Repository.UnitofWork;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitofWork _db;

        public HomeController(ILogger<HomeController> logger, IUnitofWork db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productlist = _db.Product.GetAll(IncludeProperties: "Category,CoverType");
            return View(productlist);
        }
        public IActionResult Details(int ProductId)
        {
            ShoppingCart cartobj = new()
            {
                Count = 1,
                ProductId = ProductId,
                product = _db.Product.GetFirstorDefault(u => u.Id == ProductId, IncludeProperties: "Category,CoverType")
            };
            return View(cartobj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claims.Value;
                ShoppingCart cartfromdb = _db.ShoppingCart.GetFirstorDefault(
                    u => u.ApplicationUserId == claims.Value && u.ProductId == shoppingCart.ProductId
                    );

                if (cartfromdb == null)
                {
                    _db.ShoppingCart.Add(shoppingCart);
                }
                else
                {
                    _db.ShoppingCart.IncrementCount(cartfromdb, shoppingCart.Count);
                }


                _db.Save();
                return RedirectToAction(nameof(Index));
            


            //return RedirectToAction("Index"); or you can do this as well :-
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}