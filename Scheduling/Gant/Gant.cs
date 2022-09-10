using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Scheduling
{
	public class Gant
	{
		public Gant()
		{
			Work.Indexator = 0;
		}

		public Worker Worker1 { get; set; } = new();

		public Worker Worker2 { get; set; } = new();

		public int Length()
		{
			return Math.Max(Worker1.Length(), Worker2.Length());
		}


		public XDocument ToXml()
		{
			XDocument xDocument = new(new XElement("Gant"));
			xDocument.Root.Add(new XElement("length", new XAttribute("value", Length())));
			XElement w1 = new("Worker1", new XAttribute("length", Worker1.Length()));
			foreach (Work w in Worker1.Works)
			{
				w1.Add(new XElement("work", new XAttribute("Id", w.Id), new XAttribute("legnth", w.Length), new XAttribute("start", w.Start), new XAttribute("end", w.End)));
			}

			XElement w2 = new("Worker2", new XAttribute("length", Worker2.Length()));
			foreach (Work w in Worker2.Works)
			{
				w2.Add(new XElement("work", new XAttribute("Id", w.Id), new XAttribute("legnth", w.Length), new XAttribute("start", w.Start), new XAttribute("end", w.End)));
			}
			xDocument.Root.Add(w1, w2);
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
