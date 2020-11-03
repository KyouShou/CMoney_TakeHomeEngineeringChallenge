using CMoney_TakeHomeEngineeringChallenge.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CMoney_TakeHomeEngineeringChallenge
{
    //本類別用以向TWSE(臺灣證券交易所)取得資料
    public class TWSEService
    {
        CommonService service = new CommonService();

        public List<CSVModel> GetCSVFromTWSE(string targetDate)
        {
            if (!service.ChkIsDate(targetDate))
            {
                throw new Exception("輸入的日期格式有誤");
            }

            try
            {
                string url = "https://www.twse.com.tw/exchangeReport/BWIBBU_d?response=csv&date=" + targetDate + "&selectType=ALL";

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(950), true);

                string csvString = "";

                //先讀兩行，把csv的前兩行無用資料去掉
                sr.ReadLine();
                sr.ReadLine();

                //讀完所有資料
                while (!sr.EndOfStream)
                {
                    csvString += sr.ReadLine();
                }

                sr.Close();

                //周末假日無資料，csvString會是空字串，return空的List
                if (String.IsNullOrEmpty(csvString))
                {
                    return new List<CSVModel>();
                }

                //把結尾的說明及空白行數去掉
                csvString = csvString.Substring(0, csvString.IndexOf("說明：") - 4);

                //將字串轉換為model的List後輸出
                var result = Convert_CSVString_To_CSVModel(csvString, targetDate);

                return result;
            }
            catch(Exception e)
            {
                throw new Exception("連接台灣證券交易所取得資料時發生問題，請確認網路連線是否正常、台灣證券交易所伺服器是否維修");
            }
        }

        public List<CSVModel> GetCSVFromTWSE(string startDate, string endDate)
        {
            if (!service.ChkIsDate(startDate) || !service.ChkIsDate(endDate))
            {
                throw new Exception("輸入的日期格式有誤");
            }
            try
            {
                List<CSVModel> result = new List<CSVModel>();

                //計算開始日期和結束日期相差幾天，計算出相差天數後+1才能讓迴圈進行正確次數的迴圈
                var startDateTime = service.Parse_yyyyMMdd_To_DateTime(startDate);
                var endDateTime = service.Parse_yyyyMMdd_To_DateTime(endDate);
                int days = (int)(endDateTime - startDateTime).TotalDays + 1;

                var targetDate = service.Parse_yyyyMMdd_To_DateTime(startDate);

                //使用迴圈呼叫資料，並塞到result的List
                for (int i = 0; i < days; i++)
                {
                    var csvModelList = GetCSVFromTWSE(targetDate.ToString("yyyyMMdd"));
                    result = result.Concat(csvModelList).ToList();
                    //把目標日期+1天，用以下次迴圈呼叫資料
                    targetDate = targetDate.AddDays(1);
                    Thread.Sleep(5000);
                }

                return result;

            }
            catch (Exception e)
            {
                throw new Exception("連接台灣證券交易所取得資料時發生問題，請確認網路連線是否正常、台灣證券交易所伺服器是否維修");
            }
        }


        List<CSVModel> Convert_CSVString_To_CSVModel(string CSVString, string targetDate)
        {
            var result = new List<CSVModel>();
            var model = new CSVModel();

            //由於檔案中，資料欄裡會出現逗號，因此不能用string.Split(',')，資料會錯亂
            List<string> data = CSVString.Split(new string[] { "\",\"" }, StringSplitOptions.None).ToList<string>();

            //用dataType來控制接下來要塞入的資料類型，0證券代號，1證券名稱，2殖利率，3股利年度，4本益比，5股價淨值，6財報時間，6檔案日期
            int dataType = 0;

            foreach (var str in data)
            {
                string tmpString = str.Replace("\"", "");
                tmpString = tmpString.Replace("\\", "");
                tmpString = tmpString.Replace(" ", "");


                //0證券代號
                if (dataType % 7 == 0)
                {
                    model.SecuritiesSymbol = tmpString;
                }
                //1證券名稱
                else if (dataType % 7 == 1)
                {
                    model.SecuritiesName = tmpString;
                }
                //2殖利率
                else if (dataType % 7 == 2)
                {
                    model.YieldRate = tmpString;
                }
                //3股利年度
                else if (dataType % 7 == 3)
                {
                    model.DividendYear = tmpString;
                }
                //4本益比
                else if (dataType % 7 == 4)
                {
                    model.PERatio = tmpString;
                }
                //股價淨值
                else if (dataType % 7 == 5)
                {
                    model.NetSharePrice = tmpString;
                }
                //財報時間
                else if (dataType % 7 == 6)
                {
                    model.FinancialReportingTime = tmpString;
                    //檔案時間在此一同塞入
                    model.DataDate = targetDate;
                    //model塞滿之後，把model加入List並初始化model
                    result.Add(model);
                    model = new CSVModel();
                }

                //每塞入一筆資料，dataType+1來控制下一筆資料要塞入的位置
                dataType++;
            }


            return result;

        }

    }
}
