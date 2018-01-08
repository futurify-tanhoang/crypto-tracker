using CryptoTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.ServiceInterfaces
{
    public interface ICryptoCurrencyService
    {
        Task<List<CryptoCurrency>> AllAsync();
        Task<CryptoCurrency> GetAsync(int id);
        Task<CryptoCurrency> CreateAsync(CryptoCurrency cryptoCurrency);
        Task UpdateAsync(int id, string name);
    }
}
