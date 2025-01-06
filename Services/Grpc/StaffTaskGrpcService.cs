using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProjectWarrantlyRecordGrpcServer.DTO;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Protos;
using System;

namespace ProjectWarrantlyRecordGrpcServer.Services.Grpc
{
    public class StaffTaskGrpcService : RepairManagement.RepairManagementBase
    {
        private readonly IStaffTaskService _staffTask;
        private readonly ILogger<StaffTaskGrpcService> _logger;

        public StaffTaskGrpcService(IStaffTaskService staffTask, ILogger<StaffTaskGrpcService> logger)
        {
            _staffTask = staffTask;
            _logger = logger;
        }

        public override async Task<CreateRepairManagementResponse> CreateRepairManagement(CreateRepairManagementRequest request, ServerCallContext context)
        {
            if (request.IdWarrantRecord == 0 || request.CustomerPhone == null || request.CustomerEmail == null || request.CustomerName == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không được để trống các thông tin truyền"));
            }
            ItemInsertStaffTaskDto itemInsertStaffTask = new ItemInsertStaffTaskDto
            {
                CustomerEmail = request.CustomerEmail,
                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                DeviceName = request.DeviceName,
                IdWarrantRecord = request.IdWarrantRecord,
                ReasonBringFix = request.ReasonBringFix
            };
            var result = await _staffTask.CreateNewStaffTask(itemInsertStaffTask);

            return await Task.FromResult(new CreateRepairManagementResponse { IdTask = result });
        }

        public override async Task<GetListRepairManagementResponse> ListRepairManagement(GetListRepairManagementRequest request, ServerCallContext context)
        {
            if (request.IdStaff == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không được để trống các thông tin truyền"));
            }
            var list = _staffTask.GetListStaffTask(request.IdStaff);
            var response = new GetListRepairManagementResponse();
            foreach (var item in list)
            {
                response.ToList.Add(new ReadItemRepairManagementResponse
                {
                    IdTask = item.IdTask,
                    CustomerName = item.CustomerName,
                    CustomerPhone = item.CustomerPhone,
                    DateOfTask = item.DateOfTask.ToString(),
                    DateOfWarranty = item.DateOfWarranty.ToString(),
                    IdWarrantRecord = item.IdWarrantyRecord,
                    StatusTask = item.StatusTask,
                });
            }
            return await Task.FromResult(response);
        }

        public override async Task<ReadRepairManagementResponse> ReadRepairManagement(ReadToRequest request, ServerCallContext context)
        {
            if (request.IdTask == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không được để trống các thông tin truyền"));
            }

            var staffTask = _staffTask.GetStaffTask(request.IdTask);
            if (staffTask == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không tìm thấy thông tin"));
            }

            return await Task.FromResult(new ReadRepairManagementResponse
            {
                IdTask = staffTask.IdTask,
                CustomerName = staffTask.CustomerName,
                CustomerPhone = staffTask.CustomerPhone,
                StatusTask = staffTask.StatusTask,
                ReasonBringFix = staffTask.ReasonBringFix,
            });
        }

        public override async Task<UpdateRepairManagementResponse> UpdateRepairManagement(UpdateRepairManagementRequest request, ServerCallContext context)
        {
            if (request.IdTask == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không được để trống các thông tin truyền"));
            }
            var result = _staffTask.UpdateStaffTask(request.IdTask);
            
            return await Task.FromResult(new UpdateRepairManagementResponse { IdTask = result });
        }
    }
}
