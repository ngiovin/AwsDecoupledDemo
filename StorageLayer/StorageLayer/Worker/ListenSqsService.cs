
using Common.Models;
using Common.Services;
using Microsoft.Extensions.Hosting;
using StorageLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StorageLayer.Worker
{
    public class ListenSqsService : BackgroundService
    {
        private readonly SqsCustomerService _sqsService;

        private string _lastDisplayRecived = "";
        private readonly CustomerStorageService _customerStorageService;
        public ListenSqsService(SqsCustomerService sqsService, CustomerStorageService customerStorageService) : base()
        {
            _sqsService = sqsService;
            _customerStorageService = customerStorageService;
                
        }


        private async void Handle(object sender, List<Customer> cust)
        {
            cust.RemoveAll(c => c.Name == "nKHSBLHyxpGumNJPFtpxRbtcFhZPIf");
            int truncate = cust.RemoveAll(c => c.Name == "QNdVlMuUPZfWRKDmztoePkvwsYbgym");
            if(truncate > 0)
            {
                await _customerStorageService.Truncate();
            }
            else
            {
                cust.ForEach(c => c.Id = Guid.NewGuid().ToString());
                await _customerStorageService.AddCustomers(cust);
            }      
            await _sqsService.PublishCustomerToDisplayAsync(await _customerStorageService.GetCustomers());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _sqsService.PublishCustomerToDisplayAsync(await _customerStorageService.GetCustomers());
            _sqsService.NotifyCustomerToAddBus += Handle;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                   
                    var res = await _sqsService.ReciveCustomersToAddAsync();
                        
                    if (res != null)
                    {
                        _sqsService.NotifyCustomerToAdd(res);
                    }
                   

                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            _sqsService.NotifyCustomerToAddBus += Handle;
        }
    }
}
