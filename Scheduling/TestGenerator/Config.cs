using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Scheduling
{
	public static class Config
	{
		public static int AverageWorkCount { get; } = int.Parse(ConfigurationManager.AppSettings.Get("AverageWorkCount"));

		public static int MaxWorkLength { get; } = int.Parse(ConfigurationManager.AppSettings.Get("MaxWorkLength"));

		public static int MinWorkLength { get; } = int.Parse(ConfigurationManager.AppSettings.Get("MinWorkLength"));

		public static int TestCount { get; } = int.Parse(ConfigurationManager.AppSettings.Get("TestCount"));

		public static string Path { get; } = ConfigurationManager.AppSettings.Get("Path");

		public static double P { get; } = int.Parse(ConfigurationManager.AppSettings.Get("P"));
	}
}
