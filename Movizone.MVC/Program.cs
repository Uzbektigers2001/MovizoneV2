using Minio;
using Movizone.MVC.Interfaces;
using Movizone.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var endpoint = "192.168.1.140:9000";
var accesskey = "CcjWBl1HXig5YFklpFIO";
var secretkey = "FrbNXKZNEOXfyZXj4CWiIqhmccs3z8tt5SUQwHeV";
var minio = new MinioClient()
    .WithEndpoint(endpoint)
    .WithCredentials(accesskey,secretkey)
    .WithSSL(false)
    .Build();


builder.Services.AddSingleton(minio);
builder.Services.AddSingleton<IMinioService, MinioService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FirstHome}/{action=Index}/{id?}");

app.Run();
