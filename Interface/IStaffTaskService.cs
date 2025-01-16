
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface IStaffTaskService
    {
        Task<int> CreateNewStaffTask(CreateRepairManagementRequest itemInsertStaffTask);
        Task<ReadRepairManagementResponse> GetStaffTaskDone(int idStaffTask);

        Task<GetListRepairManagementResponse> GetListStaffTask(int idStaff);

        Task<ReadItemCustomerResponse> GetStaffTaskCustomer(int idStaffTask);
        Task<string> UpdateWorkScheduleAutomatically(int  idStaff);
        Task<int> UpdateStaffTask(UpdateRepairManagementRequest idStaffTask);
    }
}
