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
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

//NOTE: Add service to the container
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
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
        throw new UnexpectedErrorException("JWT key is not configured properly.");  
    }
    
    var issuer = jwtSettings["Issuer"];
    var audience = jwtSettings["Audience"];

    if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience)){
        throw new UnexpectedErrorException("JWT issuer or audience is not configured properly."); 
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
        policy.RequireClaim("Permission", "write.all"));//NOTE:Create and update all modules
    options.AddPolicy("WritePolicy", policy =>
        policy.RequireClaim("Permission", "write"));// Create Only Task
    options.AddPolicy("ReadAllPolicy", policy =>
        policy.RequireClaim("Permission", "read.all"));// Read all modules
    options.AddPolicy("ReadPolicy", policy =>
        policy.RequireClaim("Permission", "read"));//Read only Task
    options.AddPolicy("UpdateAllPolicy", policy =>
        policy.RequireClaim("Permission", "update.all"));//Update all modules
     options.AddPolicy("UpdatePolicy", policy =>
        policy.RequireClaim("Permission", "update"));//UPdate only Task
    options.AddPolicy("DeleteAllPolicy", policy =>
        policy.RequireClaim("Permission", "delete.all"));//Delete all Modules
    options.AddPolicy("DeletePolicy", policy =>
        policy.RequireClaim("Permission", "delete"));//Delete only Task

        options.AddPolicy("WriteOrWriteAllPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "Permission" && 
                (c.Value == "write.all" || c.Value == "write"))
        ));

    options.AddPolicy("ReadOrReadAllPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "Permission" && 
                (c.Value == "read.all" || c.Value == "read"))
        ));

    options.AddPolicy("UpdateOrUpdateAllPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "Permission" && 
                (c.Value == "update.all" || c.Value == "update"))
        ));

    options.AddPolicy("DeleteOrDeleteAllPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "Permission" && 
                (c.Value == "delete.all" || c.Value == "delete"))
        ));
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