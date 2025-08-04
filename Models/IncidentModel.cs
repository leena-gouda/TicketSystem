using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketSystem.Migrations;
using TicketSystem.Views.Home;

namespace TicketSystem.Models
{
    public enum IncidentState
    {
        Opened = 0,
        WorkinProgress = 1,
        Done = 2,
        Rejected = 3,
        Accepted = 4
    }
    public class IncidentModel 
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TicketId { get; set; }
        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }
        public int CallerId { get; set; }
        [ForeignKey("CallerId")]
        public Caller caller { get; set; }
        public IncidentState State { get; set; } = IncidentState.Opened;
        public DateTime openDate { get; set; }
        public DateTime? closedDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [NotMapped]
        public string? AdditionalComments { get; set; }
        public ICollection<TicketWatcher> IncidentWatchers { get; set; }
        public ICollection<PreviousComments>? previousComments { get; set; }
        public int? AssignedAdminId { get; set; }
        public Admin AssignedAdmin { get; set; }


    }
}
