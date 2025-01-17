using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectWarrantlyRecordGrpcServer.Data;
using ProjectWarrantlyRecordGrpcServer.Interface;
using ProjectWarrantlyRecordGrpcServer.Services.Grpc;
using ProjectWarrantlyRecordGrpcServer.Services.Logic;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// add Builde DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// add Serilog -> folder log tự sinh
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Information()
//    .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day) //Chọn rolling DAY HAY MINUTE THÌ NÓ SẼ LOADING LOG THEO KIỂU CHỈ ĐỊNH
//    .CreateLogger();

builder.Host.UseSerilog((context, config) =>
{
    config
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
});
//Thay thế hệ thống logging mặc định của ASP.NET Core bằng Serilog
builder.Host.UseSerilog();

// builder services Interface
builder.Services.AddScoped<IStaffTaskService, StaffTaskService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRepairPart, RepairPartService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IWarranyRecordService, WarrantyRecordService>();
builder.Services.AddScoped<IMailSevice, EmailSevice>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICheckOut,CheckOutService>();
builder.Services.AddScoped<IDataService, DataService>();
// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();


// Cấu hình kết nối angular
//session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true; // Chỉ cho phép truy cập qua HTTP
    options.Cookie.IsEssential = true; // Cookie cần thiết cho session
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Chỉ gửi cookie qua HTTPS
});
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Authorization", "Cache-Control", "Content-Type"); // Tùy chọn: Cho phép các headers này xuất hiện trong response
    });
});


var app = builder.Build();

// app Cấu hình kết nối angular
//app.UseCors("AllowAngularApp");

app.UseCors("AllowAllOrigins");
app.UseSession();
// cho phép quyền hạn để truy cập -- khai báo thêm bên controller
app.Use(async (context, next) =>
{
    if (context.Request.Method == HttpMethods.Options)
    {
        context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, OPTIONS, PUT, DELETE");
        context.Response.Headers.Append("Access-Control-Allow-Headers", "Authorization, Content-Type, Cache-Control");
        context.Response.StatusCode = 200;
        return;
    }
    await next();
});
//////



// Configure the HTTP request pipeline.

app.MapGrpcService<StaffTaskGrpcService>();
app.MapGrpcService<CustomerGrpcService>();
app.MapGrpcService<RepairPartGrpcService>();
app.MapGrpcService<LoginGrpcService>();
app.MapGrpcService<WarrantyRecordGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



// Middleware để xử lý Preflight Request (OPTIONS)
app.UseAuthorization();
app.MapControllers();