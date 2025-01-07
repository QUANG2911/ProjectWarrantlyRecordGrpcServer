using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.DTO;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Services.Logic
{
    public class StaffTaskService : IStaffTaskService
    {
        private readonly ApplicationDbContext _context;
        public StaffTaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateNewStaffTask(CreateRepairManagementRequest itemInsertStaffTask)
        {
            var checkWarrantlyRecord = _context.WarrantyRecords.FirstOrDefault(x => x.IdWarrantRecord == itemInsertStaffTask.IdWarrantRecord);

            if (checkWarrantlyRecord == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Phiếu bảo hành này không tồn tại"));
            }
            if (checkWarrantlyRecord.TimeEnd < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Phiếu bảo hành đã hết hạn"));
            }
           
            var staffTask = new StaffTask
            {
                IdWarantyRecord = itemInsertStaffTask.IdWarrantRecord,
                DateOfTask = DateOnly.FromDateTime(DateTime.Now),
                StatusTask = -1, 
                ReasonBringFix = itemInsertStaffTask.ReasonBringFix,
            };
            await _context.StaffTasks.AddAsync(staffTask);
            _context.SaveChanges();

            var result = _context.StaffTasks.OrderByDescending(p => p.IdStaff).FirstOrDefault();

            if (result == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Lỗi add dữ liệu"));
            }
            return result.IdTask;
        }

        public DetailStaffTaskDto GetStaffTask(int idStaffTask)
        {
            var listStaffTask = from st in _context.StaffTasks.Where(p => p.IdStaff == idStaffTask)
                                from wr in _context.WarrantyRecords
                                from cs in _context.Customers
                                where st.IdWarantyRecord == wr.IdWarrantRecord && wr.IdCustomer == cs.IdCustomer
                                select new
                                {
                                    st.IdTask,
                                    cs.CustomerName,
                                    cs.CustomerPhone,
                                    st.ReasonBringFix,
                                    st.StatusTask
                                };

            DetailStaffTaskDto detailStaffTask = new DetailStaffTaskDto
            {
                CustomerName = listStaffTask.First().CustomerName,
                CustomerPhone = listStaffTask.First().CustomerPhone,
                IdTask = listStaffTask.First().IdTask,
                ReasonBringFix = listStaffTask.First().ReasonBringFix,
                StatusTask = listStaffTask.First().StatusTask,
            };
            return detailStaffTask;
        }

        public void UpdateWorkScheduleAutomatically(int idStaff)
        {
            var checkStatusStaff = _context.Staffs.Where(p=>p.IdStaff == idStaff).FirstOrDefault();

            if(checkStatusStaff == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không tìm thấy nhân viên này"));
            }
            if (checkStatusStaff.Status == 0)
            {
                var stafTask = _context.StaffTasks.Where(p=>p.IdStaff == null).OrderByDescending(p=>p.DateOfTask).FirstOrDefault();
                if(stafTask != null)
                {
                    stafTask.IdStaff = idStaff;
                    stafTask.StatusTask = 0;
                    checkStatusStaff.Status = 1;
                    _context.SaveChanges();
                }    
            }
        }

        public List<ItemInListStaffTaskDto> GetListStaffTask(int idStaff)
        {
            UpdateWorkScheduleAutomatically(idStaff);

            var listStaffTask = from st in _context.StaffTasks.Where(p=>p.IdStaff == idStaff)
                                from wr in _context.WarrantyRecords
                                from cs in _context.Customers
                                where st.IdWarantyRecord == wr.IdWarrantRecord && wr.IdCustomer == cs.IdCustomer
                                select new
                                {
                                    st.IdTask,
                                    cs.CustomerName,
                                    cs.CustomerPhone,
                                    st.DateOfTask,
                                    wr.DateOfResig,
                                    wr.IdWarrantRecord,
                                    st.StatusTask
                                };
            if(listStaffTask == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không có danh sách công việc"));
            }    

            List<ItemInListStaffTaskDto> list = new List<ItemInListStaffTaskDto>();

            foreach (var item in listStaffTask)
            {
                ItemInListStaffTaskDto tmp = new ItemInListStaffTaskDto
                {
                    CustomerName = item.CustomerName,
                    CustomerPhone = item.CustomerPhone,
                    DateOfWarranty = item.DateOfResig,
                    DateOfTask = item.DateOfResig,
                    IdTask = item.IdTask,
                    IdWarrantyRecord = item.IdWarrantRecord,
                    StatusTask = item.StatusTask
                };

                list.Add(tmp);
            }

            return list;
        }

        public int UpdateStaffTask(int idStaffTask)
        {
            var staffTask = _context.StaffTasks.Find(idStaffTask);

            if (staffTask == null) {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không có công việc này"));
            }
            staffTask.StatusTask = 1;

            var staff = _context.Staffs.Find(staffTask.IdStaff);
            if (staff == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không có nhân viên này ??? sao gán được dữ liệu hay vậy"));
            }
            staff.Status = 0;
            _context.SaveChanges();
            return staffTask.IdTask;
        }
    }
}
