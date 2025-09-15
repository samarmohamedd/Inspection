# Serilog Integration Guide

This document describes the Serilog logging implementation in the Inspection API project.

## Overview

Serilog has been integrated into the Inspection API to provide structured logging capabilities with the following features:

- **Structured Logging**: Log events with structured data instead of plain text
- **Multiple Sinks**: Console and file logging with different configurations
- **Request Logging**: Automatic HTTP request/response logging
- **Environment-specific Configuration**: Different log levels for Development and Production
- **Performance Logging**: High-performance logging patterns
- **Enriched Context**: Additional context information in logs

## Packages Installed

The following Serilog packages have been added to the project:

```xml
<PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
<PackageReference Include="Serilog.Sinks.Console" Version="6.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="6.0.0" />
<PackageReference Include="Serilog.Settings.Configuration" Version="8.0.2" />
<PackageReference Include="Serilog.Enrichers.Environment" Version="3.0.1" />
<PackageReference Include="Serilog.Enrichers.Thread" Version="4.0.0" />
```

## Configuration

### appsettings.json (Production)

```json
{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.AspNetCore": "Warning",
        "Microsoft.EntityFrameworkCore": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/inspection-.log",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 30,
          "fileSizeLimitBytes": 10485760,
          "rollOnFileSizeLimit": true,
          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj} <s:{SourceContext}> <id:{RequestId}>{NewLine}{Exception}"
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ],
    "Properties": {
      "Application": "InspectionAPI"
    }
  }
}
```

### appsettings.Development.json

Development environment uses more verbose logging with Debug level enabled and shorter file retention.

## Log File Structure

Logs are written to the `logs/` directory with the following structure:

- **Production**: `logs/inspection-YYYYMMDD.log`
- **Development**: `logs/inspection-dev-YYYYMMDD.log`

Files are rotated daily and old files are automatically cleaned up (30 days for production, 7 days for development).

## Usage Examples

### Basic Logging in Controllers

```csharp
public class MyController : ControllerBase
{
    private readonly ILogger<MyController> _logger;

    public MyController(ILogger<MyController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetData(int id)
    {
        _logger.LogInformation("Fetching data for ID: {Id}", id);
        
        try
        {
            var data = await GetDataAsync(id);
            _logger.LogInformation("Successfully retrieved data for ID: {Id}", id);
            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data for ID: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
```

### Structured Logging with Complex Objects

```csharp
var inspection = new { Id = "INS-001", Status = "Completed", ViolationCount = 3 };
_logger.LogInformation("Inspection completed: {@Inspection}", inspection);
```

### Enriched Logging with Context

```csharp
using (LogContext.PushProperty("UserId", userId))
using (LogContext.PushProperty("CorrelationId", correlationId))
{
    _logger.LogInformation("Processing user request");
    // All logs within this scope will include UserId and CorrelationId
}
```

## Demo Endpoints

The following endpoints have been added to demonstrate Serilog features:

- `GET /api/LoggingDemo/log-levels` - Demonstrates different log levels
- `GET /api/LoggingDemo/structured-logging` - Shows structured logging with objects
- `GET /api/LoggingDemo/enriched-logging` - Demonstrates context enrichment
- `GET /api/LoggingDemo/performance-logging` - Shows performance logging patterns
- `GET /api/LoggingDemo/conditional-logging` - Demonstrates conditional logging
- `GET /api/LoggingDemo/scoped-logging` - Shows scoped properties
- `GET /api/LoggingDemo/error-logging` - Demonstrates error logging
- `POST /api/LoggingDemo/custom-properties` - Shows custom property logging

## Best Practices

1. **Use Structured Logging**: Always use structured parameters instead of string interpolation
   ```csharp
   // Good
   _logger.LogInformation("User {UserId} performed action {Action}", userId, action);
   
   // Bad
   _logger.LogInformation($"User {userId} performed action {action}");
   ```

2. **Use Appropriate Log Levels**:
   - `Trace`: Very detailed information, typically only of interest when diagnosing problems
   - `Debug`: Internal system events that aren't necessarily observable from the outside
   - `Information`: The lifeblood of operational intelligence
   - `Warning`: Service is degraded or endangered
   - `Error`: Functionality is unavailable, invariants are broken or data is lost
   - `Critical`: The service/app is going to stop or become unusable

3. **Include Context**: Use LogContext.PushProperty() for request-scoped information

4. **Performance Considerations**: Use conditional logging for expensive operations
   ```csharp
   if (_logger.IsEnabled(LogLevel.Debug))
   {
       var expensiveData = GenerateExpensiveDebugInfo();
       _logger.LogDebug("Debug info: {@Data}", expensiveData);
   }
   ```

5. **Exception Logging**: Always include the exception object when logging errors
   ```csharp
   _logger.LogError(ex, "Error processing request for {UserId}", userId);
   ```

## Monitoring and Analysis

- Log files can be analyzed using tools like:
  - **Seq**: Structured log server (recommended for development)
  - **ELK Stack**: Elasticsearch, Logstash, and Kibana
  - **Splunk**: Enterprise log analysis platform
  - **Azure Application Insights**: Cloud-based application monitoring

## Configuration Tips

- Adjust log levels based on environment (Debug for development, Information for production)
- Configure appropriate file retention policies
- Consider using different sinks for different log levels (e.g., errors to email/Slack)
- Use enrichers to add consistent context to all logs

## Troubleshooting

1. **Logs not appearing**: Check the minimum log level configuration
2. **File permission errors**: Ensure the application has write access to the logs directory
3. **Performance issues**: Reduce log verbosity or use asynchronous sinks for high-volume logging
