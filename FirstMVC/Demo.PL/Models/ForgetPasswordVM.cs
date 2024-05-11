using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
	public class ForgetPasswordVM
	{
		[Required(ErrorMessage = "Email Is Required"), EmailAddress(ErrorMessage = "Email Is Required")]
		public string Email { get; set; }
	}
}
