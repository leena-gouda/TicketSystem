using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class TicketWatcher
    {
        //public TicketWatcher(int ticketId, int watcherId)
        //{
        //    TicketId = ticketId;
        //    WatcherId = watcherId;
        //}
        public TicketWatcher(Ticket ticket, Watcher watcher)
        {
            Ticket = ticket;
            Watcher = watcher;
        }
        public TicketWatcher() { }
        
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public int WatcherId { get; set; }
        public Watcher Watcher { get; set; }
    }
}
