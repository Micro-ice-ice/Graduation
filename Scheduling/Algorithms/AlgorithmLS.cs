using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Scheduling
{
	public class AlgorithmLS
	{
		public Graph Graph { get; }
		public Gant Gant { get; } = new Gant();
		public AlgorithmLS(XDocument xDocument)
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

				Top addingTop = availableTops.FindLast(t => t.MinStart <= worker.Length());

				if (addingTop == null)
				{
					int minMinStart = int.MaxValue;

					foreach (Top t in availableTops)
					{
						if (t.MinStart < minMinStart)
						{
							addingTop = t;
							minMinStart = t.MinStart;
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
