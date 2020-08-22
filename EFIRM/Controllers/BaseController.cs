using EFIRM.DAL;
using EFIRM.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace EFIRM.Controllers
{
	public class BaseController : Controller
	{
		public EFIRMDBEntities db;
		public string SMTPserver;
		public string EmailPassword;
		public string EmailUsername;
		public string EmailPort;

		public BaseController()
		{
			db = new EFIRMDBEntities();
			SMTPserver = ConfigurationManager.AppSettings["SMTPSERVER"];
			EmailPassword = ConfigurationManager.AppSettings["PASSWORD"];
			EmailUsername = ConfigurationManager.AppSettings["USERNAME"];
			EmailPort = ConfigurationManager.AppSettings["PORT"];
		}

		private List<tblWorkOrder> _tblWorkOrders;

		public List<tblWorkOrder> tblWorkOrders
		{
			get
			{
				if (_tblWorkOrders == null)
				{
					_tblWorkOrders = db.tblWorkOrders.ToList();
				}
				return _tblWorkOrders;
			}
		}

		private List<tblServiceRequest> _tblServiceRequest;

		public List<tblServiceRequest> tblServiceRequest
		{
			get
			{
				if (_tblServiceRequest == null)
				{
					if (Convert.ToString(Session["UserRole"]).ToLower().Contains("admin"))
					{
						_tblServiceRequest = db.tblServiceRequests?.OrderByDescending(x => x.RequestDate).ToList();
					}
					else
					{
						var Occupant = Convert.ToInt32(Session["OccupantId"]);
						_tblServiceRequest = db.tblServiceRequests.Where(y => y.OccupantName == Occupant)
							?.OrderByDescending(x => x.RequestDate).ToList();
					}
				}
				return _tblServiceRequest;
			}
		}

		private List<tblBuilding> _tblBuilding;

		public List<tblBuilding> tblBuilding
		{
			get
			{
				if (_tblBuilding == null)
				{
					_tblBuilding = db.tblBuildings.ToList();
				}
				return _tblBuilding;
			}
		}

		private List<tblServiceReport> _tblServiceReport;

		public List<tblServiceReport> tblServiceReport
		{
			get
			{
				if (_tblServiceReport == null)
				{
					if (Convert.ToString(Session["UserRole"]).ToLower().Contains("admin"))
					{
						_tblServiceReport = db.tblServiceReports.OrderByDescending(x => x.ReportDate).ToList();
					}
					else
					{
						var WorkForceId = Convert.ToInt32(Session["WorkForceId"]);
						var workorderresources = db.tblWorkOrderResources.Where(x => x.WorkforceId == WorkForceId)?.Select(y => y.WorkOrderId).ToList();

						var workorder = db.tblWorkOrders.Where(y => workorderresources.Contains(y.Id))?.Select(z => z.WorkOrderRefNo).ToList();

						_tblServiceReport = db.tblServiceReports.Where(y => workorder.Contains(y.WORefNo)).OrderByDescending(x => x.ReportDate).ToList();

					}

				}
				return _tblServiceReport;
			}
		}

		private List<tblWorkOrder> _tblWorkOrder;

		public List<tblWorkOrder> tblWorkOrder
		{
			get
			{
				if (_tblWorkOrder == null)
				{
					if (Convert.ToString(Session["UserRole"]).ToLower().Contains("admin"))
					{
						_tblWorkOrder = db.tblWorkOrders.OrderByDescending(x => x.WorkOrderDate).ToList();
					}
					else
					{
						var WorkForceId = Convert.ToInt32(Session["WorkForceId"]);
						var workorderresources = db.tblWorkOrderResources.Where(x => x.WorkforceId == WorkForceId)?.Select(y => y.WorkOrderId).ToList();

						_tblWorkOrder = db.tblWorkOrders.Where(y => workorderresources.Contains(y.Id))?.OrderByDescending(x => x.WorkOrderDate).ToList();
						
					}

				}
				return _tblWorkOrder;
			}
		}

		public class SMTPemail
		{
			public string FromName { get; set; }
			public string SMTPservers { get; set; }
			public string EmailPasswords { get; set; }
			public string EmailUsernames { get; set; }
			public string EmailPorts { get; set; }

		}

		public static bool SendEmail(SMTPemail smtp, string Email, string Password, string UserName)
		{
			string fromStr = smtp.EmailUsernames;
			string fromName = smtp.FromName;

			System.Net.Mail.MailAddress fromAddr = new System.Net.Mail.MailAddress(fromStr, fromName, System.Text.Encoding.UTF8);
			System.Net.Mail.MailAddress toAddr = new System.Net.Mail.MailAddress(Email, "user", System.Text.Encoding.UTF8);
			System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(fromAddr, toAddr);

			string body = "<html><body><p>Hi ,<br/><br/>";
			body += "Your Password is:<strong>" + Password + "</strong><br/>";
			body += "Your UserName is:<strong>" + UserName + "</strong><br/>";
			body += "Please do not share these details with anybody.<br/>";
			body += "Regards<br/>";
			body += "Password Manager<br/>";
			body += "<br/></p></body></html>";



			System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(body, null, "text/html");
			System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(System.Text.RegularExpressions.Regex.Replace(body, @"<(.|\n)*?>", string.Empty), null, "text/plain");

			System.Net.Mail.SmtpClient mailclient = new System.Net.Mail.SmtpClient(smtp.SMTPservers, int.Parse(smtp.EmailPorts));

			message.IsBodyHtml = true;

			message.Subject = "Password " + fromName;
			message.SubjectEncoding = System.Text.Encoding.UTF8;

			message.Headers.Add("Return-Path", fromName + "<" + fromStr + ">");
			message.Headers.Add("Reply-To", fromName + "<" + fromStr + ">");
			message.Headers.Add("From", fromName + "<" + fromStr + ">\r\n");
			message.Headers.Add("Organization", "Get Balance");
			message.Headers.Add("MIME-Version", "1.0\r\n");
			message.Headers.Add("Content-type", "text/html; charset=ISO-8859-1\r\n");
			DateTime now = DateTime.Now;
			message.Headers.Add("Message-Id", String.Concat("<", now.ToString("yyMMdd"), ".", now.ToString("HHmmss"), "@Tetramind.com"));
			message.Priority = System.Net.Mail.MailPriority.High;
			message.ReplyTo = fromAddr;
			message.From = fromAddr;

			message.Sender = fromAddr;


			mailclient.UseDefaultCredentials = false;

			message.BodyEncoding = System.Text.Encoding.UTF8;
			message.Body = body;


			message.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnFailure;

			mailclient.EnableSsl = true;
			mailclient.Credentials = new System.Net.NetworkCredential(fromStr, smtp.EmailPasswords);
			mailclient.Send(message);

			return true;
		}


		public JsonResult GetBuildingsForFacility(int FacilityID)
		{
			var BuildingList = new List<SelectListItem>();
			if (db.tblBuildings.Any(x => x.FacilityId == FacilityID))
			{
				BuildingList = new SelectList(db.tblBuildings.Where(x => x.FacilityId == FacilityID),
				"Id", "BuildingName").ToList();
			}
			return Json(BuildingList, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetFloorForBuilding(int BuildingId)
		{
			var FloorList = new List<SelectListItem>();
			if (db.tblFloors.Any(x => x.BuildingId == BuildingId))
			{
				FloorList = new SelectList(db.tblFloors.Where(x => x.BuildingId == BuildingId),
				"Id", "FloorName").ToList();
			}
			return Json(FloorList, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetAreaForBuilding(int FloorId)
		{
			var AreaList = new List<SelectListItem>();
			if (db.tblAreas.Any(x => x.FloorId == FloorId))
			{
				AreaList = new SelectList(db.tblAreas.Where(x => x.FloorId == FloorId),
					"Id", "AreaName").ToList();
			}
			return Json(AreaList, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetRoomForArea(int AreaId)
		{
			var RoomList = new List<SelectListItem>();
			if (db.tblRooms.Any(x => x.AreaId == AreaId))
			{
				RoomList = new SelectList(db.tblRooms.Where(x => x.AreaId == AreaId),
					"Id", "RoomName").ToList();
			}
			return Json(RoomList, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCategoryForGroup(int AssetGroupId)
		{
			var CategoryList = new List<SelectListItem>();
			if (db.tblAssetTypes.Any(x => x.AssetGroupId == AssetGroupId))
			{
				CategoryList = new SelectList(db.tblAssetTypes.Where(x => x.AssetGroupId == AssetGroupId).OrderBy(y=>y.AssetTypeName),
					"Id", "AssetTypeName").ToList();
			}
			return Json(CategoryList, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetResourceForCategory(int CategoryId)
		{
			var ResourceList = new List<SelectListItem>();

			var TblAssetToMaintenanceTeamMappingsID =
				db.TblAssetToMaintenanceTeamMappings.Where(x => x.AssetId == CategoryId).Select(x => x.MaintenanceTeamId).ToList();

			var TblWorkForceToMaintenanceTeamMapping =
				from Mappings in db.tblWorkForceToMaintenanceTeamMappings
				where TblAssetToMaintenanceTeamMappingsID.Contains(Mappings.MaintenanceTeamId)
				select Mappings;

			var tem = TblWorkForceToMaintenanceTeamMapping.ToList();

			var TblWorkForce =
				db.tblWorkForces.Where(x => TblWorkForceToMaintenanceTeamMapping.Select(y => y.WorkForceId).Contains(x.Id)).ToList();

			ResourceList = new SelectList(TblWorkForce,
					"Id", "PersonName").ToList();

			return Json(ResourceList, JsonRequestBehavior.AllowGet);
		}

		public FileResult Download(string ImageName)
		{
			var FileVirtualPath = "C:\\Program Files (x86)\\Johnson Controls\\EFIRM\\ImagesHelpDesk\\ServiceRequest\\" + ImageName;
			return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
		}

		public FileResult Download2(string ImageName)
		{
			var FileVirtualPath = "C:\\Program Files (x86)\\Johnson Controls\\EFIRM\\ImagesHelpDesk\\ServiceReport\\" + ImageName;
			return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
		}
	}
}
