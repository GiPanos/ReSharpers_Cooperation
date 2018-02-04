using System;
using System.ComponentModel.DataAnnotations;

namespace ReSharpersCooperation.Data
{
    public class Transaction
    {
        [Key]
        public int TransactionNo { get; set; }
        public string SourceAccount { get; set; }
        public string DestinationAccount { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public DateTime Date { get; set; }
        public int? OrderId { get; set; }
        public int? ProductNo { get; set; }


        public Transaction()
        {

        }
    }
   
}