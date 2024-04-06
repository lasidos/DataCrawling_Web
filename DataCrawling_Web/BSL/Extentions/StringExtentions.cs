using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace DataCrawling_Web.BSL.Extentions
{
    public static class StringExtentions
    {
        /// <summary>
        /// "20180101" > "2018.01.01변환 String날짜 형식 변환
        /// </summary>
        /// <param name="value">값</param>
        /// <param name="spliter">구분자</param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// "20180101".ToDateFormat(".")
        /// "2018.01.01"
        /// "20180101".ToDateFormat("-")
        /// "2018-01-01"
        /// "20180101".ToDateFormat(":")
        /// "2018:01:01"
        /// </code>
        /// </example>
        public static string ToDateFormat(this string value, string spliter = ".")
        {
            if (string.IsNullOrEmpty(value) || value.Length != 8)
            {
                return value;
            }


            string input_format = "yyyyMMdd";
            string output_format = "yyyy.MM.dd";
            if (spliter != ".")
            {
                output_format = output_format.Replace(".", spliter);
            }

            DateTime dateTime;
            var success = DateTime.TryParseExact(value, input_format,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            if (success)
            {
                return dateTime.ToString(output_format);
            }
            else
            {
                if (value.Length == 8)
                {

                    return value.Insert(6, spliter).Insert(4, spliter);
                }
            }
            //이도 저도 아니면
            return value;
        }


        /// <summary>
        /// 현재 문자열이 Null 또는 Empty 또는 공백인지 확인
        /// </summary>
        /// <param name="value">검사할 문자열</param>
        /// <returns>검사 여부</returns>
        /// <example>
        /// <code>
        /// string s1 = string.Empty;
        /// string s2 = null;
        /// string s3 = "  ";
        /// string s4 = "abc";
        /// bool result1 = s1.IsNullOrEmptyOrWhiteSpace();
        /// bool result2 = s2.IsNullOrEmptyOrWhiteSpace();
        /// bool result3 = s3.IsNullOrEmptyOrWhiteSpace();
        /// bool result4 = s4.IsNullOrEmptyOrWhiteSpace();
        ///
        /// Console.Write("result1 : " + result1.ToString());
        /// Console.Write("result2 : " + result2.ToString());
        /// Console.Write("result3 : " + result3.ToString());
        /// Console.Write("result4 : " + result4.ToString());
        ///
        /// result1 : false
        /// result2 : false
        /// result3 : false
        /// result4 : true
        /// </code>
        /// </example>
        public static bool IsNullOrEmptyOrWhiteSpace(this string value)
        {
            return (string.IsNullOrEmpty(value) || value.All(x => char.IsWhiteSpace(x)));
        }

        /// <summary>
        /// 문자열이 Null 또는 Empty가 아닌지 확인
        /// </summary>
        /// <param name="value">검사할 문자열</param>
        /// <returns>검사여부</returns>
        /// <example>
        /// <code>
        /// string s1 = string.Empty;
        /// string s2 = null;
        /// string s3 = "  ";
        /// string s4 = "abc";
        /// bool result1 = s1.IsNotEmpty();
        /// bool result2 = s2.IsNotEmpty();
        /// bool result3 = s3.IsNotEmpty();
        /// bool result4 = s4.IsNotEmpty();
        ///
        /// Console.Write("result1 : " + result1.ToString());
        /// Console.Write("result2 : " + result2.ToString());
        /// Console.Write("result3 : " + result3.ToString());
        /// Console.Write("result4 : " + result4.ToString());
        ///
        /// result1 : true
        /// result2 : true
        /// result3 : false
        /// result4 : false
        /// </code>
        /// </example>
        public static bool IsNotEmpty(this string value)
        {
            //return (!value.IsEmpty());
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 현재 문자열에서 특정 문자를 제거
        /// </summary>
        /// <param name="value">현재 문자열</param>
        /// <param name="chars">제거할 문자</param>
        /// <returns>제거된 문자열</returns>
        /// <example>
        ///   <code>
        ///   string abc = "nice to meet you";
        ///   string result = abc.RemoveChar('e');
        ///   Console.Write("result : " + result.ToString());
        ///   result : nic to mt you
        /// </code>
        /// </example>
        public static string RemoveChar(this string value, params char[] chars)
        {
            var result = value;
            if (!string.IsNullOrEmpty(result) && chars != null)
            {
                Array.ForEach(chars, x => result = result.Remove(x.ToString()));
            }
            return result;
        }

        /// <summary>
        /// 현재 문자열에서 특정 문자열을 제거
        /// </summary>
        /// <param name="value">현재 문자열</param>
        /// <param name="strings">제거할 문자열</param>
        /// <returns>제거된 문자열</returns>
        /// <example>
        ///   <code>
        ///   string abc = "nice to meet you";
        ///   string result = abc.RemoveChar("e");
        ///   Console.Write("result : " + result.ToString());
        ///   result : nice to mt you
        /// </code>
        /// </example>
        public static string Remove(this string value, params string[] strings)
        {
            return strings.Aggregate(value, (current, x) => current.Replace(x, string.Empty));
        }

        /// <summary>
        /// 문자열을 모든 조건식에 맞는 문자열로 변환
        /// 기본적으로 대소문자 상관없이 변경가능 NICE == nice
        /// </summary>
        /// <param name="value">원본 문자열</param>
        /// <param name="oldValues">변환할 문자열</param>
        /// <param name="predicate">변환 조건 절</param>
        /// <returns>변환된 문자열</returns>
        /// <example>
        /// <code>
        /// public string UppercaseString(string ss)
        /// {
        ///     return ss.ToUpper();
        /// }
        /// string abc = "nice to meet you";
        ///   string result = abc.ReplaceAll(new string[] { "nice", "two", "you" }, UppercaseString);
        ///   Console.Write("reulst : " + result);
        /// reulst : NICE to meet YOU
        /// </code>
        /// </example>
        public static string ReplaceAll(this string value, IEnumerable<string> oldValues, Func<string, string> predicate)
        {
            var str = new StringBuilder(value);
            foreach (var oldValue in oldValues)
            {
                var newValue = predicate(oldValue);
                str.Replace(oldValue, newValue);
            }

            return str.ToString();
        }

        /// <summary>
        /// 문자열내에 특정 문자열을 새로운 문자열로 변환
        /// 기본적으로 대소문자 상관없이 변경가능 NICE == nice
        /// </summary>
        /// <param name="value">원본 문자열</param>
        /// <param name="oldValues">변환될 문자열</param>
        /// <param name="newValue">변환할 문자열</param>
        /// <returns>변환된 문자열</returns>
        /// <example>
        /// <code>
        ///  string abc = "nice to meet you";
        ///  string result = abc.ReplaceAll(new string[] { "nice", "two", "you" }, " ");
        ///  Console.Write("reulst : " + result);
        ///  reulst :  to meet
        ///  </code>
        /// </example>
        public static string ReplaceAll(this string value, IEnumerable<string> oldValues, string newValue)
        {
            var str = new StringBuilder(value);
            foreach (var oldValue in oldValues)
            {
                str.Replace(oldValue, newValue);
            }

            return str.ToString();
        }


        /// <summary>
        /// 현재 문자열이 주어진 Regex 패턴에 맞는지 검사
        /// </summary>
        /// <param name="value">현재 문자열</param>
        /// <param name="pattern">Regex 패턴</param>
        /// <param name="options">Regex 옵션</param>
        /// <returns>매칭여부</returns>
        public static bool IsMatched(this string value, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.IsMatch(value, pattern, options);
        }

        /// <summary>
        /// Regex 를 이용하여 현재 문자열을 치환
        /// </summary>
        /// <param name="value">현재 문자열</param>
        /// <param name="pattern">Regex 패턴</param>
        /// <param name="replace">치환할 문자열</param>
        /// <returns>치환된 문자열</returns>
        public static string ReplaceWith(this string value, string pattern, string replace)
        {
            return ReplaceWith(value, pattern, replace, RegexOptions.None);
        }

        /// <summary>
        /// Regex 를 이용하여 현재 문자열을 치환
        /// </summary>
        /// <param name="value">현재 문자열</param>
        /// <param name="pattern">Regex 패턴</param>
        /// <param name="replace">치환할 문자열</param>
        /// <param name="options">Regex 옵션</param>
        /// <returns>치환된 문자열</returns>
        public static string ReplaceWith(this string value, string pattern, string replace, RegexOptions options)
        {
            return Regex.Replace(value, pattern, replace, options);
        }

        /// <summary>
        /// Regex 를 이용하여 현재 문자열을 치환
        /// </summary>
        /// <param name="value">현재 문자열</param>
        /// <param name="pattern">Regex 패턴</param>
        /// <param name="evaluator">치환 함수 또는 람다식</param>
        /// <returns>치환된 문자열</returns>
        /// <example>
        /// <code>
        /// var test = "12345";
        /// var replaced = test.ReplaceWith(@"\d", m=> string.Concat(" -", m.Value, "- "));
        /// Console.Write("result : " + replaced);
        /// result :  -1- -2- -3- -4- -5-
        /// </code>
        /// </example>
        public static string ReplaceWith(this string value, string pattern, MatchEvaluator evaluator)
        {
            return ReplaceWith(value, pattern, RegexOptions.None, evaluator);
        }

        /// <summary>
        /// Regex 를 이용하여 현재 문자열을 치환
        /// </summary>
        /// <param name="value">현재 문자열</param>
        /// <param name="pattern">Regex 패턴</param>
        /// <param name="options">Regex 옵션</param>
        /// <param name="evaluator">치환 함수 또는 람다식</param>
        /// <returns>치환된 문자열</returns>
        /// <example>
        /// <code>
        ///   // System.Text.RegularExpressions.RegexOptions  참고
        /// var test = "12345";
        /// var replaced = test.ReplaceWith(@"\d", System.Text.RegularExpressions.RegexOptions.None, m=> string.Concat(" -", m.Value, "- "));
        /// Console.Write("result : " + replaced);
        /// result :  -1-  -2-  -3-  -4-  -5-
        /// </code>
        /// </example>
        public static string ReplaceWith(this string value, string pattern, RegexOptions options, MatchEvaluator evaluator)
        {
            return Regex.Replace(value, pattern, evaluator, options);
        }

        /// <summary>
        /// HTML제거
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveHTML(this string value)
        {
            string temp = ReplaceWith(value, "<[^>]*>", string.Empty);
            //개행제거
            return RemoveCrlf(temp);
        }

        /// <summary>
        /// 공백제거
        /// </summary>
        /// <param name="value">공백제거 문자열</param>
        /// <returns></returns>
        public static string RemoveHtmlWhiteSpace(this string value)
        {
            return ReplaceWith(value, @"(?<=>)\s+(?=</?)", string.Empty);
        }

        /// <summary>
        /// 개행문자 제거
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveCrlf(this string value)
        {
            return ReplaceWith(value, @"([*\r])\n", string.Empty);
        }

        /// <summary>
        /// 개행문자 제거
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NewLineToBr(this string value)
        {
            return ReplaceWith(value, @"([*\r])\n", "<br />");
        }

        /// <summary>
        /// Regex 의 pattern 값의 매칭된 컬렉션(MatchCollection)
        /// </summary>
        /// <param name="value">현재 문자열</param>
        /// <param name="pattern">Regex 패턴</param>
        /// <returns>추출된 컬렉션</returns>
        /// <example>
        /// <code>
        /// var test = "12345abc";
        /// MatchCollection col = test.GetMatches(@"\d");
        /// Console.Write("matched count : " + col.Count.ToString());
        /// matched count : 5
        /// </code>
        /// </example>
        public static MatchCollection GetMatches(this string value, string pattern)
        {
            return GetMatches(value, pattern, RegexOptions.None);
        }

        /// <summary>
        /// Regex 의 pattern 값의 매칭된 컬렉션(MatchCollection)
        /// </summary>
        /// <param name="value">현재 문자열</param>
        /// <param name="pattern">Regex 패턴</param>
        /// <param name="options">Regex 옵션</param>
        /// <returns>추출된 컬렉션</returns>
        /// <example>
        /// <code>
        ///   // System.Text.RegularExpressions.RegexOptions  참고
        /// var test = "12345abc";
        /// MatchCollection col = test.GetMatches(@"\d", System.Text.RegularExpressions.RegexOptions.None);
        /// Console.Write("matched count : " + col.Count.ToString());
        /// matched count : 5
        /// </code>
        /// </example>
        public static MatchCollection GetMatches(this string value, string pattern, RegexOptions options)
        {
            return Regex.Matches(value, pattern, options);
        }

        /// <summary>
        /// Regex 의 pattern 값의 매칭된 값들
        /// </summary>
        /// <param name="value">현재 문자열</param>
        /// <param name="pattern">Regex 패턴</param>
        /// <returns> 매칭된 값들</returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///   // System.Text.RegularExpressions.RegexOptions  참고
        /// var test = "12345abc";
        /// IEnumerable<string> col = test.GetMatchedValues(@"\d");
        /// foreach(var s in col)
        /// {
        ///      Console.Write("matched value : " + s);
        /// }
        /// matched value : 1
        /// matched value : 2
        /// matched value : 3
        /// matched value : 4
        /// matched value : 5
        /// ]]>
        /// </code>
        /// </example>
        public static IEnumerable<string> GetMatchedValues(this string value, string pattern)
        {
            return GetMatchedValues(value, pattern, RegexOptions.None);
        }

        /// <summary>
        /// Regex 의 pattern 값의 매칭된 값들
        /// </summary>
        /// <param name="value">현재 문자열</param>
        /// <param name="pattern">Regex 패턴</param>
        /// <param name="options">System.Text.RegularExpressions.RegexOptions</param>
        /// <returns>매칭된 값들</returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///   // System.Text.RegularExpressions.RegexOptions  참고
        /// var test = "12345abc6";
        /// IEnumerable<string> col = test.GetMatchedValues(@"\d", System.Text.RegularExpressions.RegexOptions.None);
        /// foreach(var s in col)
        /// {
        ///      Console.Write("matched value : " + s);
        /// }
        /// matched value : 1
        /// matched value : 2
        /// matched value : 3
        /// matched value : 4
        /// matched value : 5
        /// matched value : 6
        /// ]]>
        /// </code>
        /// </example>
        public static IEnumerable<string> GetMatchedValues(this string value, string pattern, RegexOptions options)
        {
            foreach (Match match in GetMatches(value, pattern, options))
            {
                if (match.Success)
                    yield return match.Value;
            }
        }

        /// <summary>
        /// 패턴값을 구분하여 배열로 변환함
        /// </summary>
        /// <param name="value">현재 문자열</param>
        /// <param name="pattern">Regex 패턴</param>
        /// <returns>구분한 문자열 배열</returns>
        /// <example>
        /// <code>
        /// var test = "a1b2c3";
        /// string[] strArray = test.Split(@"\d");
        /// foreach(var s in strArray)
        /// {
        ///      Console.Write("str : " + s);
        /// }
        ///
        /// str : a
        /// str : b
        /// str : c
        /// str :
        /// </code>
        /// </example>
        public static string[] Split(this string value, string pattern)
        {
            return value.Split(pattern, RegexOptions.None);
        }

        /// <summary>
        /// 패턴값을 구분하여 배열로 변환함
        /// </summary>
        /// <param name="value">현재 문자열</param>
        /// <param name="pattern">Regex 패턴</param>
        /// <param name="options">System.Text.RegularExpressions.RegexOptions</param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var test = "1a2b35c4";
        /// var strs = test.Split(@"[a-zA-Z]", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        /// foreach(var s in strArray)
        /// {
        ///      Console.Write("str : " + s);
        /// }
        ///
        /// str : 1
        /// str : 2
        /// str : 35
        /// str : 4
        /// </code>
        /// </example>
        public static string[] Split(this string value, string pattern, RegexOptions options)
        {
            return Regex.Split(value, pattern, options);
        }

        /// <summary>
        /// 문자열에서 숫자만 추출하여 반환
        /// </summary>
        /// <param name="value">추출할 문자열</param>
        /// <returns>추출된 숫자 문자열</returns>
        public static string ExtractDigits(this string value)
        {
            return value.Where(Char.IsDigit).Aggregate(new StringBuilder(value.Length), (sb, c) => sb.Append(c)).ToString();
        }



        /// <summary>
        /// 문자 값이 날짜인지 여부 확인
        /// </summary>
        /// <param name="value">체크하고자 하는 String 값</param>
        /// <returns>날짜인지 여부 확인 bool 값</returns>
        /// <example>
        /// <div style="font-size:10pt;"></div>
        /// <code>
        /// if (checkVal.IsDateTime("yyyyMMdd"))
        /// {
        ///     날짜 형식인 경우
        /// }
        /// else
        /// {
        ///     날짜 형식이 아닌 경우
        /// }
        /// </code>
        /// </example>
        public static bool IsDateTime(this string value, string format)
        {
            DateTime result;
            return DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        /// <summary>
        /// 문자 값이 이메일 형식인지 여부 확인
        /// </summary>
        /// <param name="value">체크하고자 하는 Sring 값</param>
        /// <returns>이메일 형식인지 여부 확인 bool 값</returns>
        /// <example>
        /// <div style="font-size:10pt;"></div>
        /// <code>
        /// if (checkVal.IsEmail())
        /// {
        ///     이메일 형식인 경우
        /// }
        /// else
        /// {
        ///     이메일 형식이 아닌 경우
        /// }
        /// </code>
        /// </example>
        public static bool IsEmail(this string value)
        {
            if (value.IsNullOrEmptyOrWhiteSpace())
                return false;
            else
                return Regex.IsMatch(value.Trim(), @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }


        /// <summary>
        /// url 이 맞는지 비교
        /// Cotains Scheme (file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)
        /// </summary>
        /// <param name="url">체크 할 Url 문자열</param>
        /// <returns>url이 맞는지의 여부</returns>
        /// <example>
        /// <div style="font-size:10pt;"></div>
        /// <code>
        /// Cotains Scheme 'file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp'
        /// var checkVal = "ftp://aaa.co.kr/";
        /// var result = checkVal.ValidUrl();
        /// Console.Write("result : " + result.ToString());
        /// result : true
        /// </code>
        /// </example>
        public static bool IsValidUrl(this string url)
        {
            Regex urlchk = new Regex(@"((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,15})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            return urlchk.IsMatch(url);
        }

        /// <summary>
        /// http url 이 맞는지 비교
        /// Cotains Scheme (http|https)
        /// </summary>
        /// <param name="url">체크 할 Url 문자열</param>
        /// <returns>url이 맞는지의 여부</returns>
        /// <example>
        /// <div style="font-size:10pt;"></div>
        /// <code>
        /// var checkVal = "ftp://aaa.co.kr/";
        /// var result = checkVal.ValidUrl();
        /// Console.Write("result : " + result.ToString());
        /// result : false
        /// var checkVal1 = "https://aaa.co.kr/";
        /// var result1 = checkVal1.ValidUrl();
        /// Console.Write("result1 : " + result1.ToString());
        /// result1 : true
        /// </code>
        /// </example>
        public static bool IsValidHttpUrl(this string url)
        {
            if (url.IsContainKorean())
            {
                //http, https
                if (url.StartsWith("http"))
                {
                    return true;
                }
                return false;
            }

            Regex urlchk = new Regex(@"((http|https)://)+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,15})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            return urlchk.IsMatch(url);
        }

        /// <summary>
        /// 한글포함여부 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public static bool IsContainKorean(this string value)
        {
            char[] charArr = value.ToCharArray();
            foreach (char c in charArr)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 문자 바이트 수 구하기
        /// </summary>
        /// <param name="value">바이트 수 확인하려는 문자</param>
        /// <returns>문자 바이트 수</returns>
        /// <example>
        /// <div style="font-size:10pt;"></div>
        /// <code>
        /// var checkVal = "aa홍";
        /// int byteCount = checkVal.ByteCount();
        /// Console.Write("byteCount : " + byteCount.ToString());
        /// byteCount : 4
        /// var checkVal1 = "aa";
        /// int byteCount1 = checkVal1.ByteCount();
        /// Console.Write("byteCount1 : " + byteCount1.ToString());
        /// byteCount1 : 2
        /// </code>
        /// </example>
        public static int ByteCount(this string value)
        {
            if (value.IsNullOrEmptyOrWhiteSpace())
                return 0;
            else
                return Encoding.Default.GetByteCount(value);
        }

        /// <summary>
        /// Split 배열시 Enumerable 으로 리턴 Type지정시 변환 (IConvertible) .NET기본 타입 변환구현체
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">원본</param>
        /// <param name="separator">구분자</param>
        /// <example>
        /// <code>
        /// "1,2,3,".SplitTo<Int32>(',');
        /// { 1,2,3 }
        /// </code>
        /// </example>
        /// <returns></returns>
        public static IEnumerable<T> SplitTo<T>(this string value, params char[] separator) where T : IConvertible
        {
            foreach (var s in value.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                yield return (T)Convert.ChangeType(s, typeof(T));
        }


        /// <summary>
        /// 해당문자열이 전달받은 문자열에 포함되어  있는지 판단합니다.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool Inlist(this string value, params string[] list)
        {
            return list.Contains(value);
        }


        /// <summary>
        /// 해당문자가 포함되었는지 체크
        /// String.Contains() 기 있으나 대소문자 체크기능추가
        /// </summary>
        /// <param name="text">원본</param>
        /// <param name="search">검색어</param>
        /// <param name="stringComparison">인자가 없을경우 .NET내장개체라 인자가 반드시 넘어와야함 </param>
        /// <returns></returns>
        public static bool Contains(this string value, string search, StringComparison stringComparison)
        {
            return value.IndexOf(search, stringComparison) > -1;
        }
    }

    public static class StringHelper
    {
        static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        static readonly string nums = "0123456789";
        static readonly string charWithNums = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        //이하 프로세스 공용
        static readonly Random random = new Random();

        /// <summary>
        /// 랜덤 문자열 생성
        /// </summary>
        /// <param name="length">생성하고자 하는 문자열 길이</param>
        /// <returns>Length 에 맞는 랜덤 문자열</returns>
        /// <example>
        /// <code>
        /// string str = StringEx.GenerateRandomString(5);
        /// </code>
        /// </example>
        public static string GenerateRandomString(int length)
        {

            var result = new string(Enumerable.Repeat(chars, length).Select(x => x[random.Next(x.Length)]).ToArray());
            return result;
        }

        /// <summary>
        /// 랜덤 숫자 생성
        /// </summary>
        /// <param name="length">생성하고자 하는 문자열 길이</param>
        /// <returns>Length 에 맞는 랜덤 숫자</returns>
        /// <example>
        /// <code>
        /// string str = StringEx.GenerateNumber(5);
        ///   bool result = str.IsInteger(); //StringEx Extension
        ///   Console.Write("result : " + result.ToString());
        ///   result : true
        /// </code>
        /// </example>
        public static string GenerateNumber(int length)
        {
            var result = new string(Enumerable.Repeat(nums, length).Select(x => x[random.Next(x.Length)]).ToArray());
            return result;
        }

        /// <summary>
        /// 난수생성
        /// </summary>
        /// <returns>Guid를 이용한 난수생성</returns>
        public static string NewStringId()
        {
            return Guid.NewGuid().ToString().GetHashCode().ToString("x");
        }

        /// <summary>
        /// 랜덤 문자 + 숫자 생성
        /// </summary>
        /// <param name="length">생성하고자 하는 문자열 길이</param>
        /// <returns>Length 에 맞는 랜덤 문자 + 숫자</returns>
        /// <example>
        /// <code>
        /// string str = StringEx.GetRandomStringWithNumber(5);
        /// </code>
        /// </example>
        public static string GenerateRandomStringWithNumber(int length)
        {
            var result = new string(Enumerable.Repeat(charWithNums, length).Select(x => x[random.Next(x.Length)]).ToArray());
            return result;
        }


        /// <summary>
        /// 은/는 조사 반환   (받침에 의한 조사반환) 
        /// https://namu.wiki/w/%ED%95%9C%EA%B5%AD%EC%96%B4%EC%9D%98%20%EC%A1%B0%EC%82%AC#s-3      
        /// 기존사용 함수 변경
        /// 함수명참고 사전 : https://endic.naver.com/search.nhn?sLn=kr&isOnlyViewEE=N&query=postposition      
        /// </summary>
        /// <param name="value">원본</param>
        /// <param name="hasLastIdxPostposition">받침이 있는 문자의 조사</param>
        /// <param name="hasNoLastIdxPostposition">받침이 없는 문자의 조사</param>
        /// <remarks>
        /// (은/는, 이/가, 을/를, 과/와, 아/야, 이여/여, 이랑/랑, 으로/로, 으로서/로서, 으로써/로써)
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// string str = StringExtention.Postposition("홍길동", "은", "는");
        /// //아래와 같을경우
        /// string str = StringExtention.Postposition("삼성", "은", "는");
        /// string str = StringExtention.Postposition("삼성전자", "은", "는");
        /// ]]>
        /// </code>
        /// </example>
        /// <returns></returns>
        public static string Postposition(string value, string hasLastIdxPostposition, string hasNoLastIdxPostposition)
        {
            bool isSupprt = false;
            string lastName = (!string.IsNullOrEmpty(value)) ? value.Substring(value.Length - 1) : ""; //마지막 한 글자

            if (lastName.Length == 1)
            {
                char lastChar;
                lastChar = char.Parse(lastName);

                isSupprt = HasLastIdx(lastChar);
            }

            return value + ((isSupprt) ? hasLastIdxPostposition : hasNoLastIdxPostposition);

            //4.7.2 이후
            //종성이 있는지 여부
            //bool HasLastIdx(char c한글자)
            //{
            //   ushort UniCodeKoreanBase = 0xAC00;
            //   ushort UniCodeKoreanLast = 0xD79F;
            //   string LastTbl = " ㄱㄲㄳㄴㄵㄶㄷㄹㄺㄻㄼㄽㄾㄿㅀㅁㅂㅄㅅㅆㅇㅈㅊㅋㅌㅍㅎ";
            //   int firstIdx, middleIdx, lastIdx; // 초성,중성,종성의 인덱스
            //   ushort uTempCode = 0x0000;       // 임시 코드용
            //                            //Char을 16비트 부호없는 정수형 형태로 변환 - Unicode
            //   uTempCode = Convert.ToUInt16(c한글자);
            //   // 캐릭터가 한글이 아닐 경우 처리
            //   if ((uTempCode < UniCodeKoreanBase) || (uTempCode > UniCodeKoreanLast))
            //   {
            //      return false;
            //   }
            //   // iUniCode에 한글코드에 대한 유니코드 위치를 담고 이를 이용해 인덱스 계산.
            //   int iUniCode = uTempCode - UniCodeKoreanBase;
            //   firstIdx = iUniCode / (21 * 28);
            //   iUniCode = iUniCode % (21 * 28);
            //   middleIdx = iUniCode / 28;

            //   iUniCode = iUniCode % 28;
            //   lastIdx = iUniCode;

            //   return LastTbl[iUniCode] != ' ';
            //}
        }

        /// <summary>
        /// 종성이 있는지 여부
        /// </summary>
        /// <param name="c한글자"></param>
        /// <returns></returns>
        private static bool HasLastIdx(char c한글자)
        {
            ushort UniCodeKoreanBase = 0xAC00;
            ushort UniCodeKoreanLast = 0xD79F;
            string LastTbl = " ㄱㄲㄳㄴㄵㄶㄷㄹㄺㄻㄼㄽㄾㄿㅀㅁㅂㅄㅅㅆㅇㅈㅊㅋㅌㅍㅎ";
            int firstIdx, middleIdx, lastIdx; // 초성,중성,종성의 인덱스
            ushort uTempCode = 0x0000;       // 임시 코드용
                                             //Char을 16비트 부호없는 정수형 형태로 변환 - Unicode
            uTempCode = Convert.ToUInt16(c한글자);
            // 캐릭터가 한글이 아닐 경우 처리
            if ((uTempCode < UniCodeKoreanBase) || (uTempCode > UniCodeKoreanLast))
            {
                return false;
            }
            // iUniCode에 한글코드에 대한 유니코드 위치를 담고 이를 이용해 인덱스 계산.
            int iUniCode = uTempCode - UniCodeKoreanBase;
            firstIdx = iUniCode / (21 * 28);
            iUniCode = iUniCode % (21 * 28);
            middleIdx = iUniCode / 28;

            iUniCode = iUniCode % 28;
            lastIdx = iUniCode;

            return LastTbl[iUniCode] != ' ';
        }

    }
}