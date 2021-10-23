
using Common.Models;
using Common.Services;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PresentationLayer.Worker
{
    public class ListenSqsService : BackgroundService
    {
        private readonly SqsCustomerService _sqsService;
        private string _lastDisplayRecived = "";
        public ListenSqsService(SqsCustomerService sqsService) : base()
        {
            _sqsService = sqsService;
                
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _sqsService.AddCustomerAsync(new Customer() { Name = "nKHSBLHyxpGumNJPFtpxRbtcFhZPIf" });
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                   
                    var res = (await _sqsService.ReciveCustomersToDisplayAsync());
                    if(res.Customers != null && res.msgId != _lastDisplayRecived)
                    {
                        _lastDisplayRecived = res.msgId;
                        _sqsService.NotifyCustomersToDisplay(res.Customers);
                    }
                        
                   


                }
                catch (Exception e)
                {
                    throw e;
                }

            }
        }
    }
}
