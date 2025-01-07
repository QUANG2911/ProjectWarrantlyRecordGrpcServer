using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProjectWarrantlyRecordGrpcServer.DTO;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Protos;
using System;
using static Google.Rpc.Context.AttributeContext.Types;

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
            var result = await _staffTask.CreateNewStaffTask(request);

            return await Task.FromResult(new CreateRepairManagementResponse { IdTask = result });
        }

        public override async Task<GetListRepairManagementResponse> ListRepairManagement(GetListRepairManagementRequest request, ServerCallContext context)
        {
            if (request.IdStaff == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không được để trống các thông tin truyền"));
            }
            var response = _staffTask.GetListStaffTask(request.IdStaff);
            
            return await Task.FromResult(response);
        }

        public override async Task<ReadRepairManagementResponse> ReadRepairDone(ReadToRequest request, ServerCallContext context)
        {
            if (request.IdTask == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không được để trống các thông tin truyền"));
            }

            var response = _staffTask.GetStaffTaskDone(request.IdTask);
            if (response.ToRepairPartList.Count() == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không tìm thấy thông tin"));
            }

            return await Task.FromResult(response);

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

        public override async Task<ReadItemRepairNotDoneResponse> ReadRepairNotDone(ReadToRequest request, ServerCallContext context)
        {
            var response = _staffTask.GetStaffTaskNotDone(request.IdTask);
            if (response == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không tìm thấy thông tin"));
            }
            return await Task.FromResult(response);
        }
    }
}
