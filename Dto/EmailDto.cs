using System.ComponentModel.DataAnnotations;
namespace MyApplication.Dto
{
    public class EmailDto
    {

        [Required]
        public string email { get; set; }
    }
}