using Microsoft.AspNetCore.RateLimiting;

using MySqlConnector;

using Smart.Data;
using Smart.Data.Accessor;
using Smart.Data.Accessor.Extensions.DependencyInjection;

using WebPerformance.Accessors;
using WebPerformance.Models;
using WebPerformance.Settings;

var builder = WebApplication.CreateBuilder(args);

var setting = builder.Configuration.GetSection("Server").Get<ServerSetting>()!;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddSingleton<IDbProvider>(new DelegateDbProvider(() => new MySqlConnection(connectionString)));
builder.Services.AddDataAccessor();

builder.Services.AddRateLimiter(config =>
{
    config.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromMilliseconds(setting.RateLimit.Window);
        options.PermitLimit = setting.RateLimit.PermitLimit;
        options.QueueLimit = setting.RateLimit.QueueLimit;
    });
    config.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

var app = builder.Build();

// Prepare
var accessor = app.Services.GetRequiredService<IAccessorResolver<IDataAccessor>>().Accessor;
var count = await accessor.CountAsync().ConfigureAwait(false);
if (count == 0)
{
    var provider = app.Services.GetRequiredService<IDbProvider>();
    var con = provider.CreateConnection();
    await con.OpenAsync().ConfigureAwait(false);
    var tx = con.BeginTransaction();
    for (var i = 1; i <= 1000; i++)
    {
        await accessor.InsertAsync(tx, new DataEntity { Id = $"{i:D13}", Name = $"Data-{i}" }).ConfigureAwait(false);
    }

    await tx.CommitAsync().ConfigureAwait(false);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRateLimiter();

await app.RunAsync().ConfigureAwait(false);
