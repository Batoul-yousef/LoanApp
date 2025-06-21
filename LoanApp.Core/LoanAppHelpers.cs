using Xunit;
using LoanApp;
using System.Reflection;

namespace LoanApp.Tests
{
    public class LoanAppHelpers
    {
        private readonly LoanEvaluator evaluator = new LoanEvaluator();

        // استخدام Reflection لاستدعاء التوابع الخاصة
        private T InvokePrivateMethod<T>(string methodName, params object[] parameters)
        {
            var method = typeof(LoanEvaluator).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)method.Invoke(evaluator, parameters);
        }

        [Theory]
        [InlineData(1500, false)]
        [InlineData(1999, false)]
        [InlineData(2000, true)]
        [InlineData(3000, true)]
        public void Test_IsIncomeEligible(int income, bool expected)
        {
            var result = InvokePrivateMethod<bool>("IsIncomeEligible", income);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(750, 0, "Eligible")]
        [InlineData(750, 1, "Review Manually")]
        [InlineData(750, 3, "Not Eligible")]
        [InlineData(650, 0, "Not Eligible")]
        [InlineData(599, 0, "Not Eligible")]
        public void Test_EvaluateWithJob(int creditScore, int dependents, string expected)
        {
            var result = InvokePrivateMethod<string>("EvaluateWithJob", creditScore, dependents);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(760, 6000, 1, true, "Eligible")]
        [InlineData(660, 3000, 0, false, "Review Manually")]
        [InlineData(640, 3000, 1, false, "Not Eligible")]
        public void Test_EvaluateWithoutJob(int creditScore, int income, int dependents, bool ownsHouse, string expected)
        {
            var result = InvokePrivateMethod<string>("EvaluateWithoutJob", creditScore, income, dependents, ownsHouse);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1500, true, 750, 0, true, "Not Eligible")] // دخل غير مؤهل
        [InlineData(2500, true, 750, 0, true, "Eligible")]      // مؤهل مع وظيفة
        [InlineData(2500, false, 760, 1, true, "Eligible")]     // مؤهل بدون وظيفة
        [InlineData(2500, false, 640, 2, false, "Not Eligible")]// غير مؤهل بدون وظيفة
        public void Test_GetLoanEligibility(int income, bool hasJob, int creditScore, int dependents, bool ownsHouse, string expected)
        {
            var result = evaluator.GetLoanEligibility(income, hasJob, creditScore, dependents, ownsHouse);
            Assert.Equal(expected, result);
        }
    }
}
