using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.DTO;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;
using System.Collections.Generic;

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

        public ReadRepairManagementResponse GetStaffTaskDone(int idStaffTask)
        {
            var listStaffTask = from st in _context.StaffTasks.Where(p => p.IdStaff == idStaffTask)
                                from wr in _context.WarrantyRecords
                                from cs in _context.Customers
                                from rp in _context.RepairParts
                                from rd in _context.RepairDetails
                                where st.IdWarantyRecord == wr.IdWarrantRecord && wr.IdCustomer == cs.IdCustomer && rp.IdRepairPart == rd.IdRepairPart && rd.IdTask == st.IdTask
                                select new
                                {
                                    st.IdTask,
                                    cs.CustomerName,
                                    cs.CustomerPhone,
                                    st.ReasonBringFix,
                                    st.StatusTask,
                                    rp.RepairPartName,
                                    rp.Price,
                                    rd.Amount
                                };
            var response = new ReadRepairManagementResponse();
            foreach (var item in listStaffTask)
            {
                response.ToRepairPartList.Add(new GetItemRepaitPartInWarrantyReponce
                {
                    IdTask = item.IdTask,
                    CustomerName = item.CustomerName,
                    CustomerPhone = item.CustomerPhone,
                    Amount = item.Amount,
                    Price = item.Price,
                    RepairPartName = item.RepairPartName,
                    ReasonBringFix = item.ReasonBringFix,
                    StatusTask = item.StatusTask,
                });
            }
            return response;

        }

        public ReadItemRepairNotDoneResponse GetStaffTaskNotDone(int idStaffTask)
        {
            var staffTask = _context.StaffTasks.Where(p => p.IdStaff == idStaffTask).FirstOrDefault();
            if (staffTask == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không có phiếu sửa chữa này"));
            }
            var warrantyRecord = _context.WarrantyRecords.Where(p => p.IdWarrantRecord == staffTask.IdWarantyRecord).FirstOrDefault();

            if (warrantyRecord == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không có phiếu bảo hành này"));
            }
            int IdCustomer = warrantyRecord.IdCustomer;
            var customer = _context.Customers.Where(p => p.IdCustomer == IdCustomer).FirstOrDefault();
            if (customer == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Khách hàng này chưa từng mua hàng ở đâu này"));
            }

            var response = new ReadItemRepairNotDoneResponse
            {
                IdTask = staffTask.IdTask,
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
                ReasonBringFix = staffTask.ReasonBringFix,
                StatusTask = staffTask.StatusTask,
            };
          
            return response;
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


        public GetListRepairManagementResponse GetListStaffTask(int idStaff)
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
            if(listStaffTask.Count() == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không có danh sách công việc"));
            }

            var response = new GetListRepairManagementResponse();
            foreach (var item in listStaffTask)
            {
                response.ToList.Add(new ReadItemRepairManagementResponse
                {
                    IdTask = item.IdTask,
                    CustomerName = item.CustomerName,
                    CustomerPhone = item.CustomerPhone,
                    DateOfTask = item.DateOfTask.ToString(),
                    DateOfWarranty = item.DateOfResig.ToString(),
                    IdWarrantRecord = item.IdWarrantRecord,
                    StatusTask = item.StatusTask,
                });
            }

            return response;
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
