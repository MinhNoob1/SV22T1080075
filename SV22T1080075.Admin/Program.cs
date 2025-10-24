var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Khởi tạo cấu hình cho Business Layer
string connectionString = builder.Configuration.GetConnectionString("LiteCommerceDB") ?? throw new Exception("Connection Error");
SV22T1080075.BusinessLayers.Configuration.Initialize(connectionString);

app.Run();
