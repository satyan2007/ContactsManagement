using ContactsManagement.Server;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("https://localhost:4200") // Allow requests from the Angular app
                .AllowAnyMethod() // Allow any HTTP method (GET, POST, PUT, DELETE, etc.)
                .AllowAnyHeader(); // Allow any headers
        });
});
// Add services to the container.
// Register the DataContactService as a singleton
builder.Services.AddSingleton<DataContactService>(); 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var errorDetails = new { message = "Internal Server Error" };
        await context.Response.WriteAsJsonAsync(errorDetails);
    });
});
// Use CORS policy
app.UseCors("AllowAngularApp");
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
