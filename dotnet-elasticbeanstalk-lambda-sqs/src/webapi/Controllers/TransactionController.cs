using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Demo.Aws.Entities;
using Demo.Aws.Services.Interfaces;
using System.Collections.Generic;
using System.Text.Json;

namespace Demo.Aws.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        ITransactionService TransactionService;
        public TransactionController(ITransactionService transactionService)
        {   
            TransactionService=transactionService;
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> Create(Transaction transaction)
        {            
            return Ok(await TransactionService.AddTransaction(transaction));            
        }
        [HttpGet]       
        public async Task<ActionResult<List<Transaction>>>  Get()
        {
            return Ok(await TransactionService.GetTransactions());            
        }
        /// <summary>
        /// This endpoint will push message to SQS Queue
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("transactionasync")]
        public async Task<ActionResult<Transaction>> AddTransactionAsync(Transaction transaction)
        {            
            return Ok(await TransactionService.SendMessage(JsonSerializer.Serialize(transaction)));            
        }
    }
}