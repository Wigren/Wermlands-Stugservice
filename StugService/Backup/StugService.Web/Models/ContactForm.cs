using System.ComponentModel.DataAnnotations;

namespace StugService.Web.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Du måste fylla i en mejl-adress")]
        [DataType(DataType.EmailAddress)]
        public string From { get; set; }

        [Required(ErrorMessage = "Du måste fylla i ett meddelande")]
        public string Message { get; set; }
    }
}