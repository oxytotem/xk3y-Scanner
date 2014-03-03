using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeLib.Csv
{
    public class CsvDecoder
    {
        public static CsvData Decode(string text)
        {
            return Decode(text, false);
        }

        public static CsvData Decode(string text, bool hasHeader)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            bool success;
            Parser parser = new Parser();
            CsvData csvData = parser.ParseCsvData(new TextInput(text), out success);
            if (success)
            {
                if (hasHeader)
                {
                    csvData.Header = csvData.Records[0];
                    csvData.Records.RemoveAt(0);
                }
                return csvData;
            }
            else
            {
                throw new Exception("There are syntax errors in the csv text.");
            }
        }
    }
}
