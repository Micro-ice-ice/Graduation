using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduling
{
	public class Test
	{
		public Test(int lengthOpt, int legnthAlg)
		{
			LengthOpt = lengthOpt;
			LengthAlg = legnthAlg;
			Approximation = (double)legnthAlg / (double)lengthOpt;
		}
		public int LengthOpt { get; set; }
		public int LengthAlg { get; set; }
		public double Approximation { get; set; }
	}
}
