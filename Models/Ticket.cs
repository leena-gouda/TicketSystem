using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class Ticket
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string ServiceType { get; set; }
        [Required] 
        public string Impact {  get; set; }

        [Required]
        [StringLength(100)]
        public string ShortDescription { get; set; }

        public string Description { get; set; }
        [ValidateNever]

        public string? AttachmentPath { get; set; }
        [ValidateNever]

        public string? AttachmentName { get; set; }
        [BindNever]
        public ICollection<TicketWatcher>? TicketWatchers { get; set; }
        [ValidateNever]
        [BindNever]
        public LoginModel Login { get; set; }
        [ValidateNever]
        [BindNever]
        public int LoginId { get; set; }
    }
}
