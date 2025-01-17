using Microsoft.EntityFrameworkCore;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Protos;

namespace ProjectWarrantlyRecordGrpcServer.Services.Logic
{
    public class RepairPartService : IRepairPart
    {
        private readonly ApplicationDbContext _context;

        public RepairPartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetListRepairPartResponse> GetListRepairPart()
        {
            var listRepairPart = await _context.RepairParts.ToListAsync();
            var response = new GetListRepairPartResponse();
            foreach (var item in listRepairPart)
            {
                response.ToListRepairPast.Add(new GetRepairPartResponse
                {
                    IdRepairPart = item.IdRepairPart,
                    Price = item.Price,
                    RepairPartName = item.RepairPartName,
                });
            }
            return response;
        }
    }
}
