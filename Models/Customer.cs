using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectWarrantlyRecordGrpcServer.Model
{
    [Index(nameof(CustomerEmail), IsUnique = true)]

    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCustomer { get; set; }
        [MaxLength(30)]
        public required string CustomerName { get; set; }
        [MaxLength(30)]
        public required string CustomerAddress { get; set; }
        [MaxLength(30)]
        public required string CustomerEmail { get; set; }
        [MaxLength(10)]
        public required string CustomerPhone { get; set; }

        public ICollection<WarrantyRecord>? WarrantyRecords { get; set; }
    }
}
