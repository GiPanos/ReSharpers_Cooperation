using ReSharpersCooperation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReSharpersCooperation.Models
{
    public class TransactionRepository
    {
        private ApplicationDbContext db;
        public TransactionRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IQueryable<Transaction> Transactions => db.Transaction;

        public void RegisterTransaction(string source,string destination,decimal amount,string type, int? productNo=null ,int? orderid = null)
        {
            var transaction = new Transaction();
            transaction.Amount = amount;
            transaction.Date = DateTime.Now;
            transaction.DestinationAccount = destination;
            
            transaction.TransactionType = type;
            transaction.SourceAccount = source;
            
            if (orderid != null)
            {
                transaction.OrderId = orderid;
            }
            if (productNo != null)
            {
                transaction.ProductNo = productNo;
            }
            db.Transaction.Add(transaction);
            db.SaveChanges();
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return db.Transaction.ToList();
        }

        public IEnumerable<Transaction> GetUsersTransactions(string username)
        {
            return db.Transaction.Where(t => t.SourceAccount==username || t.DestinationAccount == username);
        }

    }
}
