using SkincareProductSalesSystem.Services.Configs;
using SkincareProductSalesSystem.Repositories.Database;
using SkincareProductSalesSystem.Services;
using System.Text.Json.Serialization;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Services.ExtendServices;
using SkincareProductSalesSystem.Services.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using SkincareProductSalesSystem.Repositories.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

//App Services
builder.Services.AddGrpc();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailServices, OrderDetailServices>();
builder.Services.AddScoped<IBrandService, BrandServices>();
builder.Services.AddScoped<IPaymentServices, PaymentServices>();
builder.Services.AddScoped<IPaymentMethodServices, PaymentMethodServices>();
builder.Services.AddScoped<ISkinTestService, SkinTestService>();
builder.Services.AddScoped<ISkinTypeService, SkinTypeService>();
builder.Services.AddScoped<IChatBotService, ChatBotService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IPromotionUsageService, PromotionUsageService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddSingleton<FirebaseAuth>(_ =>
{
    var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "firebase-appsettings.json");
    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile(credentialPath)
    });
    return FirebaseAuth.DefaultInstance;
});
//Google 
//builder.Services.AddAuthentication()
//            .AddGoogle(options =>
//            {
//                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//                options.CallbackPath = "/signin-google";
//            });

builder.Services.AddControllersWithViews();
//Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = async context =>
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    context.Fail("UnAuthenticated");
                    return;
                }

                try
                {
                    var firebaseAuth = context.HttpContext.RequestServices.GetRequiredService<FirebaseAuth>();
                    FirebaseToken decodedToken = await firebaseAuth.VerifyIdTokenAsync(token);

                    if (decodedToken == null)
                    {
                        context.Fail("UnAuthenticated");
                        return;
                    }
                    UserRecord firebaseUser = await firebaseAuth.GetUserAsync(decodedToken.Uid);
                    if (firebaseUser == null)
                    {
                        context.Fail("UnAuthenticated");
                        return;
                    }
                    // Get UserService from DI Container
                    var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

                    //Check user
                    User user = await userService.GetUserAsync(firebaseUser.Uid);
                    if (user == null)
                        // create user 
                        user = await userService.CreateViaFirebase(firebaseUser);


                    // Get claims
                    var claims = decodedToken.Claims
                        .Select(c => new System.Security.Claims.Claim(c.Key, c.Value.ToString()))
                        .ToList();

                    // Add role to claims
                    claims.Add(new System.Security.Claims.Claim(ClaimTypes.Role, user.RoleName));

                    context.Principal = new System.Security.Claims.ClaimsPrincipal(
                        new System.Security.Claims.ClaimsIdentity(claims, "Firebase"));

                    context.Success();
                }
                catch (UnauthorizedAccessException ex)
                {
                    context.Fail($"Firebase authentication failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
        };
    });



//Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
});


//var config = new ConfigurationBuilder()
//.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
//.AddJsonFile("appsettings.json")
//.Build();
//builder.Services.AddSingleton<IConfiguration>(config);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.Run();