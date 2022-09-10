using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Scheduling
{
	public class Indicators
	{
		public List<Test> Tests = new();

		public double Average { get; set; }
		public double Worst { get; set; }
		public int WorstId { get; set; }
		public double Best { get; set; }
		public double Dispersion { get; set; }
		public double LowerBoundOfAverage { get; set; }
		public double UpperBoundOfAverage { get; set; }

		public void Calc()
		{
			double sum = 0;
			double worst = 0;
			int worstId = 0;
			double best = int.MaxValue;
			double sum2 = 0;

			foreach (Test test in Tests)
			{
				sum += test.Approximation;
				
				if (test.Approximation < best)
				{
					best = test.Approximation;
				}
				if (test.Approximation > worst)
				{
					worst = test.Approximation;
					worstId = Tests.IndexOf(test);
				}
			}

			Average = sum / Tests.Count;

			foreach (Test test in Tests)
			{
				sum2 += Math.Pow(test.Approximation - Average, 2);
			}	

			Dispersion = sum2 / (Tests.Count - 1);

			Worst = worst;
			WorstId = worstId;


			LowerBoundOfAverage = Average - 1.96 * Math.Sqrt(Dispersion) / Math.Sqrt(Tests.Count);
			UpperBoundOfAverage = Average + 1.96 * Math.Sqrt(Dispersion) / Math.Sqrt(Tests.Count);


		}

		public override string ToString()
		{

			return $"Average = {Average}\n" +
				$"Dispersion = {Dispersion}\n" +
				$"Worst = {Worst}\n" +
				$"Worst Id = {WorstId + 1}\n" +
				//$"Best = {Best}\n" +
				$"Confidence interval for Average ({LowerBoundOfAverage}; {UpperBoundOfAverage})";
		}

		public XDocument ToXml()
		{
			XDocument xDocument = new(new XElement("Indecators"));
			xDocument.Root.Add(new XElement("Average", new XAttribute("value", Average)));
			xDocument.Root.Add(new XElement("Dispersion", new XAttribute("value", Dispersion)));
			xDocument.Root.Add(new XElement("IntervalOfAverage", 
				new XElement("LowerBound", new XAttribute("value", LowerBoundOfAverage)), 
				new XElement("UpeerBound", new XAttribute("value", UpperBoundOfAverage))));
			xDocument.Root.Add(new XElement("Worst", new XAttribute("value", Worst)));
			return xDocument;
		}

		public void SaveToXml(string name)
		{
			StreamWriter sw = new(name);
			sw.Write(ToXml().ToString());
			sw.Close();
		}
	}
}
