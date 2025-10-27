using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace Infrastructure.Models
{
    public class Procedures
    {
        [Required,Key]
        public int Id { get; set; } 


        public DateTime? Date { get; set; }

        [MaxLength(100)]
        public string? ProcedureType { get; set; }

        [Required]
        public int ProfessorId1 { get; set; }

        [Required]
        public int ProfessorId2 { get; set; }

        [Required]
        public int ProfessorId3 { get; set; }

        [Required]
        public int ProfessorId4 { get; set; }

        [Required]
        public int ProfessorId5 { get; set; }

        public int? ProfessorId6 { get; set; }

        public int? ProfessorId7 { get; set; }

        public int ReserveInternalId { get; set; }

        public int ReserveExternalId { get; set; }
    }
}
