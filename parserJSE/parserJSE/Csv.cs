using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;


namespace parserJSE
{
    internal class Csv
    {
        private CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);

        public Csv(string delimeter = "\t")
        {
            config.Delimiter = delimeter;
        }
        public void Write(List<TableData> tableDatas)
        {
            using (var writer = new StreamWriter($"{DateTime.Now:yyyy.MM.dd HH.mm.ss}.csv"))
            using (var csvWriter = new CsvWriter(writer, config))
            {
                csvWriter.WriteRecords(tableDatas);
            }
        }

    }
}
