using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectWarrantlyRecordGrpcServer.Model
{
    public class Staff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdStaff { get; set; }

        [MaxLength(30)]
        public required string StaffName { get; set; }
        [MaxLength(10)]
        public required string StaffPhone { get; set; }
        [MaxLength(30)]
        public required string StaffPosition { get; set; }
        [MaxLength(10)]
        public required string Pass { get; set; }
        public int Status { get; set; }

        public ICollection<StaffTask>? staffTasks { get; set; }
    }
}
