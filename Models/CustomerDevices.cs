using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectWarrantlyRecordGrpcServer.Model
{
    public class CustomerDevices
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDevice { get; set; }
        [MaxLength(30)]
        public required string DeviceName { get; set; }
        public ICollection<WarrantyRecord>? warrantyRecords { get; set; }
    }
}
