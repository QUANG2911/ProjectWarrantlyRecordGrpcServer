using Microsoft.EntityFrameworkCore;
using ProjectWarrantlyRecordGrpcServer.Model;

namespace ProjectWarrantlyRecordGrpcServer.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerDevices> CustomerDevices { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StaffTask> StaffTasks { get; set; }
        public DbSet<RepairDetail> RepairDetails { get; set; }
        public DbSet<RepairPart> RepairParts { get; set; }
        public DbSet<WarrantyRecord> WarrantyRecords { get; set; }
        public DbSet<Bill> Bills { get; set; }


    }
}
