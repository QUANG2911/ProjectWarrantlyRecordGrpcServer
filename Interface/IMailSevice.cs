using ProjectWarrantlyRecordGrpcServer.MessageContext;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface IMailSevice
    {
        string SendEmailAsync(NotificationParameters notificationParameters, int TypeTable);

    }
}
