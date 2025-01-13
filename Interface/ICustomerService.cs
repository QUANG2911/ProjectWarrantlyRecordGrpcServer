
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface ICustomerService
    {
        GetListCustomerManagementResponse GetListCustomer();
        ReadCustomerManagementResponse GetDetailCustomer(int IdCustomer);

    }
}
