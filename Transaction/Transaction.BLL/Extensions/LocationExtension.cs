using GeoTimeZone;
using System.Globalization;

namespace Transaction.BLL.Extensions;

public static class LocationExtension
{
    public static string ParseToTimeZone(this string location)
    {
        var parts = location.Split(',');

        double latitude = double.Parse(parts[0].Trim(), CultureInfo.InvariantCulture);
        double longitude = double.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);
        var timeZone = TimeZoneLookup.GetTimeZone(latitude, longitude).Result;

        return timeZone;
    }
}
