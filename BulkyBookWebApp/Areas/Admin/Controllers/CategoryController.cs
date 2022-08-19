using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.CategoryRepositories;
using BulkyBook.DataAccess.Repository.UnitofWork;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BulkyBookWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitofWork _db;

        public CategoryController(IUnitofWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> ObjectCategoryList = _db.Category.GetAll();
            return View(ObjectCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category cat)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(cat);
                _db.Save();
                return RedirectToAction("Index");
            }
            return View(cat);


        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryfromdb = _db.Category.GetFirstorDefault(c => c.Id == id);
            return View(categoryfromdb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category cat)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(cat);
                _db.Save();
                return RedirectToAction("Index");
            }
            return View(cat);


        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryfromdb = _db.Category.GetFirstorDefault(c => c.Id == id);
            return View(categoryfromdb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var idfromdb = _db.Category.GetFirstorDefault(c => c.Id == id);
            if (idfromdb == null)
            {
                return NotFound();
            }
            _db.Category.Remove(idfromdb);
            _db.Save();
            return RedirectToAction("Index");
        }
    }
}
