﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models
{
    public enum TransactionAction
    {
        Deposit, Withdraw, Transfer, Receive
    }

    public enum CryptoAction
    {
        Deposit, Withdraw, Buy, Sell
    }
}
