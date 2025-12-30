using CourierEstimator.Core.Helpers;

namespace CourierEstimator.Test.Helpers
{
    public class RoundHleperFacts
    {
        public class RoundHelperTests
        {
            [Theory]
            [InlineData(1.7857143, 1.78)]
            [InlineData(1.789, 1.78)]
            [InlineData(1.781, 1.78)]  
            [InlineData(2.0, 2.00)]
            [InlineData(3.456, 3.45)]
            [InlineData(0.0049, 0.00)]
            [InlineData(-1.234, -1.23)]
            public void Round_TruncatesToTwoDecimals(decimal input, decimal expected)
            {
                var result = RoundHelper.Round(input);
                Assert.Equal(expected, result);
            }
        }

    }
}
