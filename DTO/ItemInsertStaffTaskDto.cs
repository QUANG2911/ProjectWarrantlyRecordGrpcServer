namespace ProjectWarrantlyRecordGrpcServer.DTO
{
    public class ItemInsertStaffTaskDto
    {
        public required string CustomerName { get; set; }
        public required string CustomerEmail { get; set; }
        public required string CustomerPhone { get; set; }
        public required string DeviceName  { get; set; }
        public int IdWarrantRecord { get; set; }
        public required string ReasonBringFix { get; set; }
    }
}
