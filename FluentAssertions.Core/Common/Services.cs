﻿using FluentAssertions.Execution;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Maintains the framework-specific services.
    /// </summary>
    public static class Services
    {
        static Services()
        {
            ResetToDefaults();
        }

        public static void ResetToDefaults()
        {
            Configuration = new Configuration(new NullConfigurationStore());
        }

        public static Configuration Configuration { get; set; }
        public static ITestFramework TestFramework { get; set; }
        public static IReflector Reflector { get; set; }
    }
}