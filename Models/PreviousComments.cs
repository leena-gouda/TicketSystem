using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class PreviousComments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        [Required]
        public string CommentText { get; set; }
        [ForeignKey("Incident")]
        public int IncidentId { get; set; }
        public IncidentModel Incident { get; set; }
        public DateTime? ClosedTime { get; set; }
        public int LoginId { get; set; }
        public LoginModel Login { get; set; }
    }
}
