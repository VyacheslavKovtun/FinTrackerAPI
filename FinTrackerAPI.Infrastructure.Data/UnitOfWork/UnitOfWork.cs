using FinTrackerAPI.Infrastructure.Data.Database;
using FinTrackerAPI.Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrackerAPI.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        FinDbContext finDbCtx;

        UserRepository _userRepository;
        public UserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(finDbCtx));
       
        TransactionRepository _transactionRepository;
        public TransactionRepository TransactionRepository => _transactionRepository ?? (_transactionRepository = new TransactionRepository(finDbCtx));
        
        CategoryRepository _categoryRepository;
        public CategoryRepository CategoryRepository => _categoryRepository ?? (_categoryRepository = new CategoryRepository(finDbCtx));

        CurrencyRepository _currencyRepository;
        public CurrencyRepository CurrencyRepository => _currencyRepository ?? (_currencyRepository = new CurrencyRepository(finDbCtx));

        
        public UnitOfWork(FinDbContext finDbCtx)
        {
            this.finDbCtx = finDbCtx;
        }

        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    finDbCtx.Dispose();
                }

                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
