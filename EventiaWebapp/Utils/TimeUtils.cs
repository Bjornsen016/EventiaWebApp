using System.Globalization;

namespace EventiaWebapp.Utils;

public static class TimeUtils
{
    public static string ConvertUtcToSpecificTimeZone(DateTime dateTime, int timeDifferenceFromUtc)
    {
        var convertedDateTime = dateTime + new TimeSpan(timeDifferenceFromUtc, 0, 0);
        if (convertedDateTime.IsDaylightSavingTime()) convertedDateTime += new TimeSpan(1, 0, 0);
        return convertedDateTime.ToString("g", CultureInfo.GetCultureInfo("sv-SE"));
    }
}