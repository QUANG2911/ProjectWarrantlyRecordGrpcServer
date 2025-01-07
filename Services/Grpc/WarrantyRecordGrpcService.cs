using Grpc.Core;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Services.Grpc
{
    public class WarrantyRecordGrpcService : WarrantyRecordManagement.WarrantyRecordManagementBase
    {
        private readonly IWarranyRecordService _warrantyRecordService;
        private readonly ILogger<WarrantyRecordGrpcService> _logger;

        public WarrantyRecordGrpcService(IWarranyRecordService warranyRecordService, ILogger<WarrantyRecordGrpcService> logger)
        {
            _warrantyRecordService = warranyRecordService;
            _logger = logger;
        }

        public override async Task<GetWarrantyListResponse> GetListWarrantyRecordManagement(GetWarrantyListRequest request, ServerCallContext context)
        {
            var response = _warrantyRecordService.GetListWarrantyList();
            
            return await Task.FromResult(response);
        }
    }
}
