using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Demo.Aws.Entities;
using Demo.Aws.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

//Create One Instance per request , it is not thread safe
namespace Demo.Aws.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IAsyncRepository<Transaction> TransactionRepository;
        private readonly IConfiguration Configuration;
        private readonly IAmazonSQS _sqsClient;


        public TransactionService(IAsyncRepository<Transaction> transactionRepository,
        IConfiguration configuration,
        IAmazonSQS sqsClient)
        {
            TransactionRepository = transactionRepository;
            Configuration = configuration;
            _sqsClient=sqsClient;

        }

        public async Task<Transaction> AddTransaction(Transaction transaction)
        {
            
            return await TransactionRepository.AddAsync(transaction);
        }      

        public async Task<IEnumerable<Transaction>> GetTransactions()
        {
            return await TransactionRepository.ListAllAsync();
        }
        // Method to put a message on a queue
        // Could be expanded to include message attributes, etc., in a SendMessageRequest
        public async Task<SendMessageResponse> SendMessage(string messageBody)
        {
            Console.WriteLine($"Send message to queue\n  {Configuration.GetValue<string>("sqsqueue")}");  
            Console.WriteLine(messageBody);
            SendMessageResponse responseSendMsg =
                await _sqsClient.SendMessageAsync(Configuration.GetValue<string>("sqsqueue"), messageBody);
            return responseSendMsg;
        }
    }
}