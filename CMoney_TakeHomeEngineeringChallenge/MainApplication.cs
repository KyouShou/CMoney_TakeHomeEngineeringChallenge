using CMoney_TakeHomeEngineeringChallenge.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMoney_TakeHomeEngineeringChallenge
{
    //此類別為程式進入點，也可以視為程式的目錄
    public class MainApplication
    {
        CommonService service = new CommonService();
        TWSEService twseService = new TWSEService();

        public void Start()
        {
            while (true)
            {
                var functionType = Ask_For_FunctionType();

                switch (functionType)
                {
                    //功能1：依照證券代號 搜尋最近n天的資料
                    case "1":
                        SecuritiesSymbolAndDateModel condition_CaseOne = Ask_For_SecuritiesSymbolAndDate();
                        Show_SecuritiesData_By_SecuritiesSymbolAndDate(condition_CaseOne);
                        break;
                    //功能2：指定特定日期 顯示當天本益比前n名
                    case "2":
                        RankingAndDateModel condition_CaseTwo = Ask_For_RankingAndDate();
                        Show_PERatioRank_By_Date(condition_CaseTwo);
                        break;
                    //功能3：指定日期範圍、證券代號 顯示這段時間內殖利率 為嚴格遞增的最長天數並顯示開始、結束日期
                    case "3":
                        SecuritiesSymbolAndDateModel Condition_CaseThree = Ask_For_SecuritiesSymbolAndDate();
                        Show_IncreaseDate_By_SecuritiesSymbolAndDate(Condition_CaseThree);
                        break;
                    default:
                        Console.WriteLine("輸入錯誤，請再試一次");
                        break;
                }
            }
        }

        string Ask_For_FunctionType()
        {
            Console.WriteLine("請輸入數字並按下Enter以執行對應功能");
            Console.WriteLine("1.依照證券代號 搜尋最近n天的資料");
            Console.WriteLine("2.指定特定日期 顯示當天本益比前n名");
            Console.WriteLine("3.指定日期範圍、證券代號 顯示這段時間內殖利率 為嚴格遞增的最長天數並顯示開始、結束日期");

            var result = Console.ReadLine();
            return result;
        }



        SecuritiesSymbolAndDateModel Ask_For_SecuritiesSymbolAndDate()
        {
            var result = new SecuritiesSymbolAndDateModel();


            Console.WriteLine("請輸入證券代號");
            result.SecuritiesSymbol = Console.ReadLine();

            while (true)
            {
                Console.WriteLine("請輸入起始日期(格式範例：20200503)");
                var startDate = Console.ReadLine();

                if (service.ChkIsDate(startDate))
                {
                    result.StartDate = startDate;

                    //取得正確資料後離開迴圈
                    break;
                }
                else
                {
                    Console.WriteLine("日期格式輸入錯誤，請再試一次(格式範例：20200503)");
                }
            }


            while (true)
            {
                Console.WriteLine("請輸入結束日期(格式範例：20200503)");
                var endDate = Console.ReadLine();
                if (service.ChkIsDate(endDate))
                {
                    result.EndDate = endDate;

                    //取得正確資料後離開迴圈
                    break;
                }
                else
                {
                    Console.WriteLine("日期格式輸入錯誤，請再試一次(格式範例：20200503)");
                }
            }


            return result;

        }

        RankingAndDateModel Ask_For_RankingAndDate()
        {
            var result = new RankingAndDateModel();

            while (true)
            {
                Console.WriteLine("請輸入排名(阿拉伯數字)");
                var ranking = Console.ReadLine();

                if (service.ChkIsPositiveInteger(ranking))
                {
                    result.Ranking = ranking;
                    //取得正確資料後離開迴圈
                    break;
                }
                else
                {
                    Console.WriteLine("排名格式輸入錯誤，請再試一次");
                }
            }

            while (true)
            {
                Console.WriteLine("請輸入指定日期(格式範例：20200503)");
                var targetDate = Console.ReadLine();
                if (service.ChkIsDate(targetDate))
                {
                    result.TargetDate = targetDate;
                    //取得正確資料後離開迴圈
                    break;
                }
                else
                {
                    Console.WriteLine("日期格式輸入錯誤，請再試一次(格式範例：20200503)");
                }
            }

            return result;
        }


        void Show_SecuritiesData_By_SecuritiesSymbolAndDate(SecuritiesSymbolAndDateModel securitiesSymbolAndDate)
        {
            var dataInDateRange = twseService.GetCSVFromTWSE(securitiesSymbolAndDate.StartDate, securitiesSymbolAndDate.EndDate);
            var dataList = dataInDateRange.FindAll(x => x.SecuritiesSymbol == securitiesSymbolAndDate.SecuritiesSymbol);

            Console.WriteLine("證券代號\t證券名稱\t殖利率(%)\t股利年度\t本益比\t\t股價淨值\t財報年/季\t資料日期");
            Console.WriteLine("======================================================================================");
            foreach (var data in dataList)
            {
                Console.WriteLine(data.SecuritiesSymbol + "\t\t" + data.SecuritiesName + "\t\t" + data.YieldRate + "\t\t" + data.DividendYear + "\t\t" + data.NetSharePrice + "\t\t" + data.FinancialReportingTime + "\t\t" + data.DataDate);
            }
        }

        void Show_PERatioRank_By_Date(RankingAndDateModel model)
        {
            string ranking = model.Ranking;
            string date = model.TargetDate;

            var dataList = twseService.GetCSVFromTWSE(date);
            //本益比會有"-"號出現，要先全部處理掉
            var listHaveMinusSign = dataList.FindAll(x => x.PERatio == "-");
            dataList = dataList.Except(listHaveMinusSign).ToList();
            dataList = dataList.OrderByDescending(x => double.Parse(x.PERatio)).ToList();

            Console.WriteLine("證券代號\t證券名稱\t殖利率(%)\t股利年度\t本益比\t\t股價淨值\t財報年/季\t資料日期");
            Console.WriteLine("======================================================================================");

            int rankingInt = int.Parse(ranking);
            for (int i = 0; i < rankingInt; i++)
            {
                Console.WriteLine(dataList[i].SecuritiesSymbol + "\t\t" + dataList[i].SecuritiesName + "\t\t" + dataList[i].YieldRate + "\t\t" + dataList[i].DividendYear + "\t\t" + dataList[i].PERatio + "\t\t" + dataList[i].NetSharePrice + "\t\t" + dataList[i].FinancialReportingTime + "\t\t" + dataList[i].DataDate);
            }
        }

        void Show_IncreaseDate_By_SecuritiesSymbolAndDate(SecuritiesSymbolAndDateModel securitiesSymbolAndDate)
        {
            var dataInDateRange = twseService.GetCSVFromTWSE(securitiesSymbolAndDate.StartDate, securitiesSymbolAndDate.EndDate);
            var result = service.Get_PERatio_Lasting_Date(dataInDateRange, securitiesSymbolAndDate.SecuritiesSymbol);

            Console.WriteLine("該商品殖利率最大遞增天數為：" + result.Days);
            Console.WriteLine("開始日期" + result.StartDate);
            Console.WriteLine("結束日期" + result.EndDate);
        }


    }


}
