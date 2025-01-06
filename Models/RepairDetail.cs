using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWarrantlyRecordGrpcServer.Model
{
    [PrimaryKey(nameof(IdRepairPart), nameof(IdTask))]
    public class RepairDetail
    {
        public int IdRepairPart { get; set; }
        public int IdTask { get; set; }

        public int Amount { get; set; }

        [ForeignKey("IdTask")]
        public StaffTask? StaffTask { get; set; }

        [ForeignKey("IdRepairPart")]
        public RepairPart? RepairPart { get; set; }
    }
}
