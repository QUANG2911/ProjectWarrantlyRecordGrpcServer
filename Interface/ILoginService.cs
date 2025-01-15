namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface ILoginService
    {
       string GetLogin(int idStaff, string password);
    }
}
