using ProjectWarrantlyRecordGrpcServer.MessageContext;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface IMailSevice
    {
        Task<string> SendEmailAsync(NotificationParameters notificationParameters);

    }
}
