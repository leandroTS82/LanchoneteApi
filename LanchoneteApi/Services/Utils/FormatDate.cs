using Microsoft.AspNetCore.Mvc;

public static class FormatDate
{
    internal static string BrasiliaNow()
    {
        DateTime utcNow = DateTime.UtcNow;
        TimeZoneInfo brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        DateTime brasiliaNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, brasiliaTimeZone);
        return brasiliaNow.ToString("yyyy-MM-dd");
    }
}