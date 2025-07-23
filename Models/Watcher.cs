using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketSystem.Views.Home;

namespace TicketSystem.Models
{
    public class Watcher
    {
        

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
      
        
        public int LoginId { get; set; }
        public LoginModel Login { get; set; }

        public ICollection<TicketWatcher> TicketWatchers { get; set; }

        public Watcher( int Id)
        {
            
            this.Id = Id;
        }

        public Watcher() { }
    }
}

