using FrontEnd.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient<WeatherForecastClient>(c =>
{
    var url = builder.Configuration["WEATHER_URL"] 
        ?? throw new InvalidOperationException("WEATHER_URL is not set");
    c.BaseAddress = new Uri(url);
});

// Configure logging
builder.Logging.ClearProviders(); // Clears default logging providers
builder.Logging.AddConsole();     // Adds console logging
builder.Logging.AddDebug();       // Adds debug logging

// Add environment-specific configurations
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

var app = builder.Build();

// Configure middleware for different environments
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // Custom error handling for production
    app.UseHsts();                     // Enforces strict transport security
}
else
{
    app.UseDeveloperExceptionPage(); // Detailed error page for development
}

// Enforce HTTPS redirection and use static files
app.UseHttpsRedirection();
app.UseStaticFiles();

// Configure routing
app.UseRouting();

// Map Blazor Hub and fallback to the default host page
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Run the application
app.Run();