using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduling
{
	public class Work
	{
		public static int Indexator { get; set; } = 0;

		public int Id { get; set; }

		public int Start { get; set; }

		public int End { get; set; }

		public int Length { get; set; }

		public Work(int start, int length)
		{
			Start = start;
			End = start + length;
			Length = length;
			Id = Indexator++;
		}

		public Work(int start, int length, int id)
		{
			Start = start;
			End = start + length;
			Length = length;
			Id = id;
		}
	}
}
