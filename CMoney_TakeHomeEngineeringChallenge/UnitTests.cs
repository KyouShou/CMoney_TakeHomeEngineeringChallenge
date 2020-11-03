using CMoney_TakeHomeEngineeringChallenge.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMoney_TakeHomeEngineeringChallenge
{
    [TestFixture]
    public class UnitTests
    {
        [Test]
        public void CommonService_ChkIsDate_Test()
        {
            var service = new CommonService();
            var result = service.ChkIsDate("20201111");
            Assert.IsTrue(result, "輸入20201111，結果應為true");

            result = service.ChkIsDate("12345678");
            Assert.IsFalse(result, "輸入12345678，結果應為false");

            result = service.ChkIsDate("中文");
            Assert.IsFalse(result, "輸入中文，結果應為false");

            result = service.ChkIsDate("123456789");
            Assert.IsFalse(result, "輸入123456789，結果應為false");

            result = service.ChkIsDate("123");
            Assert.IsFalse(result, "輸入123，結果應為false");

            result = service.ChkIsDate("abc");
            Assert.IsFalse(result, "輸入123，結果應為false");
        }

        [Test]
        public void CommonService_ChkIsPositiveInteger_Test()
        {
            var service = new CommonService();
            var result = service.ChkIsPositiveInteger("1");
            Assert.IsTrue(result, "輸入1，結果應為true");

            result = service.ChkIsPositiveInteger("-1");
            Assert.IsFalse(result, "輸入-1，結果應為false");

            result = service.ChkIsPositiveInteger("0.5");
            Assert.IsFalse(result, "輸入0.5，結果應為false");

            result = service.ChkIsPositiveInteger("abc");
            Assert.IsFalse(result, "輸入abc，結果應為false");

            result = service.ChkIsPositiveInteger("中文");
            Assert.IsFalse(result, "輸入中文，結果應為false");
        }

        [Test]
        public void CommonService_Get_PERatio_Lasting_Date_Test()
        {
            var service = new CommonService();
            //塞假資料
            var fakeData = SetFakeCSVModelListData();
            var result = service.Get_PERatio_Lasting_Date(fakeData , "2330");
            Assert.IsTrue(int.Parse(result.Days) == 1 , "最大天數應該為1");
            Assert.IsTrue(result.StartDate == "20201031", "開始日期應為20201031");
            Assert.IsTrue(result.EndDate == "20201101", "結束日期應為20201101");
        }

        List<CSVModel> SetFakeCSVModelListData()
        {
            var result = new List<CSVModel>();
            var model = new CSVModel();


            //塞10/31資料
            model.DividendYear = "108";
            model.DataDate = "20201031";
            model.FinancialReportingTime = "109/2";
            model.NetSharePrice = "1.50";
            model.PERatio = "10.55";
            model.SecuritiesName = "台積電";
            model.SecuritiesSymbol = "2330";
            model.YieldRate = "1.1";
            result.Add(model);
            model = new CSVModel();

            model.DividendYear = "108";
            model.DataDate = "20201031";
            model.FinancialReportingTime = "109/2";
            model.NetSharePrice = "1.50";
            model.PERatio = "12.58";
            model.SecuritiesName = "黑松";
            model.SecuritiesSymbol = "1234";
            model.YieldRate = "1.31";
            result.Add(model);
            model = new CSVModel();


            //塞11/1資料
            model.DividendYear = "108";
            model.DataDate = "20201101";
            model.FinancialReportingTime = "109/2";
            model.NetSharePrice = "1.50";
            model.PERatio = "10.55";
            model.SecuritiesName = "台積電";
            model.SecuritiesSymbol = "2330";
            model.YieldRate = "1.5";
            result.Add(model);
            model = new CSVModel();

            model.DividendYear = "108";
            model.DataDate = "20201101";
            model.FinancialReportingTime = "109/2";
            model.NetSharePrice = "1.50";
            model.PERatio = "12.58";
            model.SecuritiesName = "黑松";
            model.SecuritiesSymbol = "1234";
            model.YieldRate = "1.31";
            result.Add(model);
            model = new CSVModel();

            //塞11/2資料
            model.DividendYear = "108";
            model.DataDate = "20201102";
            model.FinancialReportingTime = "109/2";
            model.NetSharePrice = "1.50";
            model.PERatio = "10.55";
            model.SecuritiesName = "台積電";
            model.SecuritiesSymbol = "2330";
            model.YieldRate = "0.8";
            result.Add(model);
            model = new CSVModel();

            model.DividendYear = "108";
            model.DataDate = "20201102";
            model.FinancialReportingTime = "109/2";
            model.NetSharePrice = "1.50";
            model.PERatio = "12.58";
            model.SecuritiesName = "黑松";
            model.SecuritiesSymbol = "1234";
            model.YieldRate = "1.31";
            result.Add(model);
            model = new CSVModel();





            return result;
        }
    }
}
