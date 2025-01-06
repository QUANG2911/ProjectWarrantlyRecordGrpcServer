using Google.Type;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectWarrantlyRecordGrpcServer.Model
{
    public class RepairPart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRepairPart { get; set; }
        public required string RepairPartName { get; set; }
        public int Price { get; set; }
        public DateOnly DateOfManuFacture { get; set; }
        public ICollection<RepairDetail>? repairDetails { get; set; }
    }
}
