
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface ICustomerService
    {
        Task<GetListCustomerManagementResponse> GetListCustomer();
        Task<ReadCustomerManagementResponse> GetDetailCustomer(int IdCustomer);

    }
}
