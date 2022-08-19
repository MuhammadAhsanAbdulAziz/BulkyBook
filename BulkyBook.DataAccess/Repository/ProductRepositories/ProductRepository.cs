using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.ProductRepositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(Product product)
        {
            var objfromdb = _db.Products.FirstOrDefault(u => u.Id == product.Id);
            if (objfromdb != null)
            {
                objfromdb.Title = product.Title;
                objfromdb.ISBN = product.ISBN;
                objfromdb.Price = product.Price;
                objfromdb.Price50 = product.Price50;
                objfromdb.ListPrice = product.ListPrice;
                objfromdb.Price100 = product.Price100;
                objfromdb.Description = product.Description;
                objfromdb.CategoryId = product.CategoryId;
                objfromdb.CoverTypeId = product.CoverTypeId;
                objfromdb.Author = product.Author;

                if(objfromdb.ImageUrl != null)
                {
                    objfromdb.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}
