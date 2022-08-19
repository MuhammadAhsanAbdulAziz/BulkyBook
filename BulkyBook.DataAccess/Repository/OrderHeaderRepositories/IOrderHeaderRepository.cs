using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.OrderHeaderRepositories
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader order);
        void UpdateStatus(int id,string OrderStatus,string? PaymentStatus=null);

    }
}
