using AngleSharp.Dom;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace parserJSE
{
    public class TableData
    {

        string futurePrice;
        string premium;
        string vol;
        string quantity;
        string callPut;
        string strike;
        string futureExpiry;
        string shortName;
        string tradeType;
        string tradeDate;

        public string TradeDate
        {
            get => tradeDate;
            set => tradeDate = value;
        }


        public string TradeType
        {
            get => tradeType;
            set => tradeType = value;
        }


        public string ShortName
        {
            get => shortName;
            set => shortName = value;
        }


        public string FutureExpiry
        {
            get => futureExpiry;
            set => futureExpiry = value;
        }


        public string Strike
        {
            get => strike;
            set => strike = value;
        }


        public string CallPut
        {
            get => callPut;
            set => callPut = value;
        }


        public string Quantity
        {
            get => quantity;
            set => quantity = value;
        }


        public string Vol
        {
            get => vol;
            set => vol = value;
        }


        public string Premium
        {
            get => premium;
            set => premium = value;
        }


        public string FuturePrice
        {
            get => futurePrice;
            set => futurePrice = value;
        }


    }
    internal class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        static async Task Main(string[] args)
        {
            if (log.IsInfoEnabled) log.Info("Приложение Запущено");

            Parser parser = new Parser();
            Web web = new Web();

            List<IHtmlCollection<IElement>> dataRows = new List<IHtmlCollection<IElement>>();
            List<TableData> tableData = new List<TableData>();

            string htmlDoc = default(string);
            try
            {
                htmlDoc = await web.GetPage();
                log.Info($"Страница получена");

            }
            catch (AggregateException e)
            {
                log.Error($"{e.Message} \n Не удалось получить страницу");
                return;
            }
           
            IHtmlCollection<IElement> rows;
            
            rows = await parser.GetElems(htmlDoc, "tbody tr");
          
            foreach (IElement row in rows)
            {

                IHtmlCollection<IElement> cols = row.QuerySelectorAll("td");
                tableData.Add(new TableData
                {
                    TradeDate = cols[0].Text(),
                    TradeType = cols[1].Text(),
                    ShortName = cols[2].Text(),
                    FutureExpiry = cols[3].Text(),
                    Strike = cols[4].Text(),
                    CallPut = cols[5].Text(),
                    Quantity = cols[6].Text(),
                    Vol = cols[7].Text(),
                    Premium = cols[8].Text(),
                    FuturePrice = cols[9].Text()
                });

            }

            try
            {
                Csv csv = new Csv("\t");
                csv.Write(tableData);
                log.Info("СSV файл успешно сохранен");
            }
            catch (Exception e)
            {
                log.Error($"{e.Message} \n Не удалалось записать Csv файл");
            }
            return;
        }

    }
}
