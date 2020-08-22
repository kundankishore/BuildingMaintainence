using EFIRM.DAL;
using EFIRM.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EFIRM.Controllers
{
	[Authorize]
	public class DashboardController : BaseController
	{
		public ActionResult OverView(DateTime? startDate = null, DateTime? endDate = null)
		{
			DateTime StartDate = startDate ?? DateTime.Today.AddMonths(-6);
			DateTime EndDate = endDate ?? DateTime.Today;

			var TblServiceRequest = db.tblServiceRequests.Where(x => x.RequestDate >= StartDate && x.RequestDate <= EndDate).ToList();

			int ResponseCount = 0, ResponseTime = 0;
			int RectificationCount = 0, Rectificationtime = 0;

			foreach (var item in TblServiceRequest)
			{
				if (item.AcknowledgeDate != null)
				{
					ResponseCount++;
					ResponseTime += (Convert.ToDateTime(item.AcknowledgeDate).Subtract(Convert.ToDateTime(item.RequestDate))).Minutes;
				}

				if (item.RectificationDateTime != null)
				{
					RectificationCount++;
					Rectificationtime += (Convert.ToDateTime(item.RectificationDateTime).Subtract(Convert.ToDateTime(item.AcknowledgeDate))).Minutes;
				}
			}

			ViewData.Add("AVERAGE_RESPONSE_TIME", ResponseTime + " mins");
			ViewData.Add("AVERAGE_RECTIFICATION_TIME", Rectificationtime + " mins");

			ViewData.Add("AVERAGE_RESPONSE_Count", ResponseCount + " ");
			ViewData.Add("AVERAGE_RECTIFICATION_Count", RectificationCount + " ");

			GetWorkOrder(StartDate, EndDate);
			return View();
		}

		private void GetWorkOrder(DateTime startDate, DateTime endDate)
		{
			try
			{
				GetWorkTotalOrder(startDate, endDate);
				GetTotalWorkOrder_RequestType(startDate, endDate);
				GetTotalWorkOrder_RequestType(startDate, endDate, true);
				GetTotalWorkOrder_Severity(startDate, endDate);
				GetTotalWorkOrder_AssetGroup(startDate, endDate);
				GetTop5WorkOrder_AssetType(startDate, endDate);
				GetBuildingWiseTotalAndOpenWorkOrder(startDate, endDate);
			}
			catch (Exception ex)
			{

			}
		}
		private void GetWorkTotalOrder(DateTime startDate, DateTime endDate)
		{
			try
			{
				var workorderTotal = tblWorkOrders.Where(y => y.WorkOrderDate >= startDate && y.WorkOrderDate <= endDate)
					.GroupBy(x => new { ((DateTime)x.WorkOrderDate).Month, ((DateTime)x.WorkOrderDate).Year })
					.Select(grp => new { Count = grp.Count(), Month = grp.Key.Month, Year = grp.Key.Year }).ToList();

				int TOTAL_WORK_ORDERS = workorderTotal.Select(e => e.Count).Sum();
				ViewData.Add("TOTAL_WORK_ORDERS", TOTAL_WORK_ORDERS);

				var workorderOpen = tblWorkOrders.Where(y => y.WorkOrderDate >= startDate && y.WorkOrderDate <= endDate && y.Status.Equals("Open", StringComparison.InvariantCultureIgnoreCase))
					.GroupBy(x => new { ((DateTime)x.WorkOrderDate).Month, ((DateTime)x.WorkOrderDate).Year })
					.Select(grp => new { Count = grp.Count(), Month = grp.Key.Month, Year = grp.Key.Year }).ToList();

				var workorderclosed= tblWorkOrders.Where(y => y.WorkOrderDate >= startDate && y.WorkOrderDate <= endDate && (y.Status.Equals("Completed", StringComparison.InvariantCultureIgnoreCase) || y.Status.Equals("Rectified", StringComparison.InvariantCultureIgnoreCase)))
					.GroupBy(x => new { ((DateTime)x.WorkOrderDate).Month, ((DateTime)x.WorkOrderDate).Year })
					.Select(grp => new { Count = grp.Count(), Month = grp.Key.Month, Year = grp.Key.Year }).ToList();

				//var workorderrectified = tblWorkOrders.Where(y => y.WorkOrderDate >= startDate && y.WorkOrderDate <= endDate && y.Status.Equals("Rectified", StringComparison.InvariantCultureIgnoreCase))
				//	.GroupBy(x => new { ((DateTime)x.WorkOrderDate).Month, ((DateTime)x.WorkOrderDate).Year })
				//	.Select(grp => new { Count = grp.Count(), Month = grp.Key.Month, Year = grp.Key.Year }).ToList();

				int OPEN_WORK_ORDERS = workorderOpen.Select(e => e.Count).Sum();
				int CLOSED_WORK_ORDERS = workorderclosed.Select(e => e.Count).Sum();// + workorderrectified.Select(e => e.Count).Sum();
				ViewData.Add("OPEN_WORK_ORDERS", OPEN_WORK_ORDERS);
				ViewData.Add("CLOSED_WORK_ORDERS", CLOSED_WORK_ORDERS);

				var diff = Enumerable.Range(0, Int32.MaxValue)
					 .Select(e => startDate.AddMonths(e))
					 .TakeWhile(e => e <= endDate)
					 .Select(e => new { MonthName = e.ToString("MMM"), Month = e.Month, Year = e.Year }).ToList();

				var query_workorder = from diff1 in diff
									  join workorderTotal1 in workorderTotal on new { Month = diff1.Month, Year = diff1.Year } equals new
									  {
										  Month = workorderTotal1.Month,
										  Year = workorderTotal1.Year
									  } into gj
									  from fgi in gj.Where(f => f.Year == diff1.Year && f.Month == diff1.Month).DefaultIfEmpty()
									  from subpet in gj.DefaultIfEmpty()
									  select new { diff1.Month, diff1.Year, Value = subpet?.Count ?? 0 };

				var query_workoder_open = from diff1 in diff
										  join workorderOpen1 in workorderOpen on new { Month = diff1.Month, Year = diff1.Year } equals new
										  {
											  Month = workorderOpen1.Month,
											  Year = workorderOpen1.Year
										  } into gj
										  from fgi in gj.Where(f => f.Year == diff1.Year).DefaultIfEmpty()
										  from subpet in gj.DefaultIfEmpty()
										  select new { diff1.Month, diff1.Year, Value = subpet?.Count ?? 0 };

				ViewData.Add("workorder_series", "{ name: 'Total', data: [" + Convert.ToString(String.Join(",", query_workorder.ToList().Select(a => a.Value))) + "] }, { name: 'Open', data: [" + Convert.ToString(String.Join(",", query_workoder_open.ToList().Select(a => a.Value))) + "] }");
				ViewData.Add("workorder_categories", Convert.ToString(String.Join(",", diff.ToList().Select(a => "'" + a.MonthName + " " + a.Year + "'"))));
			}
			catch (Exception ex)
			{

			}
		}

		private void GetTotalWorkOrder_RequestType(DateTime startDate, DateTime endDate, bool? isOpenRequestsOnly = false)
		{
			try
			{
				var tblServiceType = db.tblServiceTypes.ToList();
				int month = 1, year = 2018;
				IDictionary<string, string> Allrequest = new Dictionary<string, string>();
				IDictionary<string, string> OpenRequest = new Dictionary<string, string>();
				while (startDate <= endDate)
				{
					month = startDate.Month;
					year = startDate.Year;

					var query = from sr in tblServiceRequest
								join st in tblServiceType on sr.RequestType equals Convert.ToString(st.Id)
								where Convert.ToDateTime(sr.RequestDate).Month == month && Convert.ToDateTime(sr.RequestDate).Year == year
								group new { sr, st } by new { st.ServiceDescription } into grouped
								select new { grouped.Key.ServiceDescription, Count = grouped.Count() };

					var topitems = query.OrderByDescending(x => x.Count).ToList();

					if (topitems.Any())
					{
						foreach (var item in topitems)
						{

							if (Allrequest.ContainsKey(item.ServiceDescription))
							{
								Allrequest[item.ServiceDescription] += "," + item.Count;
							}
							else
							{
								Allrequest.Add(item.ServiceDescription, Convert.ToString(item.Count));
							}
						}
					}
					else
					{
						if (Allrequest.ContainsKey("CM"))
						{
							Allrequest["CM"] += "," + 0;
						}
						else
						{
							Allrequest.Add("CM", Convert.ToString(0));
						}

						if (Allrequest.ContainsKey("PM"))
						{
							Allrequest["PM"] += "," + 0;
						}
						else
						{
							Allrequest.Add("PM", Convert.ToString(0));
						}

						if (Allrequest.ContainsKey("EV"))
						{
							Allrequest["EV"] += "," + 0;
						}
						else
						{
							Allrequest.Add("EV", Convert.ToString(0));
						}
					}

					var queryopen = from sr in tblServiceRequest
									join st in tblServiceType on sr.RequestType equals Convert.ToString(st.Id)
									where Convert.ToDateTime(sr.RequestDate).Month == month && Convert.ToDateTime(sr.RequestDate).Year == year && sr.Status.Equals("Open", StringComparison.InvariantCultureIgnoreCase)
									group new { sr, st } by new {st.ServiceDescription } into grouped
									select new { grouped.Key.ServiceDescription, Count = grouped.Count() };

					var topitemsopen = queryopen.OrderByDescending(x => x.Count).ToList();

					if (topitemsopen.Any())
					{
						foreach (var item in topitemsopen)
						{

							if (OpenRequest.ContainsKey(item.ServiceDescription))
							{
								OpenRequest[item.ServiceDescription] += "," + item.Count;
							}
							else
							{
								OpenRequest.Add(item.ServiceDescription, Convert.ToString(item.Count));
							}
						}
					}
					else
					{
						if (OpenRequest.ContainsKey("CM"))
						{
							OpenRequest["CM"] += "," + 0;
						}
						else
						{
							OpenRequest.Add("CM", Convert.ToString(0));
						}

						if (OpenRequest.ContainsKey("PM"))
						{
							OpenRequest["PM"] += "," + 0;
						}
						else
						{
							OpenRequest.Add("PM", Convert.ToString(0));
						}

						if (OpenRequest.ContainsKey("EV"))
						{
							OpenRequest["EV"] += "," + 0;
						}
						else
						{
							OpenRequest.Add("EV", Convert.ToString(0));
						}
					}

					startDate = startDate.AddMonths(1);
				}

				string json = "";


				foreach (var item in Allrequest)
				{

					if (json == string.Empty)
						json += "{ name: '" + item.Key.Replace(System.Environment.NewLine, string.Empty) + "', data: [" + item.Value + "] }";
					else
						json += ",{ name: '" + item.Key.Replace(System.Environment.NewLine, string.Empty) + "', data: [" + item.Value + "] }";
				}

				string json1 = "";

				foreach (var item in OpenRequest)
				{

					if (json1 == string.Empty)
						json1 += "{ name: '" + item.Key.Replace(System.Environment.NewLine, string.Empty) + "', data: [" + item.Value + "] }";
					else
						json1 += ",{ name: '" + item.Key.Replace(System.Environment.NewLine, string.Empty) + "', data: [" + item.Value + "] }";
				}


				ViewData.Add("workorder_req_series", json);

				ViewData.Add("workorder_req_open_series", json1);
			}
			catch (Exception ex)
			{

			}
		}

		private void GetTotalWorkOrder_Severity(DateTime startDate, DateTime endDate)
		{
			try
			{
				var tblSeverities = db.tblSeverities.ToList();
				int month = 1, year = 2018;
				IDictionary<string, string> Assets = new Dictionary<string, string>();
				while (startDate <= endDate)
				{
					month = startDate.Month;
					year = startDate.Year;

					var query = from sr in tblServiceRequest
								join s in tblSeverities on sr.Severity equals s.Id
								where Convert.ToDateTime(sr.RequestDate).Month == month && Convert.ToDateTime(sr.RequestDate).Year == year
								group new { sr, s } by new { sr.Severity, s.SeverityLevel } into grouped
								select new { grouped.Key.SeverityLevel, grouped.Key.Severity, Count = grouped.Count() };

					var topitems = query.OrderByDescending(x => x.Count).Take(5).ToList();

					if (topitems.Any())
					{
						foreach (var item in topitems)
						{

							if (Assets.ContainsKey(item.SeverityLevel))
							{
								Assets[item.SeverityLevel] += "," + item.Count;
							}
							else
							{
								Assets.Add(item.SeverityLevel, Convert.ToString(item.Count));
							}
						}
					}
					else
					{
						if (Assets.ContainsKey("Normal"))
						{
							Assets["Normal"] += "," + 0;
						}
						else
						{
							Assets.Add("Normal", Convert.ToString(0));
						}

						if (Assets.ContainsKey("Critical"))
						{
							Assets["Critical"] += "," + 0;
						}
						else
						{
							Assets.Add("Critical", Convert.ToString(0));
						}

						if (Assets.ContainsKey("Urgent"))
						{
							Assets["Urgent"] += "," + 0;
						}
						else
						{
							Assets.Add("Urgent", Convert.ToString(0));
						}
					}
					startDate = startDate.AddMonths(1);
				}

				string json = "";

				foreach (var item in Assets)
				{

					if (json == string.Empty)
						json += "{ name: '" + item.Key.Replace(System.Environment.NewLine, string.Empty) + "', data: [" + item.Value + "] }";
					else
						json += ",{ name: '" + item.Key.Replace(System.Environment.NewLine, string.Empty) + "', data: [" + item.Value + "] }";
				}

				ViewData.Add("workorder_severity_series", json);
			}
			catch (Exception ex)
			{

			}
		}
		private void GetTotalWorkOrder_AssetGroup(DateTime startDate, DateTime endDate)
		{
			try
			{
				var tblAssetGroup = db.tblAssetGroups.ToList();
				int month = 1, year = 2018;
				IDictionary<string, string> Assets = new Dictionary<string, string>();
				List<string> Top5Asset = new List<string>();
				var tempstartdate = startDate;

				while (tempstartdate <= endDate)
				{
					month = tempstartdate.Month;
					year = tempstartdate.Year;

					var query = from sr in tblServiceRequest
								join ag in tblAssetGroup on sr.AssetGroupId equals ag.Id
								where Convert.ToDateTime(sr.RequestDate).Month == month && Convert.ToDateTime(sr.RequestDate).Year == year
								group new { sr, ag } by new { sr.AssetGroupId, ag.AssetGroup } into grouped
								select new { grouped.Key.AssetGroup, grouped.Key.AssetGroupId, Count = grouped.Count() };

					var top5items = query.OrderByDescending(x => x.Count).Take(5).ToList();

					foreach (var item in top5items)
					{

						if (!Top5Asset.Contains(item.AssetGroup))
						{
							Top5Asset.Add(item.AssetGroup);
						}
					}
					tempstartdate= tempstartdate.AddMonths(1);

					if (Top5Asset.Count() > 4)
						break;
				}

				while (startDate <= endDate)
				{
					month = startDate.Month;
					year = startDate.Year;

					var query = from sr in tblServiceRequest
								join ag in tblAssetGroup on sr.AssetGroupId equals ag.Id
								where Convert.ToDateTime(sr.RequestDate).Month == month && Convert.ToDateTime(sr.RequestDate).Year == year
								group new { sr, ag } by new { sr.AssetGroupId, ag.AssetGroup } into grouped
								select new { grouped.Key.AssetGroup, grouped.Key.AssetGroupId, Count = grouped.Count() };

					var topitems = query.OrderByDescending(x => x.Count).Take(5).ToList();


					if (topitems.Any())
					{
						foreach (var item in topitems)
						{

							if (Assets.ContainsKey(item.AssetGroup))
							{
								Assets[item.AssetGroup] += "," + item.Count;
							}
							else
							{
								Assets.Add(item.AssetGroup, Convert.ToString(item.Count));
							}
						}
					}
					else
					{
						foreach (var item1 in Top5Asset)
						{
							if (Assets.ContainsKey(item1))
							{
								Assets[item1] += "," + 0;
							}
							else
							{
								Assets.Add(item1, "0");
							}
						}
					}
					startDate = startDate.AddMonths(1);
				}

				string json = "";

				foreach (var item in Assets)
				{

					if (json == string.Empty)
						json += "{ name: '" + item.Key.Replace(System.Environment.NewLine, string.Empty) + "', data: [" + item.Value + "] }";
					else
						json += ",{ name: '" + item.Key.Replace(System.Environment.NewLine, string.Empty) + "', data: [" + item.Value + "] }";
				}
				ViewData.Add("workorder_AssetGroup_series", json);
			}
			catch (Exception ex)
			{

			}
		}

		private void GetTop5WorkOrder_AssetType(DateTime startDate, DateTime endDate)
		{
			try
			{
				var tblAssetType = db.tblAssetTypes.ToList();
				int month = 1, year = 2018;
				IDictionary<string, string> Assets = new Dictionary<string, string>();
				while (startDate <= endDate)
				{
					month = startDate.Month;
					year = startDate.Year;

					var query = from sr in tblServiceRequest
								join at in tblAssetType on sr.AssetTypeId equals at.Id
								where Convert.ToDateTime(sr.RequestDate).Month == month && Convert.ToDateTime(sr.RequestDate).Year == year
								group new { sr, at } by new { sr.AssetTypeId, at.AssetTypeName } into grouped
								select new { grouped.Key.AssetTypeId, grouped.Key.AssetTypeName, Count = grouped.Count() };

					var topitems = query.OrderByDescending(x => x.Count).Take(5).ToList();

					foreach (var item in topitems)
					{

						if (Assets.ContainsKey(item.AssetTypeName))
						{
							Assets[item.AssetTypeName] += "," + item.Count;
						}
						else
						{
							Assets.Add(item.AssetTypeName, Convert.ToString(item.Count));
						}
					}
					startDate = startDate.AddMonths(1);
				}

				string json = "";

				foreach (var item in Assets)
				{

					if (json == string.Empty)
						json += "{ name: '" + item.Key.Replace(System.Environment.NewLine, string.Empty) + "', data: [" + item.Value + "] }";
					else
						json += ",{ name: '" + item.Key.Replace(System.Environment.NewLine, string.Empty) + "', data: [" + item.Value + "] }";
				}
				ViewData.Add("workorder_AssetType_series", json);
			}
			catch (Exception ex)
			{

			}


		}

		private void GetBuildingWiseTotalAndOpenWorkOrder(DateTime startDate, DateTime endDate)
		{
			try
			{
				var queryTotal = from sr in tblServiceRequest
								 join bg in tblBuilding on sr.BuildingId equals bg.Id
								 where sr.RequestDate >= startDate && sr.RequestDate <= endDate
								 group new { bg.BuildingName }
				   by new { bg.BuildingName };

				var queryOpen = from sr in tblServiceRequest
								join bg in tblBuilding on sr.BuildingId equals bg.Id
								where sr.RequestDate >= startDate && sr.RequestDate <= endDate && sr.Status.Equals("Open", StringComparison.InvariantCultureIgnoreCase)
								group new { bg.BuildingName }
			  by new { bg.BuildingName };

				string workorder_Buildings_categories = string.Empty;
				string json = string.Empty;

				workorder_Buildings_categories = Convert.ToString(String.Join(",", tblBuilding.ToList().Select(a => "'" + a.BuildingName + "'")));
				var Buildings_categories = from bg in tblBuilding
										   select new { bg.BuildingName };
				string json_tempQ = string.Empty;
				string json_temp_Open = string.Empty;

				var tempQ = queryTotal.Select(grp => new { Count = grp.Count(), BuildingName = grp.Key.BuildingName }).ToList();
				var temp_Open = queryOpen.Select(grp => new { Count = grp.Count(), BuildingName = grp.Key.BuildingName }).ToList();
				var query_workorder_total_building = from diff1 in Buildings_categories
													 join tempQ1 in tempQ on diff1.BuildingName equals tempQ1.BuildingName into gj
													 from subpet in gj.DefaultIfEmpty()
													 select new { diff1.BuildingName, Value = subpet?.Count ?? 0 };

				var query_workorder_Open_building = from diff1 in Buildings_categories
													join temp_Open1 in temp_Open on diff1.BuildingName equals temp_Open1.BuildingName into gj
													from subpet in gj.DefaultIfEmpty()
													select new { diff1.BuildingName, Value = subpet?.Count ?? 0 };
				//workorder_Buildings_categories

				//Total WorkOrder
				json_tempQ = "{ name: 'Total WorkOrder', data: [" + Convert.ToString(String.Join(",", query_workorder_total_building.ToList().Select(a => a.Value))) + "] }";

				// Open WorkOrder
				json_temp_Open = "{ name: 'Open WorkOrder', data: [" + Convert.ToString(String.Join(",", query_workorder_Open_building.ToList().Select(a => a.Value))) + "] }";

				ViewData.Add("workorder_Buildings_categories", workorder_Buildings_categories);
				ViewData.Add("workorder_Total_Open_Building_series", json_tempQ + "," + json_temp_Open);
			}
			catch (Exception ex)
			{

			}
		}

		// GET: ServiceRequests/Details/5
		public ActionResult KPI(DateTime? startDate = null, DateTime? endDate = null)
		{
			DateTime StartDate = startDate ?? DateTime.Today.AddMonths(-6);
			DateTime EndDate = endDate ?? DateTime.Today;

			AverageResponseRectificationTime(StartDate, EndDate);
			GetResponseTime(StartDate, EndDate);
			GetRectifyTime(StartDate, EndDate);

			return View();
		}

		private void GetRectifyTime(DateTime startDate, DateTime endDate)
		{
			try
			{
				double value1, value2, value3, value4, value5, value6;

				var serviceResponseCritical_Greater5Min_series = tblServiceRequest.Where(x =>
				x.RequestDate >= startDate && x.RequestDate <= endDate.AddDays(1) && x.RectificationDateTime != null && x.Severity == 1 && (x.RectificationDateTime.Value - x.AcknowledgeDate.Value).Minutes > 30).ToList();
				value1 = serviceResponseCritical_Greater5Min_series.Any() ? serviceResponseCritical_Greater5Min_series.Average(x => (x.RectificationDateTime.Value - x.AcknowledgeDate.Value).Minutes) : 0;

				var serviceResponseCritical_5MinOrBelow_series = tblServiceRequest.Where(x =>
				x.RequestDate >= startDate && x.RequestDate <= endDate.AddDays(1) && x.RectificationDateTime != null && x.Severity == 1 && (x.RectificationDateTime.Value - x.AcknowledgeDate.Value).Minutes <= 30).ToList();

				value2 = serviceResponseCritical_5MinOrBelow_series.Any() ? serviceResponseCritical_5MinOrBelow_series.Average(x => (x.RectificationDateTime.Value - x.AcknowledgeDate.Value).Minutes) : 0;


				var serviceResponseUrgent_Greater5Min_series = tblServiceRequest.Where(x =>
				x.RequestDate >= startDate && x.RequestDate <= endDate.AddDays(1) && x.RectificationDateTime != null && x.Severity == 2 && (x.RectificationDateTime.Value - x.AcknowledgeDate.Value).Minutes > 120).ToList();
				value3 = serviceResponseUrgent_Greater5Min_series.Any() ? serviceResponseUrgent_Greater5Min_series.Average(x => (x.RectificationDateTime.Value - x.AcknowledgeDate.Value).Minutes) : 0;

				var serviceResponseUrgent_5MinOrBelow_series = tblServiceRequest.Where(x =>
				x.RequestDate >= startDate && x.RequestDate <= endDate.AddDays(1) && x.RectificationDateTime != null && x.Severity == 2 && (x.RectificationDateTime.Value - x.AcknowledgeDate.Value).Minutes <= 120).ToList();

				value4 = serviceResponseUrgent_5MinOrBelow_series.Any() ? serviceResponseUrgent_5MinOrBelow_series.Average(x => (x.RectificationDateTime.Value - x.AcknowledgeDate.Value).Minutes) : 0;


				var serviceResponseNormal_Greater5Min_series = tblServiceRequest.Where(x =>
				x.RequestDate >= startDate && x.RequestDate <= endDate.AddDays(1) && x.RectificationDateTime != null && x.Severity == 2 && (x.RectificationDateTime.Value - x.AcknowledgeDate.Value).Minutes > 240).ToList();
				value5 = serviceResponseNormal_Greater5Min_series.Any() ? serviceResponseNormal_Greater5Min_series.Average(x => (x.RectificationDateTime.Value - x.AcknowledgeDate.Value).Minutes) : 0;

				var serviceResponsenormal_5MinOrBelow_series = tblServiceRequest.Where(x =>
				x.RequestDate >= startDate && x.RequestDate <= endDate.AddDays(1) && x.RectificationDateTime != null && x.Severity == 2 && (x.RectificationDateTime.Value - x.AcknowledgeDate.Value).Minutes <= 240).ToList();

				value6 = serviceResponsenormal_5MinOrBelow_series.Any() ? serviceResponsenormal_5MinOrBelow_series.Average(x => (x.RectificationDateTime.Value - x.AcknowledgeDate.Value).Minutes) : 0;


				ViewData.Add("RectifyTime_Critical1", value1);
				ViewData.Add("RectifyTime_Critical2", value2);
				ViewData.Add("RectifyTime_Urgent1", value3);
				ViewData.Add("RectifyTime_Urgent2", value4);
				ViewData.Add("RectifyTime_Normal1", value5);
				ViewData.Add("RectifyTime_Normal2", value6);
			}
			catch (Exception ex)
			{

			}
		}

		private void GetResponseTime(DateTime startDate, DateTime endDate)
		{
			try
			{

				double value1, value2, value3, value4, value5, value6;

				var serviceResponseCritical_Greater5Min_series = tblServiceRequest.Where(x =>
				x.RequestDate >= startDate && x.RequestDate <= endDate && x.AcknowledgeDate != null && x.Severity == 1 && (x.AcknowledgeDate.Value - x.RequestDate.Value).Minutes > 5).ToList();
				value1 = serviceResponseCritical_Greater5Min_series.Any() ? serviceResponseCritical_Greater5Min_series.Average(x => (x.AcknowledgeDate.Value - x.RequestDate.Value).Minutes) : 0;

				var serviceResponseCritical_5MinOrBelow_series = tblServiceRequest.Where(x =>
				x.RequestDate >= startDate && x.RequestDate <= endDate && x.AcknowledgeDate != null && x.Severity == 1 && (x.AcknowledgeDate.Value - x.RequestDate.Value).Minutes <= 5).ToList();

				value2 = serviceResponseCritical_5MinOrBelow_series.Any() ? serviceResponseCritical_5MinOrBelow_series.Average(x => (x.AcknowledgeDate.Value - x.RequestDate.Value).Minutes) : 0;


				var serviceResponseUrgent_Greater5Min_series = tblServiceRequest.Where(x =>
				x.RequestDate >= startDate && x.RequestDate <= endDate && x.AcknowledgeDate != null && x.Severity == 2 && (x.AcknowledgeDate.Value - x.RequestDate.Value).Minutes > 10).ToList();
				value3 = serviceResponseUrgent_Greater5Min_series.Any() ? serviceResponseUrgent_Greater5Min_series.Average(x => (x.AcknowledgeDate.Value - x.RequestDate.Value).Minutes) : 0;

				var serviceResponseUrgent_5MinOrBelow_series = tblServiceRequest.Where(x =>
				x.RequestDate >= startDate && x.RequestDate <= endDate && x.AcknowledgeDate != null && x.Severity == 2 && (x.AcknowledgeDate.Value - x.RequestDate.Value).Minutes <= 10).ToList();

				value4 = serviceResponseUrgent_5MinOrBelow_series.Any() ? serviceResponseUrgent_5MinOrBelow_series.Average(x => (x.AcknowledgeDate.Value - x.RequestDate.Value).Minutes) : 0;


				var serviceResponseNormal_Greater5Min_series = tblServiceRequest.Where(x =>
				x.RequestDate >= startDate && x.RequestDate <= endDate && x.AcknowledgeDate != null && x.Severity == 2 && (x.AcknowledgeDate.Value - x.RequestDate.Value).Minutes > 15).ToList();
				value5 = serviceResponseNormal_Greater5Min_series.Any() ? serviceResponseNormal_Greater5Min_series.Average(x => (x.AcknowledgeDate.Value - x.RequestDate.Value).Minutes) : 0;

				var serviceResponsenormal_5MinOrBelow_series = tblServiceRequest.Where(x =>
				x.RequestDate >= startDate && x.RequestDate <= endDate && x.AcknowledgeDate != null && x.Severity == 2 && (x.AcknowledgeDate.Value - x.RequestDate.Value).Minutes <= 15).ToList();

				value6 = serviceResponsenormal_5MinOrBelow_series.Any() ? serviceResponsenormal_5MinOrBelow_series.Average(x => (x.AcknowledgeDate.Value - x.RequestDate.Value).Minutes) : 0;


				ViewData.Add("ResponseTime_Critical1", value1);
				ViewData.Add("ResponseTime_Critical2", value2);
				ViewData.Add("ResponseTime_Urgent1", value3);
				ViewData.Add("ResponseTime_Urgent2", value4);
				ViewData.Add("ResponseTime_Normal1", value5);
				ViewData.Add("ResponseTime_Normal2", value6);
			}
			catch (Exception ex)
			{

			}
		}

		private void AverageResponseRectificationTime(DateTime startDate, DateTime endDate)
		{
			try
			{
				var diff = Enumerable.Range(0, Int32.MaxValue)
	 .Select(e => startDate.AddMonths(e))
	 .TakeWhile(e => e <= endDate)
	 .Select(e => new { MonthName = e.ToString("MMM"), Month = e.Month, Year = e.Year }).ToList();

				var serviceResponse = tblServiceRequest.Where(y => y.AcknowledgeDate >= startDate && y.AcknowledgeDate <= endDate)
	.Select(grp => new { grp.Id, responseDuration = ((TimeSpan)(grp.AcknowledgeDate - grp.RequestDate)).TotalMinutes, grp.RequestDate, grp.AcknowledgeDate, grp.Severity }).ToList();

				var serviceResponseCritical = serviceResponse.Where(y => y.Severity == 1)
	.GroupBy(x1 => new { Convert.ToDateTime(x1.AcknowledgeDate).Month, Convert.ToDateTime(x1.AcknowledgeDate).Year })
	.Select(x1 => new { avgresponseDuration = x1.Average(i => i.responseDuration), x1.Key.Month, x1.Key.Year }).ToList();

				var serviceResponseUrgent = serviceResponse.Where(y => y.Severity == 2)
	.GroupBy(x1 => new { Convert.ToDateTime(x1.AcknowledgeDate).Month, Convert.ToDateTime(x1.AcknowledgeDate).Year })
	.Select(x1 => new { avgresponseDuration = x1.Average(i => i.responseDuration), x1.Key.Month, x1.Key.Year }).ToList();

				var serviceResponseNormal = serviceResponse.Where(y => y.Severity == 3)
.GroupBy(x1 => new { Convert.ToDateTime(x1.AcknowledgeDate).Month, Convert.ToDateTime(x1.AcknowledgeDate).Year })
.Select(x1 => new { avgresponseDuration = x1.Average(i => i.responseDuration), x1.Key.Month, x1.Key.Year }).ToList();

				var serviceRectify = tblServiceRequest.Where(y => y.RectificationDateTime >= startDate && y.RectificationDateTime <= endDate)
.Select(grp => new { grp.Id, responseDuration = ((TimeSpan)(grp.RectificationDateTime - grp.AcknowledgeDate)).TotalMinutes, grp.RequestDate, grp.RectificationDateTime, grp.Severity }).ToList();

				var serviceRectifyCritical = serviceRectify.Where(y => y.Severity == 1)
	.GroupBy(x1 => new { Convert.ToDateTime(x1.RectificationDateTime).Month, Convert.ToDateTime(x1.RectificationDateTime).Year })
	.Select(x1 => new { avgresponseDuration = x1.Average(i => i.responseDuration), x1.Key.Month, x1.Key.Year }).ToList();

				var serviceRectifyUrgent = serviceRectify.Where(y => y.Severity == 2)
	.GroupBy(x1 => new { Convert.ToDateTime(x1.RectificationDateTime).Month, Convert.ToDateTime(x1.RectificationDateTime).Year })
	.Select(x1 => new { avgresponseDuration = x1.Average(i => i.responseDuration), x1.Key.Month, x1.Key.Year }).ToList();

				var serviceRectifyNormal = serviceRectify.Where(y => y.Severity == 3)
.GroupBy(x1 => new { Convert.ToDateTime(x1.RectificationDateTime).Month, Convert.ToDateTime(x1.RectificationDateTime).Year })
.Select(x1 => new { avgresponseDuration = x1.Average(i => i.responseDuration), x1.Key.Month, x1.Key.Year }).ToList();


				var query_serviceResponseCritical = from diff1 in diff
													join workorderTotal1 in serviceResponseCritical on new { Month = diff1.Month, Year = diff1.Year } equals new
													{
														Month = workorderTotal1.Month,
														Year = workorderTotal1.Year
													} into gj
													from fgi in gj.Where(f => f.Year == diff1.Year && f.Month == diff1.Month).DefaultIfEmpty()
													from subpet in gj.DefaultIfEmpty()
													select new { diff1.Month, diff1.Year, Value = subpet?.avgresponseDuration ?? 0 };

				var query_serviceResponseUrgent = from diff1 in diff
												  join workorderTotal1 in serviceResponseUrgent on new { Month = diff1.Month, Year = diff1.Year } equals new
												  {
													  Month = workorderTotal1.Month,
													  Year = workorderTotal1.Year
												  } into gj
												  from fgi in gj.Where(f => f.Year == diff1.Year && f.Month == diff1.Month).DefaultIfEmpty()
												  from subpet in gj.DefaultIfEmpty()
												  select new { diff1.Month, diff1.Year, Value = subpet?.avgresponseDuration ?? 0 };

				var query_serviceResponseNormal = from diff1 in diff
												  join workorderTotal1 in serviceResponseNormal on new { Month = diff1.Month, Year = diff1.Year } equals new
												  {
													  Month = workorderTotal1.Month,
													  Year = workorderTotal1.Year
												  } into gj
												  from fgi in gj.Where(f => f.Year == diff1.Year && f.Month == diff1.Month).DefaultIfEmpty()
												  from subpet in gj.DefaultIfEmpty()
												  select new { diff1.Month, diff1.Year, Value = subpet?.avgresponseDuration ?? 0 };

				var query_serviceRectifyCritical = from diff1 in diff
												   join workorderTotal1 in serviceRectifyCritical on new { Month = diff1.Month, Year = diff1.Year } equals new
												   {
													   Month = workorderTotal1.Month,
													   Year = workorderTotal1.Year
												   } into gj
												   from fgi in gj.Where(f => f.Year == diff1.Year && f.Month == diff1.Month).DefaultIfEmpty()
												   from subpet in gj.DefaultIfEmpty()
												   select new { diff1.Month, diff1.Year, Value = subpet?.avgresponseDuration ?? 0 };

				var query_serviceRectifyUrgent = from diff1 in diff
												 join workorderTotal1 in serviceResponseUrgent on new { Month = diff1.Month, Year = diff1.Year } equals new
												 {
													 Month = workorderTotal1.Month,
													 Year = workorderTotal1.Year
												 } into gj
												 from fgi in gj.Where(f => f.Year == diff1.Year && f.Month == diff1.Month).DefaultIfEmpty()
												 from subpet in gj.DefaultIfEmpty()
												 select new { diff1.Month, diff1.Year, Value = subpet?.avgresponseDuration ?? 0 };

				var query_serviceRectifyNormal = from diff1 in diff
												 join workorderTotal1 in serviceResponseNormal on new { Month = diff1.Month, Year = diff1.Year } equals new
												 {
													 Month = workorderTotal1.Month,
													 Year = workorderTotal1.Year
												 } into gj
												 from fgi in gj.Where(f => f.Year == diff1.Year && f.Month == diff1.Month).DefaultIfEmpty()
												 from subpet in gj.DefaultIfEmpty()
												 select new { diff1.Month, diff1.Year, Value = subpet?.avgresponseDuration ?? 0 };

				string serviceResponseCritical_series = "data: [" + Convert.ToString(String.Join(",", query_serviceResponseCritical.ToList().Select(a => a.Value))) + "]";
				string serviceResponseUrgent_series = "data: [" + Convert.ToString(String.Join(",", query_serviceResponseUrgent.ToList().Select(a => a.Value))) + "] ";
				string serviceResponseNormal_series = " data: [" + Convert.ToString(String.Join(",", query_serviceResponseNormal.ToList().Select(a => a.Value))) + "] ";

				string serviceRectifyCritical_series = " data: [" + Convert.ToString(String.Join(",", query_serviceRectifyCritical.ToList().Select(a => a.Value))) + "] ";
				string serviceRectifyUrgent_series = " data: [" + Convert.ToString(String.Join(",", query_serviceRectifyUrgent.ToList().Select(a => a.Value))) + "] ";
				string serviceRectifyNormal_series = " data: [" + Convert.ToString(String.Join(",", query_serviceRectifyNormal.ToList().Select(a => a.Value))) + "] ";
				ViewData.Add("AverageResponseRectificationTime", "{ name: 'Response Critical',type: 'column'," + serviceResponseCritical_series + "},{ name: 'Response Urgent',type: 'column'," + serviceResponseUrgent_series + "},{ name: 'Rectify Critical', type: 'spline',yAxis: 1," + serviceRectifyCritical_series + "},{ name: 'Rectify Urgent', type: 'spline',yAxis: 1," + serviceRectifyUrgent_series + "}");
				ViewData.Add("AverageResponseTime", "{ name: 'Critical'," + serviceResponseCritical_series + "},{ name: 'Urgent'," + serviceResponseUrgent_series + "},{ name: 'Normal'," + serviceResponseNormal_series + "}");
				ViewData.Add("AverageRectifyTime", "{ name: 'Critical'," + serviceRectifyCritical_series + "},{ name: 'Urgent'," + serviceRectifyUrgent_series + "},{ name: 'Normal'," + serviceRectifyNormal_series + "}");
				ViewData.Add("workorder_categories", Convert.ToString(String.Join(",", diff.ToList().Select(a => "'" + a.MonthName + " " + a.Year + "'"))));
			}
			catch (Exception ex)
			{

			}
		}
	}
}
