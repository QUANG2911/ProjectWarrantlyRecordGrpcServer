using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.MessageContext
{
    public class NotificationParameters
    {
        public required string CustomerName { get; set; }
        public required string CustomerEmail { get; set; }
        public required string subject { get; set; }
        public int IdTask { get; set; } = 0;
        public int IdWarrantyRecord { get; set; } = 0;
        public string? TypeMessage { get; set; }
        public string? ReasonBringFix { get; set; }
        public string? StaffName { get; set; }
        public string? DateBill { get; set; }
        public int TotalBill { get; set; } = 0;
        public UpdateRepairManagementRequest? listRepairParts {  get; set; }

        public NotificationParameters()
        {
            
        }
    }
}
