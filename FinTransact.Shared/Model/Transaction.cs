using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FinTransact.Shared.Model
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid AccountId { get; set; }
        public string Description { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
public enum TransactionStatus
{
    Pending,
    Completed,
    Failed
}
