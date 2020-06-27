using System;
using Microsoft.Extensions.Configuration;

namespace Kwyjibo.Extensions.Configuration
{
    public static class KwyjiboOptionsExtensions
    {
        public static void Configure(this KwyjiboOptions options, IConfiguration configuration)
        {
            Status GetStatus(IConfigurationSection section)
            {
                if (Enum.TryParse<Status>(section.Value, true, out var status)) {
                    return status;
                }
                else {
                    throw new ArgumentException(nameof(section));
                }
            }

            var section = configuration.GetSection("Kwyjibo");
            var root = section.GetSection("Root");
            if (root?.Value != null) {
                options.ForContext(string.Empty).SetStatus(GetStatus(root));
            }
            foreach (var key in section.GetChildren()) {
                options.ForContext(section.Key).SetStatus(GetStatus(section));
            }
        }
    }
}
