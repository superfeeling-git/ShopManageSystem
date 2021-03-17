using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shop.IRepository;
using Shop.Entity;

namespace Shop.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmsDBContext smsDBContext;
        public UnitOfWork(SmsDBContext _smsDBContext)
        {
            this.smsDBContext = _smsDBContext;
        }

        public async Task<int> SaveChangeAsync()
        {
            return await smsDBContext.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (smsDBContext == null) return;

            smsDBContext.Dispose();
        }
    }
}
