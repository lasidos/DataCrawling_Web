using System;
using System.Configuration;
using System.Globalization;

namespace DataCrawling_Web.BSL.CaChe
{
    public class ExtendedFormat
    {
        private static readonly char ARGUMENT_DELIMITER = Convert.ToChar(",", CultureInfo.CurrentUICulture);

        private static readonly char WILDCARD_ALL = Convert.ToChar("*", CultureInfo.CurrentUICulture);

        private static readonly char REFRESH_DELIMITER = Convert.ToChar(" ", CultureInfo.CurrentUICulture);

        private int[] _minutes;

        private int[] _hours;

        private int[] _days;

        private int[] _months;

        private int[] _daysOfWeek;

        public int[] Minutes => _minutes;

        public int[] Hours => _hours;

        public int[] Days => _days;

        public int[] Months => _months;

        public int[] DaysOfWeek => _daysOfWeek;

        public bool ExpireEveryMinute => _minutes[0] == -1;

        public bool ExpireEveryDay => _days[0] == -1;

        public bool ExpireEveryHour => _hours[0] == -1;

        public bool ExpireEveryMonth => _months[0] == -1;

        public bool ExpireEveryDayOfWeek => _daysOfWeek[0] == -1;

        public ExtendedFormat(string format)
        {
            //IL_0038: Unknown result type (might be due to invalid IL or missing references)
            string[] array = format.Trim().Split(REFRESH_DELIMITER);
            if (array.Length != 5)
            {
                throw new ConfigurationErrorsException("ExceptionInvalidExtendedFormatArguments");
            }

            _minutes = ParseValueToInt(array[0]);
            int[] minutes = _minutes;
            foreach (int num in minutes)
            {
                if (num > 59)
                {
                    throw new ArgumentOutOfRangeException("format", "ExceptionExtendedFormatIncorrectMinutePart");
                }
            }

            _hours = ParseValueToInt(array[1]);
            minutes = _hours;
            foreach (int num2 in minutes)
            {
                if (num2 > 23)
                {
                    throw new ArgumentOutOfRangeException("format", "ExceptionExtendedFormatIncorrectHourPart");
                }
            }

            _days = ParseValueToInt(array[2]);
            minutes = _days;
            foreach (int num3 in minutes)
            {
                if (num3 > 31)
                {
                    throw new ArgumentOutOfRangeException("format", "ExceptionExtendedFormatIncorrectDayPart");
                }
            }

            _months = ParseValueToInt(array[3]);
            minutes = _months;
            foreach (int num4 in minutes)
            {
                if (num4 > 12)
                {
                    throw new ArgumentOutOfRangeException("format", "ExceptionExtendedFormatIncorrectMonthPart");
                }
            }

            _daysOfWeek = ParseValueToInt(array[4]);
            minutes = _daysOfWeek;
            foreach (int num5 in minutes)
            {
                if (num5 > 6)
                {
                    throw new ArgumentOutOfRangeException("format", "ExceptionExtendedFormatIncorrectDayOfWeekPart");
                }
            }
        }

        private int[] ParseValueToInt(string value)
        {
            int[] array;
            if (value.IndexOf(WILDCARD_ALL) != -1)
            {
                array = new int[1] { -1 };
            }
            else
            {
                string[] array2 = value.Split(ARGUMENT_DELIMITER);
                array = new int[array2.Length];
                for (int i = 0; i < array2.Length; i++)
                {
                    array[i] = int.Parse(array2[i], CultureInfo.CurrentUICulture);
                }
            }

            return array;
        }
    }
}