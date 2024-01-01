using System;
using System.Collections.Generic;

namespace RFClassLibrary
{

    /// <summary>
    /// This class allows the created of delimited field text strings.
    /// </summary>
    public sealed class DelimitedFileLine
    {
        /// <summary>
        /// 
        /// </summary>
        public DelimitedFileLine()
        {

        }

        //public char Delimiter { get; set; }

        private List<Line> lines = new List<Line>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        public void AddLine(Line line)
        {
            lines.Add(line);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public string ToString(char delimiter)
        {
            string outS = string.Empty;

            foreach (Line line in lines)
            {
                outS += line.ToString(delimiter) + "\r\n";
            }

            return outS;
        }

        /// <summary>
        /// Do not use ToString without a parameter, use ToString(char delimiter).
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override string ToString()
        {
            throw new NotImplementedException();
        }

    }

    /// <summary>
    /// Class used with DelimitedFilleLine class.
    /// </summary>
    public class Line
    {
        private List<string> fields;

        /// <summary>
        /// 
        /// </summary>
        public Line()
        {
            fields = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldIndex"></param>
        /// <returns></returns>
        public string this[int fieldIndex]
        {
            get { return fields[fieldIndex] ?? ""; }
            set 
            { 
                if(fields.Count < fieldIndex)
                {
                    while(fields.Count < fieldIndex)
                        fields.Add(string.Empty);
                }

                fields.Insert(fieldIndex, value); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public string ToString(char delimiter)
        {
            string outString = "";
            foreach (string field in fields)
            {
                outString += field + delimiter;
            }

            return outString;
        }

        /// <summary>
        /// Do not use ToString without a parameter, use ToString(char delimiter).
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override string ToString()
        {
            throw new NotImplementedException();
        }


    }
}
