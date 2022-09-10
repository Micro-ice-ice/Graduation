using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduling
{
	public class Top
	{
		//список дочерних вершин
		public List<Top> Children { get; } = new();

		//количество выполненых дочерних вершин
		public int Flag { get; set; } = 0;

		//список родительских вершин
		public List<Top> Parents { get; } = new();

		//вес вершины
		public int Weight { get; set; } = 0;

		//униикальный Id вершины
		public int Id { get; set; }

		//время минимального старта вершины
		public int MinStart { get; set; }

		//пустой конструктор 
		public Top()
		{

		}

		//конструктор вершины (вес, id) 
		public Top(int weight, int id)
		{
			//Id = Indexator++;
			Weight = weight;
			Id = id;
		}
	}
}
