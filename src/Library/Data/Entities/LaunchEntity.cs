using System.ComponentModel.DataAnnotations;

namespace Library.Data.Entities;

public record LaunchEntity
{
    [Key]
    public int Id { get; init; }

    [Required]
    [MaxLength(256)]
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

    [Required]
    [MaxLength(64)]
    public string UserId { get; set; }
}
