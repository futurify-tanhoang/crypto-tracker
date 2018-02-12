using CryptoTracker.Exceptions;
using CryptoTracker.Models;
using CryptoTracker.Models.ViewModels;
using CryptoTracker.Resources;
using CryptoTracker.ServiceInterfaces;
using CryptoTracker.Setup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Controllers
{
    [Route("api/cryptocurrencies")]
    public class CryptocurrenciesController: Controller
    {
        private ICryptoCurrencyService _cryptoService;

        public CryptocurrenciesController(ICryptoCurrencyService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        [HttpGet("all"), Authorize(Roles = PermissionConstants.VIEW_CRYPTOCURRENCY)]
        public async Task<List<CryptocurrencyViewModel>> All()
        {
            var currencies = await _cryptoService.AllAsync();
            return currencies.Select(t => new CryptocurrencyViewModel { Id = t.Id, Name = t.Name }).ToList();
        }

        [HttpGet("{id}"), Authorize(Roles = PermissionConstants.VIEW_CRYPTOCURRENCY)]
        public async Task<CryptocurrencyViewModel> Get(int id)
        {
            var currency = await _cryptoService.GetAsync(id);

            return new CryptocurrencyViewModel { Id = currency.Id, Name = currency.Name };
        }

        [HttpPost, Authorize(Roles = PermissionConstants.MODIFY_CRYPTOCURRENCY)]
        public async Task<int> Create([FromBody] CryptocurrencyViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                throw new CustomException(Errors.INVALID_INPUT);
            }

            var currency = new CryptoCurrency { Id = model.Id, Name = model.Name };
            currency = await _cryptoService.CreateAsync(currency);

            return currency.Id;
        }

        [HttpPut, Authorize(Roles = PermissionConstants.MODIFY_CRYPTOCURRENCY)]
        public async Task Update([FromBody] CryptocurrencyViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || model.Id <= 0)
            {
                throw new CustomException(Errors.INVALID_INPUT);
            }

            await _cryptoService.UpdateAsync(model.Id, model.Name);
        }
    }
}
