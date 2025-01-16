using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface IWarranyRecordService
    {
        Task<GetWarrantyListResponse> GetListWarrantyList();
    }
}
