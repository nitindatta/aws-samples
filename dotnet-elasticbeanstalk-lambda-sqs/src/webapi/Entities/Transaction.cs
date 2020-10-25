#nullable enable

using System.ComponentModel.DataAnnotations;

namespace Demo.Aws.Entities
{
    public class Transaction : BaseEntity
    {
        [Key]
        public int TransactionId { get; set; }
        public string? Name { get; set; }
    
    }
}