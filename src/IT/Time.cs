using System;

namespace IT
{
    public static class Time
    {
        /// <summary>
        /// Разбирает время в формате
        /// </summary>
        /// <param name="format">1d 23h 59m 59s 999</param>
        public static TimeSpan Parse(String format)
        {
            int days = 0;
            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            int milliseconds = 0;

            var number = "";
            var period = 1;
            for (int i = 0; i < format.Length; i++)
            {
                var token = format[i];
                if (token == ' ') continue;
                if (Char.IsNumber(token))
                {
                    number += token;
                    continue;
                }
                if (number.Length > 0) period = Int32.Parse(number);
                if (days == 0 && token == 'd') days = period;
                else if (hours == 0 && token == 'h') hours = period;
                else if (minutes == 0 && token == 'm') minutes = period;
                else if (seconds == 0 && token == 's') seconds = period;
                else throw new FormatException($"Not supported '{format.Substring(i)}' index = '{i}'");
                period = 1;
                number = "";
            }
            if (number.Length > 0) milliseconds = Int32.Parse(number);
            return new TimeSpan(days, hours, minutes, seconds, milliseconds);
        }
    }
}