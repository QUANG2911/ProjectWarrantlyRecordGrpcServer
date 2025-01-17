
using MimeKit;
using MimeKit.Tnef;
using ProjectWarrantlyRecordGrpcServer.Protos;
using System.Text;

namespace ProjectWarrantlyRecordGrpcServer.MessageContext
{
    public class EmailMessage
    {
        //private string signCompany = "";
        private string companyName = "Cty TNHH A";
        private string companyAdrress = "123 Nguyễn Thị Định";
        private string bankName = "Mb bank";
        private string bankAccount = "098xx8900x";
        private string bankOwner = "Cty TNHH A";
        private string hotline = "09xx90xx12";
        private string emailSupport = "minhquang12@gmail.com";

        public EmailMessage() { }
        public BodyBuilder PrintRepairRegistrationConfirmation(string customerName, int idTask, int idWarrantRecord, string reasonBringFix)
        {
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                          <!DOCTYPE html>
                           <html>
                           <head>
                               <style>
                                .vertical-header-table {{
                                      width: 50%; /* Tùy chỉnh kích thước bảng */
                                      border-collapse: collapse;
                                      margin: 20px 0;
                                      font-size: 16px;
                                      text-align: left;
                                  }}

                                  .vertical-header-table th, 
                                  .vertical-header-table td {{
                                      border: 1px solid #ddd;
                                      padding: 10px;
                                  }}

                                  .vertical-header-table th {{
                                      background-color: #f4f4f4; /* Màu nền cho header */
                                      font-weight: bold;
                                      text-align: left; /* Căn lề trái cho tiêu đề */
                                      width: 40%; /* Cố định chiều rộng cho cột tiêu đề */
                                  }}
                               </style>
                           </head>
                           <body>
                               <p>Kính gửi {customerName},</p>
                               <p>Cảm ơn quý khách đã tin tưởng và sử dụng dịch vụ sửa chữa của chúng tôi.</p>
                               <p>Chúng tôi xin thông báo rằng phiếu sửa chữa của quý khách đã được đăng ký thành công. Đội ngũ của chúng tôi sẽ liên hệ lại với quý khách trong thời gian sớm nhất để xác nhận thông tin chi tiết và tiến hành sửa chữa.</p>
                               <p>Thông tin phiếu sửa chữa:</p>    
                               <table class='vertical-header-table'>
                                 <tbody>
                                   <tr>
                                     <th>Title</th>
                                     <td>Nội dung</td>
                                   </tr>
                                   <tr>
                                     <th>Mã phiếu sửa chữa</th>
                                     <td>{idTask}</td>
                                   </tr>
                                   <tr>
                                     <th>Ngày đăng ký</th>
                                     <td>{DateTime.Now.ToString("dd/MM/yyyy")} ,giờ {DateTime.Now.ToString("HH:mm")}</td>
                                   </tr>
                                   <tr>
                                     <th>Nguyên nhân sửa</th>
                                     <td>{reasonBringFix}</td>
                                   </tr>
                                 </tbody>
                               </table>
                               <p>Nếu quý khách có bất kỳ thắc mắc nào, vui lòng liên hệ với chúng tôi qua:</p>
                                   <li>Số điện thoại: {hotline}</li>
                                   <li>Email: {emailSupport}</li>
                               <p>Chúng tôi rất hân hạnh được phục vụ quý khách</p>  
                               <p>Trân trọng</p> 
                               <p>[Chữ ký công ty]</p>
                               <p>{companyName}</p>
                               <p>{companyAdrress}</p>
  
                           </body>
                        </html>";
            return bodyBuilder;
        }

        public BodyBuilder PrintRepairReceiptMessage(string customerName, int idTask)
        {
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <style>
                               
                            </style>
                        </head>
                        <body>
                            <p>Kính gửi {customerName},</p>
                            <p>Chúng tôi đã nhận được phiếu sửa chữa mà quý khách vừa đăng ký. Chúng tôi xin thông báo rằng phiếu sửa chữa có mã {idTask} của quý khách đã được nhân viên kỹ thuật tiếp nhận và xử lý.</p>
                            <p>Nhân viên kỹ thuật sẽ liên hệ với quý khách trong thời gian sớm nhất để xác nhận thông tin chi tiết và thống nhất về các bước sửa chữa tiếp theo.</p>
                            <p>Nếu quý khách có bất kỳ câu hỏi hoặc cần hỗ trợ thêm, vui lòng liên hệ với chúng tôi qua</p>                             
                                <li>Số điện thoại: {hotline}</li>
                                <li>Email: {emailSupport}</li>
                            <p>Chúng tôi rất hân hạnh được phục vụ quý khách</p>  
                            <p>Trân trọng</p> 
                            <p>[Chữ ký công ty]</p>
                            <p>{companyName}</p>
                            <p>{companyAdrress}</p>
                       
                        </body>
                     </html>";
            return bodyBuilder;
        }

        public BodyBuilder PrintBillMessage(string customerName, int idTask, int idWarrantRecord, string dateBill, int totalBill, UpdateRepairManagementRequest listRepairParts)
        {
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
            //test
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
                            <p>Số tài khoản: {bankAccount}</p>
                            <p>Ngân hàng: [{bankName}]</p>
                            <p>Chủ tài khoản: {bankOwner}</p>
                            <p>Nội dung chuyển khoản: [Mã phiếu sửa chữa] - [Tên khách hàng]</p>
                            <p>Cảm ơn quý khách đã sử dụng dịch vụ của chúng tôi!</p>
                            <p>Trân trọng,<br>{companyName}</p>                        
                        </body>
                       </html>";
            return bodyBuilder;
        }

        public BodyBuilder PrintRejectRepairMessage(string customerName, int idTask, int idWarrantRecord)
        {
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <style>
                                .vertical-header-table {{
                                      width: 50%; /* Tùy chỉnh kích thước bảng */
                                      border-collapse: collapse;
                                      margin: 20px 0;
                                      font-size: 16px;
                                      text-align: left;
                                  }}

                                  .vertical-header-table th, 
                                  .vertical-header-table td {{
                                      border: 1px solid #ddd;
                                      padding: 10px;
                                  }}

                                  .vertical-header-table th {{
                                      background-color: #f4f4f4; /* Màu nền cho header */
                                      font-weight: bold;
                                      text-align: left; /* Căn lề trái cho tiêu đề */
                                      width: 40%; /* Cố định chiều rộng cho cột tiêu đề */
                                  }}
                            </style>
                        </head>
                        <body>
                            <p>Kính gửi {customerName},</p>
                            <p>Chúng tôi rất tiếc phải thông báo rằng đơn sửa chữa và bảo hành của quý khách với thông tin như sau đã bị hủy bỏ:</p>
                            <table class='vertical-header-table'>
                                 <tbody>
                                   <tr>
                                     <th>Title</th>
                                     <td>Nội dung</td>
                                   </tr>
                                   <tr>
                                     <th>Mã phiếu sửa chữa</th>
                                     <td>{idTask}</td>
                                   </tr>
                                   <tr>
                                     <th>Lý do hủy bỏ</th>
                                     <td>Khách hàng từ chối sửa chữa</td>
                                   </tr>
                                 </tbody>
                               </table>
                            <p>Nếu quý khách cần thêm bất kỳ hỗ trợ nào hoặc muốn đăng ký lại dịch vụ sửa chữa, vui lòng liên hệ với chúng tôi qua:</p>                                
                                <li>Số điện thoại: {hotline}</li>
                                <li>Email: {emailSupport}</li>
                            <p>Chúng tôi rất hân hạnh được phục vụ quý khách</p>  
                            <p>Trân trọng</p> 
                            <p>[Chữ ký công ty]</p>
                            <p>{companyName}</p>
                            <p>{companyAdrress}</p>
                       
                        </body>
                     </html>";
            return bodyBuilder;
        }
    }
}
