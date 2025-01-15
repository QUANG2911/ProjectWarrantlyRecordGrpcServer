using Google.Api;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using ProjectWarrantlyRecordGrpcServer.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectWarrantlyRecordGrpcServer.Services.Logic
{
    public class TokenService : ITokenService
    {
        public string CheckTokenIdStaff(int idStaff, ServerCallContext context)
        {
            var authHeader = context.RequestHeaders.FirstOrDefault(h => h.Key == "authorization");

            if (authHeader == null)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Không tìm được token header"));
            }
            var handler = new JwtSecurityTokenHandler();
            //lấy list chuyền từ schema
            var token = handler.ReadJwtToken(authHeader.Value.Replace("Bearer ", ""));


            var idStaffFromToken = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var timeLife = token.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;


            if (idStaffFromToken == null || idStaffFromToken != idStaff.ToString())
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Mã nhân viên này không thể sở hữu được token này"));
            }
            if (timeLife == null)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Không có timelife của token"));
            }    
            int timeNum = int.Parse(timeLife);
            int dateNow = (int)((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            if (timeNum <= dateNow)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Token hết hạn out"));
            }    

            return "done";
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
