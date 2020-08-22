using EFIRM.DAL;
using EFIRM.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace EFIRM.Controllers
{
	[Authorize]
	public class ServiceReportsController : BaseController
	{
		// GET: ServiceReports
		public ActionResult Index(int? page)
		{
			ServiceReportsViewModels Model = new ServiceReportsViewModels();
			Model.Items = new List<ServiceReportsModels>();
			var TempModelList = new List<ServiceReportsModels>();
			var tblWorkkOrders = tblWorkOrder;
			var tblFacilities = db.tblFacilities.ToList();
			var tblBuildings = db.tblBuildings.ToList();
			var tblFloors = db.tblFloors.ToList();
			var tblAreas = db.tblAreas.ToList();
			var tblRooms = db.tblRooms.ToList();
			//var tblServiceRequests = db.tblServiceRequests.ToList();
			//var tblWorkOrders = db.tblWorkOrders.ToList();
			Model.Pager = new Pager(tblWorkkOrders.Count(), page);
			List<int?> WorkOrderIdList = new List<int?>();
			foreach (var WorkOrder in tblWorkkOrders.Skip((Convert.ToInt32(page) * Model.Pager.PageSize)).Take(Model.Pager.PageSize))
			{

				ServiceReportsModels ServiceReportsModel = new ServiceReportsModels
				{
					WORefNo = WorkOrder.WorkOrderRefNo,
					ReportDate =
					(Convert.ToDateTime(WorkOrder.WorkOrderDate)).ToString("yyyy/MM/dd, hh:mm:ss tt")
				};

				ServiceReportsModel.RequestorDescription = WorkOrder.tblServiceRequest.RequestDescription;
				ServiceReportsModel.UserName = WorkOrder.tblServiceRequest.ContactName;
				ServiceReportsModel.UserNumber = WorkOrder.tblServiceRequest.ContactNumber;

				ServiceReportsModel.FacilityId = WorkOrder.tblServiceRequest.FacilityId;
				ServiceReportsModel.Facility = tblFacilities.Where(x => x.Id == ServiceReportsModel.FacilityId)?.FirstOrDefault().FacilityName;

				ServiceReportsModel.BuildingId = WorkOrder.tblServiceRequest.BuildingId;
				ServiceReportsModel.Building = tblBuilding.Where(x => x.Id == ServiceReportsModel.BuildingId)?.FirstOrDefault().BuildingName;

				ServiceReportsModel.FloorId = WorkOrder.tblServiceRequest.FloorId;
				ServiceReportsModel.Floor = (ServiceReportsModel.FloorId != null && ServiceReportsModel.FloorId != 0) ?
					tblFloors.Where(x => x.Id == ServiceReportsModel.FloorId).FirstOrDefault()?.FloorName : string.Empty;

				ServiceReportsModel.AreaId = WorkOrder.tblServiceRequest.AreaId;
				ServiceReportsModel.Area = (ServiceReportsModel.AreaId != null && ServiceReportsModel.AreaId != 0) ?
					tblAreas.Where(x => x.Id == ServiceReportsModel.AreaId).FirstOrDefault()?.AreaName : string.Empty;

				ServiceReportsModel.RoomId = WorkOrder.tblServiceRequest.RoomId;
				ServiceReportsModel.Room = (ServiceReportsModel.RoomId != null && ServiceReportsModel.RoomId != 0) ?
					tblRooms.Where(x => x.Id == ServiceReportsModel.RoomId).FirstOrDefault()?.RoomName : string.Empty;

				ServiceReportsModel.Status = WorkOrder.Status;

				Model.Items.Add(ServiceReportsModel);
			}

			return View(Model);
		}

		// Get details of selected service report
		public ActionResult Edit(string WORefNo)
		{
			ServiceRequestsModels ServiceRequestsViewModel = new ServiceRequestsModels();

			var tblServiceReport = db.tblServiceReports.Where(x => x.WORefNo == WORefNo).FirstOrDefault();
			var tblWorkOrder = db.tblWorkOrders.Where(x => x.WorkOrderRefNo == WORefNo).FirstOrDefault();
			var tblServiceRequest = db.tblServiceRequests.
				 Where(x => x.Id == tblWorkOrder.ServiceRequestId).FirstOrDefault();

			ServiceRequestsViewModel.OccupantName = tblServiceRequest.OccupantName;
			ServiceRequestsViewModel.Occupant = db.tblOccupants.Where(x => x.Id == tblServiceRequest.OccupantName).FirstOrDefault()?.OccupantName;
			ServiceRequestsViewModel.ServiceRequestNumber = tblServiceRequest.ServiceRequestNumber;
			ServiceRequestsViewModel.ContactName = tblServiceRequest.ContactName;
			ServiceRequestsViewModel.ContactNumber = tblServiceRequest.ContactNumber;
			ServiceRequestsViewModel.ContactEmail = tblServiceRequest.ContactEmail;
			ServiceRequestsViewModel.Facility = db.tblFacilities.Where(x => x.Id == tblServiceRequest.FacilityId).FirstOrDefault()?.FacilityName;
			ServiceRequestsViewModel.Building = db.tblBuildings.Where(x => x.Id == tblServiceRequest.BuildingId).FirstOrDefault()?.BuildingName;
			ServiceRequestsViewModel.Floor = db.tblFloors.Where(x => x.Id == tblServiceRequest.FloorId).FirstOrDefault()?.FloorName;
			ServiceRequestsViewModel.Area = db.tblAreas.Where(x => x.Id == tblServiceRequest.AreaId).FirstOrDefault()?.AreaName;
			ServiceRequestsViewModel.Room = db.tblRooms.Where(x => x.Id == tblServiceRequest.RoomId).FirstOrDefault()?.RoomName;
			Int32.TryParse(tblServiceRequest.RequestType, out int RequestType);
			ServiceRequestsViewModel.RequestType = RequestType;
			ServiceRequestsViewModel.AssetGroupId = tblServiceRequest.AssetGroupId;
			ServiceRequestsViewModel.Category = db.tblCategories.Where(x => x.Id == tblServiceRequest.CategoryId).FirstOrDefault()?.CategoryName;
			ServiceRequestsViewModel.RequestDescription = tblServiceRequest.RequestDescription;
			ServiceRequestsViewModel.ISeniorTechId = tblServiceRequest.ISeniorTechId;
			ServiceRequestsViewModel.SeverityName = db.tblSeverities.Where(x => x.Id == tblServiceRequest.Severity).FirstOrDefault()?.SeverityLevel;
			ServiceRequestsViewModel.RequestTypeName = db.tblServiceTypes.Where(x => x.Id == ServiceRequestsViewModel.RequestType).FirstOrDefault()?.ServiceDescription;
			ServiceRequestsViewModel.Group = db.tblAssetGroups.Where(x => x.Id == tblServiceRequest.AssetGroupId).FirstOrDefault()?.AssetGroup;
			ServiceRequestsViewModel.Status = tblServiceRequest.Status;

			var TblWorkOrder = db.tblWorkOrders.Where(x => x.ServiceRequestId == tblServiceRequest.Id).FirstOrDefault();

			var TblWorkOrderResources = db.tblWorkOrderResources.Where(x => x.WorkOrderId == TblWorkOrder.Id).Select(y => y.WorkforceId).ToList();

			var TblWorkForces = db.tblWorkForces;

			var ResourceList =
				from tblWorkForces in TblWorkForces
				where TblWorkOrderResources.Contains(tblWorkForces.Id)
				select tblWorkForces;

			ServiceRequestsViewModel.Resources = ResourceList.Select(x => x.PersonName).ToList();
			ServiceRequestsViewModel.ISeniorTech = TblWorkForces.Where(x => x.Id == tblServiceRequest.ISeniorTechId).FirstOrDefault()?.PersonName;
			ServiceRequestsViewModel.FilesUploaded = new List<string>();
			ServiceRequestsViewModel.FilesUploaded = db.tblServiceRequestImages.Where(x => x.nServiceRequestId == tblServiceRequest.Id).Select(x => x.sImageName).ToList();
			var ServiceReport = db.tblServiceReports.Where(x => x.WorkOrderId == TblWorkOrder.Id).FirstOrDefault();
			ServiceRequestsViewModel.Comments = ServiceReport?.Comments;
			try
			{
				if (!string.IsNullOrEmpty(ServiceReport?.SignaturePath))
				{
					string path = Path.Combine("C:\\Program Files (x86)\\Johnson Controls\\EFIRM\\ImagesSignature\\",
															   Path.GetFileName(ServiceReport?.SignaturePath));
					using (Image image = Image.FromFile(path))
					{
						using (MemoryStream m = new MemoryStream())
						{
							image.Save(m, image.RawFormat);
							byte[] imageBytes = m.ToArray();

							// Convert byte[] to Base64 String
							ServiceRequestsViewModel.DigitalSignatureImageData = Convert.ToBase64String(imageBytes);
						}
					}
				}
			}
			catch (Exception ex)
			{

			}
			ServiceRequestsViewModel.WorkOrderId = TblWorkOrder.Id;
			ServiceRequestsViewModel.WorkOrderRef = WORefNo;
			return View(ServiceRequestsViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ServiceRequestsModels Model)
		{
			if (ModelState.IsValid)
			{
				var TblServiceReport = db.tblServiceReports.Where(x => x.WorkOrderId == Model.WorkOrderId)?.FirstOrDefault();

				var tblServiceRequest = db.tblServiceRequests.
						 Where(x => x.ServiceRequestNumber == Model.ServiceRequestNumber).FirstOrDefault();

				var TblWorkOrder = db.tblWorkOrders.Where(x => x.ServiceRequestId == tblServiceRequest.Id)?.FirstOrDefault();

				if (tblServiceReport == null)
				{
					var tblServiceReport = new tblServiceReport();

					tblServiceReport.ContactNo = tblServiceRequest.ContactNumber;
					tblServiceReport.ContactEmail = tblServiceRequest.ContactEmail;
					tblServiceReport.FacilityId = tblServiceRequest.FacilityId;
					tblServiceReport.BuildingId = tblServiceRequest.BuildingId;
					tblServiceReport.FloorId = tblServiceRequest.FloorId;
					tblServiceReport.AreaId = tblServiceRequest.AreaId;
					tblServiceReport.RoomId = tblServiceRequest.RoomId;
					tblServiceReport.RequestType = tblServiceRequest.RequestType;
					tblServiceReport.AssetTypeId = tblServiceRequest.AssetTypeId;
					tblServiceReport.CategoryIdParentId = tblServiceRequest.CategoryIdParentId;
					tblServiceReport.ServiceReportNo = Model.ServiceRequestNumber;
					tblServiceReport.ReportDate = DateTime.Now;
					tblServiceReport.Status = "Acknowledged";

					tblServiceReport.WorkOrderId = TblWorkOrder.Id;
					tblServiceReport.WORefNo = TblWorkOrder.WorkOrderRefNo;

					tblServiceRequest.Status = "Acknowledged";
					TblWorkOrder.Status = "Acknowledged";
					tblServiceRequest.AcknowledgeDate = DateTime.Now;

					tblServiceRequestStatu TBLStatus = new tblServiceRequestStatu();
					TBLStatus.dtTimeStamp = DateTime.Now;
					TBLStatus.iServiceRequestId = tblServiceRequest.Id;
					TBLStatus.iServiceRequestStatusId = db.TblServiceStates.Where(x => x.ServiceState == "Acknowledged").FirstOrDefault().id;
					db.tblServiceRequestStatus.Add(TBLStatus);

					db.tblServiceReports.Add(tblServiceReport);

					db.SaveChanges();
					TblServiceReport = tblServiceReport;
				}
				if (!string.IsNullOrEmpty(Model.DigitalSignatureImageData))
				{
					Model.DigitalSignatureImageData = Model.DigitalSignatureImageData.Replace("data:image/png;base64,", String.Empty);
					string filename = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString()
						+ DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
						DateTime.Now.Second.ToString();
					var UniqueNumber = Convert.ToString(Guid.NewGuid());
					string path = Path.Combine("C:\\Program Files (x86)\\Johnson Controls\\EFIRM\\ImagesSignature",
														   Path.GetFileName(filename + UniqueNumber) + ".png");
					using (FileStream fs = new FileStream(path, FileMode.Create))
					{
						using (BinaryWriter bw = new BinaryWriter(fs))

						{
							byte[] data = Convert.FromBase64String(Model.DigitalSignatureImageData);
							bw.Write(data);
							bw.Close();
						}
					}
					TblServiceReport.SignaturePath = filename + UniqueNumber + ".png";
					TblServiceReport.Status = "Completed";
					TblWorkOrder.Status = "Completed";
					tblServiceRequest.Status = "Completed";
					tblServiceRequest.ClosingDate = DateTime.Now;


					tblServiceRequestStatu TBLStatus = new tblServiceRequestStatu();
					TBLStatus.dtTimeStamp = DateTime.Now;
					TBLStatus.iServiceRequestId = tblServiceRequest.Id;
					TBLStatus.iStatusId = db.TblServiceStates.Where(x => x.ServiceState == "Completed").FirstOrDefault().id;
					db.tblServiceRequestStatus.Add(TBLStatus);

				}
				TblServiceReport.Comments = Model.Comments;
				db.SaveChanges();

				//Adding files to table
				if (Model.Files != null && Model.Files.Any())
				{
					foreach (var file in Model.Files)
					{
						if (file != null && file.ContentLength > 0)
						{
							string newfilename = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid()+"SREP" + Path.GetExtension(file.FileName);
							string path = Path.Combine("C:\\Program Files (x86)\\Johnson Controls\\EFIRM\\ImagesHelpDesk\\ServiceReport",
														   newfilename);
							file.SaveAs(path);

							var TblServiceRequestImages = new tblServiceRequestImage
							{
								sImageName = newfilename,
								nServiceRequestId = tblServiceRequest.Id
							};
							db.tblServiceRequestImages.Add(TblServiceRequestImages);

						}
					}
					db.SaveChanges();
				}
			}
			return RedirectToAction("Edit", new { WORefNo = Model.WorkOrderRef });
		}

		// acknowledge selected service report
		public ActionResult Acknowledge(string ServiceRequestNumber)
		{
			var tblServiceReport = new tblServiceReport();

			var tblServiceRequest = db.tblServiceRequests.
				 Where(x => x.ServiceRequestNumber == ServiceRequestNumber).FirstOrDefault();

			tblServiceReport.ContactNo = tblServiceRequest.ContactNumber;
			tblServiceReport.ContactEmail = tblServiceRequest.ContactEmail;
			tblServiceReport.FacilityId = tblServiceRequest.FacilityId;
			tblServiceReport.BuildingId = tblServiceRequest.BuildingId;
			tblServiceReport.FloorId = tblServiceRequest.FloorId;
			tblServiceReport.AreaId = tblServiceRequest.AreaId;
			tblServiceReport.RoomId = tblServiceRequest.RoomId;
			tblServiceReport.RequestType = tblServiceRequest.RequestType;
			tblServiceReport.AssetTypeId = tblServiceRequest.AssetTypeId;
			tblServiceReport.CategoryIdParentId = tblServiceRequest.CategoryIdParentId;
			tblServiceReport.CategoryTypeId = tblServiceRequest.CategoryId;
			tblServiceReport.ServiceReportNo = ServiceRequestNumber;
			tblServiceReport.ReportDate = DateTime.Now;
			tblServiceReport.Status = "Acknowledged";

			var TblWorkOrder = db.tblWorkOrders.Where(x => x.ServiceRequestId == tblServiceRequest.Id).FirstOrDefault();

			tblServiceReport.WorkOrderId = TblWorkOrder.Id;
			tblServiceReport.WORefNo = TblWorkOrder.WorkOrderRefNo;

			tblServiceRequest.Status = "Acknowledged";
			TblWorkOrder.Status = "Acknowledged";
			tblServiceRequest.AcknowledgeDate = DateTime.Now;

			tblServiceRequestStatu TBLStatus = new tblServiceRequestStatu();
			TBLStatus.dtTimeStamp = DateTime.Now;
			TBLStatus.iServiceRequestId = tblServiceRequest.Id;
			TBLStatus.iStatusId = db.TblServiceStates.Where(x => x.ServiceState == "Acknowledged").FirstOrDefault().id;
			db.tblServiceRequestStatus.Add(TBLStatus);

			db.tblServiceReports.Add(tblServiceReport);

			db.SaveChanges();

			return RedirectToAction("Edit", new { WORefNo = TblWorkOrder.WorkOrderRefNo });
		}

		// rectify selected service report
		public ActionResult Rectify(string ServiceRequestNumber)
		{

			var tblServiceRequest = db.tblServiceRequests.
				 Where(x => x.ServiceRequestNumber == ServiceRequestNumber).FirstOrDefault();

			var TblWorkOrder = db.tblWorkOrders.Where(x => x.ServiceRequestId == tblServiceRequest.Id).FirstOrDefault();

			var tblServiceReport = db.tblServiceReports.Where(x => x.WorkOrderId == TblWorkOrder.Id).FirstOrDefault();

			tblServiceRequest.Status = "Rectified";
			TblWorkOrder.Status = "Rectified";
			tblServiceReport.Status = "Rectified";
			tblServiceRequest.RectificationDateTime = DateTime.Now;

			tblServiceRequestStatu TBLStatus = new tblServiceRequestStatu();
			TBLStatus.dtTimeStamp = DateTime.Now;
			TBLStatus.iServiceRequestId = tblServiceRequest.Id;
			TBLStatus.iStatusId = db.TblServiceStates.Where(x => x.ServiceState == "Rectified").FirstOrDefault().id;
			db.tblServiceRequestStatus.Add(TBLStatus);

			db.SaveChanges();

			return RedirectToAction("Edit", new { WORefNo = TblWorkOrder.WorkOrderRefNo });
		}

	}
}
