using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
	public class ResetPasswordVM
	{
		[DataType(DataType.Password), Required(ErrorMessage = "Password Is Required")]
		public string Password { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Confirm Password Is Required")]
		[Compare("Password", ErrorMessage = "Confirm Pasword Dosen't Match Password")]
		public string ConfirmPassword { get; set; }
	}
}
