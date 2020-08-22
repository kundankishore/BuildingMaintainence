using System;
using System.Collections.Generic;
using System.Web;

namespace EFIRM.Models
{
	public class ServiceRequestsViewModels
	{
		public IList<ServiceRequestsModels> Items { get; set; }

		//used for paggination
		public Pager Pager { get; set; }

	}

	public class ServiceRequestsModels
	{
		public int Id { get; set; }
		public Nullable<int> FacilityId { get; set; }
		public string Facility { get; set; }
		public string ServiceRequestNumber { get; set; }
		public int? OccupantName { get; set; }
		public string Occupant { get; set; }
		public Nullable<int> BuildingId { get; set; }
		public string Building { get; set; }
		public Nullable<int> FloorId { get; set; }
		public string Floor { get; set; }
		public Nullable<int> AreaId { get; set; }
		public string Area { get; set; }
		public string ContactNumber { get; set; }
		public string ContactEmail { get; set; }
		public int RequestType { get; set; }
		public string RequestDate { get; set; }
		public Nullable<int> AssetTypeId { get; set; }
		public Nullable<int> AssetId { get; set; }
		public string RequestDescription { get; set; }
		public Nullable<int> ConfirmSLA { get; set; }
		public Nullable<int> Severity { get; set; }
		public string RequestMode { get; set; }
		public string StatusUpdatePreference { get; set; }
		public Nullable<int> AssignedTo { get; set; }
		public Nullable<System.DateTime> RequestClosureBy { get; set; }
		public string AcknowledgedBy { get; set; }
		public string Status { get; set; }
		public Nullable<int> ServiceTypeId { get; set; }
		public Nullable<int> CategoryId { get; set; }
		public string ContactName { get; set; }
		public string WhoLogged { get; set; }
		public Nullable<System.DateTime> AcknowledgeDate { get; set; }
		public Nullable<System.DateTime> ClosingDate { get; set; }
		public Nullable<int> CategoryIdParentId { get; set; }
		public string SpecialInstructionForServiceRequest { get; set; }
		public Nullable<bool> IsThroughService { get; set; }
		public Nullable<int> RoomId { get; set; }
		public string Room { get; set; }
		public Nullable<int> ISeniorTechId { get; set; }
		public string ISeniorTech { get; set; }
		public Nullable<int> AssetGroupId { get; set; }
		public Nullable<System.DateTime> RectificationDateTime { get; set; }
		public string ChangeRequestTypeStatus { get; set; }
		public List<HttpPostedFileBase> Files { get; set; }
		public List<string> FilesUploaded { get; set; }
		public int[] ResourceID { get; set; }
		public List<string> Resources { get; set; }
		public string Group { get; set; }
		public string RequestTypeName { get; set; }
		public string Category { get; set; }
		public string SeverityName { get; set; }
		//Below are used in service reports
		public string Comments { get; set; }
		public string DigitalSignatureImageData { get; set; }
		public int WorkOrderId { get; set; }
		public string WorkOrderRef { get; set; }
	}
}