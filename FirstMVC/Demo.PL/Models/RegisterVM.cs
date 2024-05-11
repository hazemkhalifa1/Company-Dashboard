using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
	public class RegisterVM
	{
		public string FName { get; set; }
		public string LName { get; set; }
		[Required(ErrorMessage = "Email Is Required"), EmailAddress(ErrorMessage = "Email Is Required")]
		public string Email { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Password Is Required")]
		public string Password { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Confirm Password Is Required")]
		[Compare("Password", ErrorMessage = "Confirm Pasword Dosen't Match Password")]
		public string ConfirmPassword { get; set; }
		public bool Agree { get; set; }
	}
}
