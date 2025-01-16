using Grpc.Core;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Services.Grpc
{
    public class WarrantyRecordGrpcService : WarrantyRecordManagement.WarrantyRecordManagementBase
    {
        private readonly IWarranyRecordService _warrantyRecordService;
        private readonly ILogger<WarrantyRecordGrpcService> _logger;
        private readonly ITokenService _tokenService;
        public WarrantyRecordGrpcService(IWarranyRecordService warranyRecordService, ILogger<WarrantyRecordGrpcService> logger, ITokenService tokenService)
        {
            _warrantyRecordService = warranyRecordService;
            _logger = logger;
            _tokenService = tokenService;
        }

        public override async Task<GetWarrantyListResponse> GetListWarrantyRecordManagement(GetWarrantyListRequest request, ServerCallContext context)
        {
            var checkToken =  await _tokenService.CheckTokenIdStaff(request.IdStaff, context);
            if (checkToken != "done")
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Lỗi thông tin token nhận được"));
            }
            var response = await _warrantyRecordService.GetListWarrantyList();
            _logger.LogInformation("Thông tin nhân viên truy xuất danh sách phiếu bảo hành IdStaff: {" + request.IdStaff + "} và kết quả trả ra là response:{" + response + "}");
            return await Task.FromResult(response);
        }
    }
}
