namespace FinPlanner360.Business.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime GetEndDate(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 23, 59, 59);
        }
    }
}
