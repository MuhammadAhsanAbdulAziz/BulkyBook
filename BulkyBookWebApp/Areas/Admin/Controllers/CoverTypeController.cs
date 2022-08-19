using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.CoverTypeRepositories;
using BulkyBook.DataAccess.Repository.UnitofWork;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BulkyBookWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitofWork _db;

        public CoverTypeController(IUnitofWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> ObjectCoverTypeList = _db.CoverType.GetAll();
            return View(ObjectCoverTypeList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType cat)
        {
            if (ModelState.IsValid)
            {
                _db.CoverType.Add(cat);
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
            var CoverTypefromdb = _db.CoverType.GetFirstorDefault(c => c.Id == id);
            return View(CoverTypefromdb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType cat)
        {
            if (ModelState.IsValid)
            {
                _db.CoverType.Update(cat);
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
            var CoverTypefromdb = _db.CoverType.GetFirstorDefault(c => c.Id == id);
            return View(CoverTypefromdb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var idfromdb = _db.CoverType.GetFirstorDefault(c => c.Id == id);
            if (idfromdb == null)
            {
                return NotFound();
            }
            _db.CoverType.Remove(idfromdb);
            _db.Save();
            return RedirectToAction("Index");
        }
    }
}
