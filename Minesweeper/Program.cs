var builder = WebApplication.CreateBuilder(args);
var configurationName = "AllowAll";

builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy(configurationName,
        builder =>
        {
            builder
                .AllowAnyHeader()
                .SetIsOriginAllowed(_ => true);
        });
});

var app = builder.Build();

app.UseRouting();
app.UseCors(configurationName);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();