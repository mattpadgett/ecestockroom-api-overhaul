using ecestockroom_api_v2.Contracts.Configuration;
using ecestockroom_api_v2.Database;
using ecestockroom_api_v2.Helpers.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJWTBearer(builder.Configuration["JwtSettings:Key"],
    builder.Configuration["JwtSettings:Issuer"], builder.Configuration["JwtSettings:Audience"]);

builder.Services.AddSwaggerDoc();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policyBuilder => { policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services.Configure<PostgresDbSettings>(builder.Configuration.GetSection(nameof(PostgresDbSettings)));

#region Dependency Injection

builder.Services.AddSingleton<IJwtSettings>(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);
builder.Services.AddSingleton<IPostgresDbSettings>(sp => sp.GetRequiredService<IOptions<PostgresDbSettings>>().Value);

builder.Services.AddDbContext<StockroomDbContext>(
    o => { o.UseNpgsql(builder.Configuration["PostgresDbSettings:ConnectionString"]); }, ServiceLifetime.Singleton);

builder.Services.AddSingleton<JwtHelpers>();

builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<RoleService>();
builder.Services.AddSingleton<PermissionService>();
builder.Services.AddSingleton<TokenFamilyService>();

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3(s => s.ConfigureDefaults());
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();

app.Run();