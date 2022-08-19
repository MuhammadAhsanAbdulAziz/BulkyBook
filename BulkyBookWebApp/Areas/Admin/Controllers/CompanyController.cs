using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.CompanyRepositories;
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
    public class CompanyController : Controller
    {
        private readonly IUnitofWork _db;

        public CompanyController(IUnitofWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Company company = new();
            if (id == null || id == 0)
            {

                return View(company);
            }
            else
            {
                company = _db.Company.GetFirstorDefault(u=>u.Id == id);
                return View(company);

            }
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _db.Company.Add(obj);
                }
                else
                {
                    _db.Company.Update(obj);
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
            var Companylist = _db.Company.GetAll();
            return Json(new { data = Companylist });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var idfromdb = _db.Company.GetFirstorDefault(u => u.Id == id);
            if (idfromdb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Company.Remove(idfromdb);
            _db.Save();
            return Json(new { success = true, message = "Deleted Successfully" });
        }

        #endregion

    }
}
