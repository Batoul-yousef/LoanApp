 using Xunit;
 using LoanApp;
 namespace LoanApp.Tests
 {
 public class LoanEvaluatorTests
 {
 [Fact]
 public void GetLoanEligibility_Should_Return_NotEligible_When_Income_Low()
 {
 var evaluator = new LoanEvaluator();
 var result = evaluator.GetLoanEligibility(1500, true, 800, 5, true);
 Assert.Equal("Not Eligible", result);
 }
 }
 }