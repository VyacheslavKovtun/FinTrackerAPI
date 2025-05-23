using FinTrackerAPI.Infrastructure.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrackerAPI.Infrastructure.Data.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        UserRepository UserRepository { get; }
        TransactionRepository TransactionRepository { get; }
        CategoryRepository CategoryRepository { get; }
        CurrencyRepository CurrencyRepository { get; }
    }
}
