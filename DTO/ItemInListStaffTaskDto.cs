using System;

namespace ProjectWarrantlyRecordGrpcServer.DTO
{
    public class ItemInListStaffTaskDto
    {
        public int IdTask { get; set; }
        public required string CustomerName { get; set; }
        public required string CustomerPhone { get; set; }
        public DateOnly DateOfWarranty { get; set; }

        public DateOnly DateOfTask { get; set; }
        public int IdWarrantyRecord { get; set; }
        public int StatusTask { get; set; }
    }
}
