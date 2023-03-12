﻿using System.Globalization;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;

namespace Benchmarks;

internal static class Program
{
    public static void Main()
    {
        var exporter = new CsvExporter(
            CsvSeparator.CurrentCulture,
            new SummaryStyle(
                cultureInfo: CultureInfo.GetCultureInfo("nl-NL"),
                printUnitsInHeader: true,
                sizeUnit: SizeUnit.KB,
                timeUnit: TimeUnit.Microsecond,
                printUnitsInContent: false
            ));

        var config = ManualConfig.CreateMinimumViable().AddExporter(exporter);

        _ = BenchmarkRunner.Run<CheckIfMemberIsBrowsableBenchmarks>(config);
    }
}
