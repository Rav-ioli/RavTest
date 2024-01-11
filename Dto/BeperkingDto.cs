using System.ComponentModel.DataAnnotations;
namespace MyApplication.Dto
{
    public class BeperkingDto
    {

        [Required]
        public string beperkingsnaam { get; set; }
    }
}