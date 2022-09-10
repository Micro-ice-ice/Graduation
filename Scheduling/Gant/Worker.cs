using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduling
{
	public class Worker
	{
		public List<Work> Works { get; } = new();

		public void AddWork(int length)
		{
			if (Works.Count == 0)
			{
				Works.Add(new Work(0, length));
				return;
			}

			Works.Add(new Work(Works[^1].End, length));
			return;
		}

		public void AddWork(int length, int id)
		{
			if (Works.Count == 0)
			{
				Works.Add(new Work(0, length, id));
				return;
			}

			Works.Add(new Work(Works[^1].End, length, id));
			return;
		}

		public void AddWork(int start, int length, int id)
		{
			if (Works.Count == 0)
			{
				Works.Add(new Work(0, length, id));
				return;
			}

			Works.Add(new Work(start, length, id));
			return;
		}

		public int Length()
		{
			try
			{
				return Works[^1].End;
			}
			catch
			{
				return 0;
			}
		}

	}
}
