using MimeKit;
using ProjectWarrantlyRecordGrpcServer.Interface;
using MailKit.Net.Smtp;
using ProjectWarrantlyRecordGrpcServer.MessageContext;
using Npgsql.Replication.PgOutput.Messages;
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;


namespace ProjectWarrantlyRecordGrpcServer.Services.Logic
{
    public class EmailSevice : IMailSevice
    {
        public MimeEntity SendNotification(NotificationParameters notificationParameters)
        {
            EmailMessage emailMessage = new EmailMessage();

            if (notificationParameters.TypeMessage == "Bill")
            {
                return emailMessage.PrintBillMessage(notificationParameters.CustomerName, notificationParameters.IdTask, notificationParameters.IdWarrantyRecord, notificationParameters.DateBill, notificationParameters.TotalBill, notificationParameters.listRepairParts).ToMessageBody();
            }
            else if (notificationParameters.TypeMessage == "RegistrationTask")
            {
                return emailMessage.PrintRepairRegistrationConfirmation(notificationParameters.CustomerName, notificationParameters.IdTask, notificationParameters.IdWarrantyRecord, notificationParameters.ReasonBringFix).ToMessageBody();
            }
            else if (notificationParameters.TypeMessage == "ReceiptTask")
            {
                return emailMessage.PrintRepairReceiptMessage(notificationParameters.CustomerName, notificationParameters.IdTask, notificationParameters.StaffName).ToMessageBody();
            }
            else if (notificationParameters.TypeMessage == "RejectTask")
            {
                return emailMessage.PrintRejectRepairMessage(notificationParameters.CustomerName, notificationParameters.IdTask, notificationParameters.IdWarrantyRecord).ToMessageBody();
            }
            else
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Hết nội dùng mail sẵn có"));
            }
        }

        public async Task<string> SendEmailAsync(NotificationParameters notificationParameters,int TypeTable)
        {
            string result = "";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("QuanLy", "buiminhquangquang80@gmail.com"));
            // message.To.Add(new MailboxAddress(notificationParameters.CustomerName, notificationParameters.CustomerEmail));
            message.To.Add(new MailboxAddress(notificationParameters.CustomerName, "buiminhquangquang8@gmail.com"));
            message.Subject = notificationParameters.subject;

            message.Body = SendNotification(notificationParameters);

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate("buiminhquangquang80@gmail.com", "venz evnl kuhb lcla");///app password

                    client.Send(message);
                    client.Disconnect(true);
                }
                result = "done";
            }
            catch 
            {
                result = "fail";                
            }
           
            return await Task.FromResult(result);
        }
    }
}
