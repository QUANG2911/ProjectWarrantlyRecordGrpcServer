using Grpc.Core;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface ITokenService
    {
        Task<string> CheckTokenIdStaff(int idStaff, ServerCallContext context);

        Task<string> GetToken(int idStaff);
    }
}
