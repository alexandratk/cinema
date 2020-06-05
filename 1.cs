using System.Xml;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

class Program
{
	public static string[,,] cinema;
	public static int ms;
	public static int mm;
	public static int d;
	
	public static void write(){
		XmlDocument xDoc = new XmlDocument();
		xDoc.Load("1.xml");
		XmlElement xRoot = xDoc.DocumentElement;
		foreach(XmlNode xnode in xRoot)
		{
			if(xnode.Attributes.Count>0)
			{
				XmlNode attr = xnode.Attributes.GetNamedItem("name");
				if (attr!=null)
				{
					Console.WriteLine(attr.Value);
				}
			}
			foreach(XmlNode childnode in xnode.ChildNodes)
			{
				Console.Write(childnode.InnerText + " ");
			}
			Console.WriteLine();
			Console.WriteLine();
		}
		Console.WriteLine();
		Console.WriteLine("Нажмите любую клавишу для продолжения");
		while(!Console.KeyAvailable){
		}
	}
	
	public static void choose(){
		Console.WriteLine("Выберите дату");
		Console.WriteLine();
		for(int i = 0; i < d; i++){
			Console.WriteLine(Convert.ToString(i+1)+" "+cinema[i,0,0]);
		}
		int cd = 1000000;
		while(cd > d || cd <= 0){
			cd = Convert.ToInt32(Console.ReadLine());
		}
		Console.WriteLine(cinema[cd-1,0,0]);
		Console.WriteLine();
		Console.WriteLine("Выберите фильм");
		Console.WriteLine();
		for(int i = 1; i <= mm && cinema[cd-1,i,0] != null; i++){
			Console.WriteLine(Convert.ToString(i)+" "+cinema[cd-1,i,0]);
		}
		int cm = 1000000;
		while(cm > mm || cm <= 0){
			cm = Convert.ToInt32(Console.ReadLine());
		}
		Console.WriteLine(cinema[cd-1,cm,0]);
		Console.WriteLine();
		Console.WriteLine("Выберите время");
		Console.WriteLine();
		for(int i = 2; i <= ms + 1 && cinema[cd-1,cm,i] != null; i++){
			Console.WriteLine(Convert.ToString(i-1)+" "+cinema[cd-1,cm,i]);
		}
		int cs = 1000000;
		while(cs > ms || cs <= 0){
			cs = Convert.ToInt32(Console.ReadLine());
		}
		Console.WriteLine(cinema[cd-1,cm,cs+1]);
		Console.WriteLine();
		Console.WriteLine("Введите имя");
		string name = Console.ReadLine();
		Console.WriteLine("Забронировать?(Нажмите 1)");
		Console.WriteLine(name);
		Console.WriteLine(cinema[cd - 1, 0, 0] + " " + cinema[cd-1, cm, 0] + " " + cinema[cd-1, cm, cs + 1]);
		while(!Console.KeyAvailable){
		}
		var key = Console.ReadKey();
        if (key.KeyChar == '1')
        {
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("1.xml");
			XmlElement xRoot = xDoc.DocumentElement;
			XmlElement userElem = xDoc.CreateElement("user");
			XmlAttribute nameAttr = xDoc.CreateAttribute("name");
			XmlText nameText = xDoc.CreateTextNode(name);
			XmlElement dateElem = xDoc.CreateElement("date");
			XmlElement movieElem = xDoc.CreateElement("movie");
			XmlElement sessionElem = xDoc.CreateElement("session");
			XmlText dateText = xDoc.CreateTextNode(cinema[cd - 1, 0, 0]);
			XmlText movieText = xDoc.CreateTextNode(cinema[cd-1, cm, 0]);
			XmlText sessionText = xDoc.CreateTextNode(cinema[cd-1, cm, cs + 1]);
			nameAttr.AppendChild(nameText);
			dateElem.AppendChild(dateText);
			movieElem.AppendChild(movieText);
			sessionElem.AppendChild(sessionText);
			userElem.Attributes.Append(nameAttr);
			userElem.AppendChild(dateElem);
			userElem.AppendChild(movieElem);
			userElem.AppendChild(sessionElem);
			xRoot.AppendChild(userElem);
			xDoc.Save("1.xml");
			Console.WriteLine();
			Console.WriteLine("Забранировано:");
			Console.WriteLine(name);
			Console.WriteLine(cinema[cd - 1, 0, 0] + " " + cinema[cd-1, cm, 0] + " " + cinema[cd-1, cm, cs + 1]);
			Console.WriteLine();
			Console.WriteLine("Для продолжения нажмите любую клавишу");
			while(!Console.KeyAvailable){
			}
		}
	}
	
	public static string convert(string t){
		string y = "";
		int i;
		for(i = 0; t[i] != '-'; i++){
			y = y + t[i];
		}
		i++;
		string m = "";
		for(int k = i; t[k] != '-'; k++){
			m = m + t[k];
			i++;
		}
		i++;
		string d = "";
		for(int k = i; k < t.Length; k++){
			d = d + t[k];
			i++;
		}
		DateTime now = new DateTime(Convert.ToInt32(y), Convert.ToInt32(m), Convert.ToInt32(d));
		t = now.ToString("d MMMM (ddd)");
		return t;
	}
	
    static void Main(string[] args)
    {
		while (true)
		{
			cinema = new string [20, 100, 100];
			d = 0;
			int m = 0;
			mm = 0;
			int s = 0;
			ms = 0; 
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("users.xml");
			XmlElement xRoot = xDoc.DocumentElement;
			foreach(XmlNode xnode in xRoot)
			{
				if(xnode.Attributes.Count>0)
				{
					XmlNode attr = xnode.Attributes.GetNamedItem("value");
					if (attr!=null)
					{
						d++;
						cinema[d-1, 0, 0] = convert(attr.Value);
					}
				}
				m = 0;
				foreach(XmlNode childnode in xnode.ChildNodes)
				{
					XmlNode attr = childnode.Attributes.GetNamedItem("name");
					if (attr!=null)
					{
						m++;
						cinema[d-1, m, 0] = attr.Value;
					}
					s = 0;
					foreach(XmlNode childnode1 in childnode.ChildNodes)
					{
						if(childnode1.Name=="session")
						{
							XmlNode attr1 = childnode1.Attributes.GetNamedItem("time");
							s++;
							cinema[d-1, m, s+1] = attr1.Value;
						}
					}
					if (s > ms){
						ms = s;
					}
				}
				if (m > mm){
					mm = m;
				}
			}
			for(int i = 0; i < d; i++){
				Console.WriteLine(cinema[i,0,0]);
				for (int j = 1; j <= mm && cinema[i,j,0] != null; j++){
					Console.Write("    "+cinema[i,j,0]);
					int p = 0;
					while(p + cinema[i,j,0].Length != 20){
						p++;
						Console.Write(" ");
					}
					for(int k = 2; k <= ms+1 && cinema[i,j,k] != null; k++){
						Console.Write(cinema[i,j,k]+" ");
						if(k+1 <= ms+1 && cinema[i,j,k+1] != null){
							Console.Write("| ");
						}
					}
					Console.WriteLine();
				}
				Console.WriteLine();
			}
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Команды:");
			Console.WriteLine("1. Забронировать сеанс");
			Console.WriteLine("2. Список забронированных сеансов");
			if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();
                if (key.KeyChar == '1')
                {
					Console.Clear();
					choose();
				}
				if (key.KeyChar == '2')
                {
					Console.Clear();
					write();
				}
            }
			Thread.Sleep(500);
			Console.Clear();
		}
	}
}
