using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Entities
{
    public record ArticleEntity
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public int ApiId { get; init; }

        [Required]
        [MaxLength(512)]
        public string Title { get; init; }

        [Required]
        [MaxLength(512)]
        public string Summary { get; init; }

        [Required]
        [MaxLength(256)]
        public string Url { get; init; }

        [Required]
        [MaxLength(256)]
        public string ImageUrl { get; init; }

        [Required]
        [MaxLength(64)]
        public string NewsSite { get; init; }

        [Required]
        public DateTime PublishDate { get; init; }

        [Required]
        public DateTime UpdateDate { get; init; }

        [Required]
        [MaxLength(64)]
        public string UserId { get; set; }
    }
}
