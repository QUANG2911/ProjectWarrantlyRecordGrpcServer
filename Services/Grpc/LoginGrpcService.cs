using Grpc.Core;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Services.Grpc
{
    public class LoginGrpcService : LoginManagement.LoginManagementBase
    {
        private readonly ILoginService _loginService;
        private readonly ILogger<LoginGrpcService> _logger;

        public LoginGrpcService(ILoginService loginService, ILogger<LoginGrpcService> logger)
        {
            _logger = logger;
            _loginService = loginService;
        }

        public override async Task<GetLoginResponse> GetLogin(GetLoginRequest request, ServerCallContext context)
        {
            if(request.IdStaff == 0 || request.Pass == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Vui lòng điền đầy đủ thông tin đăng nhập"));
            }    
            var response = _loginService.GetLogin(request.IdStaff, request.Pass);
            if (response == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không tìm thấy thông tin tài khoản nhân viên này"));
            }
            _logger.LogInformation("Thông tin đăng nhập đã truyền là IdStaff: {" + request.IdStaff + "} pass: {" + request.Pass + "} và kết quả trả ra là Position:{" + response + "}");
            return await Task.FromResult(new GetLoginResponse { StaffPosition = response });
        }
    }
}
