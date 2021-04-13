using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shop.IRepository;
using Microsoft.Extensions.DependencyInjection;
using Shop.Repository;
using Shop.Entity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Shop.EmailService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<SmsDBContext>(action =>
            {
                action.UseSqlServer("Data Source=.;Initial Catalog=SmsManageSystem;Integrated Security=True");
            });

            var serviceProvider = serviceCollection.AddSingleton<ISmsGoodsRepository, SmsGoodsRepository>().BuildServiceProvider();

            var goods = serviceProvider.GetService<ISmsGoodsRepository>();
            var list = await goods.GetAll();
            foreach (var item in list)
            {
                _logger.LogError(item.GoodsName);
            }
        }
    }
}
