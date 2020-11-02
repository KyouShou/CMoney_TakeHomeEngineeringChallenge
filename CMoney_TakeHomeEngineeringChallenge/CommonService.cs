﻿using System;
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
        //檢查日期格式是否為yyyyMMdd
        public bool ChkIsDate(string date)
        {
            DateTime dateTime = new DateTime();

            IFormatProvider ifp = new CultureInfo("zh-TW", true);

            var result = DateTime.TryParseExact(date, "yyyyMMdd", ifp, DateTimeStyles.None, out dateTime);

            return result;
        }

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

    }
}