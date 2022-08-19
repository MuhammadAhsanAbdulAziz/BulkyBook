using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{
    public class ShoppinCartVM
    {
        public IEnumerable<ShoppingCart> listCart { get; set; }
        public OrderHeader orderHeader { get; set; }
    }
}
