using System;
using System.Collections;
using System.Globalization;

namespace DataCrawling_Web.BSL.CaChe
{
    public class ExtendedFormatHelper
    {
        private static readonly char REFRESH_DELIMITER = Convert.ToChar(" ", CultureInfo.CurrentUICulture);

        private static readonly char WILDCARD_ALL = Convert.ToChar("*", CultureInfo.CurrentUICulture);

        private static Hashtable _parsedFormatCache = new Hashtable();

        public static bool IsExtendedExpired(string format, DateTime getTime, DateTime nowTime)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            if (format.Length == 0)
            {
                return true;
            }

            getTime = getTime.AddSeconds(getTime.Second * -1);
            nowTime = nowTime.AddSeconds(nowTime.Second * -1);
            ExtendedFormat extendedFormat = (ExtendedFormat)_parsedFormatCache[format];
            if (extendedFormat == null)
            {
                extendedFormat = new ExtendedFormat(format);
                lock (_parsedFormatCache.SyncRoot)
                {
                    _parsedFormatCache[format] = extendedFormat;
                }
            }

            if (nowTime.Subtract(getTime).TotalMinutes < 1.0)
            {
                return false;
            }

            int[] minutes = extendedFormat.Minutes;
            foreach (int num in minutes)
            {
                int[] hours = extendedFormat.Hours;
                foreach (int num2 in hours)
                {
                    int[] days = extendedFormat.Days;
                    foreach (int num3 in days)
                    {
                        int[] months = extendedFormat.Months;
                        foreach (int num4 in months)
                        {
                            int minute = ((num == -1) ? getTime.Minute : num);
                            int hour = ((num2 == -1) ? getTime.Hour : num2);
                            int num5 = ((num3 == -1) ? getTime.Day : num3);
                            int num6 = ((num4 == -1) ? getTime.Month : num4);
                            int num7 = getTime.Year;
                            if (num == -1 && num2 != -1)
                            {
                                minute = 0;
                            }

                            if (num2 == -1 && num3 != -1)
                            {
                                hour = 0;
                            }

                            if (num == -1 && num3 != -1)
                            {
                                minute = 0;
                            }

                            if (num3 == -1 && num4 != -1)
                            {
                                num5 = 1;
                            }

                            if (num2 == -1 && num4 != -1)
                            {
                                hour = 0;
                            }

                            if (num == -1 && num4 != -1)
                            {
                                minute = 0;
                            }

                            if (DateTime.DaysInMonth(num7, num6) < num5)
                            {
                                if (num6 == 12)
                                {
                                    num6 = 1;
                                    num7++;
                                }
                                else
                                {
                                    num6++;
                                }

                                num5 = 1;
                            }

                            DateTime dateTime = new DateTime(num7, num6, num5, hour, minute, 0);
                            if (dateTime < getTime)
                            {
                                if (num4 != -1 && getTime.Month >= num4)
                                {
                                    dateTime = dateTime.AddYears(1);
                                }
                                else if (num3 != -1 && getTime.Day >= num3)
                                {
                                    dateTime = dateTime.AddMonths(1);
                                }
                                else if (num2 != -1 && getTime.Hour >= num2)
                                {
                                    dateTime = dateTime.AddDays(1.0);
                                }
                                else if (num != -1 && getTime.Minute >= num)
                                {
                                    dateTime = dateTime.AddHours(1.0);
                                }
                            }

                            if (extendedFormat.ExpireEveryDayOfWeek)
                            {
                                if (nowTime >= dateTime)
                                {
                                    return true;
                                }

                                continue;
                            }

                            int[] daysOfWeek = extendedFormat.DaysOfWeek;
                            foreach (int num8 in daysOfWeek)
                            {
                                DateTime dateTime2 = getTime;
                                dateTime2 = dateTime2.AddHours(-1 * dateTime2.Hour);
                                dateTime2 = dateTime2.AddMinutes(-1 * dateTime2.Minute);
                                while (dateTime2.DayOfWeek != (DayOfWeek)num8)
                                {
                                    dateTime2 = dateTime2.AddDays(1.0);
                                }

                                if (nowTime >= dateTime2 && nowTime >= dateTime)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}