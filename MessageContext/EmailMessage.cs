
using MimeKit.Tnef;

namespace ProjectWarrantlyRecordGrpcServer.MessageContext
{
    public class EmailMessage
    {
        private string signCompany = "";
        private string companyName = "Cty TNHH A";
        private string companyAdrress = "123 Nguyễn Thị Định";
        private string bankName = "Mb bank";
        private string bankAccount = "098xx8900x";
        private string bankOwner = "Cty TNHH A";
        private string hotline = "09xx90xx12";
        private string emailSupport = "minhquang12@gmail.com";

        public EmailMessage() { }
        public string PrintRepairRegistrationConfirmation(string customerName, int idTask, int idWarrantRecord, string reasonBringFix)
        {
            string message = "Kính gửi " + customerName + "," +
                             "\r\nCảm ơn quý khách đã tin tưởng và sử dụng dịch vụ sửa chữa của chúng tôi." +
                             "\r\nChúng tôi xin thông báo rằng phiếu sửa chữa của quý khách đã được đăng ký thành công. Đội ngũ của chúng tôi sẽ liên hệ lại với quý khách trong thời gian sớm nhất để xác nhận thông tin chi tiết và tiến hành sửa chữa." +
                             "\r\nThông tin phiếu sửa chữa:" +
                                "\r\nMã phiếu sửa chữa: " + idTask +
                                "\r\n\tTên khách hàng: " + customerName +
                                "\r\n\tNgày đăng ký: " + DateTime.Now.ToString("dd/MM/yyyy") + " , giờ: " + DateTime.Now.ToString("HH:mm") +
                                "\r\n\tMô tả vấn đề:" + reasonBringFix +
                                "\r\nNếu quý khách có bất kỳ thắc mắc nào, vui lòng liên hệ với chúng tôi qua:" +
                                "\r\n\tHotline: " + hotline +
                                "\r\n\tEmail: " + emailSupport +
                                "\r\nChúng tôi rất hân hạnh được phục vụ quý khách." +
                                "\r\nTrân trọng," +
                                "\r\n[Chữ ký công ty]" +
                                "\r\n" + companyName + 
                                "\r\n" + companyAdrress;
            return message;
        }

        public string PrintRepairReceiptMessage(string customerName, int idTask, string staffName)
        {
            string message = "Kính gửi " + customerName + "," +
                             "\r\nChúng tôi đã nhận được phiếu sửa chữa mà quý khách vừa đăng ký. Chúng tôi xin thông báo rằng phiếu sửa chữa có mã " + idTask +" của quý khách đã được nhân viên kỹ thuật " + staffName + " tiếp nhận và xử lý." +
                             "\r\nNhân viên kỹ thuật sẽ liên hệ với quý khách trong thời gian sớm nhất để xác nhận thông tin chi tiết và thống nhất về các bước sửa chữa tiếp theo." +
                             "\r\nNếu quý khách có bất kỳ câu hỏi hoặc cần hỗ trợ thêm, vui lòng liên hệ với chúng tôi qua:" +
                                "\r\nSố điện thoại: " + hotline +
                                "\r\nEmail: " + emailSupport +
                                "\r\nChúng tôi rất hân hạnh được phục vụ quý khách!" +
                                "\r\nTrân trọng," +
                                "\r\n[Chữ ký công ty]" +
                                "\r\n" + companyName +
                                "\r\n" + companyAdrress;
            return message;
        }

        public string PrintBillMessage(string customerName, int idTask, int idWarrantRecord, string dateBill, int totalBill)
        {

            string message = "Kính gửi "+ customerName +"," +
                             "\r\nChúng tôi trân trọng thông báo rằng quá trình sửa chữa và bảo hành cho phiếu số " + idWarrantRecord + " của quý khách đã hoàn tất. Dưới đây là thông tin chi tiết hóa đơn:" +
                             "\r\nThông tin hóa đơn:" +
                             "\r\n\tMã phiếu sửa chữa: " + idTask +
                             "\r\n\tNgày hoàn thành: " + dateBill + 
                             "\r\n\tChi tiết sửa chữa: [Mô tả các hạng mục sửa chữa/bảo hành]" +
                             "\r\n\tTổng chi phí: " + totalBill +
                             "\r\nPhương thức thanh toán:" +
                             "\r\n\tSố tài khoản: "+ bankAccount +
                             "\r\n\tNgân hàng: " + bankName +
                             "\r\n\tChủ tài khoản: "+  bankOwner +
                             "\r\n\tNội dung chuyển khoản: "+ idTask +"-" + customerName +
                             "\r\nNếu quý khách có bất kỳ câu hỏi hoặc cần hỗ trợ thêm, vui lòng liên hệ với chúng tôi qua:" +
                                "\r\nSố điện thoại: " + hotline +
                                "\r\nEmail: " + emailSupport +
                                "\r\nChúng tôi rất hân hạnh được phục vụ quý khách!" +
                                "\r\nTrân trọng," +
                                "\r\n[Chữ ký công ty]" +
                                "\r\n" + companyName +
                                "\r\n" + companyAdrress;
            return message;
        }

        public string PrintRejectRepairMessage(string customerName, int idTask, int idWarrantRecord)
        {
            string message = "Kính gửi " + customerName +"," +
                             "\r\nChúng tôi rất tiếc phải thông báo rằng đơn sửa chữa và bảo hành của quý khách với thông tin như sau đã bị hủy bỏ:" +
                             "\r\nThông tin đơn sửa chữa:" +
                             "\r\nMã phiếu sửa chữa: "+ idTask +
                             "\r\nLý do hủy bỏ: Khách hàng từ chối sửa chữa" +
                             "\r\nNếu quý khách cần thêm bất kỳ hỗ trợ nào hoặc muốn đăng ký lại dịch vụ sửa chữa, vui lòng liên hệ với chúng tôi qua:" +
                             "\r\nSố điện thoại: " + hotline +
                                "\r\nEmail: " + emailSupport +
                                "\r\nChúng tôi rất hân hạnh được phục vụ quý khách!" +
                                "\r\nTrân trọng," +
                                "\r\n[Chữ ký công ty]" +
                                "\r\n" + companyName +
                                "\r\n" + companyAdrress;

            return message;
        }
    }
}
