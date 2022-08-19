using BulkyBook.DataAccess.Repository.ApplicationuserRepositories;
using BulkyBook.DataAccess.Repository.CategoryRepositories;
using BulkyBook.DataAccess.Repository.CompanyRepositories;
using BulkyBook.DataAccess.Repository.CoverTypeRepositories;
using BulkyBook.DataAccess.Repository.OrderDetailsRepositories;
using BulkyBook.DataAccess.Repository.OrderHeaderRepositories;
using BulkyBook.DataAccess.Repository.ProductRepositories;
using BulkyBook.DataAccess.Repository.ShoppingcartRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.UnitofWork
{
    public class UnitofWork : IUnitofWork
    {
        public ICategoryRepository Category { get;private set; }

        public ICoverTypeRepository CoverType { get; private set; }

        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository orderHeader { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }

        private readonly ApplicationDbContext _db;

        public UnitofWork(ApplicationDbContext db) 
        {
            _db = db;
            Category = new CategoryRepository(_db);
            CoverType = new CoverTypeRepository(_db);
            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            orderHeader = new OrderHeaderRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
