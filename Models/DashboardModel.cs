using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class DashboardModel
    {
            [Required]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
            public int Id { get; set; }
            public LoginModel user { get; set; }                  
            public ICollection<Ticket> CreateTickets { get; set; }        
            public ICollection<IncidentModel> WatcherIncidents { get; set; }
            public ICollection<IncidentModel> callerIncidents { get; set; }

            public IncidentModel CreateIncident { get; set; }
            public bool IsWatcher { get; set; }
            public bool IsCaller { get; set; }
            public bool IsAdmin { get; set; }
            public IncidentModel? PendingIncident { get; set; }
            public ICollection<IncidentModel>? AssignedIncidents { get; set; }
            public IncidentModel? IncidentsToReview { get; set; }


    }
}
