using System;
using System.Collections.Generic;
using System.Web;

namespace EFIRM.Models
{
	public class ServiceReportsViewModels
	{
		public IList<ServiceReportsModels> Items { get; set; }

		//used for paggination
		public Pager Pager { get; set; }
	}

	public class ServiceReportsModels
	{
		public int Id { get; set; }
		public Nullable<int> FacilityId { get; set; }
		public Nullable<int> WorkOrderId { get; set; }
		public string WORefNo { get; set; }
		public string ContractCoverage { get; set; }
		public Nullable<int> BuildingId { get; set; }
		public Nullable<int> FloorId { get; set; }
		public Nullable<int> AreaId { get; set; }
		public string ContactNo { get; set; }
		public string ContactEmail { get; set; }
		public string RequestType { get; set; }
		public Nullable<int> AssetTypeId { get; set; }
		public Nullable<int> AssetId { get; set; }
		public Nullable<int> ServiceStatus { get; set; }
		public Nullable<bool> CustomerSignOffreceived { get; set; }
		public string UploadSR { get; set; }
		public string ServiceReportNo { get; set; }
		public string ReportDate { get; set; }
		public Nullable<bool> NotificationSend { get; set; }
		public Nullable<int> MaintenanceTaskId { get; set; }
		public string SignaturePath { get; set; }
		public Nullable<int> CategoryIdParentId { get; set; }
		public Nullable<int> CategoryTypeId { get; set; }
		public Nullable<bool> WorkHazardous { get; set; }
		public Nullable<bool> TestedForDefect { get; set; }
		public string Status { get; set; }
		public string Comments { get; set; }
		public Nullable<int> RoomId { get; set; }
		public Nullable<int> CustomerFeedbackId { get; set; }
		public Nullable<int> ReportSignedUserId { get; set; }
		public string RequestorDescription { get; set; }
		public string UserName { get; set; }
		public string UserNumber { get; set; }
		public string Facility { get; set; }
		public string Building { get; set; }
		public string Floor { get; set; }
		public string Area { get; set; }
		public string Room { get; set; }
	}
}