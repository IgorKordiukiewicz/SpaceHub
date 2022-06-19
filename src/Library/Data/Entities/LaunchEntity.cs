using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Entities
{
    public record LaunchEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ApiId { get; init; }

        [Required]
        [MaxLength(128)]
        public string Name { get; init; }

        [Required]
        [MaxLength(256)]
        public string ImageUrl { get; init; }

        [Required]
        [MaxLength(64)]
        public string AgencyName { get; init; }

        [Required]
        [MaxLength(64)]
        public string PadName { get; init; }

        [Required]
        [MaxLength(64)]
        public string Status { get; init; }

        public DateTime? Date { get; init; }
    }
}
