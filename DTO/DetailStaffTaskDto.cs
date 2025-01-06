using Google.Type;
using System;

namespace ProjectWarrantlyRecordGrpcServer.DTO
{
    public class DetailStaffTaskDto
    {
        public int IdTask {  get; set; }
        public required string CustomerName {  get; set; }
        public required string CustomerPhone { get; set; }
        public required string ReasonBringFix { get; set; }
        public required int StatusTask { get; set; }
    }
}
