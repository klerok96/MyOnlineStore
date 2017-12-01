using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class BasketModel
    {
        public int BasketId { get; set; }
        public int Quantity { get; set; }
        public string BasketName { get; set; }
        public decimal Price { get; set; }
    }
}