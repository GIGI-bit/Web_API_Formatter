using Microsoft.EntityFrameworkCore;
using WbApiDemo3_22_5.Formatters;
using Web_API_Formatter.Data;
using Web_API_Formatter.Formatters;
using Web_API_Formatter.Repository.Abstract;
using Web_API_Formatter.Repository.Concrete;
using Web_API_Formatter.Services.Abstract;
using Web_API_Formatter.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Insert(0, new VCardOutputFormatter());
    options.InputFormatters.Insert(0, new VCardInputFormatter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();

var con = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<SchoolDbContext>(opt =>
{
    opt.UseSqlServer(con);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
