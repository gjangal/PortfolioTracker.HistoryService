﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Contracts
{
    interface ICashDeposited
    {
        int PortfolioId { get; set; }
        decimal Amount { get; set; }
    }
}
