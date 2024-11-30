using GraphQL.AspNet.Configuration;
using hw_26._11._24.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Local")));

builder.Services.AddGraphQL();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseGraphQL();

app.Run();
