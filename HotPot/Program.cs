using HotPot.Contexts;
using HotPot.Interfaces;
using HotPot.Models;
using HotPot.Repositories;
using HotPot.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace HotPot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactPolicy", opts =>
                {
                    opts.WithOrigins("http://localhost:3000", "http://localhost:3003", "http://localhost:3002").AllowAnyMethod().AllowAnyHeader();
                });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            builder.Services.AddDbContext<RequestTrackerContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("requestTrackerConnection"));
            });

            //for repositories
            builder.Services.AddScoped<IRepository<int, String, City>, CityRepository>();
            builder.Services.AddScoped<IRepository<int, String, Restaurant>, RestaurantRepository>();
            builder.Services.AddScoped<IRepository<int, String, Menu>, MenuRepository>();
            builder.Services.AddScoped<IRepository<int, String, Payment>, PaymentRepository>();
            builder.Services.AddScoped<IRepository<int, String, Order>, OrderRepository>();
            builder.Services.AddScoped<IRepository<int, String, User>, UserRepository>();
            builder.Services.AddScoped<IRepository<int, String, Customer>, CustomerRepository>();
            builder.Services.AddScoped<IRepository<int, String, Menu>, MenuRepository>();
            builder.Services.AddScoped<IRepository<int, String, Cart>, CartRepository>();
            builder.Services.AddScoped<IRepository<int, String, Order>, OrderRepository>();
            builder.Services.AddScoped<IRepository<int, String, OrderItem>, OrderItemRepository>();
            builder.Services.AddScoped<IRepository<int, String, Payment>, PaymentRepository>();
            builder.Services.AddScoped<IRepository<int, String, RestaurantOwner>, RestaurantOwnerRepository>();
            builder.Services.AddScoped<IRepository<int, String, CustomerAddress>, CustomerAddressRepository>();
            builder.Services.AddScoped<IRepository<int, String, CustomerReview>, CustomerReviewRepository>();
            builder.Services.AddScoped<IRepository<int, String, DeliveryPartner>, DeliveryPartnerRepository>();
            builder.Services.AddScoped<IRepository<int, String, RestaurantSpeciality>, RestaurantSpecialitiesRepository>();

            //for services
            builder.Services.AddScoped<IRestaurantUserServices, RestaurantUserServices>();
            builder.Services.AddScoped<ICustomerServices, CustomerServices>();
            builder.Services.AddScoped<ITokenServices, TokenServices>();
            builder.Services.AddScoped<IDeliveryPartnerServices, DeliveryPartnerServices>();
            builder.Services.AddScoped<IAdminServices, AdminServices>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("ReactPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}