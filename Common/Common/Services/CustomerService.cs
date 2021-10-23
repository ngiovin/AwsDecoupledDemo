using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Common.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace Common.Services
{


    public class SqsCustomerService
    {
        private readonly IAmazonSQS _sqs;
        private readonly ServiceConfiguration _settings;
        public SqsCustomerService(
           IAmazonSQS sqs,
           IOptions<ServiceConfiguration> settings)
        {
            this._sqs = sqs;
            this._settings = settings.Value;
        }


        private List<Customer> _currentCustomer = new List<Customer>();

        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            try
            {
                string message = JsonConvert.SerializeObject(customer);
                var sendRequest = new SendMessageRequest(_settings.AWSSQS.QueueUrlAddUser, message);
                // Post message or payload to queue  
                var sendResult = await _sqs.SendMessageAsync(sendRequest);

                return sendResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<bool> PublishCustomerToDisplayAsync(List<Customer> customers)
        {
            try
            {

                string message = JsonConvert.SerializeObject(customers);
                var sendRequest = new SendMessageRequest(_settings.AWSSQS.QueueUrlGetUsers, message);
                // Post message or payload to queue  
                var sendResult = await _sqs.SendMessageAsync(sendRequest);
                

                return sendResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public bool IsWriter() => _settings.Type == "writer";

        public async Task<List<Customer>> ReciveCustomersToAddAsync()
        {
            try
            {
                //Create New instance  
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _settings.AWSSQS.QueueUrlAddUser,
                    MaxNumberOfMessages = 10,
                    WaitTimeSeconds = 5
                };
                //CheckIs there any new message available to process  
                var result = await _sqs.ReceiveMessageAsync(request);
                foreach (var recepitHandle in result.Messages.Select(m => m.ReceiptHandle))
                {
                    if(!await _deleteMessageAsync(recepitHandle, _settings.AWSSQS.QueueUrlAddUser))
                    {
                        throw new Exception("Messaggio non cancellato");
                    }
                }
                return result.Messages.Any() ? result.Messages.Select(m => JsonConvert.DeserializeObject<Customer>(m.Body)).ToList() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(List<Customer> Customers, string RecepitHandle, string msgId)> ReciveCustomersToDisplayAsync()
        {
            try
            {
                //Create New instance  
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _settings.AWSSQS.QueueUrlGetUsers,
                    MaxNumberOfMessages = 10,
                    WaitTimeSeconds = 5
                };
                //CheckIs there any new message available to process  
                var result = await _sqs.ReceiveMessageAsync(request);
                foreach (var recepitHandle in result.Messages.Select(m => m.ReceiptHandle))
                {
                    if (!await _deleteMessageAsync(recepitHandle, _settings.AWSSQS.QueueUrlGetUsers))
                    {
                        throw new Exception("Messaggio non cancellato");
                    }
                }
                if (result.Messages.Any())
                {
                    return (result.Messages.Select(m => JsonConvert.DeserializeObject<List<Customer>>(m.Body)).Last(), result.Messages.Select(m => m.ReceiptHandle).Last(), result.Messages.Last().MessageId);
                }
                else
                {
                    return (null, null, null);
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public event EventHandler<List<Customer>> NotifyCustomersToDisplayBus;

        public List<Customer> GetCurrentCustomer() => _currentCustomer;

        public void NotifyCustomersToDisplay(List<Customer> customers)
        {
            _currentCustomer = customers;
            NotifyCustomersToDisplayBus?.Invoke(this, customers);
        }


        public event EventHandler<List<Customer>> NotifyCustomerToAddBus;


        public void NotifyCustomerToAdd(List<Customer> customers)
        {
            NotifyCustomerToAddBus?.Invoke(this, customers);
        }





        private async Task<bool> _deleteMessageAsync(string messageReceiptHandle, string queueUrl)
        {
            try
            {
                //Deletes the specified message from the specified queue  
                var deleteResult = await _sqs.DeleteMessageAsync(queueUrl, messageReceiptHandle);
                return deleteResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
