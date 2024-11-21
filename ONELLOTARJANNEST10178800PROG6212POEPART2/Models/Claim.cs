using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONELLOTARJANNEST10178800PROG6212POEPART2.Models
{
    public class Claim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClaimId { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Rate { get; set; }

        public int LecturerId { get; set; }

        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int Hours { get; set; }
        public string ClaimStatus { get; set; } = "Pending";
        public DateTime ClaimDate { get; set; }
        public int ClaimAmount => Rate * Hours;
        public string? UploadedFilePath { get; set; }
    }
}
