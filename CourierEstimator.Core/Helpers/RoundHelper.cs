namespace CourierEstimator.Core.Helpers
{
    public static class RoundHelper
    {
        public static decimal Round(decimal value)
        {
            return (decimal)(Math.Truncate(value * 100) / 100);
        }
    }
}
