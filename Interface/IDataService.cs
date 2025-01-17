using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface IDataService
    {
        Task<int> AddNewStaffTaskAsync(CreateRepairManagementRequest request);

        Task<ReadRepairManagementResponse> GetListRepairPartInStaffTaskAsync(int idStaffTask);
        Task<int> UpdateUpdateWorkScheduleAsync( int idStaff);
        Task<int> UpdateStaffTasksStatusAsync(int idStaffTask, int statusTask);

        Task<int> UpdateStaffStatusAsync(int idStaff, int statusStaff);
        Task<StaffTask> GetTaskNotHaveStaffDoAsync();

        Task<GetListRepairManagementResponse> GetListRepairAsync(int idStaff);

        Task<int> AddNewRepairPartInTaskAsync(UpdateRepairManagementRequest request);

        Task<GetListCustomerManagementResponse> GetListCustomerAsync();

        Task<ReadCustomerManagementResponse> GetListDetalOfCustomerAsync(int idCustomer);

        Task<Bill> AddNewBillAsync (int idTask, int totalBill);
    }
}
