using EFIRM.DAL;
using EFIRM.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace EFIRM.Controllers
{
	[Authorize]
	public class ServiceRequestsController : BaseController
	{
		// GET: ServiceRequests
		public ActionResult Index(int? page, bool ServiceRequestCreated = false)
		{
			ServiceRequestsViewModels Model = new ServiceRequestsViewModels
			{
				Items = new List<ServiceRequestsModels>()
			};

			var tblServiceRequests = tblServiceRequest;
			var tblFacilities = db.tblFacilities.ToList();
			var tblBuildings = db.tblBuildings.ToList();
			var tblFloors = db.tblFloors.ToList();
			var tblAreas = db.tblAreas.ToList();
			var tblRooms = db.tblRooms.ToList();
			Model.Pager = new Pager(tblServiceRequests.Count(), page);

			foreach (var ServiceRequest in tblServiceRequests.Skip((Convert.ToInt32(page) * Model.Pager.PageSize)).Take(Model.Pager.PageSize))
			{
				ServiceRequestsModels ServiceRequestModel = new ServiceRequestsModels
				{
					ServiceRequestNumber = ServiceRequest.ServiceRequestNumber,
					RequestDate = (Convert.ToDateTime(ServiceRequest.RequestDate)).ToString("yyyy/MM/dd, hh:mm:ss tt"),
					RequestDescription = ServiceRequest.RequestDescription,
					ContactName = ServiceRequest.ContactName,
					ContactNumber = ServiceRequest.ContactNumber,
					Status = ServiceRequest.Status,
					FacilityId = ServiceRequest.FacilityId,
					Facility = (ServiceRequest.FacilityId != null && ServiceRequest.FacilityId != 0) ?
					tblFacilities.Where(x => x.Id == ServiceRequest.FacilityId).FirstOrDefault()?.FacilityName : string.Empty,

					BuildingId = ServiceRequest.BuildingId,
					Building = (ServiceRequest.BuildingId != null && ServiceRequest.BuildingId != 0) ?
				tblBuildings.Where(x => x.Id == ServiceRequest.BuildingId).FirstOrDefault()?.BuildingName : string.Empty,

					FloorId = ServiceRequest.FloorId,
					Floor = (ServiceRequest.FloorId != null && ServiceRequest.FloorId != 0) ?
				  tblFloors.Where(x => x.Id == ServiceRequest.FloorId).FirstOrDefault()?.FloorName : string.Empty,

					AreaId = ServiceRequest.AreaId,
					Area = (ServiceRequest.AreaId != null && ServiceRequest.AreaId != 0) ?
					tblAreas.Where(x => x.Id == ServiceRequest.AreaId).FirstOrDefault()?.AreaName : string.Empty,

					RoomId = ServiceRequest.RoomId,
					Room = (ServiceRequest.RoomId != null && ServiceRequest.RoomId != 0) ?
					tblRooms.Where(x => x.Id == ServiceRequest.RoomId).FirstOrDefault()?.RoomName : string.Empty
				};

				Model.Items.Add(ServiceRequestModel);
			}

			if (ServiceRequestCreated)
				ViewBag.ServiceRequestCreated = true;

			return View(Model);
		}

		// GET: ServiceRequests/Create
		public ActionResult Create()
		{
			ViewBag.Occupants = new SelectList(db.tblOccupants, "Id", "OccupantName");
			ViewBag.Facilities = new SelectList(db.tblFacilities, "Id", "FacilityName");
			ViewBag.Groups = new SelectList(db.tblAssetGroups.OrderBy(x => x.AssetGroup), "Id", "AssetGroup");
			ViewBag.RequestTypes = new SelectList(db.tblServiceTypes, "Id", "ServiceDescription");
			ViewBag.Severities = new SelectList(db.tblSeverities, "Id", "SeverityLevel");
			var ResquestDescription = db.tblRequestDescriptions.Select(x => x.RequestDescription).ToList();
			ViewBag.RequestDescriptionDropDown = JsonConvert.SerializeObject(ResquestDescription);

			//For tenant
			if (!Convert.ToString(Session["UserRole"]).ToLower().Contains("admin"))
			{
				var WorkForceId = Convert.ToInt32(Session["WorkForceId"]);
				var Occupant = db.tblOccupants.Where(x => x.Id == WorkForceId).FirstOrDefault();

				if (Occupant != null)
				{
					ViewBag.TenantOcuupantId = Occupant.Id;
					ViewBag.TenantFacilityId = Occupant.FacilityId;
					ViewBag.TenantBuildingId = Occupant.BuildingId;
					ViewBag.TenantFloorId = Occupant.FloorId;
					ViewBag.RoomId = Occupant.RoomId;
					ViewBag.AreaId = Occupant.AreaId;
					ViewBag.ContactName = Occupant.ContactName;
					ViewBag.ContactNumber = Occupant.ContactNumber;
					ViewBag.ContactEmail = Occupant.ContactEmail;
				}

				ViewBag.Occupants = new SelectList(db.tblOccupants.Where(x => x.Id == Occupant.Id), "Id", "OccupantName");

			}

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ServiceRequestsModels ServiceRequest)
		{
			if (ModelState.IsValid)
			{
				var RequestId = Convert.ToInt32(ServiceRequest.RequestType);
				var requesttype = db.tblServiceTypes.Where(x => x.Id == RequestId).FirstOrDefault()?.ServiceDescription;

				if (!string.IsNullOrEmpty(ServiceRequest.Area))
				{
					var TblArea = new tblArea
					{
						FacilityId = ServiceRequest.FacilityId,
						BuildingId = ServiceRequest.BuildingId,
						FloorId = ServiceRequest.FloorId,
						AreaName = ServiceRequest.Area
					};
					//TblArea.Id = db.tblAreas.Max(p => p.Id) + 1;
					db.tblAreas.Add(TblArea);
					db.SaveChanges();
					ServiceRequest.AreaId = TblArea.Id;
				}

				if (!string.IsNullOrEmpty(ServiceRequest.Room))
				{
					var TblRoom = new tblRoom
					{
						FacilityId = ServiceRequest.FacilityId,
						BuildingId = ServiceRequest.BuildingId,
						FloorId = ServiceRequest.FloorId,
						AreaId = ServiceRequest.AreaId,
						RoomName = ServiceRequest.Room
					};
					//TblRoom.Id = db.tblRooms.Max(p => p.Id) + 1;
					db.tblRooms.Add(TblRoom);
					db.SaveChanges();
					ServiceRequest.RoomId = TblRoom.Id;
				}

				var TblServiceRequest = new tblServiceRequest
				{
					OccupantName = ServiceRequest.OccupantName,
					ContactName = ServiceRequest.ContactName,
					ContactNumber = ServiceRequest.ContactNumber,
					ContactEmail = ServiceRequest.ContactEmail,
					FacilityId = ServiceRequest.FacilityId,
					BuildingId = ServiceRequest.BuildingId,
					FloorId = ServiceRequest.FloorId,
					AreaId = ServiceRequest.AreaId,
					RoomId = ServiceRequest.RoomId,
					RequestType = Convert.ToString(ServiceRequest.RequestType),
					AssetGroupId = ServiceRequest.AssetGroupId,
					CategoryId = ServiceRequest.CategoryId,
					AssetTypeId = ServiceRequest.CategoryId,
					RequestDescription = ServiceRequest.RequestDescription,
					ISeniorTechId = ServiceRequest.ISeniorTechId,
					Severity = ServiceRequest.Severity ?? 3,
					WhoLogged = System.Web.HttpContext.Current.User.Identity.Name,
					Status = "Open",
					RequestDate = System.DateTime.Now,
					IsThroughService = false,
					CategoryIdParentId= Convert.ToInt32(db.tblServiceTypes.Where(x=>x.Id == ServiceRequest.RequestType).FirstOrDefault()?.ServiceType)
				};
				string ServiceRequestNumber = string.Empty;
				var LastServiceRequestNumber = (db.tblServiceRequests.Where(y => y.ServiceRequestNumber.Contains(requesttype)).OrderByDescending(x => x.Id).FirstOrDefault()?.ServiceRequestNumber).Split('/'); ;
				DateTime LastServiceRequestNumberDate = DateTime.ParseExact(LastServiceRequestNumber[2], "yyMMdd", CultureInfo.InvariantCulture);

				if (LastServiceRequestNumberDate.Year == DateTime.Today.Year &&
					LastServiceRequestNumberDate.Month == DateTime.Today.Month)
				{
					ServiceRequestNumber = "SR/" + requesttype + "/" +
						DateTime.Today.ToString("yy") + DateTime.Now.Month.ToString("d2") +
						DateTime.Now.Day.ToString("d2")
						+ "/" + (Convert.ToInt32(LastServiceRequestNumber[3]) + 1);

				}
				else
				{

					ServiceRequestNumber = "SR/" + requesttype + "/" +
						DateTime.Today.ToString("yy") + DateTime.Now.Month.ToString("d2") +
						DateTime.Now.Day.ToString("d2")
							+ "/" + 1;
				}
				TblServiceRequest.ServiceRequestNumber = ServiceRequestNumber;
				db.tblServiceRequests.Add(TblServiceRequest);
				db.SaveChanges();


				string WorkOrderRefNumber = string.Empty;
				var LastWorkOrderRefNumber = (db.tblWorkOrders.Where(y => y.WorkOrderRefNo.Contains(requesttype)).OrderByDescending(x => x.Id).FirstOrDefault()?.WorkOrderRefNo).Split('/'); ;
				DateTime LastWorkOrderRefNumberDate = DateTime.ParseExact(LastWorkOrderRefNumber[2], "yyMMdd", CultureInfo.InvariantCulture);
				var WorkOrderId = Convert.ToInt32(ServiceRequest.RequestType);

				if (LastWorkOrderRefNumberDate.Year == DateTime.Today.Year &&
					LastWorkOrderRefNumberDate.Month == DateTime.Today.Month)
				{
					WorkOrderRefNumber = "WO/" + requesttype + "/" +
						DateTime.Today.ToString("yy") + DateTime.Now.Month.ToString("d2") +
						DateTime.Now.Day.ToString("d2")
						+ "/" + (Convert.ToInt32(LastWorkOrderRefNumber[3]) + 1);

				}
				else
				{

					WorkOrderRefNumber = "WO/" + requesttype + "/" +
						DateTime.Today.ToString("yy") + DateTime.Now.Month.ToString("d2") +
						DateTime.Now.Day.ToString("d2")
							+ "/" + 1;
				}

				var TblWorkOrders = new tblWorkOrder
				{
					WorkOrderRefNo = WorkOrderRefNumber,
					ServiceRequestId = TblServiceRequest.Id,
					WorkOrderDate = DateTime.Now,
					Status = "Open",
					RequesterId = Convert.ToInt32(Session["UserID"]),
					CategoryTypeId = ServiceRequest.CategoryId,					
					IsThroughService = false,
					CategoryIdParentId = Convert.ToInt32(db.tblServiceTypes.Where(x => x.Id == ServiceRequest.RequestType).FirstOrDefault()?.ServiceType)

				};
				db.tblWorkOrders.Add(TblWorkOrders);
				db.SaveChanges();

				var NewServiceRequest = tblServiceRequest.Where(x => x.ServiceRequestNumber == ServiceRequestNumber)?.FirstOrDefault();
				List<int> ResourcesList = new List<int>();
				//Adding reosurces in tblWorkOrderResource
				if (ServiceRequest.ResourceID != null && ServiceRequest.ResourceID.Any())
				{
					foreach (var resource in ServiceRequest.ResourceID)
					{
						ResourcesList.Add(resource);
						var Tblworkorderresources = new tblWorkOrderResource
						{
							WorkforceId = resource,
							WorkOrderId = TblWorkOrders.Id
						};

						var TblServiceRequestResources = new tblServiceRequestResource
						{
							ServiceRequestId = NewServiceRequest.Id,
							workforceId = resource
						};

						db.tblWorkOrderResources.Add(Tblworkorderresources);
						db.tblServiceRequestResources.Add(TblServiceRequestResources);
					}
					db.SaveChanges();
				}
				else
				{
					//Service Request is not created from Admin mode so we need to so auto assign of resource
					var TblShifts = db.tblShifts?.ToArray();
					var TblShiftRostering = db.tblShiftRosterings;
					var TblShiftException = db.tblShiftExceptions;
					var TimeNow = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));

					var ShiftNow = (TimeNow.Hours > 17 || TimeNow.Hours < 7) ? TblShifts[1] : TblShifts[0];

					


					var ResouceonLeaveInCurrentShiftToday = TblShiftException.Where(x => x.ShiftId == ShiftNow.Id && x.FromDate <= DateTime.Now && x.ToDate >= DateTime.Now).Select(x => x.WorkForceId);

					foreach (var resource in ResouceonLeaveInCurrentShiftToday)
					{
						var res = db.tblShiftRosterings.FirstOrDefault(x => x.WorkForceId == resource);
						TblShiftRostering.Remove(res);
					}
					
					var dummy = TblShifts.ToList();
					var LeastUsedResourceInCurrentShift = TblShiftRostering.Where(x => x.ShiftId == ShiftNow.Id).OrderBy(x => x.LastUsedDateTime).FirstOrDefault();
					
					ResourcesList.Add(Convert.ToInt32(LeastUsedResourceInCurrentShift?.WorkForceId));

					var Tblworkorderresources = new tblWorkOrderResource
					{
						WorkforceId = LeastUsedResourceInCurrentShift?.WorkForceId,
						WorkOrderId = TblWorkOrders.Id
					};
					var TblServiceRequestResources = new tblServiceRequestResource
					{
						ServiceRequestId = NewServiceRequest.Id,
						workforceId = LeastUsedResourceInCurrentShift?.WorkForceId,
					};

					db.tblServiceRequestResources.Add(TblServiceRequestResources);

					db.tblWorkOrderResources.Add(Tblworkorderresources);
					NewServiceRequest.ISeniorTechId = LeastUsedResourceInCurrentShift?.WorkForceId;

					LeastUsedResourceInCurrentShift.LastUsedDateTime = DateTime.Now;

					db.SaveChanges();
				}

				foreach (var resource in ResourcesList)
				{
					if (resource != 0)
					{
						tblNotificationQueue notification = new tblNotificationQueue();
						notification.ContactNumber = db.tblWorkForces.Where(x => x.Id == resource).FirstOrDefault().ContactNumber;
						notification.MessageBody = TblWorkOrders.WorkOrderRefNo + " " +
							ServiceRequest.RequestDescription + " " + ServiceRequest.ContactName + " " +
							ServiceRequest.ContactNumber + Convert.ToString(ServiceRequest.Severity ?? 3) + " " +
							db.tblBuildings.Where(x => x.Id == ServiceRequest.BuildingId).FirstOrDefault()?.BuildingName + "/" +
							db.tblFloors.Where(x => x.Id == ServiceRequest.FloorId).FirstOrDefault()?.FloorName + "/" +
							db.tblAreas.Where(x => x.Id == ServiceRequest.AreaId).FirstOrDefault()?.AreaName + "/" +
							db.tblRooms.Where(x => x.Id == ServiceRequest.RoomId).FirstOrDefault()?.RoomName;

						notification.IsSMSSent = false;
						notification.SMSSentDateTime = null;
						notification.QueuedDate = DateTime.Now;
						notification.FormRequestType = "WorkOrderAdd";

						db.tblNotificationQueues.Add(notification);
					}
				}
				db.SaveChanges();

				//Adding files to table
				if (ServiceRequest.Files != null && ServiceRequest.Files.Any())
				{
					foreach (var file in ServiceRequest.Files)
					{
						if (file != null && file.ContentLength > 0)
						{
							string newfilename = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + "SREQ" + Path.GetExtension(file.FileName);
							string path = Path.Combine("C:\\Program Files (x86)\\Johnson Controls\\EFIRM\\ImagesHelpDesk\\ServiceRequest",
														   newfilename);
							file.SaveAs(path);

							var TblServiceRequestImages = new tblServiceRequestImage
							{
								sImageName = newfilename,
								nServiceRequestId = TblServiceRequest.Id
							};
							db.tblServiceRequestImages.Add(TblServiceRequestImages);

						}
					}
					db.SaveChanges();
				}

				tblServiceRequestStatu TBLStatus = new tblServiceRequestStatu();
				TBLStatus.dtTimeStamp = DateTime.Now;
				TBLStatus.iServiceRequestId = NewServiceRequest.Id;
				TBLStatus.iStatusId = db.TblServiceStates.Where(x => x.ServiceState == "Open").FirstOrDefault().id;
				db.tblServiceRequestStatus.Add(TBLStatus);

				return RedirectToAction("Index", new { ServiceRequestCreated = true });
			}
			ViewBag.Occupants = new SelectList(db.tblOccupants, "Id", "OccupantName");
			ViewBag.Facilities = new SelectList(db.tblFacilities, "Id", "FacilityName");
			ViewBag.Groups = new SelectList(db.tblAssetGroups, "Id", "AssetGroup");
			ViewBag.RequestTypes = new SelectList(db.tblServiceTypes, "Id", "ServiceDescription");
			ViewBag.Severities = new SelectList(db.tblSeverities, "Id", "SeverityLevel");
			ViewBag.LeadResources = new SelectList(db.tblWorkForces, "Id", "PersonName");
			return View(ServiceRequest);
		}

		// Get details of selected service request
		public ActionResult Edit(string ServiceRequestNumber)
		{
			ServiceRequestsModels ServiceRequestsViewModel = new ServiceRequestsModels();

			var tblServiceRequest = db.tblServiceRequests.
				 Where(x => x.ServiceRequestNumber == ServiceRequestNumber).FirstOrDefault();

			ServiceRequestsViewModel.OccupantName = tblServiceRequest.OccupantName;
			ServiceRequestsViewModel.ContactName = tblServiceRequest.ContactName;
			ServiceRequestsViewModel.ContactNumber = tblServiceRequest.ContactNumber;
			ServiceRequestsViewModel.ContactEmail = tblServiceRequest.ContactEmail;
			ServiceRequestsViewModel.FacilityId = tblServiceRequest.FacilityId;
			ServiceRequestsViewModel.BuildingId = tblServiceRequest.BuildingId;
			ServiceRequestsViewModel.FloorId = tblServiceRequest.FloorId;
			ServiceRequestsViewModel.AreaId = tblServiceRequest.AreaId;
			ServiceRequestsViewModel.RoomId = tblServiceRequest.RoomId;
			Int32.TryParse(tblServiceRequest.RequestType, out int RequestType);
			ServiceRequestsViewModel.RequestType = RequestType;
			ServiceRequestsViewModel.AssetGroupId = tblServiceRequest.AssetGroupId;
			ServiceRequestsViewModel.CategoryId = tblServiceRequest.CategoryId;
			ServiceRequestsViewModel.RequestDescription = tblServiceRequest.RequestDescription;
			ServiceRequestsViewModel.ISeniorTechId = tblServiceRequest.ISeniorTechId;
			ServiceRequestsViewModel.Severity = tblServiceRequest.Severity;
			ServiceRequestsViewModel.ServiceRequestNumber = ServiceRequestNumber;
			ServiceRequestsViewModel.Status = tblServiceRequest.Status;
			ServiceRequestsViewModel.Id = tblServiceRequest.Id;

			var TblWorkOrder = db.tblWorkOrders.Where(x => x.ServiceRequestId == tblServiceRequest.Id).FirstOrDefault();

			var TblWorkOrderResources = db.tblWorkOrderResources.Where(x => x.WorkOrderId == TblWorkOrder.Id).Select(y => y.WorkforceId).ToList();

			var ResourceList =
				from tblWorkForces in db.tblWorkForces
				where TblWorkOrderResources.Contains(tblWorkForces.Id)
				select tblWorkForces;

			ServiceRequestsViewModel.FilesUploaded = new List<string>();
			ServiceRequestsViewModel.FilesUploaded = db.tblServiceRequestImages.Where(x => x.nServiceRequestId == tblServiceRequest.Id).Select(x => x.sImageName).ToList();

			ViewBag.ResourceListID = JsonConvert.SerializeObject(ResourceList.Select(x => x.Id).ToList());
			ViewBag.Occupants = new SelectList(db.tblOccupants, "Id", "OccupantName");
			ViewBag.Facilities = new SelectList(db.tblFacilities, "Id", "FacilityName");
			ViewBag.Groups = new SelectList(db.tblAssetGroups.OrderBy(x => x.AssetGroup), "Id", "AssetGroup");
			ViewBag.RequestTypes = new SelectList(db.tblServiceTypes, "Id", "ServiceDescription");
			ViewBag.Severities = new SelectList(db.tblSeverities, "Id", "SeverityLevel");

			ViewBag.RequestDescriptionDropDown = JsonConvert.SerializeObject(
				db.tblRequestDescriptions.Select(x => x.RequestDescription).ToList());

			return View(ServiceRequestsViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ServiceRequestsModels ServiceRequest)
		{
			if (ModelState.IsValid)
			{
				var RequestId = Convert.ToInt32(ServiceRequest.RequestType);
				var requesttype = db.tblServiceTypes.Where(x => x.Id == RequestId).FirstOrDefault()?.ServiceDescription;

				if (!string.IsNullOrEmpty(ServiceRequest.Area))
				{
					var TblArea = new tblArea
					{
						FacilityId = ServiceRequest.FacilityId,
						BuildingId = ServiceRequest.BuildingId,
						FloorId = ServiceRequest.FloorId,
						AreaName = ServiceRequest.Area
					};
					//TblArea.Id = db.tblAreas.Max(p => p.Id) + 1;
					db.tblAreas.Add(TblArea);
					db.SaveChanges();
					ServiceRequest.AreaId = TblArea.Id;
				}

				if (!string.IsNullOrEmpty(ServiceRequest.Room))
				{
					var TblRoom = new tblRoom
					{
						FacilityId = ServiceRequest.FacilityId,
						BuildingId = ServiceRequest.BuildingId,
						FloorId = ServiceRequest.FloorId,
						AreaId = ServiceRequest.AreaId,
						RoomName = ServiceRequest.Room
					};
					//TblRoom.Id = db.tblRooms.Max(p => p.Id) + 1;
					db.tblRooms.Add(TblRoom);
					db.SaveChanges();
					ServiceRequest.RoomId = TblRoom.Id;
				}

				var TblServiceRequest = db.tblServiceRequests.Where(x => x.ServiceRequestNumber == ServiceRequest.ServiceRequestNumber).FirstOrDefault();

				tblServiceRequestStatu TBLStatus = new tblServiceRequestStatu();
				TBLStatus.dtTimeStamp = DateTime.Now;
				TBLStatus.iServiceRequestId = ServiceRequest.Id;
				TBLStatus.iStatusId = db.TblServiceStates.Where(x => x.ServiceState == "Open").FirstOrDefault().id;
				db.tblServiceRequestStatus.Add(TBLStatus);

				TblServiceRequest.OccupantName = ServiceRequest.OccupantName;
				TblServiceRequest.ContactName = ServiceRequest.ContactName;
				TblServiceRequest.ContactNumber = ServiceRequest.ContactNumber;
				TblServiceRequest.ContactEmail = ServiceRequest.ContactEmail;
				TblServiceRequest.FacilityId = ServiceRequest.FacilityId;
				TblServiceRequest.BuildingId = ServiceRequest.BuildingId;
				TblServiceRequest.FloorId = ServiceRequest.FloorId;
				TblServiceRequest.AreaId = ServiceRequest.AreaId;
				TblServiceRequest.RoomId = ServiceRequest.RoomId;
				TblServiceRequest.RequestType = Convert.ToString(ServiceRequest.RequestType);
				TblServiceRequest.CategoryIdParentId = Convert.ToInt32(db.tblServiceTypes.Where(x => x.Id == ServiceRequest.RequestType).FirstOrDefault()?.ServiceType);
				TblServiceRequest.AssetGroupId = ServiceRequest.AssetGroupId;
				TblServiceRequest.CategoryId = ServiceRequest.CategoryId;
				TblServiceRequest.AssetTypeId = ServiceRequest.CategoryId;
				TblServiceRequest.RequestDescription = ServiceRequest.RequestDescription;
				TblServiceRequest.ISeniorTechId = ServiceRequest.ISeniorTechId ?? TblServiceRequest.ISeniorTechId;
				TblServiceRequest.Severity = ServiceRequest.Severity ?? (TblServiceRequest.Severity ?? 3);

				db.SaveChanges();

				List<int> ResourcesList = new List<int>();
				var WorkOrder = db.tblWorkOrders.Where(x => x.ServiceRequestId == TblServiceRequest.Id).FirstOrDefault();

				WorkOrder.CategoryTypeId = ServiceRequest.CategoryId;
				WorkOrder.CategoryIdParentId = Convert.ToInt32(db.tblServiceTypes.Where(x => x.Id == ServiceRequest.RequestType).FirstOrDefault()?.ServiceType);

				var ExistingResources = db.tblWorkOrderResources.Where(x => x.WorkOrderId == WorkOrder.Id).Select(y => y.WorkforceId)?.ToList();

				//Adding moddified reosurces in tblWorkOrderResource
				if (ServiceRequest.ResourceID != null)
				{
					var NewResources = ServiceRequest.ResourceID.ToList();

					if (NewResources.Any())
					{
						foreach (var resource in NewResources)
						{
							if (!ExistingResources.Contains(resource))
							{
								var Tblworkorderresources = new tblWorkOrderResource
								{
									WorkforceId = resource,
									WorkOrderId = WorkOrder.Id
								};

								var TblServiceRequestResources = new tblServiceRequestResource
								{
									ServiceRequestId = TblServiceRequest.Id,
									workforceId = resource
								};

								ResourcesList.Add(resource);
								db.tblServiceRequestResources.Add(TblServiceRequestResources);

								db.tblWorkOrderResources.Add(Tblworkorderresources);
							}
						}

						foreach (var resource in ExistingResources)
						{
							if (!NewResources.Contains(Convert.ToInt32(resource)))
							{
								var Tblworkorderresources = db.tblWorkOrderResources.Where(x => x.WorkforceId == resource && x.WorkOrderId == WorkOrder.Id).FirstOrDefault();

								var TblServiceRequestResources = db.tblServiceRequestResources.Where(x => x.workforceId == resource && x.ServiceRequestId == TblServiceRequest.Id).FirstOrDefault();

								db.tblWorkOrderResources.Remove(Tblworkorderresources);
								db.tblServiceRequestResources.Remove(TblServiceRequestResources);
							}
						}
						db.SaveChanges();
					}
				}

				foreach (var resource in ResourcesList)
				{
					if (resource != 0)
					{
						tblNotificationQueue notification = new tblNotificationQueue();
						notification.ContactNumber = db.tblWorkForces.Where(x => x.Id == resource).FirstOrDefault().ContactNumber;
						notification.MessageBody = WorkOrder.WorkOrderRefNo + " " +
							ServiceRequest.RequestDescription + " " + ServiceRequest.ContactName + " " +
							ServiceRequest.ContactNumber + Convert.ToString(ServiceRequest.Severity ?? 3) + " " +
							db.tblBuildings.Where(x => x.Id == ServiceRequest.BuildingId).FirstOrDefault()?.BuildingName + "/" +
							db.tblFloors.Where(x => x.Id == ServiceRequest.FloorId).FirstOrDefault()?.FloorName + "/" +
							db.tblAreas.Where(x => x.Id == ServiceRequest.AreaId).FirstOrDefault()?.AreaName + "/" +
							db.tblRooms.Where(x => x.Id == ServiceRequest.RoomId).FirstOrDefault()?.RoomName;

						notification.IsSMSSent = false;
						notification.SMSSentDateTime = null;
						notification.FormRequestType = "WorkOrderUpdate";
						notification.QueuedDate = DateTime.Now;

						db.tblNotificationQueues.Add(notification);
					}
				}
				db.SaveChanges();

				//Adding files to table
				if (ServiceRequest.Files != null && ServiceRequest.Files.Any())
				{
					foreach (var file in ServiceRequest.Files)
					{
						if (file != null && file.ContentLength > 0)
						{
							string newfilename = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + "SREQ" + Path.GetExtension(file.FileName);
							string path = Path.Combine("C:\\Program Files (x86)\\Johnson Controls\\EFIRM\\ImagesHelpDesk\\ServiceRequest",
														   newfilename);
							file.SaveAs(path);

							var TblServiceRequestImages = new tblServiceRequestImage
							{
								sImageName = newfilename,
								nServiceRequestId = TblServiceRequest.Id
							};

							db.tblServiceRequestImages.Add(TblServiceRequestImages);

						}
					}
					db.SaveChanges();
				}

				return RedirectToAction("Index");
			}
			ViewBag.Occupants = new SelectList(db.tblOccupants, "Id", "OccupantName");
			ViewBag.Facilities = new SelectList(db.tblFacilities, "Id", "FacilityName");
			ViewBag.Groups = new SelectList(db.tblAssetGroups, "Id", "AssetGroup");
			ViewBag.RequestTypes = new SelectList(db.tblServiceTypes, "Id", "ServiceDescription");
			ViewBag.Severities = new SelectList(db.tblSeverities, "Id", "SeverityLevel");
			ViewBag.LeadResources = new SelectList(db.tblWorkForces, "Id", "PersonName");
			return View(ServiceRequest);
		}


		// acknowledge selected service request
		public ActionResult AcknowledgeServiceRequest(string ServiceRequestNumber)
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
			tblServiceReport.Status = "Acknowledged";
			tblServiceReport.ServiceStatus = tblServiceRequest.Severity;

			tblServiceRequestStatu TBLStatus = new tblServiceRequestStatu();
			TBLStatus.dtTimeStamp = DateTime.Now;
			TBLStatus.iServiceRequestId = tblServiceRequest.Id;
			TBLStatus.iStatusId = db.TblServiceStates.Where(x => x.ServiceState == "Acknowledged").FirstOrDefault().id;
			db.tblServiceRequestStatus.Add(TBLStatus);

			var TblWorkOrder = db.tblWorkOrders.Where(x => x.ServiceRequestId == tblServiceRequest.Id).FirstOrDefault();

			tblServiceReport.WorkOrderId = TblWorkOrder.Id;
			tblServiceReport.WORefNo = TblWorkOrder.WorkOrderRefNo;

			tblServiceRequest.Status = "Acknowledged";
			TblWorkOrder.Status = "Acknowledged";
			tblServiceRequest.AcknowledgeDate = DateTime.Now;

			db.tblServiceReports.Add(tblServiceReport);

			db.SaveChanges();

			return RedirectToAction("Edit", new { ServiceRequestNumber });
		}

		public FileResult CreatePdf(string ServiceRequestNumber)
		{
			MemoryStream workStream = new MemoryStream();
			StringBuilder status = new StringBuilder("");
			DateTime dTime = DateTime.Now;
			//file name to be created   
			string strPDFFileName = string.Format("Ticket" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
			Document doc = new Document();
			doc.SetMargins(0f, 0f, 0f, 0f);
			//Create PDF Table with 5 columns  
			PdfPTable tableLayout = new PdfPTable(5);
			doc.SetMargins(0f, 0f, 0f, 0f);
			//Create PDF Table  

			//file will created in this path  
			string strAttachment = Server.MapPath("~/Downloads/" + strPDFFileName);


			PdfWriter.GetInstance(doc, workStream).CloseStream = false;
			doc.Open();

			//Add Content to PDF   
			doc.Add(Add_Content_To_PDF(tableLayout, ServiceRequestNumber));

			// Closing the document  
			doc.Close();

			byte[] byteInfo = workStream.ToArray();
			workStream.Write(byteInfo, 0, byteInfo.Length);
			workStream.Position = 0;


			return File(workStream, "application/pdf", strPDFFileName);

		}

		protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, string ServiceRequestNumber)
		{
			List<ServiceRequestsModels> ModelList = new List<ServiceRequestsModels>();
			var ServiceRequest = db.tblServiceRequests.Where(x => x.ServiceRequestNumber == ServiceRequestNumber).FirstOrDefault();
			ServiceRequestsModels ServiceRequestModel = new ServiceRequestsModels();
			ServiceRequestModel.Facility = db.tblFacilities.Where(x => x.Id == ServiceRequest.FacilityId).FirstOrDefault()?.FacilityName;
			ServiceRequestModel.Building = db.tblBuildings.Where(x => x.Id == ServiceRequest.BuildingId).FirstOrDefault()?.BuildingName;
			ServiceRequestModel.Floor = (ServiceRequest.FloorId != null && ServiceRequest.FloorId != 0) ?
								db.tblFloors.Where(x => x.Id == ServiceRequest.FloorId).FirstOrDefault()?.FloorName : string.Empty;

			ServiceRequestModel.Area = (ServiceRequest.AreaId != null && ServiceRequest.AreaId != 0) ?
				db.tblAreas.Where(x => x.Id == ServiceRequest.AreaId).FirstOrDefault()?.AreaName : string.Empty;

			ServiceRequestModel.Room = (ServiceRequest.RoomId != null && ServiceRequest.RoomId != 0) ?
				db.tblRooms.Where(x => x.Id == ServiceRequest.RoomId).FirstOrDefault()?.RoomName : string.Empty;


			PdfPTable table = new PdfPTable(1);
			table.TotalWidth = 200f;
			PdfPCell cell = new PdfPCell(new Phrase(ServiceRequest.tblWorkOrders.FirstOrDefault()?.WorkOrderRefNo));
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			table.AddCell(cell);


			cell = new PdfPCell(new Phrase(Convert.ToString(ServiceRequest.RequestDate)));
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			table.AddCell(cell);

			cell = new PdfPCell(new Phrase("Requestor:\n" + ServiceRequest.ContactName));
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			table.AddCell(cell);


			cell = new PdfPCell(new Phrase("EXT:\n" + ServiceRequest.ContactNumber));
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			table.AddCell(cell);

			cell = new PdfPCell(new Phrase("Location\n" + ServiceRequestModel.Facility + ">"
				+ ServiceRequestModel.Building + ">" + ServiceRequestModel.Floor + ">" +
				ServiceRequestModel.Area + ">" + ServiceRequestModel.Room));
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			table.AddCell(cell);

			cell = new PdfPCell(new Phrase("\n\n\n\n\n"));
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			table.AddCell(cell);

			cell = new PdfPCell(new Phrase("Signature"));
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			table.AddCell(cell);

			return table;
		}

		// Method to add single cell to the Header  
		private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
		{

			tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW)))


			{
				HorizontalAlignment = Element.ALIGN_LEFT,
				Padding = 5,
				BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)

			});
		}

		// Method to add single cell to the body  
		private static void AddCellToBody(PdfPTable tableLayout, string cellText)
		{
			tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))


			{
				HorizontalAlignment = Element.ALIGN_LEFT,
				Padding = 5,
				BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)

			});
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

	}
}
