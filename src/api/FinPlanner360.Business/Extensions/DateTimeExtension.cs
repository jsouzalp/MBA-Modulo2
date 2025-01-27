namespace FinPlanner360.Business.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime GetEndDate(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 23, 59, 59);
        }
        public static DateTime GetStartDate(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 00, 00, 00);
        }
    }
}
