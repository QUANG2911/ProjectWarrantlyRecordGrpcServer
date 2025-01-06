using ProjectWarrantlyRecordGrpcServer.DTO;
using ProjectWarrantlyRecordGrpcServer.Model;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface ICustomerService
    {
        List<Customer> GetListCustomer();
        List<DetailCustomerDto> GetDetailCustomer(int IdCustomer);

    }
}
