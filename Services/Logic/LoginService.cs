using Grpc.Core;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICheckOut _checkOut;
        public LoginService( ICheckOut checkOut)
        {
            _checkOut = checkOut;
        }

        public async Task<string> GetLogin(int idStaff, string password)
        {
            var Staff = await _checkOut.CheckStaffLoginByIdStaffPassAsync(idStaff, password);
            return Staff.StaffPosition;
        }
    }
}
