using EFIRM.DAL;
using EFIRM.Models;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFIRM.Controllers
{
	[Authorize]
	public class SettingsController : BaseController
	{
		// GET: Profile
		public ActionResult UserProfile()
		{
			var LoggedInUser = db.tblUserLoginInfoes.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
			var WorkForceUser = db.tblWorkForces.Where(x => x.Id == LoggedInUser.WorkforceId)?.FirstOrDefault();
			var Designation = WorkForceUser!= null? db.tblDesignations.Where(x => x.Id == WorkForceUser.DesignationId)?.FirstOrDefault():null;

			var UserProfile = new UserProfileViewModels
			{
				PersonName = WorkForceUser?.PersonName,
				ContactEmail = WorkForceUser?.ContactEmail,
				ContactNumber = WorkForceUser?.ContactNumber,
				Designation = Designation?.Designation,
				FacilityName = LoggedInUser.tblFacility?.FacilityName,
				IsEmployee = (WorkForceUser?.IsEmployee == true) ? "Employee" : "Vendor",
				UserName = LoggedInUser?.UserName,
				Password = LoggedInUser?.Password
			};
			return View(UserProfile);
		}

		// GET: ChangePassword
		public ActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ChangePassword(ResetPasswordViewModel Model)
		{
			if (ModelState.IsValid)
			{
				var LoggedInUser = db.tblUserLoginInfoes.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();

				if (Model.OldPassword == Model.ConfirmPassword)
				{
					ViewBag.Error = "Old Password and new password cannot be same.";
				}
				else
				{
					if (LoggedInUser.Password == Model.OldPassword)
					{

						LoggedInUser.Password = Model.ConfirmPassword;

						db.SaveChanges();

						ViewBag.Success = "Password changed successfully.";
					}
					else
					{
						ViewBag.Error = "Old Password does not match with existing password.";
					}
				}
			}
			else
			{
				string Message = string.Empty;
				foreach (ModelState modelState in ViewData.ModelState.Values)
				{
					foreach (ModelError error in modelState.Errors)
					{
						Message = error.ErrorMessage;
					}
				}
				ModelState.AddModelError(string.Empty, Message);
			}
			return View();
		}

		// GET: Company Logo
		public ActionResult CompanyLogo()
		{
			ViewBag.LogoImage = db.LogoImages.FirstOrDefault()?.LogoImages;
			return View();
		}

		// GET: Building Logo
		public ActionResult BuildingLogo()
		{
			ViewBag.BuildingImage = db.BuildingImages.FirstOrDefault()?.imagename;
			return View();
		}

		// GET: Features
		public ActionResult Features()
		{
			var AdminFeatureList = db.Admin_Features_Activation.ToList();

			FeaturesViewModels Model = new FeaturesViewModels();

			Model.DigitalSignatureForTechnician = AdminFeatureList.Where(x => x.Features.Equals
			("Access of Digital Signature for technician / lead Technician"))?.FirstOrDefault()?.Status == "Enable"
				? true : false;

			Model.DigitalSignatureForTenant = AdminFeatureList.Where(x => x.Features.Equals
			("Access of Digital Signature for tenant"))?.FirstOrDefault()?.Status == "Enable"
				? true : false;

			Model.ServiceRequest = AdminFeatureList.Where(x => x.Features.Equals
			("Access of Service Request Page for technician / lead Technician"))?.FirstOrDefault()?.Status == "Enable"
				? true : false;

			Model.WorkOrderAssigning = AdminFeatureList.Where(x => x.Features.Equals
			("Auto Work order assigning to technician / lead Technician"))?.FirstOrDefault()?.Status == "Enable"
				? true : false;
			

			return View(Model);
		}

		[HttpPost]
		public ActionResult Features(FeaturesViewModels Model)
		{
			if (ModelState.IsValid)
			{
				db.Database.ExecuteSqlCommand(
					  "update[dbo].[Admin_Features_Activation] set Status ='" + (Model.ServiceRequest == true ? "Enable" : "Disable") + "' where Features = 'Access of Service Request Page for technician / lead Technician'");

				db.Database.ExecuteSqlCommand(
					  "update[dbo].[Admin_Features_Activation] set Status ='" + (Model.DigitalSignatureForTenant == true ? "Enable" : "Disable") + "' where Features = 'Access of Digital Signature for tenant'");

				db.Database.ExecuteSqlCommand(
					  "update[dbo].[Admin_Features_Activation] set Status ='" + (Model.DigitalSignatureForTechnician == true ? "Enable" : "Disable") + "' where Features = 'Access of Digital Signature for technician / lead Technician'");

				db.Database.ExecuteSqlCommand(
					  "update[dbo].[Admin_Features_Activation] set Status ='" + (Model.WorkOrderAssigning == true ? "Enable" : "Disable") + "' where Features = 'Auto Work order assigning to technician / lead Technician'");

			}
			else
			{
				string Message = string.Empty;
				foreach (ModelState modelState in ViewData.ModelState.Values)
				{
					foreach (ModelError error in modelState.Errors)
					{
						Message = error.ErrorMessage;
					}
				}
				ModelState.AddModelError(string.Empty, Message);
			}
			return View();
		}


		public ActionResult CompanyLogoUpload(HttpPostedFileBase file)
		{
			if (file != null)
			{
				string path = Path.Combine(Server.MapPath("~/images/LogoImage"),
														   Path.GetFileName(file.FileName));
				file.SaveAs(path);
				var LogoImage = db.LogoImages.FirstOrDefault();

				if (LogoImage != null)
				{

					db.Database.ExecuteSqlCommand("Update [dbo].[LogoImage] set LogoImages = '" + file.FileName + "' where logoId =" + LogoImage.logoId);
				}
				else
				{
					db.Database.ExecuteSqlCommand("insert into [dbo].[LogoImage] (LogoImages) values('" + file.FileName + "')");

				}

			}
			// after successfully uploading redirect the user
			return RedirectToAction("CompanyLogo");
		}

		public ActionResult BuildingLogoUpload(HttpPostedFileBase file)
		{
			if (file != null)
			{
				string path = Path.Combine(Server.MapPath("~/images/BuildingImage"),
														   Path.GetFileName(file.FileName));
				file.SaveAs(path);
				var BuildingImage = db.BuildingImages.FirstOrDefault();

				if (BuildingImage != null)
				{

					db.Database.ExecuteSqlCommand("Update [dbo].[BuildingImages] set imagename = '" + file.FileName + "' where id =" + BuildingImage.id);
				}
				else
				{
					db.Database.ExecuteSqlCommand("insert into [dbo].[BuildingImages] (imagename) values('" + file.FileName + "')");

				}

			}
			// after successfully uploading redirect the user
			return RedirectToAction("BuildingLogo");
		}


	}
}
