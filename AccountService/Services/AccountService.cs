using AccountService.Models;
using FinTransact.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace AccountService.Services
{
    public class AccountService
    {
        private readonly AccountDbContext _context;
        private readonly RabbitMQPublisher _rabbitMqPublisher;

        public AccountService(AccountDbContext context, RabbitMQPublisher rabbitMqPublisher)
        {
            _context = context;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        public async Task<IEnumerable<BankAccount>> GetAllAccounts()
        {
            return await _context.BankAccounts.ToListAsync();
        }

        public async Task<BankAccount> GetAccountById(int id)
        {
            return await _context.BankAccounts.FindAsync(id);
        }

        public async Task<BankAccount> CreateAccount(BankAccount account)
        {
            _context.BankAccounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<BankAccount> UpdateAccount(BankAccount account)
        {
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task DeleteAccount(int id)
        {
            var account = await _context.BankAccounts.FindAsync(id);
            if (account != null)
            {
                _context.BankAccounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public void ProcessTransaction(Transaction transaction)
        {
            _rabbitMqPublisher.PublishTransaction(transaction);
        }
    }
}
