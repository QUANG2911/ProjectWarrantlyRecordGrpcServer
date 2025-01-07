using Grpc.Core;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Services.Grpc
{
    public class RepairPartGrpcService : RepairPartManagement.RepairPartManagementBase
    {
        private readonly IRepairPart _repairPart;
        private readonly ILogger<RepairPartGrpcService> _logger;

        public RepairPartGrpcService(IRepairPart repairPart, ILogger<RepairPartGrpcService> logger)
        {
            _repairPart = repairPart;
            _logger = logger;
        }

        public override async Task<GetListRepairPartResponse> ListRepairPartManagement(GetListRepairPartRequest request, ServerCallContext context)
        {
            var response = _repairPart.GetListRepairPart();

            if (response.ToListRepairPast.Count == 0) {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không có danh sách linh kiện ??"));
            }
            _logger.LogInformation("Thông tin truy xuất danh sách linh kiện sửa chữa: {" + request + "} và kết quả trả ra là response:{" + response + "}");
            return await Task.FromResult(response);
        }
    }
}
