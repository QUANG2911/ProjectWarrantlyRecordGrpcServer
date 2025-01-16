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
        public WarrantyRecord? WarrantyRecord { get; set; }
        [ForeignKey("IdStaff")]
        public Staff? Staff { get; set; }

        public ICollection<Bill>? Bills { get; set; }

        public ICollection<RepairDetail>? RepairDetails { get; set; }

    }
}
