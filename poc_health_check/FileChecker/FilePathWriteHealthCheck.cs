using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace poc_health_check.FileChecker
{
    public class FilePathWriteHealthCheck : IHealthCheck
    {
        private readonly string _filePath;
        private IReadOnlyDictionary<string, object> _HealthCheckData;

        public FilePathWriteHealthCheck(string filePath)
        {
            _filePath = filePath;
            _HealthCheckData = new Dictionary<string, object>
            {
                {"filePath", _filePath }
            };
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var testFile = $"{_filePath}\\test.txt";
                var fs = File.Create(testFile);
                fs.Close();
                File.Delete(testFile);

                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (Exception ex)
            {
                switch (context.Registration.FailureStatus)
                {
                    case HealthStatus.Degraded:
                        return Task.FromResult(HealthCheckResult.Degraded($"Issues writing to file path", ex, _HealthCheckData));
                    default:
                        return Task.FromResult(HealthCheckResult.Unhealthy($"Issues writing to file path", ex, _HealthCheckData));
                }
            }
        }
    }
}
