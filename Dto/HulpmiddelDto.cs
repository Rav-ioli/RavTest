using System.ComponentModel.DataAnnotations;
namespace MyApplication.Dto
{
    public class HulpmiddelDto
    {

        [Required]
        public string hulpmiddelnaam { get; set; }
    }
}