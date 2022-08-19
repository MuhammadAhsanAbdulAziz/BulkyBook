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
    public interface IUnitofWork
    {
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IOrderHeaderRepository orderHeader { get; }
        IOrderDetailsRepository OrderDetails { get; }
        void Save();
    }
}
