using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
	public class LoginVM
	{
		[Required(ErrorMessage = "Email Is Required"), EmailAddress(ErrorMessage = "Email Is Required")]
		public string Email { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Password Is Required")]
		public string Password { get; set; }
		public bool KeepMeIn { get; set; }
	}
}
