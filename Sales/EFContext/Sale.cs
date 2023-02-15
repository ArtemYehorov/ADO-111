using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.EFContext
{
    public class Sale
    {
        public Guid Id { get; set; }
        public Guid IdManager { get; set; }
        public Guid IdProduct { get; set; }
        public int Count { get; set; }
        public DateTime Moment {get; set;}

        public String ToShortString()
        {
            return $"{Id.ToString()[..4]} ... {IdManager.ToString()[..4]} {IdProduct.ToString()[..4]} ({Count} шт) {Moment}";
            //return $"{Id.ToString()[..4]} ... {Name} ({Count} шт.)";
        }
    }
}
