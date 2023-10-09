using System.Reflection;
using System.Text;
using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyFreezer.API.GraphQL.Queries;
using MyFreezer.API.GraphQL.Schemas;
using MyFreezer.API.GraphQL.Types.Identity;
using MyFreezer.API.GraphQL.Types.UserTypes;
using MyFreezer.API.Repositories;
using MyFreezer.API.Repositories.Interfaces;


var builder = WebApplication.CreateBuilder(args);

AuthOptions.key = builder.Configuration["JWT:Key"];

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["JWT:Author"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
    };
});

builder.Services.AddAuthorization();


builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IUserRepository, UserDapperRepository>();
builder.Services.AddTransient<IAuthorizationRepository, AuthorizationDapperRepository>();
builder.Services.AddSingleton<IAuthorizationManager, AuthorizationManager>();

builder.Services.AddGraphQL(c =>
    c.AddSystemTextJson()
        .AddSchema<GraphQLSchema>()
        .AddSchema<IdentityGraphQLSchema>()
        .AddGraphTypes(typeof(GraphQLSchema).Assembly)
        .AddGraphTypes(typeof(IdentityGraphQLSchema).Assembly)
        .AddAuthorization(config =>
        {
            config.AddPolicy("CRUDUsers", policy => policy.RequireClaim("CRUDUsers", "True"));
        })
);


builder.Services.AddSingleton<ISchema, GraphQLSchema>(services =>
{
    var schema = new GraphQLSchema(new SelfActivatingServiceProvider(services));
    return schema;
});
builder.Services.AddSingleton<ISchema, IdentityGraphQLSchema>(services =>
{
    var schema = new IdentityGraphQLSchema(new SelfActivatingServiceProvider(services));
    return schema;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .WithOrigins("https://localhost:3500")
            .WithOrigins("http://localhost:3500")
            .AllowCredentials()
            .AllowAnyHeader()
            .WithMethods("POST"));
});
var app = builder.Build();

app.UseCors("CorsPolicy");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();


app.UseGraphQL<GraphQLSchema>("/graphql").UseAuthorization();
app.UseGraphQL<IdentityGraphQLSchema>("/graphql-login");

app.MapControllers();

app.UseGraphQLAltair();

app.Run();

public class AuthOptions
{
    public static string key;
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
}