using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketSystem.Migrations;
using TicketSystem.Views.Home;

namespace TicketSystem.Models
{
    public class IncidentModel 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Ticket Ticket { get; set; }
        public Caller caller { get; set; }
        public string State { get; set; }
        public DateTime openDate { get; set; }
        public DateTime? closedDate { get; set; }
        [NotMapped]
        public string? AdditionalComments { get; set; }
        public ICollection<TicketWatcher> IncidentWatchers { get; set; }
        public ICollection<PreviousComments>? previousComments { get; set; }


    }
}
