﻿namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface ILoginService
    {
       string GetLogin(int idStaff, string password);

       string GetToken(int idStaff);
    }
}
