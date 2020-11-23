using System;

namespace TeamsBotApi.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToDuration(this TimeSpan timeSpan) => timeSpan.ToString(@"hh\:mm\:ss");
    }
}