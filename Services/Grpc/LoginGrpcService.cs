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
        private readonly ITokenService _tokenService;
        public LoginGrpcService(ILoginService loginService, ILogger<LoginGrpcService> logger, ITokenService tokenService)
        {
            _logger = logger;
            _loginService = loginService;
            _tokenService = tokenService;
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
            var token = _tokenService.GetToken(request.IdStaff);
            if (token == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Lỗi tạo token"));
            }
            _logger.LogInformation("Thông tin đăng nhập đã truyền là IdStaff: {" + request.IdStaff + "} pass: {" + request.Pass + "} và kết quả trả ra là Position:{" + response + "}");
            return await Task.FromResult(new GetLoginResponse { StaffPosition = response, Token = token });
        }
    }
}
