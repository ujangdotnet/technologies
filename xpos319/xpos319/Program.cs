using Microsoft.EntityFrameworkCore;
using xpos319.datamodels;
using xpos319.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<VariantService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<OrderService>();

//add session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromHours(1);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});


//add connection string
builder.Services.AddDbContext<XPOS_319Context>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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

//add session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
