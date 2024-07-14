using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTransact.Shared.Model
{
    public class Account
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
    }
}
