namespace System
{
    public static class DateTimeExtensions
    {
        public static bool IsTheSameDay(this DateTime dateTime, DateTime other)
        {
            return dateTime.Year == other.Year &&
                   dateTime.Month == other.Month &&
                   dateTime.Day == other.Day;

        }
    }
}
  
