using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface IRepairPart
    {
        Task<GetListRepairPartResponse> GetListRepairPart();
    }
}
