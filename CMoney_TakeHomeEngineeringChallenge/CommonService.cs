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
    }
}
