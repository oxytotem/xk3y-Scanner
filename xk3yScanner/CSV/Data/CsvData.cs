using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeLib.Csv
{
	public partial class CsvData
	{
        public CsvRecord Header;

        public List<CsvRecord> Records = new List<CsvRecord>();

        /// <summary>
        /// Check whether has header record
        /// </summary>
        public bool HasHeader
        {
            get { return Header != null; }
        }
	}
}
