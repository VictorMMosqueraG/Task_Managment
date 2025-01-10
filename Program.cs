using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Interfaces;
using TaskManagement.Repositories;
using TaskManagement.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure the DbContext with the connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//NOTE:  Add repository to the container
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//NOTE: Add service to the container
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<AuthServiceJwt>();
builder.Services.AddScoped<IAuthService,AuthServices>();

//NOTE: JWT Configuration
builder.Services.AddAuthentication(options =>{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    
    var key = jwtSettings["Key"];
    if (string.IsNullOrEmpty(key)){
        throw new InvalidOperationException("JWT key is not configured properly."); //COMEBACK: Handle Error 
    }

    var issuer = jwtSettings["Issuer"];
    var audience = jwtSettings["Audience"];

    if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience)){
        throw new InvalidOperationException("JWT issuer or audience is not configured properly."); //COMEBACK: Handle Error
    }

    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

//NOTE: Add Controllers to the container
builder.Services.AddControllers();

//NOTE: Add Authorization Policies
builder.Services.AddAuthorization(options =>{
    options.AddPolicy("WriteAllPolicy", policy =>
        policy.RequireClaim("Permission", "write.all"));
    options.AddPolicy("WritePolicy", policy =>
        policy.RequireClaim("Permission", "write"));
    options.AddPolicy("ReadAllPolicy", policy =>
        policy.RequireClaim("Permission", "read.all"));
    options.AddPolicy("ReadPolicy", policy =>
        policy.RequireClaim("Permission", "read"));
    options.AddPolicy("UpdateAllPolicy", policy =>
        policy.RequireClaim("Permission", "update.all"));
    options.AddPolicy("DeleteAllPolicy", policy =>
        policy.RequireClaim("Permission", "delete.all"));
});

//NOTE: Build the application
var app = builder.Build();

//NOTE: Middleware for handling errors
app.UseMiddleware<TaskManagement.Middleware.ExceptionHandle>();

//NOTE: Use routing, authentication and authorization
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//Apply migrations (if necessary) at runtime
//NOTE: This is not recommended for production
using (var scope = app.Services.CreateScope()){
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.MapControllers();

app.Run();