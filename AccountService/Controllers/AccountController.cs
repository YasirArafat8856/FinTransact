using AccountService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace AccountService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly AccountService.Services.AccountService _accountService;

        public AccountController(AccountService.Services.AccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAllAccounts();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _accountService.GetAccountById(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BankAccount account)
        {
            var createdAccount = await _accountService.CreateAccount(account);
            return CreatedAtAction(nameof(GetById), new { id = createdAccount.Id }, createdAccount);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BankAccount account)
        {
            if (id != account.Id) return BadRequest();
            var updatedAccount = await _accountService.UpdateAccount(account);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _accountService.DeleteAccount(id);
            return NoContent();
        }
    }
}
