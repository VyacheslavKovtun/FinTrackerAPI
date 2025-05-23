using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrackerAPI.Domain.Interfaces.Interfaces
{
    public interface IRepository<TKey, TValue>
        where TKey : struct
        where TValue : class
    {
        Task<TValue> GetAsync(TKey id);

        Task CreateAsync(TValue value);
    }
}
