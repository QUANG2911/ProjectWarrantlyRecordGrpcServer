using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface IMailSevice
    {
        string SendEmailAsync(string customerName, int idTask, int idWarrantyRecord, string email, string subject, string TypeMessage, string ReasonBringFix, string staffName, string dateBill, int totalBill);

        string SendEmailWithTable(string customerName, string subject, string email, int idTask, int idWarrantRecord, string dateBill, int totalBill, UpdateRepairManagementRequest listRepairParts);
    }
}
