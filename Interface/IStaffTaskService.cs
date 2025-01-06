using ProjectWarrantlyRecordGrpcServer.DTO;
using ProjectWarrantlyRecordGrpcServer.Model;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface IStaffTaskService
    {
        Task<int> CreateNewStaffTask(ItemInsertStaffTaskDto itemInsertStaffTask);
        DetailStaffTaskDto GetStaffTask(int idStaffTask);

        List<ItemInListStaffTaskDto> GetListStaffTask(int idStaff);
        void UpdateWorkScheduleAutomatically(int  idStaff);
        int UpdateStaffTask(int idStaffTask);
    }
}
