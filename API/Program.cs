using API.Extensions;
using API.Middleware;
using Core.Entites.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddApplicartionservices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
// this below lines cmntd in swagger 1 video if we remove if  condition swagger document will used in btoh dev & prod mode aswell
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
 app.UseSwagger();
    app.UseSwaggerUI();

app.UseStaticFiles();

app.UseCors("CorsPolicy");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
using var scope=app.Services.CreateScope();
var services=scope.ServiceProvider;
var context=services.GetRequiredService<StoreContext>();
var identitycontext=services.GetRequiredService<AppIdentityDbContext>();
var userManager=services.GetRequiredService<UserManager<AppUser>>();
var logger=services.GetRequiredService<ILogger<Program>>();
try
{
await context.Database.MigrateAsync();
await identitycontext.Database.MigrateAsync();
await StoreContextSeed.SeedAsync(context);
await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
}
catch(Exception ex)
{
    logger.LogError(ex,"An Error Occured during migrartion");
}


app.Run();
