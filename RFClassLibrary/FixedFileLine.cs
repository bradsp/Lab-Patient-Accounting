using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace RFClassLibrary
{

    /// <summary>
    /// This class allows the creation of fixed field length text strings. 
    /// </summary>
    public class FixedFileLine
    {
        private struct FieldData
        {
            public string strData;
            public int nLen;
            public int nStartPos;
            public int nEndPos;
        }

        private int nFields;
        private FieldData[] fldData;

        /// <summary>
        /// Pass the number of fields the line will contain.
        /// </summary>
        /// <param name="_numFields"></param>
        public FixedFileLine(int _numFields)
        {
            fldData = new FieldData[_numFields];
            nFields = _numFields;
        }

        /// <summary>
        /// Places the field at the appropriate location in the line.
        /// </summary>
        /// <param name="fieldNum">Pass the 1 based number of the field within the fixed length record</param>
        /// <param name="startPos">The starting position of the field</param>
        /// <param name="endPos">The ending position of the field</param>
        /// <param name="strData">The text to write into the line</param>
        public void SetField(int fieldNum, int startPos, int endPos, string strData)
        {
            fldData[fieldNum - 1].nStartPos = startPos;
            fldData[fieldNum - 1].nEndPos = endPos;
            fldData[fieldNum - 1].strData = strData;
            fldData[fieldNum - 1].nLen = endPos - startPos + 1;
        }

        /// <summary>
        /// Places the field at the appropriate location in the line. Computes the location based on specified length and field placement.
        /// </summary>
        /// <param name="fieldNum">The one-based number of the field within the fixed length record</param>
        /// <param name="len">The length of the field</param>
        /// <param name="strData">The text to write into the line</param>
        public void SetField(int fieldNum, int len, string strData)
        {
            fldData[fieldNum - 1].strData = strData;
            fldData[fieldNum - 1].nLen = len;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldNum"></param>
        /// <returns></returns>
        public string GetFieldValue(int fieldNum)
        {
            return fldData[fieldNum - 1].strData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string OutputLine()
        {
            StringBuilder sb = new StringBuilder(2538, 2538);

            for (int i = 0; i < nFields; i++)
            {
                if (fldData[i].nLen == 0)
                {
                    string msg = string.Format("FixedLength Field Parameter cannot be zero length. Field Number: {0} Value: {1}", i, fldData[i].strData);
                    throw new Exception(msg);
                }
                sb.AppendFixed(fldData[i].nLen, fldData[i].strData);
            }

            return sb.ToString();
        }
    }
}
