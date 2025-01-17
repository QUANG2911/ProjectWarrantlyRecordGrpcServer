using ProjectWarrantlyRecordGrpcServer.Model;

namespace ProjectWarrantlyRecordGrpcServer.Interface
{
    public interface ICheckOut
    {
        // CHECK idWarrantRecord
        Task<WarrantyRecord> CheckWarrantyRecordByIdWarrantAsync(int idWarrantRecord);
        Task CheckStaffTaskByIdWarrantAsync(int idWarrantRecord);


        // CHECK idTask
        Task<StaffTask> CheckStaffTaskByIdTaskAsync(int idTask);

        // CHECK idCustomer
        Task<Customer> CheckCustomerByIdCustomerAsync(int idCustomer);


        // CHECK idStaff
        Task<Staff> CheckStaffByIdStaffAsync(int idStaff);


        //CHECK LOGIN
        Task<Staff> CheckStaffLoginByIdStaffPassAsync(int idStaff, string pass);

    }
}
