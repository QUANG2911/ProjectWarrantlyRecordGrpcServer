using Grpc.Core;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface ITokenService
    {
        string CheckTokenIdStaff(int idStaff, ServerCallContext context);

        string GetToken(int idStaff);
    }
}
