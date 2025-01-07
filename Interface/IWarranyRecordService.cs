using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface IWarranyRecordService
    {
        GetWarrantyListResponse GetListWarrantyList();
    }
}
