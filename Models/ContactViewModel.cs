using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloudant.Models
{
    public class ContactViewModel
    {
        public string Id { get; set; }
        public string Rev { get; set; }

        [Display(Name = "First name")]        
        public string FirstName { get; set; }

        [Display(Name = "Last name")] 
        public string LastName { get; set; }

        [Display(Name = "E-mail")] 
        public string Email { get; set; }
    }

}