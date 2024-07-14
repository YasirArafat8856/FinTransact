using AccountService.Models;
using FinTransact.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace AccountService.Services
{
    public class AccountService
    {
        private readonly AccountDbContext _context;
        private readonly RabbitMQPublisher _rabbitMqPublisher;
        private readonly RedisCacheService _redisCacheService;
        public AccountService(AccountDbContext context, RabbitMQPublisher rabbitMqPublisher, RedisCacheService redisCacheService)
        {
            _context = context;
            _rabbitMqPublisher = rabbitMqPublisher;
            _redisCacheService = redisCacheService;
        }

        public async Task<IEnumerable<BankAccount>> GetAllAccounts()
        {
            var cacheKey = "all_accounts";
            var cachedData = await _redisCacheService.GetCacheAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonConvert.DeserializeObject<IEnumerable<BankAccount>>(cachedData);
            }

            var accounts = await _context.BankAccounts.ToListAsync();
            await _redisCacheService.SetCacheAsync(cacheKey, JsonConvert.SerializeObject(accounts));
            return accounts;
        }

        public async Task<BankAccount> GetAccountById(int id)
        {
            var cacheKey = $"account_{id}";
            var cachedData = await _redisCacheService.GetCacheAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonConvert.DeserializeObject<BankAccount>(cachedData);
            }

            var account = await _context.BankAccounts.FindAsync(id);
            if (account != null)
            {
                await _redisCacheService.SetCacheAsync(cacheKey, JsonConvert.SerializeObject(account));
            }
            return account;
        }

        public async Task<BankAccount> CreateAccount(BankAccount account)
        {
            _context.BankAccounts.Add(account);
            await _context.SaveChangesAsync();
            await _redisCacheService.RemoveCacheAsync("all_accounts");
            return account;
        }

        public async Task<BankAccount> UpdateAccount(BankAccount account)
        {
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _redisCacheService.RemoveCacheAsync("all_accounts");
            await _redisCacheService.RemoveCacheAsync($"account_{account.Id}");
            return account;
        }

        public async Task DeleteAccount(int id)
        {
            var account = await _context.BankAccounts.FindAsync(id);
            if (account != null)
            {
                _context.BankAccounts.Remove(account);
                await _context.SaveChangesAsync();
                await _redisCacheService.RemoveCacheAsync("all_accounts");
                await _redisCacheService.RemoveCacheAsync($"account_{id}");
            }
        }

        public void ProcessTransaction(Transaction transaction)
        {
            _rabbitMqPublisher.PublishTransaction(transaction);
        }
    }
}
