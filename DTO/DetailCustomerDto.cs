using System;

namespace ProjectWarrantlyRecordGrpcServer.DTO
{
    public class DetailCustomerDto
    {
        public int IdCusomer {  get; set; }
        public required string CustomerName { get; set; }
        public required string CustomerEmail { get; set; }
        public required string CustomerPhone { get; set; }
        public required string CustomerAdrress { get; set; }
        public required string CustomerDevice { get; set; }
        public int IdWarrantReport { get; set; }
        public DateOnly DateOfWarrant { get; set; }
        public DateOnly TimeEnd { get; set; }
    }
}
