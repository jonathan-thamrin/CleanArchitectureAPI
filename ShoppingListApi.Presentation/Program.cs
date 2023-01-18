using Microsoft.EntityFrameworkCore;
using ShoppingListApi.Application;
using ShoppingListApi.Infrastructure;
using ShoppingListApi.Infrastructure.utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Must specify how ShoppingListRepository & ShoppingListContext instances are created. Recall Controller and Repository
// are dependency injected with interfaces. Below explicitly states what concrete implementation to inject.
builder.Services.AddScoped<IShoppingListRepository, ShoppingListRepository>();
builder.Services.AddScoped<IShoppingListContext, ShoppingListContext>();

// Adds database context to the Dependency Injection (DI) Container (part of "Services").
// DI: Framework which creates dependencies and injects them automatically when required.

builder.Services.AddDbContext<ShoppingListContext>(
    options => options.UseNpgsql(Configuration.GetDbConnectionString())
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
