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
    }
}
