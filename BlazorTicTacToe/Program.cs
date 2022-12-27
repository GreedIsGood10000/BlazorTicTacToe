using BlazorTicTacToe.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

static async void Initialize(IServiceProvider services)
{
    using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
    {
        var manager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var user = new IdentityUser { UserName = "yeashik@mail.ru" };
        var result = await manager.CreateAsync(user, "1");


        var user2 = new IdentityUser { UserName = "yeashik2@mail.ru" };
        var result2 = await manager.CreateAsync(user2, "1");


        //var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<IdentityUser>>();
        //await signInManager.PasswordSignInAsync("yeashik@mail.ru", "1", true, lockoutOnFailure: false);
    }
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("test"));

builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 1;
        options.Password.RequiredUniqueChars = 1;
    });

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<IGameFieldService, GameFieldService>();
builder.Services.AddSingleton<IGameRoomService, GameRoomService>();
builder.Services.AddSingleton<IStatsService, StatsService>();

var app = builder.Build();

Initialize(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.UseAuthentication();;

app.Run();
