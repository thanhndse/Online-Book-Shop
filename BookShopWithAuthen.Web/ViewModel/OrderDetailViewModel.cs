﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShopWithAuthen.Web.ViewModel
{
    public class OrderDetailViewModel
    {
        public int BookID { get; set; }
        public string BookName { get; set; }
        public string Image { get; set; }
        public int OrderID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get { return Price * Quantity; } }
    }
}