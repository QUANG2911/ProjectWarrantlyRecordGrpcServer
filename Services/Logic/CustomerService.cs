using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Model;
using ProjectWarrantlyRecordGrpcServer.Protos;
using System;

namespace ProjectWarrantlyRecordGrpcServer.Services.Logic
{
    public class CustomerService : ICustomerService
    {
        private readonly IDataService _dataService;
        public CustomerService( IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<GetListCustomerManagementResponse> GetListCustomer()
        {
            var response =  await _dataService.GetListCustomerAsync();
            return response;
        }

        public async Task<ReadCustomerManagementResponse> GetDetailCustomer(int idCustomer)
        {
            var response = await _dataService.GetListDetalOfCustomerAsync(idCustomer);
            return response;
        }
    }
}
