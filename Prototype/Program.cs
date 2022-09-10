using System;
using System.IO;
using System.Xml.Linq;
using Scheduling;

namespace Prototype
{
	class Program
	{
		static void Main(string[] args)
		{
			Indicators IndicatorsLW = new();
			Indicators IndicatorsLS = new();
			Indicators IndicatorsLPT = new();
			for (int i = 1; i <= Config.TestCount; i++)
			{

				Generator g = new();
				g.Gant.SaveToXml(Config.Path + $"gant{i}.xml");
				g.Graph.SaveToXml(Config.Path + $"graph{i}.xml");
				Console.WriteLine($"Test {i} generated, length = {g.Gant.Length()}");
				StreamReader sr = new(Config.Path + $"graph{i}.xml");
				XDocument xDocument = XDocument.Parse(sr.ReadToEnd());
				AlgorithmLW algorithmLW = new(xDocument);
				algorithmLW.Gant.SaveToXml(Config.Path + $"gantLW{i}.xml");
				Console.WriteLine($"Test {i} done by algorithm LW, length = {algorithmLW.Gant.Length()}");
				AlgorithmLS listScheduling = new(xDocument);
				listScheduling.Gant.SaveToXml(Config.Path + $"gantLS{i}.xml");
				Console.WriteLine($"Test {i} done by algorithm LS, length = {listScheduling.Gant.Length()}");
				AlgorithmLPT lpt = new(xDocument);
				lpt.Gant.SaveToXml(Config.Path + $"gantLPT{i}.xml");
				Console.WriteLine($"Test {i} done by algorithm LPT, length = {lpt.Gant.Length()}");
				IndicatorsLW.Tests.Add(new Test(g.Gant.Length(), algorithmLW.Gant.Length()));
				IndicatorsLS.Tests.Add(new Test(g.Gant.Length(), listScheduling.Gant.Length()));
				IndicatorsLPT.Tests.Add(new Test(g.Gant.Length(), lpt.Gant.Length()));

			}
			Console.WriteLine("LW:");
			IndicatorsLW.Calc();
			Console.WriteLine(IndicatorsLW.ToString());
			IndicatorsLW.SaveToXml(Config.Path + "IndicatorsLW.xml");

			Console.WriteLine("LS:");
			IndicatorsLS.Calc();
			Console.WriteLine(IndicatorsLS.ToString());
			IndicatorsLS.SaveToXml(Config.Path + "IndicatorsLS.xml");

			Console.WriteLine("LPT:");
			IndicatorsLPT.Calc();
			Console.WriteLine(IndicatorsLPT.ToString());
			IndicatorsLPT.SaveToXml(Config.Path + $"IndicatorsLPT.xml");
		}
	}
}
