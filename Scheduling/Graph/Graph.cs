using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Scheduling
{
	public class Graph
	{
		public Top Head { get; set; }
		public List<Top> Tops { get; } = new();

		public Graph()
		{
			//Top.Indexator = 0;
			Head = new Top();
		}

		public Graph(XDocument xDocument)
		{
			Head = new Top();
			foreach (XElement xElement in xDocument.Root.Elements())
			{
				Tops.Add(new Top(int.Parse(xElement.Attribute("length").Value), int.Parse(xElement.Attribute("Id").Value)));
			}

			int i = 0;
			foreach (XElement xElement in xDocument.Root.Elements())
			{
				foreach (XElement xElement1 in xElement.Elements())
				{
					int id = int.Parse(xElement1.Attribute("Id").Value);
					Top top = Tops.Find(t => t.Id == id);
					Tops[i].Children.Add(top);
					top.Parents.Add(Tops[i]);
				}

				if (Tops[i].Parents.Count == 0)
				{
					Head.Children.Add(Tops[i]);
					Tops[i].Parents.Add(Head);
				}
				i++;
			}
		}

		//функция преобразования графа в XML файл
		public XDocument ToXml()
		{
			XDocument xDocument = new(new XElement("Graph"));
			foreach (Top t in Tops)
			{
				XElement xElement = new XElement("top", new XAttribute("Id", t.Id), new XAttribute("length", t.Weight));
				foreach (Top ch in t.Children)
				{
					xElement.Add(new XElement("top", new XAttribute("Id", ch.Id)));
				}
				xDocument.Root.Add(xElement);
			}
			return xDocument;
		}

		//сохранение графа в файл с имненем name
		public void SaveToXml(string name)
		{
			StreamWriter sw = new(name);
			sw.Write(ToXml().ToString());
			sw.Close();
		}
	}
}
