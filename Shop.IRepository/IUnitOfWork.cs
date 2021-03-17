using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.IRepository
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync();
    }
}
