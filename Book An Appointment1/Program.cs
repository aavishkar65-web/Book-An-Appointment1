using Book_An_Appointment1.AppService;
using Book_An_Appointment1.Handlers;
using Book_An_Appointment1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();


// ── Session ─────────────────────────────
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// ── Services ───────────────────────────
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ClientBookAppoints>();
builder.Services.AddTransient<AuthHandler>();


var baseUrl = builder.Configuration["ApiSettings:BaseUrl"]!;


// ── TokenClient (NO handler) 🔥 ────────
builder.Services.AddHttpClient("TokenClient", client =>
{
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});


// ── ApiClient (WITH handler) 🔥 ────────
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddHttpMessageHandler<AuthHandler>();


// ──────────────────────────────────────
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();   // 🔥 IMPORTANT (yahi rehna chahiye)

app.UseAuthorization();

app.MapRazorPages();

app.Run();