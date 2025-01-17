using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Services.Logic
{
    public class WarrantyRecordService : IWarranyRecordService
    {
        private readonly ApplicationDbContext _context;

        public WarrantyRecordService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetWarrantyListResponse> GetListWarrantyList()
        {
            var listWarrantyRecord = await Task.FromResult( 
                                            from wr in _context.WarrantyRecords.AsNoTracking()
                                            from cs in _context.Customers.AsNoTracking()
                                            from dv in _context.CustomerDevices.AsNoTracking()
                                            where wr.IdCustomer == cs.IdCustomer && dv.IdDevice == wr.IdDevice
                                            select new
                                            {
                                                 wr.IdWarrantRecord,
                                                 wr.TimeEnd,
                                                 wr.DateOfResig,
                                                 cs.CustomerName,
                                                 dv.DeviceName,
                                                 cs.IdCustomer,
                                            });
            if (listWarrantyRecord == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Không có phiếu bảo hành này"));
            }    
            var response = new GetWarrantyListResponse();
            foreach (var item in listWarrantyRecord)
            {
                response.ToWarrantyList.Add(new GetWarrantyResponse
                {
                    IdCustomer = item.IdCustomer,
                    CustomerName = item.CustomerName,
                    DateOfResig = item.DateOfResig.ToString(),
                    DeviceName = item.DeviceName,
                    TimeEnd = item.TimeEnd.ToString(),
                    IdWarrantyRecord = item.IdWarrantRecord
                });
            }
            return response;
        }
    }
}
