using Scheduling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Scheduling
{
	public class AlgorithmLW 
	{
		public GraphLW Graph { get; }
		public Gant Gant { get; } = new Gant();


		public AlgorithmLW(XDocument xDocument)
		{
			Graph = new GraphLW(xDocument);
			Run();
		}

		private void Dfs(TopLW top)
		{
			if (top.Children.Count == 0)
			{
				top.L = top.Weight;
				top.W = top.Weight;
				top.LW = 1.0;
				return;
			}

			top.W = top.Weight;
			foreach (TopLW t in top.Children)
			{
				if (t.LW == 0)
				{
					Dfs(t);
				}
				top.W += t.W;
				top.L = top.L > t.L ? top.L : t.L;
			}
			top.L += top.Weight;
			top.LW = (double)top.L / (double)top.W;
			return;
		}

		private void Run()
		{
			Dfs((TopLW)Graph.Head);

			List<TopLW> availableTops = new();

			foreach (TopLW tChild in Graph.Head.Children)
			{
				availableTops.Add(tChild);
			}

			while (availableTops.Count != 0)
			{
				Worker worker = Gant.Worker1.Length() < Gant.Worker2.Length() ? Gant.Worker1 : Gant.Worker2;

				var tops = availableTops.FindAll(t => t.MinStart <= worker.Length());
				TopLW addingTop = null;

				if (tops.Count == 0)
				{
					int minMinStart = int.MaxValue;
					double maxLW = 0;

					foreach (TopLW t in availableTops)
					{
						if (t.MinStart < minMinStart)
						{
							addingTop = t;
							minMinStart = t.MinStart;
							maxLW = t.LW;
						}
						if (t.MinStart == minMinStart)
						{
							if (t.LW >= maxLW)
							{
								if (t.LW == maxLW)
								{
									if (t.W > addingTop.W)
									{
										addingTop = t;
										maxLW = t.LW;
									}
								}
								else
								{
									addingTop = t;
									maxLW = t.LW;
								}
							}
						}
					}

					worker.AddWork(addingTop.MinStart, addingTop.Weight, addingTop.Id);

					foreach (TopLW tChild in addingTop.Children)
					{
						tChild.Flag++;
						tChild.MinStart = worker.Length();
						if (tChild.Flag == tChild.Parents.Count)
						{
							availableTops.Add(tChild);
						}
					}

					availableTops.Remove(addingTop);

				}
				else
				{
					double maxLW = 0;

					foreach (TopLW t in tops)
					{
						if (t.LW >= maxLW)
						{
							if (t.LW == maxLW)
							{
								if (t.W > addingTop.W)
								{
									addingTop = t;
									maxLW = t.LW;
								}
							}
							else
							{
								addingTop = t;	
								maxLW = t.LW;
							}
						}
					}
					worker.AddWork(addingTop.Weight, addingTop.Id);

					foreach (TopLW tChild in addingTop.Children)
					{
						tChild.Flag++;
						tChild.MinStart = worker.Length();
						if (tChild.Flag == tChild.Parents.Count)
						{
							availableTops.Add(tChild);
						}
					}

					availableTops.Remove(addingTop);


				}
			}
		}
	}

	public class TopLW : Top
	{
		public TopLW()
		{

		}

		public TopLW(int weight, int id) : base(weight, id)
		{

		}
		public int L { get; set; } = 0;
		public int W { get; set; } = 0;
		public double LW { get; set; } = 0;
	}

	public class GraphLW : Graph
	{
		public GraphLW(XDocument xDocument)
		{
			Head = new TopLW();
			foreach (XElement xElement in xDocument.Root.Elements())
			{
				Tops.Add(new TopLW(int.Parse(xElement.Attribute("length").Value), int.Parse(xElement.Attribute("Id").Value)));
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
	}
}
