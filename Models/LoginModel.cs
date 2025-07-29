using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class LoginModel 
    {
      //  [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
      //  [Required]
        [EmailAddress]
        public string Email { get; set; }

       // [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindNever]
        [ValidateNever]
        public string Name { get; set; }
        [BindNever]
        [ValidateNever]
        public string Role { get; set; }
        public bool RememberMe { get; set; }
        public bool isCaller { get; set; }
        public bool isWatcher { get; set; }
        public bool isAdmin { get; set; }
        //public Caller? caller { get; set; }
        //public Watcher? watcher { get; set; }

    }
}
