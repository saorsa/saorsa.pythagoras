using Saorsa.Pythagoras;
using Saorsa.Pythagoras.Domain;
using Saorsa.Pythagoras.Domain.Business;
using Serilog;

Log.Logger = PythagorasRuntime.GetConfiguredLogger();

try
{
    Log.Information("Creating REST application...");
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Host.UseSerilog();
    
    Log.Information("Configuring Pythagoras...");
    builder.Services.AddPythagoras("Pythagoras");

    Log.Information("Configuring REST services...");
    builder.Services.AddControllers();

   
    Log.Warning(
        "Adding Swagger / Endpoint Explorer. Environment = {Environment}...", 
        'x');
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        Log.Warning(
            "Enabling Swagger UI for environment {Environment}...", 
            app.Environment.EnvironmentName);
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    Log.Information("Using HTTPS redirect...");
    app.UseHttpsRedirection();

    Log.Information("Using authorization...");
    app.UseAuthentication();
    app.UseAuthorization();

    Log.Information("Mapping endpoints...");
    app.MapControllers();

    Log.Information("Starting up...");
    app.Run();
    
    return 0;
}
catch (Exception ex)
{
    var exitCode = ErrorCodes.ApplicationExit.GeneralError;
    Log.Fatal(ex, "Host terminated unexpectedly. Exiting ({ExitCode})", exitCode);
    return exitCode;
}
finally
{
    Log.Information("Closing...");
    Log.CloseAndFlush();
}
