using BulkyBookWebApp.Data;
using BulkyBookWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BulkyBookWebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> ObjectCategoryList = _db.categories;
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
            if (ModelState.IsValid) {
                _db.categories.Add(cat);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cat);
             
            
        }

        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var categoryfromdb = _db.categories.FirstOrDefault(c => c.Id == id);
            return View(categoryfromdb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category cat)
        {
            if (ModelState.IsValid)
            {
                _db.categories.Update(cat);
                _db.SaveChanges();
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
            var categoryfromdb = _db.categories.FirstOrDefault(c => c.Id == id);
            return View(categoryfromdb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var idfromdb = _db.categories.Find(id);
            if(idfromdb == null)
            {
                return NotFound();
            }
            _db.categories.Remove(idfromdb);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
