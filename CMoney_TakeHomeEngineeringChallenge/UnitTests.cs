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

    }
}
