using Grpc.Core;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;
using System.Collections.Generic;

namespace ProjectWarrantlyRecordGrpcServer.Services.Grpc
{
    public class CustomerGrpcService : CustomerManagement.CustomerManagementBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<StaffTaskGrpcService> _logger;

        public CustomerGrpcService(ICustomerService customerService, ILogger<StaffTaskGrpcService> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        public override async Task<GetListCustomerManagementResponse> ListCustomerManagement(GetListCustomerManagementRequest request, ServerCallContext context)
        {
            var listCustomer = _customerService.GetListCustomer();
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
            return await Task.FromResult(response);
        }

        public override async Task<ReadCustomerManagementResponse> ReadCustomerManagement(ReadCustomerRequest request, ServerCallContext context)
        {
            if(request.IdCusomer == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không được để trống các thông tin truyền"));
            }
            var listDetailCustomer = _customerService.GetDetailCustomer(request.IdCusomer);

            var response = new ReadCustomerManagementResponse();

            foreach (var item in listDetailCustomer)
            {
                response.ToDeviceList.Add(new ReadItemDeviceCustomerManagementResponse
                {
                    IdCusomer = item.IdCusomer,
                    CustomerName = item.CustomerName,
                    CustomerEmail = item.CustomerEmail,
                    CustomerPhone = item.CustomerPhone,
                    CustomerAdrress = item.CustomerAdrress,
                    CustomerDevice = item.CustomerDevice,
                    IdWarrantReport = item.IdWarrantReport,
                    DateOfWarrant = item.DateOfWarrant.ToString(),
                    TimeEnd = item.TimeEnd.ToString(),
                });
            }
            return await Task.FromResult(response);
        }
    }
}
