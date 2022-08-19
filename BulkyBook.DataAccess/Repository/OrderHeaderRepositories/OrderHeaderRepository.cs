﻿using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.OrderHeaderRepositories
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(OrderHeader order)
        {
            _db.OrderHeaders.Update(order);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderfromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if(orderfromDb!=null)
            {
                orderfromDb.OrderStatus = orderStatus;
                if(paymentStatus!=null)
                {
                    orderfromDb.PaymentStatus = paymentStatus;
                }
            }
        }
    }
}