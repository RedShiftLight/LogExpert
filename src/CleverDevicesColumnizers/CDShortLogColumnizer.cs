using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogExpert;

namespace CleverDevices
{
    public class CDShortLogColumnizer : ILogLineColumnizer
    {
        protected string[] columnNames;
        protected int columnCount = 0;
        public string Text => GetName();

        public CDShortLogColumnizer()
        {
            columnNames = new string[] { "Date", "Time", "LogLevel", "Application", "Module", "Message" };
            columnCount = columnNames.Length;
        }

        public int GetColumnCount()
        {
            return columnCount;
        }

        public string[] GetColumnNames()
        {
            return columnNames;
        }

        public string GetDescription()
        {
            return "Displays the 6 main columns";
        }

        public string GetName()
        {
            return "CleverWare Short Log Columnizer";
        }

        public int GetTimeOffset()
        {
            //Only needed if doing time shifting
            return 0;
        }

        public DateTime GetTimestamp(ILogLineColumnizerCallback callback, ILogLine line)
        {
            //Only needed if doing time shifting
            return DateTime.MinValue;
        }

        public bool IsTimeshiftImplemented()
        {
            return false;
        }

        public void PushValue(ILogLineColumnizerCallback callback, int column, string value, string oldValue)
        {
            //Only needed if doing time shifting
        }

        public void SetTimeOffset(int msecOffset)
        {
            //Only needed if doing time shifting
        }

        public IColumnizedLogLine SplitLine(ILogLineColumnizerCallback callback, ILogLine line)
        {
            ColumnizedLogLine columnizedLogLine = new ColumnizedLogLine();
            columnizedLogLine.LogLine = line; // Add the reference to the LogLine 
            string[] textData = line.FullLine.Split('|');
            List<Column> colData = new List<Column>();

            if (textData.Length >= 6)
            {
                for (int i = 0; i < 5; i++)
                {
                    Column col = new Column
                    {
                        FullValue = textData[i],
                        Parent = columnizedLogLine
                    };
                    colData.Add(col);
                }

                //The message is the last column
                Column msgCol = new Column
                {
                    FullValue = textData[textData.Length - 1],
                    Parent = columnizedLogLine
                };
                colData.Add(msgCol);
            }
            else
            {
                //This is for log entries that extended to the next line(s)
                //Put all of it into the message
                for (int i = 0; i < columnCount - 1; i++)
                {
                    Column col = new Column
                    {
                        FullValue = string.Empty,
                        Parent = columnizedLogLine
                    };
                    colData.Add(col);
                }

                Column msgCol = new Column
                {
                    FullValue = line.FullLine,
                    Parent = columnizedLogLine
                };
                colData.Add(msgCol);
            }

            columnizedLogLine.ColumnValues = colData.ToArray();
            return columnizedLogLine;
        }
    }
}
