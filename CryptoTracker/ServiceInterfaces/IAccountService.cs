using CryptoTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.ServiceInterfaces
{
    public interface IAccountService
    {
        Task<Account> GetAsync(int id);
        Task<Account> CreateAsync(Account account);
        Task UpdateAsync(Account account);
        Task UpdateEmailAsync(int id, string email);
        Task UpdatePhoneNumberAsync(int id, string phoneNumber);
        Task<Account> CheckEmailAsync(string email);
    }
}
