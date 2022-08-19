using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.ProductRepositories;
using BulkyBook.DataAccess.Repository.UnitofWork;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;

namespace BulkyBookWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitofWork _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitofWork db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _db.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _db.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };
            if (id == null || id == 0)
            {
                //Create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                //Update Product
                productVM.Product = _db.Product.GetFirstorDefault(u=>u.Id == id);
                return View(productVM);

            }
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file!= null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath,@"Images\Products");
                    var extension = Path.GetExtension(file.FileName);

                    if(obj.Product.ImageUrl!=null)
                    {
                        var oldimage = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldimage))
                        {
                            System.IO.File.Delete(oldimage);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\Images\Products\" + filename + extension;
                }
                if (obj.Product.Id == 0)
                {
                    _db.Product.Add(obj.Product);
                }
                else
                {
                    _db.Product.Update(obj.Product);
                }
                _db.Save();
                return RedirectToAction("Index");
            }
            return View(obj);


        }
        #region APICALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var productlist = _db.Product.GetAll(IncludeProperties:"Category");
            return Json(new { data = productlist });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var idfromdb = _db.Product.GetFirstorDefault(u => u.Id == id);
            if (idfromdb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            var oldimage = Path.Combine(_webHostEnvironment.WebRootPath, idfromdb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldimage))
            {
                System.IO.File.Delete(oldimage);
            }
            _db.Product.Remove(idfromdb);
            _db.Save();
            return Json(new { success = true, message = "Deleted Successfully" });
        }

        #endregion

    }
}
