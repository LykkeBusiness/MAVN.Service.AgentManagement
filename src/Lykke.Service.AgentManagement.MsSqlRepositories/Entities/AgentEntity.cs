using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lykke.Service.AgentManagement.Domain.Entities.Agents;

namespace Lykke.Service.AgentManagement.MsSqlRepositories.Entities
{
    [Table("agents")]
    public class AgentEntity
    {
        [Key]
        [Column("customer_id")]        
        public Guid CustomerId { get; set; }

        [Column("salesforce_id", TypeName = "varchar(100)")]
        public string SalesforceId { get; set; }

        [Required]
        [Column("status")]
        public AgentStatus Status { get; set; }
    }
}
