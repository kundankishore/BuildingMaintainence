
namespace EFIRM.Models
{
	public class FeaturesViewModels
	{
		public bool ServiceRequest { get; set; }
		public bool WorkOrderAssigning { get; set; }
		public bool DigitalSignatureForTechnician { get; set; }
		public bool DigitalSignatureForTenant { get; set; }
		public bool AcknowledgeAndRectify { get; set; }
		public bool EsclationNotification { get; set; }
	}
}