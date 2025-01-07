using Microsoft.EntityFrameworkCore;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Services;
using ProjectWarrantlyRecordGrpcServer.Services.Grpc;
using ProjectWarrantlyRecordGrpcServer.Services.Logic;

var builder = WebApplication.CreateBuilder(args);

// add Builde DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder services Interface
builder.Services.AddScoped<IStaffTaskService, StaffTaskService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGrpcService<StaffTaskGrpcService>();
app.MapGrpcService<CustomerGrpcService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
