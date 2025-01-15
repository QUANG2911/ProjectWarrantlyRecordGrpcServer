using Google.Api;
using Grpc.Core;
using ProjectWarrantlyRecordGrpcServer.Interface;
using System.IdentityModel.Tokens.Jwt;

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

            //dùng khi kiểm tra postman
            ////tách lấy mỗi chuỗi token chuyền qua
            //var tokenString = token.Header.FirstOrDefault().Value;
            ////đọc chuỗi token trên mã JWT
            //var AccessToken = handler.ReadJwtToken(tokenString.ToString());

            var idStaffFromToken = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (idStaffFromToken == null || idStaffFromToken != idStaff.ToString())
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Mã nhân viên này không thể sở hữu được token này"));
            }

            return "done";
        }
    }
}
