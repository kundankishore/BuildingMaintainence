using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFIRM.Models
{
	public class ExternalLoginConfirmationViewModel
	{
		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}

	public class ExternalLoginListViewModel
	{
		public string ReturnUrl { get; set; }
	}

	public class SendCodeViewModel
	{
		public string SelectedProvider { get; set; }
		public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
		public string ReturnUrl { get; set; }
		public bool RememberMe { get; set; }
	}

	public class VerifyCodeViewModel
	{
		[Required]
		public string Provider { get; set; }

		[Required]
		[Display(Name = "Code")]
		public string Code { get; set; }
		public string ReturnUrl { get; set; }

		[Display(Name = "Remember this browser?")]
		public bool RememberBrowser { get; set; }

		public bool RememberMe { get; set; }
	}

	public class ForgotViewModel
	{
		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}

	public class LoginViewModel
	{
		[Required]
		[Display(Name = "Email")]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}

	public class RegisterViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class ResetPasswordViewModel
	{
		[Required]
		[Display(Name = "Old Password")]
		public string OldPassword { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}

	[Serializable]
	public class User
	{
		public long ID { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public DateTime Date { get; set; }

		public bool IsEmployee { get; set; }
	}

	public class Login
	{
		public string Email { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}

	public class UserChangePassUI
	{

		public long ID { get; set; }

		public string EmailId { get; set; }

		[Required]
		public string OldPassword { get; set; }



		[Required]
		[Compare("Password", ErrorMessage = "The  Password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
		
		[Required]
		public string Password { get; set; }



	}

	public class UserResetPassUI
	{
		public long ID { get; set; }

		public string EmailId { get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "The  Password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required]
		public string Password { get; set; }
	}


	public class UserForgotPassUI
	{
		public long ID { get; set; }

		[Required]
		public string Email { get; set; }


	}

	public class UserSession
	{
		public long ID { get; set; }
		public string Email { get; set; }
		public string Mobile { get; set; }
		public int Status { get; set; }
		public string Date { get; set; }
		public string Address { get; set; }
		public string ImgStr { get; set; }
		public string Name { get; set; }
		public string Qualification { get; set; }

		public int DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public int RoleId { get; set; }
		public string RoleName { get; set; }
		public bool IsEmployee { get; set; }

	}
}
