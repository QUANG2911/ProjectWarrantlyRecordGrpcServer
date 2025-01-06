using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWarrantlyRecordGrpcServer.Model
{
    public class WarrantyRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdWarrantRecord { get; set; }
        public int IdDevice { get; set; }
        public required int IdCustomer { get; set; }
        public  DateOnly DateOfResig { get; set; }
        public DateOnly TimeEnd { get; set; }
        public int status { get; set; }

        [ForeignKey("IdCustomer")]
        public Customer? Customer { get; set; }

        [ForeignKey("IdDevice")]
        public CustomerDevices? customerDevice  { get; set; }

        public ICollection<StaffTask>? staffTasks { get; set; }
    }
}
