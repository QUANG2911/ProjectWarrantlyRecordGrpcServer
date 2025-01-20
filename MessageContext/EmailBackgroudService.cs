using ProjectWarrantlyRecordGrpcServer.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Rpc;
using System.ComponentModel;

namespace ProjectWarrantlyRecordGrpcServer.MessageContext
{
    public class EmailBackgroundService : BackgroundService // Là một lớp cơ bản(abstract class) trong ASP.NET Core được thiết kế để triển khai các dịch vụ chạy ngầm (background tasks)
                                                            // Chỉ cần triển khai phương thức ExecuteAsync(CancellationToken stoppingToken) để định nghĩa logic mà dịch vụ
                                                            // Mỗi công việc ngầm riêng biệt nên được triển khai trong một lớp kế thừa BackgroundService khác nhau để đảm bảo tính độc lập và dễ quản lý.
    {
        private readonly EmailQueue _emailQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory; // để tạo phạm vi dịch vụ (service scope) mới
                                                                    // vì các dịch vụ hỗ trợ chạy ngầm như BackgroundService không có ngữ cảnh request HTTP, nên không thể trực tiếp sử dụng các dịch vụ Scoped. Nếu bạn cố gắng sử dụng một dịch vụ Scoped trong Singleton, bạn sẽ nhận lỗi.
                                                                    // Nếu lập trình console hay winForm không có request Http vẫn nên sử dụng DI và có các dịch vụ Scoped hoặc yêu cầu quản lý tài nguyên trong phạm vi riêng biệt
        private readonly ILogger<EmailBackgroundService> _logger; 

        public EmailBackgroundService(EmailQueue emailQueue, IServiceScopeFactory serviceScopeFactory, ILogger<EmailBackgroundService> logger)
        {
            _emailQueue = emailQueue;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Email background service started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_emailQueue.QueueCount > 0)
                    {
                        _logger.LogInformation("Current email queue size: {QueueCount}.", _emailQueue.QueueCount);
                    }

                    var email = await _emailQueue.DequeueAsync(stoppingToken);
                    _logger.LogInformation("Processing email for {CustomerEmail} with subject {Subject}.", email.CustomerEmail, email.subject);
                    using (var scope = _serviceScopeFactory.CreateScope()) // .CreateScope() tạo ra một phạm vi mới trong đó các dịch vụ Scoped có thể được khởi tạo và quản lý.
                                                                           // Khi phạm vi kết thúc (khi using đóng), tất cả các dịch vụ được tạo trong phạm vi này sẽ được hủy một cách an toàn.
                    {
                        var mailService = scope.ServiceProvider.GetRequiredService<IMailSevice>(); //scope.ServiceProvider cung cấp các dịch vụ trong phạm vi vừa tạo.
                                                                                                   //GetRequiredService<T>() lấy một dịch vụ cụ thể muốn triển khai chạy.

                        _logger.LogInformation("Processing email for {CustomerEmail} with subject {Subject}.", email.CustomerEmail, email.subject);

                        await mailService.SendEmailAsync(email); // logic và dịch vụ muốn chạy ngầm

                        _logger.LogInformation("Email successfully sent to {CustomerEmail}.", email.CustomerEmail);
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi nếu cần
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    _logger.LogError(ex, "Error occurred while sending email.");
                }
                // Delay giữa các lần log trạng thái để tránh ghi log quá nhiều
                await Task.Delay(5000, stoppingToken);
            }
            _logger.LogInformation("Email background service stopped.");
        }
    }

}
