using ProjectWarrantlyRecordGrpcServer.DTO;
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface IStaffTaskService
    {
        Task<int> CreateNewStaffTask(CreateRepairManagementRequest itemInsertStaffTask);
        DetailStaffTaskDto GetStaffTask(int idStaffTask);

        List<ItemInListStaffTaskDto> GetListStaffTask(int idStaff);
        void UpdateWorkScheduleAutomatically(int  idStaff);
        int UpdateStaffTask(int idStaffTask);
    }
}
