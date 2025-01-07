using Grpc.Core;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.DTO;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;
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
        public GetListCustomerManagementResponse GetListCustomer()
        {
            var listCustomer = _context.Customers.ToList();
            var response = new GetListCustomerManagementResponse();
            foreach (var item in listCustomer)
            {
                response.ToCustomerList.Add(new GetItemInListCustomerResponse
                {
                    IdCusomer = item.IdCustomer,
                    CustomerName = item.CustomerName,
                    CustomerPhone = item.CustomerPhone,
                    CustomerEmail = item.CustomerEmail,
                    CustomerAdrress = item.CustomerAddress
                });
            }
            return response;
        }

        public ReadCustomerManagementResponse GetDetailCustomer(int IdCustomer)
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

            var response = new ReadCustomerManagementResponse();

            foreach (var item in listDeviceOfCustomer)
            {
                response.ToDeviceList.Add(new ReadItemDeviceCustomerManagementResponse
                {
                    IdCusomer = customer.IdCustomer,
                    CustomerName = customer.CustomerName,
                    CustomerEmail = customer.CustomerEmail,
                    CustomerPhone = customer.CustomerPhone,
                    CustomerAdrress = customer.CustomerAddress,
                    CustomerDevice = item.DeviceName,
                    IdWarrantReport = item.IdWarrantRecord,
                    DateOfWarrant = item.DateOfResig.ToString(),
                    TimeEnd = item.TimeEnd.ToString(),
                });
            }

            return response;
        }
    }
}
