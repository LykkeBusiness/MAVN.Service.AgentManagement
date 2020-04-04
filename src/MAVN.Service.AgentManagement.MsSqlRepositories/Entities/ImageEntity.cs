using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MAVN.Service.AgentManagement.Domain.Entities;

namespace MAVN.Service.AgentManagement.MsSqlRepositories.Entities
{
    [Table("images")]
    public class ImageEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("customer_id")]
        public Guid CustomerId { get; set; }

        [Required]
        [Column("document_type")]
        public DocumentType DocumentType { get; set; }

        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column("content", TypeName = "char(64)")]
        public string Content { get; set; }
    }
}
