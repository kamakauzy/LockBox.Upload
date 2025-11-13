var builder = WebApplication.CreateBuilder(args);

// DI registration - one liner
builder.Services.AddSingleton(builder.Configuration.GetSection("FileUploadConfig").Get<FileUploadConfig>()!);

builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
app.Run();