﻿using MimeKit;
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
        public async Task<MimeEntity> SendNotification(NotificationParameters notificationParameters)
        {
            EmailMessage emailMessage = new EmailMessage();

            if (notificationParameters.TypeMessage == "Bill")
            {
                if (notificationParameters.DateBill == null || notificationParameters.listRepairParts == null)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Không tìm được tên thông tin sửa chữa"));
                }
                return await Task.FromResult(emailMessage.PrintBillMessage(notificationParameters.CustomerName, notificationParameters.IdTask, notificationParameters.IdWarrantyRecord, notificationParameters.DateBill, notificationParameters.TotalBill, notificationParameters.listRepairParts).ToMessageBody());
            }
            else if (notificationParameters.TypeMessage == "RegistrationTask")
            {
                if(notificationParameters.ReasonBringFix == null)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Không tìm được tên thông tin sửa chữa"));
                }    
                return await Task.FromResult(emailMessage.PrintRepairRegistrationConfirmation(notificationParameters.CustomerName, notificationParameters.IdTask, notificationParameters.IdWarrantyRecord, notificationParameters.ReasonBringFix).ToMessageBody());
            }
            else if (notificationParameters.TypeMessage == "ReceiptTask")
            {
                return await Task.FromResult(emailMessage.PrintRepairReceiptMessage(notificationParameters.CustomerName, notificationParameters.IdTask).ToMessageBody());
            }
            else if (notificationParameters.TypeMessage == "RejectTask")
            {
                return await Task.FromResult(emailMessage.PrintRejectRepairMessage(notificationParameters.CustomerName, notificationParameters.IdTask, notificationParameters.IdWarrantyRecord).ToMessageBody());
            }
            else
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Hết nội dùng mail sẵn có"));
            }
        }

        public async Task<string> SendEmailAsync(NotificationParameters notificationParameters)
        {
            string result = "";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("QuanLy", "buiminhquangquang80@gmail.com"));
            // message.To.Add(new MailboxAddress(notificationParameters.CustomerName, notificationParameters.CustomerEmail));
            message.To.Add(new MailboxAddress(notificationParameters.CustomerName, "buiminhquangquang8@gmail.com"));
            message.Subject = notificationParameters.subject;

            message.Body = await SendNotification(notificationParameters);

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
