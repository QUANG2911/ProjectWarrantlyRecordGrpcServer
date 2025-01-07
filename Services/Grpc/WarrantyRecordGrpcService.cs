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
            _logger.LogInformation("Thông tin nhân viên truy xuất danh sách phiếu bảo hành IdStaff: {" + request.IdStaff + "} và kết quả trả ra là response:{" + response + "}");
            return await Task.FromResult(response);
        }
    }
}
