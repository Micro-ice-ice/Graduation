using System;
using System.Collections.Generic;
using System.IO;

namespace Scheduling
{
	public class Generator
	{
		public Generator()
		{
			Run();
		}
		public Gant Gant { get; } = new Gant();

		public Graph Graph { get; } = new Graph();

		private void GenerateGant()
		{
			int sumLength = new Random().Next(Config.MinWorkLength, Config.MaxWorkLength) * Config.AverageWorkCount;

			GenerateWorksForWorker(Gant.Worker1, sumLength);
			GenerateWorksForWorker(Gant.Worker2, sumLength);

		}

		private static void GenerateWorksForWorker(Worker worker, int sumLength)
		{
			while (sumLength > Config.MaxWorkLength)
			{
				int length = new Random().Next(Config.MinWorkLength, Config.MaxWorkLength);
				worker.AddWork(length);
				sumLength -= length;
			}
			worker.AddWork(sumLength);
		}

		private void GantToGraph()
		{
			int index1 = 0;
			var stack1 = Gant.Worker1.Works;

			int index2 = 0;
			var stack2 = Gant.Worker2.Works;

			List<Work> addedWorks = new();
			List<Top> addedTops = new();



			int workCount = stack1.Count + stack2.Count;

			for (int i = 0; i < workCount; i++)
			{
				Work work;
				if (index1 == stack1.Count)
				{
					work = stack2[index2++];
				}
				else
				{
					if (index2 == stack2.Count)
					{
						work = stack1[index1++];
					}
					else
					{
						work = stack1[index1].Start < stack2[index2].Start ? stack1[index1++] : stack2[index2++];

					}
				}

				Top t = new(work.Length, work.Id);

				var works = addedWorks.FindAll(w => w.End <= work.Start);
				if (works.Count == 0)
				{
					Graph.Head.Children.Add(t);
					t.Parents.Add(Graph.Head);
				}
				else
				{
					if (new Random().NextDouble() < Config.P / (double)workCount)
					{
						Graph.Head.Children.Add(t);
						t.Parents.Add(Graph.Head);
						Graph.Tops.Add(t);
						addedTops.Add(t);
						addedWorks.Add(work);
						continue;
					}

					int index = addedWorks.IndexOf(works[new Random().Next(0, works.Count - 1)]);

					addedTops[index].Children.Add(t);
					t.Parents.Add(addedTops[index]);


					works.Remove(addedWorks[index]);

					foreach (Work w in works)
					{
						Top top = addedTops[addedWorks.IndexOf(w)];

						Top equal = null;
						foreach (Top tParent in t.Parents)
						{
							equal = tParent.Children.Find(e => e == top);
							if (equal != null)
								break;
						}
						if (equal != null)
							continue;

						foreach (Top tChildren in t.Parents)
						{
							equal = tChildren.Parents.Find(e => e == top);
							if (equal != null)
								break;
						}
						if (equal != null)
							continue;

						if (new Random().NextDouble() < Config.P / (double)works.Count)
						{
							top.Children.Add(t);
							t.Parents.Add(top);
						}
					}
				}

				Graph.Tops.Add(t);
				addedTops.Add(t);
				addedWorks.Add(work);



			}
		}

		private void Run()
		{
			GenerateGant();
			GantToGraph();
		}



	}
}
