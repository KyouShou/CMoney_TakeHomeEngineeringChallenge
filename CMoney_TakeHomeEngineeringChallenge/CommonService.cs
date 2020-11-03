using CMoney_TakeHomeEngineeringChallenge.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMoney_TakeHomeEngineeringChallenge
{
    //本類別提供有可能跨類別使用的通用方法
    public class CommonService
    {
        /// <summary>
        /// 檢查日期格式是否為yyyyMMdd，是回傳true，否則回傳false
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool ChkIsDate(string date)
        {
            DateTime dateTime = new DateTime();

            IFormatProvider ifp = new CultureInfo("zh-TW", true);

            var result = DateTime.TryParseExact(date, "yyyyMMdd", ifp, DateTimeStyles.None, out dateTime);

            return result;
        }

        /// <summary>
        /// 檢查時間是否早於台灣證券交易所的最早資料或者晚於今日(不含今日，因為證交所可能還沒上資料)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool ChkIsDateLegal(string date)
        {
            return true;
        }

        /// <summary>
        /// 檢查字串是否為正整數(包含0)，是回傳true，否則回傳false
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool ChkIsPositiveInteger(string number)
        {
            var outParseResult = 0;
            var parseResult = Int32.TryParse(number, out outParseResult);

            if (outParseResult >= 0 && parseResult)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DateTime Parse_yyyyMMdd_To_DateTime(string yyyyMMdd)
        {
            try
            {
                DateTime dateTime = new DateTime();

                IFormatProvider ifp = new CultureInfo("zh-TW", true);

                var result = DateTime.TryParseExact(yyyyMMdd, "yyyyMMdd", ifp, DateTimeStyles.None, out dateTime);

                return dateTime;
            }
            catch
            {
                throw new Exception("輸入的時間格式有誤");
            }

        }

        /// <summary>
        /// 輸入List<CSVModel>及指定的證券代碼，取得此List<CSVModel>中殖利率遞增的時間點及天數
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public DaysAndDateModel Get_PERatio_Lasting_Date(List<CSVModel> list, string securitiesSymbol)
        {
            var result = new DaysAndDateModel();
            //為防這裡的排序及篩選影響其他方法的排序，因此開新list
            var newList = new List<CSVModel>();
            newList = list.FindAll(x => x.SecuritiesSymbol == securitiesSymbol);
            newList = newList.OrderBy(x => int.Parse(x.DataDate)).ToList();

            //記錄最大長度起點
            var startDate = list[0].DataDate;

            //記錄最大持續長度
            int longestLength = 0;

            //記錄最大長度終點
            var endDate = list[0].DataDate;

            //記錄本次起點
            var startDateNow = list[0].DataDate;

            //記錄本次維持長度
            int longestLengthNow = 0;

            //記錄本次終點
            var endDateNow = list[0].DataDate;

            for (int i = 0; i < newList.Count; i++)
            {
                startDateNow = list[i].DataDate;

                while (i + 1 < newList.Count && double.Parse(newList[i + 1].YieldRate) > double.Parse(newList[i].YieldRate))
                {
                    i++;
                    longestLengthNow++;
                }

                endDateNow = newList[i].DataDate;

                if (longestLengthNow > longestLength)
                {
                    startDate = startDateNow;
                    endDate = endDateNow;
                    longestLength = longestLengthNow;
                }

                longestLengthNow = 0;
            }

            result.Days = longestLength.ToString();
            result.StartDate = startDate;
            result.EndDate = endDate;

            if (longestLength == 0)
            {
                result.Days = "查無資料";
                result.StartDate = "查無資料";
                result.EndDate = "查無資料";
            }


            return result;
        }
    }
}
