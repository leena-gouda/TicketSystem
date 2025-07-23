using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class Caller
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LoginId { get; set; }
        public LoginModel Login { get; set; }

        public ICollection<IncidentModel> incidents { get; set; }
    }
}
