using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CryptoTracker.ServiceInterfaces;
using CryptoTracker.Middlewares;
using CryptoTracker.Models.BindingModels;
using CryptoTracker.Adapters;

namespace CryptoTracker.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var account = await _accountService.CheckEmailAsync(email);

            return Json(new { IsValid = account == null ? true : false });
        }

        [HttpPost("register")]
        [ValidateModelAttribute]
        public async Task<int> Register(RegisterBindingModel bindingModel)
        {
            var account = await _accountService.CreateAsync(bindingModel.ToModel());

            return account.Id;
        }
    }
}