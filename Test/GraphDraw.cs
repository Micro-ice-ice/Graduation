using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduling;
using System.Drawing;

namespace Test
{
	public class GraphDraw
	{
		public Graph Graph { get; }

		public Graphics Graphics { get; }

		public Pen Pen { get; set; } = new Pen(Color.Black);

		public Font Font { get; set; } = new Font("Arial", 14);

		private int N { get; set; }

		private double R { get; set; }

		private double L { get; set; }

		private double Alpha { get; set; }

		private List<PointF> Points { get; } = new();

		public GraphDraw(Graph graph, Graphics graphics)
		{
			Graph = graph;
			Graphics = graphics;
			Graphics.Clear(Color.White);
			N = Graph.Tops.Count;
			R = 800 * 0.05;
			L = 800 * 0.4;
			Alpha = Math.PI * 2 / N;

			SetPoints();
			CalcR();
			DrawTops();
			DrawArrows();
		}
		private void SetPoints()
		{
			double t = 0;
			for (int i = 0; i < N; i++)
			{
				double x = L * Math.Cos(t) + 800;
				double y = L * Math.Sin(t) + 400;

				Points.Add(new PointF((float)x, (float)y));

				t += Alpha;
			}
		}

		private void CalcR()
		{
			if (Points.Count < 10)
				return;	
			PointF p1 = Points[0];
			PointF p0 = Points[1];
			PointF p2 = Points[2];

			double t = Math.Abs((p2.Y - p1.Y) * p0.X + (p1.X - p2.X) * p0.Y + p2.X * p1.Y - p2.Y * p1.X);
			double b = Math.Sqrt(Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.X - p1.X, 2));

			R = Math.Abs((p2.Y - p1.Y) * p0.X + (p1.X - p2.X) * p0.Y + p2.X * p1.Y - p2.Y * p1.X) /
				Math.Sqrt(Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.X - p1.X, 2));



		}

		private void DrawTops()
		{
			for (int i = 0; i < Points.Count; i++)
			{
				DrawTop(i);
			}
		}

		private void DrawTop(int index)
		{
			PointF point = Points[index];
			point.X = (float)(point.X - R);
			point.Y = (float)(point.Y - R);

			Graphics.DrawEllipse(Pen, new RectangleF(point, new SizeF((float)(2 * R), (float)(2 * R))));

			string Weight = Graph.Tops[index].Weight.ToString();
			string Id = Graph.Tops[index].Id.ToString();

			Graphics.DrawString(Weight, Font, Pen.Brush, new RectangleF(point, new SizeF((float)(2 * R), (float)(2 * R))), new StringFormat{ Alignment = StringAlignment.Center});

		}

		private void DrawArrows()
		{
			foreach(Top top in Graph.Tops)
			{
				int index1 = Graph.Tops.IndexOf(top);
				foreach (Top child in top.Children)
				{
					int index2 = Graph.Tops.IndexOf(child);
					DrawArrow(index1, index2);
				}
			}
		}

		private void DrawArrow(int index1, int index2)
		{
			PointF p1 = Points[index1];
			PointF p2 = Points[index2];
			

			double k = (p2.Y - p1.Y) / (p2.X - p1.X);
			double beta = Math.Atan(k);
			beta = p2.X < p1.X ? beta + Math.PI : beta;
			PointF p1_Offset = new PointF
			{
				X = (float)(p1.X + R * Math.Cos(beta)),
				Y = (float)(p1.Y + R * Math.Sin(beta))
			};
			PointF p2_Offset = new PointF
			{
				X = (float)(p2.X + R * Math.Cos(Math.PI + beta)),
				Y = (float)(p2.Y + R * Math.Sin(Math.PI + beta))
			};
			DrawArrow(p1_Offset, p2_Offset, beta);
			//DrawArrow(p1, p2, beta);

		}

		public void DrawArrow(PointF p1, PointF p2, double beta)
		{
			Graphics.DrawLine(Pen, p1, p2);

			PointF p3 = new PointF
			{
				X = (float)(p2.X + Math.Cos(Math.PI * 5 / 4 + beta) * R),
				Y = (float)(p2.Y + Math.Sin(Math.PI * 5 / 4 + beta) * R)
			};

			PointF p4 = new PointF
			{
				X = (float)(p2.X + Math.Cos(Math.PI * 3 / 4 + beta) * R),
				Y = (float)(p2.Y + Math.Sin(Math.PI * 3 / 4 + beta) * R)
			};

			Graphics.DrawLine(Pen, p3, p2);
			Graphics.DrawLine(Pen, p4, p2);
		}

	}
}
