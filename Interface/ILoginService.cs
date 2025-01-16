namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface ILoginService
    {
        Task<string> GetLogin(int idStaff, string password);
    }
}
