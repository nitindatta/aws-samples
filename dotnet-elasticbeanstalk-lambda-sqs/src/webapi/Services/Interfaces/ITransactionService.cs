using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Demo.Aws.Entities;

namespace Demo.Aws.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetTransactions();
        Task<Transaction> AddTransaction(Transaction transaction);
        public Task<SendMessageResponse> SendMessage(string messageBody);
 

    }
}