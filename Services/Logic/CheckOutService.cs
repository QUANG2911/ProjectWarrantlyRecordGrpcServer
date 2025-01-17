using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Model;
using System.Threading.Tasks;

namespace ProjectWarrantlyRecordGrpcServer.Services.Logic
{
    public class CheckOutService :ICheckOut
    {
        private readonly ApplicationDbContext _context;
        public CheckOutService(ApplicationDbContext context)
        {
            _context = context;
        }

        // CHECK idWarrantRecord
        public async Task<WarrantyRecord> CheckWarrantyRecordByIdWarrantAsync(int idWarrantRecord)
        {
            var checkWarrantlyRecord = await _context.WarrantyRecords.FindAsync(idWarrantRecord);

            if (checkWarrantlyRecord == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Phiếu bảo hành này không tồn tại"));
            }
            if (checkWarrantlyRecord.TimeEnd < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Phiếu bảo hành đã hết hạn"));
            }
            return checkWarrantlyRecord;
        }
        public async Task CheckStaffTaskByIdWarrantAsync(int idWarrantRecord)
        {
            var checkStaffTasks = await _context.StaffTasks.OrderByDescending(p => p.DateOfTask).FirstOrDefaultAsync(p => p.IdWarantyRecord == idWarrantRecord);

            if (checkStaffTasks != null && (checkStaffTasks.StatusTask == 0 || checkStaffTasks.StatusTask == -1))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Phiếu sửa chữa cho thiết bị này đã được nhận"));
            }
        }

        // Check idTask
        public async Task<StaffTask> CheckStaffTaskByIdTaskAsync(int idTask)
        {
            var staffTask = await _context.StaffTasks.FindAsync(idTask);
            if (staffTask == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không có phiếu sửa chữa này"));
            }
            return staffTask;
        }


        // Check idCustomer
        public async Task<Customer> CheckCustomerByIdCustomerAsync(int idCustomer)
        {
            var customer = await _context.Customers.Where(p => p.IdCustomer == idCustomer).FirstOrDefaultAsync();
            if (customer == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Khách hàng này chưa từng mua hàng ở đâu này"));
            }
            return customer;
        }

        // CHECK idStaff
        public async Task<Staff> CheckStaffByIdStaffAsync(int idStaff)
        {

            var checkStatusStaff = await _context.Staffs.FindAsync(idStaff);

            if (checkStatusStaff == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không tìm thấy nhân viên này"));
            }

            return checkStatusStaff;
        }

        //Check login
        public async Task<Staff> CheckStaffLoginByIdStaffPassAsync(int idStaff, string pass)
        {
            var staff = await _context.Staffs.Where(p => p.IdStaff == idStaff && pass == p.Pass).FirstOrDefaultAsync();
            if (staff == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không tìm thấy thông tin tài khoản nhân viên này"));
            }
            return staff;
        }
    }
}
