using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectWarrantlyRecordGrpcServer.Services.Logic
{
    public class LoginService :ILoginService
    {
        private readonly ApplicationDbContext _context;

        public LoginService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetLogin(int idStaff, string password)
        {
            var staff = _context.Staffs.Where( p=> p.IdStaff == idStaff && password == p.Pass).FirstOrDefault();
            if (staff == null) {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không tìm thấy thông tin tài khoản nhân viên này"));
            }
            return staff.StaffPosition;
        }
    }
}
