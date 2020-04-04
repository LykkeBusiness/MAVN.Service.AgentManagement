using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Falcon.Numerics;

namespace MAVN.Service.AgentManagement.MsSqlRepositories.Entities
{
    [Table("tokens_requirement")]
    public class TokensRequirementEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("amount")]
        public Money18 Amount { get; set; }
    }
}
