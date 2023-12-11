using System.ComponentModel.DataAnnotations;
namespace MyApplication.Dto
{
    public class LoginDto
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}