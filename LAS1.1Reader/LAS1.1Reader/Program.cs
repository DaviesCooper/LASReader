using LAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAS1._1Reader
{
    /// <summary>
    /// Will read all the points in a file, and append them to a list
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            List<LASPoint> points = new List<LASPoint>();
            LASReader reader = new LASReader(args[0]);
            while(reader.ReadPoint(out LASPoint point))
            {
                if (!point.Edge_Of_Flight_Line)
                    Console.WriteLine("false");
                points.Add(point);
                if (points.Count % 10000 == 0)
                    Console.WriteLine("1000 points read");
            }
        }
    }
}
