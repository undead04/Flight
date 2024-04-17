using Flight.Authorize;
using Flight.Authorize.DocumentAuthorize;
using Flight.Controllers;
using Flight.Data;
using Flight.Repository.DocumentFlightPermissionRepository;
using Flight.Repository.DocumentFlightRepository;
using Flight.Repository.DocumentTypeRepository;
using Flight.Repository.FlightRepository;
using Flight.Repository.GeneralRepository;
using Flight.Repository.GroupPermissionRepository;
using Flight.Repository.PermissionRepository;
using Flight.Repository.RouteRepository;
using Flight.Repository.UserRepository;
using Flight.Service.ChangeOwnerService;
using Flight.Service.ConfirmDocumentService;
using Flight.Service.FileService;
using Flight.Service.LoginService;
using Flight.Service.PaginationService;
using Flight.Service.ReadTokenService;
using Flight.Service.RoleService;
using Flight.Validation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Data;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginValidation>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });

    option.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddIdentity<ApplicationUser, GroupPermission>(option =>
{
    option.Password.RequiredLength = 8;
    option.Password.RequireLowercase = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireDigit = false;
    option.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@ầằềềờớừửừữặẫẳếềẹễếựửồổõờớồộổỏơớọỏờớườừốửồụủưứứỷỳỹ";
})
    .AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();
builder.Services.AddDbContext<MyDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDb"));
});
// Life cycle DI: AddSingleton(), AddTransient(), AddScoped()

builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IGroupPermissionRepository,GroupPermissionRepository>();
builder.Services.AddScoped<IPermissionRepository,PermissionRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IReadTokenService, ReadTokenService>();
builder.Services.AddScoped<IDocumentTypeRepository,DocumentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IPermissionRepository,PermissionRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IDocumentFlightRepository, DocumentFLightRepository>();
builder.Services.AddScoped<IDocumenttFlightPermissionRepository,DocumentFlightPermissionRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddSingleton<IAuthorizationHandler, RoleRequirementHandler>();
builder.Services.AddScoped<IChangeOwnerService, ChangeOwerService>();
builder.Services.AddScoped<IConfirmDocumentService, ConfirmDocumentService>();
builder.Services.AddScoped<IPaginationService, PaginationService>();
builder.Services.AddScoped<IDocumentAuthorize, DocumentAuthorize>();
builder.Services.AddScoped<IGeneralRepository, GeneralRepository>();
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});
// add authozire
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireRole", policy =>
        policy.Requirements.Add(new RoleRequirement()));
});
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
// phân quyền

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();
