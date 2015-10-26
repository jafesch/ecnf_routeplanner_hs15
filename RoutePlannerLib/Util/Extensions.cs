using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public static class Extensions
    {
        public static IEnumerable<string[]> GetSplittedLines(this TextReader textReader, char separator)
        {
            string textLine;
            while ((textLine = textReader.ReadLine()) != null)
            {
                yield return textLine.Split(separator);
            }
        }
    }
}
