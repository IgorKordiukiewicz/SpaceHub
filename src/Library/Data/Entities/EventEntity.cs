using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Entities
{
    public record EventEntity
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public int ApiId { get; init; }

        [Required]
        [MaxLength(128)]
        public string Name { get; init; }

        [Required]
        [MaxLength(64)]
        public string Type { get; init; }

        [Required]
        [MaxLength(1024)]
        public string Description { get; init; }

        [Required]
        [MaxLength(128)]
        public string Location { get; init; }

        [Required]
        [MaxLength(256)]
        public string ImageUrl { get; init; }

        public DateTime? Date { get; init; }

        [Required]
        [MaxLength(64)]
        public string UserId { get; set; }
    }
}
