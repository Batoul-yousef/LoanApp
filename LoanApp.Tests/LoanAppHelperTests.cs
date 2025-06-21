using Xunit;
using LoanApp;

namespace LoanApp.Tests
{
    public class LoanAppHelperTests
    {
        private readonly LoanEvaluator evaluator = new LoanEvaluator();

        [Theory]
        [InlineData(1500, true, 750, 0, true, "Not Eligible")]        // دخل غير كافٍ
        [InlineData(2500, true, 750, 0, true, "Eligible")]            // مؤهل بوظيفة ونقاط عالية بدون معالين
        [InlineData(2500, true, 750, 2, false, "Review Manually")]    // وظيفة + نقاط عالية + 2 معالين
        [InlineData(6000, false, 760, 1, true, "Eligible")]           // بدون وظيفة + بيت + نقاط عالية + دخل مرتفع
        [InlineData(3000, false, 660, 0, false, "Review Manually")]   // بدون وظيفة + نقاط متوسطة + لا معالين
        [InlineData(3000, false, 620, 1, false, "Not Eligible")]      // بدون وظيفة + نقاط منخفضة
        public void GetLoanEligibility_IntegrationScenarios(
            int income, bool hasJob, int creditScore, int dependents, bool ownsHouse, string expected)
        {
            // Arrange & Act
            var result = evaluator.GetLoanEligibility(income, hasJob, creditScore, dependents, ownsHouse);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
