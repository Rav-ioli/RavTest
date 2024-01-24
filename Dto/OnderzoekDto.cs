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
        public string uitvoerendbedrijf { get; set; }
        public string uitvoerendbedrijfemail { get; set; }
        public string uitvoerendbedrijfnaam { get; set; }
        public int typebeperking { get; set; }
        public string locatie { get; set; }
    }
}