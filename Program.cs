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

//NOTE Add service to Swagger

// Add Swagger with XML comments
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>{

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo{
        Title = "API de Gestión de Tareas",
        Version = "v1",
        Description = "Documentación de la API de Gestión de Tareas",
    });


        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme{
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Por favor, ingresa el token JWT en el formato: Bearer {token}",
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            BearerFormat = "JWT"
        });

        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement{
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
});


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


//NOTE: Razor
builder.Services.AddRazorPages();

//NOTE: Build the application
var app = builder.Build();

//NOTE: Middleware for Swagger
if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = string.Empty; 
    });
}

//NOTE: Middleware for handling errors
app.UseMiddleware<TaskManagement.Middleware.ExceptionHandle>();

//NOTE: Use routing, Map Razor, authentication and authorization
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.UseStaticFiles();// using for load files type css to html

//Apply migrations (if necessary) at runtime
//NOTE: This is not recommended for production
using (var scope = app.Services.CreateScope()){
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.MapControllers();

app.Run();