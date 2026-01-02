using Microsoft.EntityFrameworkCore;
using GestionUsersMVC.Data;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURATION DE LA TAILLE D'UPLOAD ---
long maxUploadSize = 50_000_000;
builder.Services.Configure<KestrelServerOptions>(options => options.Limits.MaxRequestBodySize = maxUploadSize);
builder.Services.Configure<IISServerOptions>(options => options.MaxRequestBodySize = maxUploadSize);

// --- 2. SERVICES ---
builder.Services.AddControllersWithViews();

// --- 3. BASE DE DONNÉES ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 4. CONFIGURATION DES SESSIONS ---
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Augmenté à 60min pour plus de confort
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".ScienceStore.Session"; // Nom unique pour éviter les conflits
});

// Facultatif : nécessaire si vous avez des problèmes de consentement de cookies
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => false; // Autorise les cookies de session sans bannière
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

var app = builder.Build();

// --- 5. PIPELINE HTTP ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Placement CRITIQUE de UseSession
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();