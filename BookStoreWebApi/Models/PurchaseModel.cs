using System;

namespace BookStoreWebApi.Models
{
    public class PurchaseModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public int price { get; set; } 
        public DateTime purchaseDate { get; set; }
        public string userName { get; set; }
    }
}
