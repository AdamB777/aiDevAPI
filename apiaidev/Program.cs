
using Application.Config;
using Application.CQRS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<Settings>();

//ExternalServiceSettings configuration
builder.Services.Configure<Settings>(builder.Configuration.GetSection("ExternalService"));


//builder.Services.Configure<ExternalServiceSettings>(
//    builder.Configuration.GetSection("ExternalService"));

//HttpClient
builder.Services.AddHttpClient();

//builder.Services.AddCors();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(TokenPost).Assembly
    ));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000", "http://localhost:5000", "https://zadania.aidevs.pl")
       
       
       ;
});


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
