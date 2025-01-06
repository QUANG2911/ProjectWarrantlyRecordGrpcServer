using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Google.Type;

namespace ProjectWarrantlyRecordGrpcServer.Model
{
    public class StaffTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTask { get; set; }
        public int? IdStaff { get; set; }
        public int IdWarantyRecord { get; set; }
        public DateOnly DateOfTask { get; set; }
        public int StatusTask { get; set; }
        public required string ReasonBringFix { get; set; }

        [ForeignKey("IdWarantyRecord")]
        public WarrantyRecord? warrantyRecord { get; set; }
        [ForeignKey("IdStaff")]
        public Staff? staff { get; set; }

        public ICollection<Bill>? bills { get; set; }

        public ICollection<RepairDetail>? repairDetails { get; set; }

    }
}
