using CryptoTracker.Exceptions;
using CryptoTracker.Models;
using CryptoTracker.Resources;
using CryptoTracker.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Services
{
    public class AccountService : IAccountService
    {
        private AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Account> GetAsync(int id)
        {
            return await _context.Accounts.Include(s => s.Wallet).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Account> CheckEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(t => t.Email == email);
        }

        public async Task<Account> CreateAsync(Account account)
        {
            var existing = await GetAsync(account.Id);
            if (existing != null)
            {
                throw new CustomException(Errors.ACCOUNT_IS_EXISTING, Errors.ACCOUNT_IS_EXISTING_MSG);
            }

            account.Wallet = new Wallet();

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task UpdateAsync(Account account)
        {
            var existing = await GetAsync(account.Id);
            if (existing == null)
            {
                throw new CustomException(Errors.ACCOUNT_NOT_FOUND, Errors.ACCOUNT_NOT_FOUND_MSG);
            }

            existing.FullName = account.FullName;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmailAsync(int id, string email)
        {
            var account = await GetAsync(id);
            if (account == null)
            {
                throw new CustomException(Errors.ACCOUNT_NOT_FOUND, Errors.ACCOUNT_NOT_FOUND_MSG);
            }

            if(await _context.Accounts.AnyAsync(t => t.Id != id && t.Email == email))
            {
                throw new CustomException(Errors.EMAIL_IS_USING_BY_ANOTHER_USER, Errors.EMAIL_IS_USING_BY_ANOTHER_USER_MSG);
            }

            account.Email = email;
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePhoneNumberAsync(int id, string phoneNumber)
        {
            var account = await GetAsync(id);
            if (account == null)
            {
                throw new CustomException(Errors.ACCOUNT_NOT_FOUND, Errors.ACCOUNT_NOT_FOUND_MSG);
            }

            if (await _context.Accounts.AnyAsync(t => t.Id != id && t.PhoneNumber == phoneNumber))
            {
                throw new CustomException(Errors.PHONE_NUMBER_IS_USED_BY_ANOTHER_USER, Errors.PHONE_NUMBER_IS_USED_BY_ANOTHER_USER_MSG);
            }

            account.PhoneNumber = phoneNumber;
            await _context.SaveChangesAsync();
        }

        public async Task<string[]> GetPermissionsOfAccountAsync(int id)
        {
            var accountPermissions = await _context.AccountsPermissions.Where(a => a.AccountId == id).Select(p => p.PermissionId).ToListAsync();

            var rolePermissions = await _context.AccountsRoles.Where(a => a.AccountId == id).SelectMany(r => r.Role.Permissions.Select(p => p.Permission.Id)).ToListAsync();

            return accountPermissions.Union(rolePermissions).Distinct().ToArray();

        }
    }
}
