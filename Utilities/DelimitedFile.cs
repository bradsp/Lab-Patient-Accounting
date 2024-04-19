using System;
using System.Collections.Generic;

namespace Utilities;


/// <summary>
/// This class allows the created of delimited field text strings.
/// </summary>
public sealed class DelimitedFile
{
    /// <summary>
    /// 
    /// </summary>
    public DelimitedFile()
    {

    }

    //public char FieldDelimiter { get; set; }
    //public string RowDelimiter { get; set; }

    private List<DelimitedFileLine> lines = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="line"></param>
    public void AddLine(DelimitedFileLine line)
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

        foreach (DelimitedFileLine line in lines)
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
/// Class used with DelimitedFile class.
/// </summary>
public class DelimitedFileLine
{
    private List<string> fields;
    private char delimiter;

    /// <summary>
    /// 
    /// </summary>
    public DelimitedFileLine(char delimiter)
    {
        fields = new List<string>();
        this.delimiter = delimiter;
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
            if (fields.Count < fieldIndex)
            {
                while (fields.Count < fieldIndex)
                    fields.Add(string.Empty);
            }

            fields.Insert(fieldIndex, value);
        }
    }

    /// <summary>
    /// Adds a field to the line at the specific location.
    /// </summary>
    /// <param name="value">Text to place in the field</param>
    /// <param name="index">Field position</param>
    public void AddField(string value, int index = -1)
    {
        if (index >= 0)
        {
            this[index] = value;
        }
        else
        {
            this.fields.Add(value);
        }
    }

    /// <summary>
    /// Using this ToString will override the delimiter entered at construction.
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
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override string ToString()
    {
        return ToString(delimiter);
    }


}
