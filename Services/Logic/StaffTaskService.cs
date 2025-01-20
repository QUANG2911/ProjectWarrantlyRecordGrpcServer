using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.MessageContext;
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWarrantlyRecordGrpcServer.Services.Logic
{
    public class StaffTaskService : IStaffTaskService
    {
        private readonly ICheckOut _checkout;
        private readonly IDataService _dataService;
        private readonly EmailQueue _emailQueue;
        public StaffTaskService( ICheckOut checkout, IDataService dataService, EmailQueue emailQueue)
        {
            _checkout = checkout;
            _dataService = dataService;
            _emailQueue = emailQueue;
        }

        public async Task<int> CreateNewStaffTask(CreateRepairManagementRequest itemInsertStaffTask)
        {
            //Check
            var checkWarrantlyRecord = await _checkout.CheckWarrantyRecordByIdWarrantAsync(itemInsertStaffTask.IdWarrantRecord);
       
            await _checkout.CheckStaffTaskByIdWarrantAsync(itemInsertStaffTask.IdWarrantRecord);

            //Add Db
            var newStaffTask = await _dataService.AddNewStaffTaskAsync(itemInsertStaffTask);

            //Add email
            // Thêm email vào hàng đợi
            _emailQueue.Enqueue(new NotificationParameters
            {
                CustomerName = itemInsertStaffTask.CustomerName,
                IdTask = 1,
                IdWarrantyRecord = itemInsertStaffTask.IdWarrantRecord,
                CustomerEmail = itemInsertStaffTask.CustomerEmail,
                subject = "Xác nhận đăng ký phiếu sửa chữa thành công",
                TypeMessage = "RegistrationTask",
                ReasonBringFix = itemInsertStaffTask.ReasonBringFix
            });

            return 1;
        }

        public async Task<ReadRepairManagementResponse> GetStaffTaskDone(int idStaffTask)
        {
            var response = await _dataService.GetListRepairPartInStaffTaskAsync(idStaffTask);
            return response;
        }

        public async Task<ReadItemCustomerResponse> GetStaffTaskCustomer(int idTask)
        {
            //Check
            var staffTask = await _checkout.CheckStaffTaskByIdTaskAsync(idTask);

            var warrantyRecord = await _checkout.CheckWarrantyRecordByIdWarrantAsync(staffTask.IdWarantyRecord);

            int idCustomer = warrantyRecord.IdCustomer;

            var customer = await _checkout.CheckCustomerByIdCustomerAsync(idCustomer);

            // get information DB
            var response = new ReadItemCustomerResponse
            {
                IdTask = staffTask.IdTask,
                CustomerName = customer.CustomerName,
                CustomerPhone = customer.CustomerPhone,
                ReasonBringFix = staffTask.ReasonBringFix,
                StatusTask = staffTask.StatusTask,
            };
          
            return response;
        }

        public async Task<string> UpdateWorkScheduleAutomatically(int idStaff)
        {
            var task = await _dataService.GetTaskNotHaveStaffDoAsync();
            if (task != null)
            {
                var checkUpdateSchedule = await _dataService.UpdateUpdateWorkScheduleAsync(idStaff);
                if (checkUpdateSchedule != 0)
                {
                    var WarrantyRecord = await _checkout.CheckWarrantyRecordByIdWarrantAsync(checkUpdateSchedule);
                    if (WarrantyRecord != null)
                    {
                        var customer = await _checkout.CheckCustomerByIdCustomerAsync(WarrantyRecord.IdCustomer);
                        if (customer != null)
                        {
                            _emailQueue.Enqueue(new NotificationParameters
                             {
                                 CustomerName = customer.CustomerName,
                                 IdTask = task.IdTask,
                                 IdWarrantyRecord = task.IdWarantyRecord,
                                 CustomerEmail = customer.CustomerEmail,
                                 subject = "Thông báo xác nhận tiếp nhận phiếu sửa chữa của quý khách",
                                 TypeMessage = "ReceiptTask"
                             });
                        }
                    }
                }
            }
            return await Task.FromResult("done");
        }


        public async Task<GetListRepairManagementResponse> GetListStaffTask(int idStaff)
        {

            var checkDoneUpdateTask = await UpdateWorkScheduleAutomatically(idStaff);
            if (checkDoneUpdateTask != "done")
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Chưa qua cập nhật tự động"));
            }

            var response = await _dataService.GetListRepairAsync(idStaff);
           
            return response;
        }

        public async Task<int> UpdateStaffTask(UpdateRepairManagementRequest request )
        {

            //CHECK
            var staffTask = await _checkout.CheckStaffTaskByIdTaskAsync(request.IdTask);

            var staff = await _checkout.CheckStaffByIdStaffAsync(request.IdStaff);

            var warrantyRecord = await _checkout.CheckWarrantyRecordByIdWarrantAsync(staffTask.IdWarantyRecord);

            var customer = await _checkout.CheckCustomerByIdCustomerAsync(warrantyRecord.IdCustomer);

            //database
            var idTask = await _dataService.UpdateStaffTasksStatusAsync(staffTask.IdTask, request.StatusTask);

            var idStaff = await _dataService.UpdateStaffStatusAsync(staff.IdStaff, 0);

            var totalBill = await _dataService.AddNewRepairPartInTaskAsync(request);

            if (request.StatusTask == 1)
            {
                var bill = await _dataService.AddNewBillAsync(idTask, totalBill);
                _emailQueue.Enqueue(new NotificationParameters
                {
                    CustomerName = customer.CustomerName,
                    IdTask = idTask,
                    IdWarrantyRecord = warrantyRecord.IdWarrantRecord,
                    CustomerEmail = customer.CustomerName,
                    subject = "Thông báo xác nhận xử lý phiếu sửa chữa của quý khách",
                    TypeMessage = "Bill",
                    DateBill = DateTime.Now.ToString("dd/MM/yyyy"),
                    TotalBill = totalBill,
                    listRepairParts = request
                });

            }
            else if (request.StatusTask == 2) 
            {
                _emailQueue.Enqueue(new NotificationParameters
                {
                    CustomerName = customer.CustomerName,
                    IdTask = idTask,
                    IdWarrantyRecord = warrantyRecord.IdWarrantRecord,
                    CustomerEmail = customer.CustomerName,
                    subject = "Thông báo hủy bỏ đơn sửa chữa khách hàng đăng ký",
                    TypeMessage = "RejectTask",
                });
            }
           

            //      catch (DbUpdateException dbEx)
            //      {
            //            throw new RpcException(new Status(StatusCode.Internal, $"Database error: {dbEx.InnerException?.Message ?? dbEx.Message}"));
            //      }
            //      catch (RpcException rpcEx)
            //      {
            //           throw rpcEx; // Giữ nguyên lỗi RPC đã được định nghĩa.
            //      }

            return 1;
        }
    }
}
