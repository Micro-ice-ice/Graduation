using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Scheduling
{
	public class AlgorithmLPT
	{
		public Graph Graph { get; }
		public Gant Gant { get; } = new Gant();
		public AlgorithmLPT(XDocument xDocument)
		{
			Graph = new Graph(xDocument);
			Run();
		}

		private void Run()
		{
			List<Top> availableTops = new();

			foreach (Top tChild in Graph.Head.Children)
			{
				availableTops.Add(tChild);
			}

			while (availableTops.Count != 0)
			{
				Worker worker = Gant.Worker1.Length() < Gant.Worker2.Length() ? Gant.Worker1 : Gant.Worker2;

				var tops = availableTops.FindAll(t => t.MinStart <= worker.Length());
				Top addingTop = null;

				if (tops.Count == 0)
				{
					int minMinStart = int.MaxValue;
					double maxWeight = 0;

					foreach (Top t in availableTops)
					{
						if (t.MinStart < minMinStart)
						{
							addingTop = t;
							minMinStart = t.MinStart;
							maxWeight = t.Weight;
						}
						if (t.MinStart == minMinStart)
						{
							if (t.Weight > maxWeight)
							{
								addingTop = t;
								maxWeight = t.Weight;

							}
						}
					}

					worker.AddWork(addingTop.MinStart, addingTop.Weight, addingTop.Id);

					foreach (Top tChild in addingTop.Children)
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
					double maxWeight = 0;

					foreach (Top t in tops)
					{
						if (t.Weight >= maxWeight)
						{
							addingTop = t;
							maxWeight = t.Weight;
						}
					}

					worker.AddWork(addingTop.Weight, addingTop.Id);

					foreach (Top tChild in addingTop.Children)
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
}
