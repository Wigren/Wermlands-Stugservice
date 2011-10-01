using System.ComponentModel.DataAnnotations;

namespace StugService.Web.Models
{
    public class ContactFormInputModel
    {
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$",
            ErrorMessage = "Du måste fylla i en korrekt mejladress")]
        [Required(ErrorMessage = "Du måste fylla i en mejl-adress")]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessage = "Du måste fylla i ett meddelande")]
        public string Text { get; set; }

        public string Name { get; set; }
    }
}