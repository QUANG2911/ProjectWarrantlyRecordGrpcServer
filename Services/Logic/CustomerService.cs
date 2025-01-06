using Grpc.Core;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.DTO;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Model;
using System;

namespace ProjectWarrantlyRecordGrpcServer.Services.Logic
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Customer> GetListCustomer()
        {
            var customer = _context.Customers.ToList();

            return customer;
        }

        public List<DetailCustomerDto> GetDetailCustomer(int IdCustomer)
        {
            var customer = _context.Customers.Where(p => p.IdCustomer == IdCustomer).FirstOrDefault();
            if (customer == null) {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không tồn tại thông tin khách hàng này ??"));
            }
            var listDeviceOfCustomer = from wr in _context.WarrantyRecords.Where(p => p.IdCustomer == IdCustomer)
                                       from dc in _context.CustomerDevices
                                       where dc.IdDevice == wr.IdDevice
                                       select new
                                       {
                                           dc.DeviceName,
                                           wr.IdWarrantRecord,
                                           wr.TimeEnd,
                                           wr.DateOfResig
                                       };


            List<DetailCustomerDto> listDetailCustomer = new List<DetailCustomerDto>();

            foreach (var wr in listDeviceOfCustomer)
            {
                DetailCustomerDto detailCustomerDto = new DetailCustomerDto 
                {
                    IdCusomer = customer.IdCustomer,
                    CustomerName = customer.CustomerName,                    
                    CustomerEmail = customer.CustomerEmail,
                    CustomerPhone = customer.CustomerPhone,
                    CustomerAdrress = customer.CustomerAddress,
                    CustomerDevice = wr.DeviceName,
                    IdWarrantReport = wr.IdWarrantRecord,
                    DateOfWarrant = wr.DateOfResig,                    
                    TimeEnd =wr.TimeEnd,                    
                };
                listDetailCustomer.Add(detailCustomerDto);
            }
            return listDetailCustomer;
        }
    }
}
