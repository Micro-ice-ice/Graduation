using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Scheduling;

namespace Test
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			
			
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			StreamReader sr = new(Config.Path + $"graph4.xml");
			XDocument xDocument = XDocument.Parse(sr.ReadToEnd());
			Bitmap bmp = new Bitmap(1600, 800);
			Graphics g = Graphics.FromImage(bmp);

			GraphDraw g1 = new GraphDraw(new Graph(xDocument), g);
			pictureBoxField.Image = bmp;
		}
	}
}
