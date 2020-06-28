using System;
using Microsoft.Extensions.Configuration;

namespace Kwyjibo.Extensions.Configuration
{
    public static class KwyjiboOptionsExtensions
    {
        public static void Configure(this KwyjiboOptions options, IConfiguration configuration)
        {
            Status GetStatus(string key, string value)
            {
                if (String.IsNullOrEmpty(value)) return Status.Inherit;

                if (Enum.TryParse<Status>(value, true, out var status)) {
                    return status;
                }

                var allowableValues = string.Join(", ", Enum.GetNames(typeof(Status)));
                throw new ArgumentException(
                    $"Kwyjibo configuration for {key} has invalid value {value}. " +
                    $"Allowed values are: {allowableValues}.");
            }

            var section = configuration.GetSection("Kwyjibo");
            var root = section.GetSection("Default");
            if (root?.Value != null) {
                options.ForContext(string.Empty).SetStatus(GetStatus("(default)", root.Value));
            }

            var contexts = section.GetSection("Contexts");
            foreach (var child in contexts.GetChildren()) {
                options.ForContext(child.Key).SetStatus(GetStatus(child.Key, child.Value));
            }
        }
    }
}
