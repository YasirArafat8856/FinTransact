using FinTransact.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace TransactionService.Services
{
    public class TransactionService
    {
        private readonly TransactionDBContext _context;

        public TransactionService(TransactionDBContext context)
        {
            _context = context;
        }
    }
}
