using ProjectWarrantlyRecordGrpcServer.DTO;
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface IStaffTaskService
    {
        Task<int> CreateNewStaffTask(CreateRepairManagementRequest itemInsertStaffTask);
        ReadRepairManagementResponse GetStaffTaskDone(int idStaffTask);

        GetListRepairManagementResponse GetListStaffTask(int idStaff);
        void UpdateWorkScheduleAutomatically(int  idStaff);
        int UpdateStaffTask(int idStaffTask);
    }
}
