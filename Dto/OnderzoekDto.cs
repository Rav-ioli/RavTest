using System.ComponentModel.DataAnnotations;
namespace MyApplication.Dto
{
    public class OnderzoekDto
    {
        [Required]
        public string titel { get; set; }
        [Required]
        public string korteBeschrijving { get; set; }
        [Required]
        public DateTime datum { get; set; }
        [Required]
        public string beloning { get; set; }
        [Required]
        public string soortOnderzoek { get; set; }//online of fysiek
    }
}