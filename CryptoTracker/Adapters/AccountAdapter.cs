using CryptoTracker.Models;
using CryptoTracker.Models.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Adapters
{
    public static class AccountAdapter
    {
        public static Account ToModel(this RegisterBindingModel bindingModel)
        {
            if (bindingModel == null) return null;

            return new Account
            {
                FullName = bindingModel.FullName,
                Email = bindingModel.Email,
                Password = bindingModel.Password,
                IsVerified = true
            };
        }
    }
}
