using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMoney_TakeHomeEngineeringChallenge.Model
{
    public class CSVModel
    {
        //證券代號
        public string SecuritiesSymbol { set; get; }
        //證券名稱
        public string SecuritiesName { set; get; }
        //殖利率
        public string YieldRate { set; get; }
        //股利年度
        public string DividendYear { set; get; }
        //本益比
        public string PERatio { set; get; }
        //股價淨值
        public string NetSharePrice { set; get; }
        //財報時間
        public string FinancialReportingTime { set; get; }
        //資料日期
        public string DataDate { set; get; }


    }
}
