using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using EFIRM.Models;
using System.Collections.Generic;
using EFIRM.DAL;
using System.Web.Security;

namespace EFIRM.Controllers
{
	public class AccountController : BaseController
	{
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult Login()
		{
			ViewBag.LogoImage = db.LogoImages.FirstOrDefault()?.LogoImages;
			ViewBag.BuildingImage = db.BuildingImages.FirstOrDefault()?.imagename;
			return View();
		}

		[HttpPost]
		public ActionResult Login(Login model)
		{
			string errors = string.Empty;

			try
			{
				if (string.IsNullOrEmpty(model.UserName))
				{
					throw new ApplicationException("Invalid User Name");
				}
				if (string.IsNullOrEmpty(model.Password))
				{
					throw new ApplicationException("Invalid Password");
				}

				var user = db.tblUserLoginInfoes.Where(m => m.UserName == model.UserName).FirstOrDefault();

				if (user == null)
				{
					errors = "User doesn't exist";
				}
				else
				{
					if (user.Password == model.Password)
					{
						FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
							1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(200), model.RememberMe, model.UserName, FormsAuthentication.FormsCookiePath);

						//For security reasons we may hash the cookies
						string hashCookies = FormsAuthentication.Encrypt(ticket);
						HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);

						// add the cookie to user browser
						Response.Cookies.Add(cookie);

						Session["UserName"] = user.UserName;
						Session["UserContactNumber"] = user.Contact_No;
						Session["UserContactEmail"] = user.Email_id;
						Session["UserID"] = user.Id;
						Session["WorkForceId"] = user.WorkforceId;
						Session["UserRole"] = db.tblUserRoles.Where(x => x.Id == user.UserRoleId).FirstOrDefault()?.RoleName;
						if (Convert.ToString(Session["UserRole"]).ToLower().Contains("admin"))
						{
							return RedirectToAction("Create", "ServiceRequests");
						}
						else
						{
							if (Convert.ToString(Session["UserRole"]).ToLower().Contains("technician"))
							{
								return RedirectToAction("Index", "ServiceReports");
							}
							else
							{
								//Occupant for tenant
								Session["OccupantId"] = db.tblOccupants.Where(x => x.ContactName == model.UserName)?.Select(y => y.Id)?.FirstOrDefault();
								return RedirectToAction("Index", "ServiceRequests");
							}
						}
					}
					else
						errors = "Invalid Password";
				}

			}
			catch (ApplicationException ex)
			{
				errors = ex.Message.ToString();
			}
			catch (Exception ex)
			{
				errors = "Server error. please try again!";
			}

			ViewBag.LogoImage = db.LogoImages.FirstOrDefault()?.LogoImages;
			ViewBag.BuildingImage = db.BuildingImages.FirstOrDefault()?.imagename;
			ViewBag.Errors = errors;
			return View();
		}

		public ActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login", "Account");
		}

		[HttpGet]
		public ActionResult ForgotPassword()
		{
			return View();
		}


		[HttpPost]
		public ActionResult ForgotPassword(UserForgotPassUI model)
		{
			string Error = string.Empty;
			try
			{
				if (ModelState.IsValid)
				{
					if (string.IsNullOrEmpty(model.Email))
					{
						throw new ApplicationException("Invalid Email");
					}

					var user = db.tblUserLoginInfoes.Where(m => m.Email_id == model.Email).FirstOrDefault();

					if (user == null)
					{
						throw new ApplicationException("User not found for this email. Please enter valid email.");
					}
					SMTPemail smtp = new SMTPemail
					{
						EmailUsernames = EmailUsername,
						EmailPorts = EmailPort,
						EmailPasswords = EmailPassword,
						SMTPservers = SMTPserver,
						FromName = "totalent"
					};

					SendEmail(smtp, user.Email_id, user.Password, user.UserName);

					ViewBag.Success = "We have sent you password please check your mail.";
				}
				else
				{
					Error = "Please enter required details";
				}
			}
			catch (ApplicationException ex)
			{
				Error = ex.Message.ToString();
			}
			catch (Exception ex)
			{
				Error = "Server error. please try again!";
			}

			ViewBag.Error = Error;
			return View();
		}

	}
}