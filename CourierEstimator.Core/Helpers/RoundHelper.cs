namespace CourierEstimator.Core.Helpers
{
    public static class RoundHelper
    {
        public static decimal Round(decimal value)
        {
            return Math.Truncate(value * 100) / 100;
        }
    }
}
