﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ReSharpersCooperation.Models.OrdersViewModel
{
    public class OrdersViewModel
    {
        [Required(ErrorMessage = "You have to provide a name!")]
        public string OrderName { get; set; }
        [Required(ErrorMessage = "You have to provide an address!")]
        public string Address { get; set; }
        [Required(ErrorMessage = "You have to provide a city!")]
        public string City { get; set; }
        [Required(ErrorMessage = "You have to provide a country!")]
        public string Country { get; set; }
        [Required(ErrorMessage = "You have to provide a zip code!")]
        public int Zip { get; set; }
        [CreditCard(ErrorMessage ="Invalid Credit Card Number!")]
        public string Creditcard { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal TotalCost { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Cvv { get; set; }
        public List<Product> OutOfStock { get; set; }
        public string CreditCardType { get; set; }

    }
}
