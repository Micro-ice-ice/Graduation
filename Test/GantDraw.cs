using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduling;
using System.Drawing;

namespace Test
{
	public class GantDraw
	{
		
		public void DrawGraph(Graph graph, Graphics graphics)
		{
			graphics.Clear(Color.White);

			int n = graph.Tops.Count;
			double r = graphics.DpiY * 0.05;
			double l = graphics.DpiY / 0.4;
			double alpha = Math.PI * (n - 2) / n;
			List<PointF> points = new();

			double t = 0;
			for (int i = 0; i < n; i++)
			{
				double x = l * Math.Cos(t);
				double y = l * Math.Sin(t);

				points.Add(new PointF((float)x, (float)y));

				t += alpha;
			}



		}

		public void DrawArrow(int idFirstPoint, int idSecondPoint)
		{

		}

		public void DrawGant(Gant gant, Graphics graphics)
		{
			graphics.Clear(Color.White);
		}
	}
}
