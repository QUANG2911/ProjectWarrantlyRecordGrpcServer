using Google.Type;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWarrantlyRecordGrpcServer.Model
{
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdBill { get; set; }
        public DateOnly DateCreateBill { get; set; }
        public required int TotalAmount { get; set; }
        public required int StatusBill { get; set; }

        public int? IdTask { get; set; }

        [ForeignKey("IdTask")]
        public StaffTask? StaffTask { get; set; }
    }
}
