using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Infrastructure.MappingProfiles;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services;
using AutoMapper;
using Azure.Storage.Blobs;
using PdfSharp.Fonts;
using Domain.Helpers.QueryObject;

namespace Presentation
{
     internal abstract class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
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


            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            var connectionString = builder.Configuration["ConnectionString"];
            builder.Services.AddDbContext<MovieDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 12;
            })
            .AddEntityFrameworkStores<MovieDbContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!))
                };

                options.Events = new JwtBearerEvents // AccessToken expiration error
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var response = new
                        {
                            status = 401,
                            error = "Unauthorized",
                            message = "The provided token is expired or invalid."
                        };

                        await context.Response.WriteAsJsonAsync(response);
                    }
                };
            });

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAutoMapper(typeof(MapProfile));

            builder.Services.AddSingleton(_ =>
            {
                var connectionString = builder.Configuration["AzureStorage:ConnectionString"];
                return new BlobServiceClient(connectionString);
            });

            builder.Services.AddScoped<IAvatarService, AvatarService>();
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IActorService, ActorService>();
            builder.Services.AddScoped<IActorRoleService, ActorRoleService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IMovieActorService, MovieActorService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<ICinemaTypeService, CinemaTypeService>();
            builder.Services.AddScoped<IUserInfoService, UserInfoService>();
            builder.Services.AddScoped<IFilesGenerationService, FilesGenerationService>();
            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(typeof(MapProfile));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            GlobalFontSettings.FontResolver = new CustomFontResolver();
            GlobalFontSettings.UseWindowsFontsUnderWindows = true;
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
