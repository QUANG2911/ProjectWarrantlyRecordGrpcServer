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

        public string GetToken(int idStaff)
        {
            var secretKey = "ThisIsA32CharLongSecretKey12345_123_344_122";
            
            // Tạo JWT token
            var claims = new List<Claim> // mã xác thực sau này
            {
                new Claim(JwtRegisteredClaimNames.Sub, idStaff.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7059",
                audience: "https://localhost:7059",
                expires: DateTime.UtcNow.AddDays(1), // set timeLife ở đây để qua kia set exp
                claims: claims,
                signingCredentials: creds);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
