using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Minimum 3 charecter")]
        [MaxLength(3, ErrorMessage = "Minimum of 3 charecter")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Max 100 charecter")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
