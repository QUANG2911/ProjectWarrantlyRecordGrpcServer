using MimeKit;
using ProjectWarrantlyRecordGrpcServer.Interface;
using MailKit.Net.Smtp;
using ProjectWarrantlyRecordGrpcServer.MessageContext;
using Npgsql.Replication.PgOutput.Messages;
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;
using System.Text;


namespace ProjectWarrantlyRecordGrpcServer.Services.Logic
{
    public class EmailSevice : IMailSevice
    {
        public string SendNotification(string customerName, int idTask, int idWarrantyRecord, string TypeMessage, string ReasonBringFix, string staffName, string dateBill, int totalBill)
        {
            string messageBody = "";

            EmailMessage emailMessage = new EmailMessage ();
            if(TypeMessage == "RegistrationTask")
            {
                messageBody = emailMessage.PrintRepairRegistrationConfirmation(customerName, idTask, idWarrantyRecord,ReasonBringFix);
            }    
            else if (TypeMessage == "ReceiptTask")
            {
                messageBody = emailMessage.PrintRepairReceiptMessage(customerName, idTask, staffName);
            }    
            else if (TypeMessage == "Bill")
            {
                messageBody = emailMessage.PrintBillMessage(customerName, idTask, idWarrantyRecord, dateBill, totalBill);
            }
            else if(TypeMessage == "RejectTask")
            {
                messageBody = emailMessage.PrintRejectRepairMessage(customerName, idTask, idWarrantyRecord);
            }    

            return messageBody;
        }

        public string SendEmailAsync(string customerName, int idTask, int idWarrantyRecord, string email, string subject, string TypeMessage, string ReasonBringFix, string staffName, string dateBill, int totalBill)
        {
            string result = "";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("QuanLy", "buiminhquangquang80@gmail.com"));
            message.To.Add(new MailboxAddress(customerName, email));
            message.Subject = subject;

            var messageBody = SendNotification(customerName,idTask,idWarrantyRecord,TypeMessage, ReasonBringFix, staffName,dateBill,totalBill);
            message.Body = new TextPart("plain")
            {
                Text = messageBody
            };

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
           
            return result;
        }

        public string SendEmailWithTable(string customerName,string subject, string email, int idTask, int idWarrantRecord, string dateBill, int totalBill, UpdateRepairManagementRequest listRepairParts)
        {
            string result = "";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("QuanLy", "buiminhquangquang80@gmail.com"));
            message.To.Add(new MailboxAddress(customerName, email));
            message.Subject = subject;

            var tableRowsBuilder = new StringBuilder();
            foreach (var item in listRepairParts.ToListUpdateRepairPart)
            {
                tableRowsBuilder.Append($@"
                <tr>
                    <td>{item.IdRepairPart}</td>
                    <td>{item.RepairPartName}</td>
                    <td>{item.Amount}</td>
                    <td>{item.Price}</td>
                </tr>");
            }

            string formattedTotalBill = totalBill.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));


            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <style>
                                table {{
                                    border-collapse: collapse;
                                    width: 80%;
                                }}
                                th, td {{
                                    border: 1px solid #ddd;
                                    padding: 8px;
                                    text-align: left;
                                }}
                                th {{
                                    background-color: #f2f2f2;
                                }}
                                .total-cost {{
                                    font-weight: bolder;
                                    color: #d9534f; /* Thêm màu đỏ để nổi bật (tuỳ chọn) */
                                }}
                            </style>
                        </head>
                        <body>
                            <p>Kính gửi {customerName},</p>
                            <p>Chúng tôi trân trọng thông báo rằng quá trình sửa chữa và bảo hành cho phiếu số {idWarrantRecord} của quý khách đã hoàn tất. Dưới đây là thông tin chi tiết hóa đơn:</p>
                            <p>Mã phiếu sửa chữa: {idTask} </p>
                            <p>Ngày hoàn thành: {dateBill} </p>
                            <p>Thông tin chi tiết về phiếu sửa chữa của quý khách như sau:</p>
                            <table>
                                <thead>
                                    <tr>
                                        <th>Mã phiếu linh kiện</th>
                                        <th>Tên linh kiện</th>
                                        <th>Giá tiền</th>
                                        <th>Số lượng</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {tableRowsBuilder} <!-- Dùng tableRowsBuilder đã được xử lý ở trên -->
                                </tbody>
                            </table>
                            <p class='total-cost'>Tổng chi phí: {formattedTotalBill}</p>
                            <p>Thông tin thanh toán chuyển khoản:</p>
                            <p>Số tài khoản: 098xx8900x</p>
                            <p>Ngân hàng: [Mb bank]</p>
                            <p>Chủ tài khoản: Cty TNHH A</p>
                            <p>Nội dung chuyển khoản: [Mã phiếu sửa chữa] - [Tên khách hàng]</p>
                            <p>Cảm ơn quý khách đã sử dụng dịch vụ của chúng tôi!</p>
                            <p>Trân trọng,<br>Cty TNHH A</p>                        
                        </body>
                       </html>";

            message.Body = bodyBuilder.ToMessageBody();

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


            return result;
        }
    }
}
