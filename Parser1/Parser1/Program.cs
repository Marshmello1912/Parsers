using AngleSharp.Dom;

using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Parser1
{
    public class TableData
    {
        string name;
        string last;
        string changes;
        string dateTime;
        string isin;
        string bidVolume;
        string askVolume;
        string maturity;
        string status;


        public string Name
        {
            get => name;
            set => name = value;
        }


        public string Last
        {
            get => last;
            set => last = value;
        }

        [Name("Chg. % 1D Chg. Abs.")]
        public string Changes
        {
            get => changes;
            set => changes = value;
        }

        [Name("Date Time")]
        public string DateTime
        {
            get => dateTime;
            set => dateTime = value;
        }

        [Name("ISIN")]
        public string Isin
        {
            get => isin;
            set => isin = value;
        }

        [Name("Bid Volume")]
        public string BidVolume
        {
            get => bidVolume;
            set => bidVolume = value;
        }

        [Name("Ask Volume")]
        public string AskVolume
        {
            get => askVolume;
            set => askVolume = value;
        }

        [Name("Maturity")]
        public string Maturity
        {
            get => maturity;
            set => maturity = value;
        }


        public string Status
        {
            get => status;
            set => status = value;
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

            int pageNum = 1;
            int lastPageNumber = pageNum;

            string htmlDoc = default(string);
            try
            {
                htmlDoc = web.GetPage(pageNum);
                log.Info($"Получнеа страница {pageNum}");

            }
            catch (AggregateException e)
            {
                log.Error($"{e.Message} \n Не удалось получить {pageNum} страницу");
                return;
            }


            IHtmlCollection<IElement> elems = await parser.GetElems(htmlDoc, "ul.pagination li a");
            IHtmlCollection<IElement> tableHeaders = await parser.GetElems(htmlDoc, "tr th");
            IHtmlCollection<IElement> rows;


            foreach (var nums in elems)
            {
                int tmp;
                if (int.TryParse(nums.Text(), out tmp))
                {
                    lastPageNumber = tmp;
                }
            }
            log.Info($"Получено количество страниц ({lastPageNumber})");

            while (true)
            {
                rows = await parser.GetElems(htmlDoc, "tbody tr");
                foreach (IElement row in rows)
                {

                    IHtmlCollection<IElement> cols = row.QuerySelectorAll("td");
                    IHtmlCollection<IElement> changes = cols[2].QuerySelectorAll("div");
                    IHtmlCollection<IElement> bidVolume = cols[5].QuerySelectorAll("div");
                    IHtmlCollection<IElement> askVolume = cols[6].QuerySelectorAll("div");
                    String dateTime = "-";
                    if (cols[3].Text() != "-")
                    {
                        dateTime = cols[3].Text().Insert(10, " ");
                    }



                    tableData.Add(new TableData
                    {
                        Name = cols[0].Text(),
                        Last = cols[1].Text(),
                        Changes = string.Concat(changes[0].Text(), " ", changes[1].Text()),
                        DateTime = dateTime,
                        Isin = cols[4].Text(),
                        BidVolume = string.Concat(bidVolume[0].Text(), " ", bidVolume[1].Text()),
                        AskVolume = string.Concat(askVolume[0].Text(), " ", askVolume[1].Text()),
                        Maturity = cols[7].Text(),
                        Status = cols[8].Text()
                    });

                }

                if (pageNum != lastPageNumber)
                {
                    try
                    {
                        htmlDoc = web.GetPage(++pageNum);
                        log.Info($"Получнеа страница {pageNum}");
                    }
                    catch (AggregateException e)
                    {
                        log.Error($"{e.Message} \n Не удалось получить {pageNum} страницу");
                        break;
                    }
                }
                else
                {
                    break;
                }
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
