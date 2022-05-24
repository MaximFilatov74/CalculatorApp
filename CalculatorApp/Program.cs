using CalculatorApp.Services;
using CalculatorApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IPriorityService, PriorityService>();
builder.Services.AddTransient<IParseService, ParseService>();
builder.Services.AddTransient<IVerificationService, VerificationService>();
builder.Services.AddTransient<ICalculationService, CalculationService>();

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
    pattern: "{controller=Calculation}/{action=Index}/{id?}");

app.Run();