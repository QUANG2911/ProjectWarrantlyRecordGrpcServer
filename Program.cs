using Microsoft.EntityFrameworkCore;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Services.Grpc;
using ProjectWarrantlyRecordGrpcServer.Services.Logic;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// add Builde DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// add Serilog -> folder log tự sinh
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day) //Chọn rolling DAY HAY MINUTE THÌ NÓ SẼ LOADING LOG THEO KIỂU CHỈ ĐỊNH
    .CreateLogger();
//Thay thế hệ thống logging mặc định của ASP.NET Core bằng Serilog
builder.Host.UseSerilog();

// builder services Interface
builder.Services.AddScoped<IStaffTaskService, StaffTaskService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRepairPart, RepairPartService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IWarranyRecordService, WarrantyRecordService>();
// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGrpcService<StaffTaskGrpcService>();
app.MapGrpcService<CustomerGrpcService>();
app.MapGrpcService<RepairPartGrpcService>();
app.MapGrpcService<LoginGrpcService>();
app.MapGrpcService<WarrantyRecordGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
