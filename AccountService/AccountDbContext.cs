using AccountService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AccountService
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options) { }
        public DbSet<BankAccount> BankAccounts { get; set; }
    }
}
