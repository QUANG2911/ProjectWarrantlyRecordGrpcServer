using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Services.Logic
{
    public class DataService : IDataService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICheckOut _checkOut;
        public DataService(ApplicationDbContext context, ICheckOut checkOut)
        {
            _context = context;
            _checkOut = checkOut;
        }
        // ***********************ADD*************************
        public async Task<int> AddNewStaffTaskAsync(CreateRepairManagementRequest request)
        {
            int idTask = _context.StaffTasks.Count() + 1;
            var StaffTask = new StaffTask
            {
                IdWarantyRecord = request.IdWarrantRecord,
                DateOfTask = DateOnly.FromDateTime(DateTime.Now),
                StatusTask = -1,
                ReasonBringFix = request.ReasonBringFix,
            };
            await _context.StaffTasks.AddAsync(StaffTask);
            await _context.SaveChangesAsync();

            var result = await _context.StaffTasks.Where(p => p.IdTask == idTask).FirstOrDefaultAsync();

            if (result == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Lỗi add dữ liệu"));
            }

            return idTask;
        }

        public async Task<int> AddNewRepairPartInTaskAsync(UpdateRepairManagementRequest request)
        {
            int total = 0;
            if (request.ToListUpdateRepairPart.Count > 0)
            {
                foreach (var item in request.ToListUpdateRepairPart)
                {
                    RepairDetail repairDetail = new RepairDetail();
                    repairDetail.IdTask = request.IdTask;
                    repairDetail.IdRepairPart = item.IdRepairPart;
                    repairDetail.Amount = item.Amount;

                    await _context.RepairDetails.AddAsync(repairDetail);
                    await _context.SaveChangesAsync();
                    total = total + item.Amount * item.Price;
                }
            }
            return total;
        }

        public async Task<Bill> AddNewBillAsync(int idTask, int totalBill)
        {
            Bill bill = new Bill
            {
                IdTask = idTask,
                TotalAmount = totalBill,
                DateCreateBill = DateOnly.FromDateTime(DateTime.Now),
                StatusBill = 0,
            };
            await _context.Bills.AddAsync(bill);
            await _context.SaveChangesAsync();
            return bill;
        }


        // ***********************GET*************************
        public async Task<ReadRepairManagementResponse> GetListRepairPartInStaffTaskAsync(int idStaffTask)
        {
            var listStaffTask = await Task.FromResult(
                                   from rp in _context.RepairParts.AsNoTracking()
                                   join rd in _context.RepairDetails.AsNoTracking().Where(p => p.IdTask == idStaffTask) on rp.IdRepairPart equals  rd.IdRepairPart
                                   select new
                                   {
                                       rp.IdRepairPart,
                                       rp.RepairPartName,
                                       rp.Price,
                                       rd.Amount
                                   });
            var response = new ReadRepairManagementResponse();
            foreach (var item in listStaffTask)
            {
                response.ToRepairPartList.Add(new GetItemRepaitPartInWarrantyReponce
                {
                    IdRepairPart = item.IdRepairPart,
                    Amount = item.Amount,
                    Price = item.Price,
                    RepairPartName = item.RepairPartName,
                });
            }
            return response;
        }

        public async Task<StaffTask> GetTaskNotHaveStaffDoAsync()
        {
            var staffTask = await _context.StaffTasks.Where(p => p.IdStaff == null).OrderByDescending(p => p.DateOfTask).FirstOrDefaultAsync();
            if (staffTask == null)
                return new StaffTask();
            return staffTask ;
        }

        public async Task<GetListRepairManagementResponse> GetListRepairAsync(int idStaff)
        {
            var listStaffTask = await Task.FromResult(
                                from st in _context.StaffTasks.AsNoTracking().Where(p => p.IdStaff == idStaff)
                                join wr in _context.WarrantyRecords.AsNoTracking() on st.IdWarantyRecord equals wr.IdWarrantRecord
                                join cs in _context.Customers.AsNoTracking()
                                on wr.IdCustomer equals cs.IdCustomer
                                select new
                                {
                                    st.IdTask,
                                    cs.CustomerName,
                                    cs.CustomerPhone,
                                    st.DateOfTask,
                                    wr.DateOfResig,
                                    wr.IdWarrantRecord,
                                    st.StatusTask
                                });

            if (listStaffTask.Count() == 0)
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
        public async Task<GetListCustomerManagementResponse> GetListCustomerAsync()
        {
            var listCustomer = await _context.Customers.AsNoTracking().ToListAsync();
            var response = new GetListCustomerManagementResponse();
            foreach (var item in listCustomer)
            {
                response.ToCustomerList.Add(new GetItemInListCustomerResponse
                {
                    IdCusomer = item.IdCustomer,
                    CustomerName = item.CustomerName,
                    CustomerPhone = item.CustomerPhone,
                    CustomerEmail = item.CustomerEmail,
                    CustomerAdrress = item.CustomerAddress
                });
            }
            return response;
        }

        public async Task<ReadCustomerManagementResponse> GetListDetalOfCustomerAsync(int idCustomer)
        {
            var customer = await _checkOut.CheckCustomerByIdCustomerAsync(idCustomer);

            var listDeviceOfCustomer = from wr in _context.WarrantyRecords.AsNoTracking().Where(p => p.IdCustomer == idCustomer)
                                       from dc in _context.CustomerDevices.AsNoTracking()
                                       where dc.IdDevice == wr.IdDevice
                                       select new
                                       {
                                           dc.DeviceName,
                                           wr.IdWarrantRecord,
                                           wr.TimeEnd,
                                           wr.DateOfResig,
                                           dc.IdDevice
                                       };

            var response = new ReadCustomerManagementResponse();

            foreach (var item in listDeviceOfCustomer)
            {
                response.ToDeviceList.Add(new ReadItemDeviceCustomerManagementResponse
                {
                    IdCusomer = customer.IdCustomer,
                    CustomerName = customer.CustomerName,
                    CustomerEmail = customer.CustomerEmail,
                    CustomerPhone = customer.CustomerPhone,
                    CustomerAdrress = customer.CustomerAddress,
                    CustomerDevice = item.DeviceName,
                    IdWarrantReport = item.IdWarrantRecord,
                    DateOfWarrant = item.DateOfResig.ToString(),
                    TimeEnd = item.TimeEnd.ToString(),
                    IdDevice = item.IdDevice,
                });
            }

            return response;
        }

        // ***********************UPDATE*************************
        public async Task<int> UpdateUpdateWorkScheduleAsync(int idStaff)
        {
            var checkStaff = await _context.Staffs.Where(p => p.IdStaff == idStaff && p.Status == 0 && p.StaffPosition == "Kĩ thuật viên").FirstOrDefaultAsync();

            if (checkStaff != null)
            {
                var staffTask = await GetTaskNotHaveStaffDoAsync();
                if (staffTask != null)
                {
                    staffTask.IdStaff = idStaff;
                    staffTask.StatusTask = 0;
                    checkStaff.Status = 1;
                    await _context.SaveChangesAsync();
                    return staffTask.IdWarantyRecord;
                }
            }
            return 0;            
        }

        public async Task<int> UpdateStaffTasksStatusAsync(int idStaffTask, int statusTask)
        {
            var staffTask = await _context.StaffTasks.Where(p => p.IdTask == idStaffTask).FirstOrDefaultAsync();
            if (staffTask == null)
            {
                return 0;
            }
            staffTask.StatusTask = statusTask;
            await _context.SaveChangesAsync();
            return staffTask.IdTask;
        }

        public async Task<int> UpdateStaffStatusAsync(int idStaff, int statusStaff)
        {
            var staff = await _context.Staffs.Where(p => p.IdStaff == idStaff && p.Status != statusStaff && p.StaffPosition == "Kĩ thuật viên").FirstOrDefaultAsync();
            if (staff == null)
            {
                return 0;
            }                
            staff.Status = statusStaff;
            await _context.SaveChangesAsync();
            return staff.IdStaff;
        }
    }
}
