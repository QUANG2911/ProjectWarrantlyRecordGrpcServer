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
        private readonly ILogger<CustomerGrpcService> _logger;

        public CustomerGrpcService(ICustomerService customerService, ILogger<CustomerGrpcService> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        public override async Task<GetListCustomerManagementResponse> ListCustomerManagement(GetListCustomerManagementRequest request, ServerCallContext context)
        {
            if (request.IdStaff == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không có dữ liệu mã nhân viên"));
            }
            var response = _customerService.GetListCustomer();
            if (response.ToCustomerList.Count == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không có dữ liệu"));
            }
            _logger.LogInformation("Thông tin nhân viên truy xuất danh sách khách hàng IdStaff: {" + request.IdStaff + "} và kết quả trả ra là response:{" + response + "}");
            return await Task.FromResult(response);
        }

        public override async Task<ReadCustomerManagementResponse> ReadCustomerManagement(ReadCustomerRequest request, ServerCallContext context)
        {
            if(request.IdCusomer == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không được để trống các thông tin truyền"));
            }
            if (request.IdStaff == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không có dữ liệu mã nhân viên"));
            }
            var response = _customerService.GetDetailCustomer(request.IdCusomer);
            _logger.LogInformation("Thông tin nhân viên truy xuất IdStaff: {" + request.IdStaff + "}truy xuất thông tin khách hàng: {" + request.IdCusomer +"} và kết quả trả ra là response:{" + response + "}");
            return await Task.FromResult(response);
        }
    }
}
