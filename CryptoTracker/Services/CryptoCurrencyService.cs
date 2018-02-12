using CryptoTracker.Exceptions;
using CryptoTracker.Models;
using CryptoTracker.Resources;
using CryptoTracker.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTracker.Services
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        private AppDbContext _context;

        public CryptoCurrencyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CryptoCurrency>> AllAsync()
        {
            return await _context.CryptoCurrencies.ToListAsync();
        }

        public async Task<CryptoCurrency> CreateAsync(CryptoCurrency cryptoCurrency)
        {
            var existing = await GetAsync(cryptoCurrency.Id);
            if (existing != null)
            {
                throw new CustomException(Errors.CURRENCY_IS_EXISTING, Errors.CURRENCY_IS_EXISTING_MSG);
            }

            _context.CryptoCurrencies.Add(cryptoCurrency);
            await _context.SaveChangesAsync();

            return cryptoCurrency;
        }

        public async Task<CryptoCurrency> GetAsync(int id)
        {
            return await _context.CryptoCurrencies.FindAsync(id);
        }

        public async Task UpdateAsync(int id, string name)
        {
            var existing = await GetAsync(id);
            if (existing == null)
            {
                throw new CustomException(Errors.CURRENCY_NOT_FOUND, Errors.CURRENCY_NOT_FOUND_MSG);
            }

            existing.Name = name;

            await _context.SaveChangesAsync();
        }
    }
}
