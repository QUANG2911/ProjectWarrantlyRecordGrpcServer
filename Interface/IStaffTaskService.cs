
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface IStaffTaskService
    {
        Task<int> CreateNewStaffTask(CreateRepairManagementRequest itemInsertStaffTask);
        ReadRepairManagementResponse GetStaffTaskDone(int idStaffTask);

        GetListRepairManagementResponse GetListStaffTask(int idStaff);

        ReadItemCustomerResponse GetStaffTaskCustomer(int idStaffTask);
        void UpdateWorkScheduleAutomatically(int  idStaff);
        Task<int> UpdateStaffTask(UpdateRepairManagementRequest idStaffTask);
    }
}
