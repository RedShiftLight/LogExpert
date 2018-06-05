using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using LogExpert;

namespace CleverDevices
{
    public class CDFullLogColumnizer : ILogLineColumnizer
    {
        protected string[] columnNames;
        protected int columnCount = 0;
        public string Text => GetName();

        public CDFullLogColumnizer()
        {
            columnNames = new string[] { "Date", "Time", "LogLevel", "Application", "Module", "Latitude", "Longitude", "Heading", "Speed", "Distance", "PulseCount", "ThreadId", "CPU %", "CPUFreq %", "Mem %", "Message" };
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

        public virtual string GetDescription()
        {
            return "Displays all columns";
        }

        public virtual string GetName()
        {
            return "CleverWare Full Log Columnizer";
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

        public virtual IColumnizedLogLine SplitLine(ILogLineColumnizerCallback callback, ILogLine line)
        {
            ColumnizedLogLine columnizedLogLine = new ColumnizedLogLine();
            columnizedLogLine.LogLine = line; // Add the reference to the LogLine 
            string[] textData = line.FullLine.Split('|');
            List<Column> colData = new List<Column>();

            if (textData.Length == columnCount)
            {
                foreach (var item in textData)
                {
                    Column col = new Column
                    {
                        FullValue = item,
                        Parent = columnizedLogLine
                    };
                    colData.Add(col);
                }
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
