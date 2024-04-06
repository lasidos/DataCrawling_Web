using System;
using System.Collections.Generic;
using System.Linq;

namespace DataCrawling_Web.BSL.Extentions
{
    public static class EnumerableExtensions
    {
        #region field
        //난수생성기
        private static readonly Random rnd = new Random();
        #endregion

        /// <summary>
        /// 무작위로 썩는다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            //if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (enumerable == null) throw new ArgumentNullException("enumerable");

            var list = enumerable.ToList();
            //Guid를 사용하면 메모리 많이 사용할것 같아서
            var seq = RandomSequence(0, list.Count());
            return seq.Select(list.ElementAt);

            //inner Method
            //난수 생성
            //IEnumerable<int> RandomSequence(int minimum, int maximum)
            //{
            //   var candidates = Enumerable.Range(minimum, maximum - minimum).ToList();
            //   while (candidates.Count > 0)
            //   {
            //      var index = rnd.Next(candidates.Count);
            //      yield return candidates[index];
            //      candidates.RemoveAt(index);
            //   }
            //}
        }

        private static IEnumerable<int> RandomSequence(int minimum, int maximum)
        {
            var candidates = Enumerable.Range(minimum, maximum - minimum).ToList();
            while (candidates.Count > 0)
            {
                var index = rnd.Next(candidates.Count);
                yield return candidates[index];
                candidates.RemoveAt(index);
            }
        }

        /// <summary>
        /// 원하는 필드의 Distinct값을 반환합니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="items"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> property)
        {
            //if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (enumerable == null) throw new ArgumentNullException("enumerable");

            return enumerable.GroupBy(property).Select(x => x.First());
        }

        /// <summary>
        /// 시퀀스의 요소가 하나라도 있는지 확인합니다. null check 와 같이
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsAny<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Any();
        }

        /// <summary>
        /// 시퀀스의 요소가 하나라도 있는지 확인합니다. null check 와 같이
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="predicate">조건</param>
        /// <returns></returns>
        public static bool IsAny<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return enumerable?.Any(predicate) == true;
        }

        /// <summary>
        /// 요소가 비어 있는지 확인 **메모리 개체는 있으면서 비었는지 확인
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            //null일경우 empty를 어떻게 판단할지 고민했으나 IsAny가 null을 같이 판단함으로 
            //메모리 영역이 있으면서 비었는지 확인하는 프로세스로 개발 하였음.
            //if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));         
            if (enumerable == null) throw new ArgumentNullException("enumerable");

            return !enumerable.Any();
        }

        /// <summary>
        /// IEnumerable 의 요소가 1개인지 체크 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool OnlyOne<T>(this IEnumerable<T> enumerable)
        {
            //if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (enumerable == null) throw new ArgumentNullException("enumerable");

            using (var iterator = enumerable.GetEnumerator())
            {
                return iterator.MoveNext() && !iterator.MoveNext();
            }
        }


        /// <summary>
        /// 모든요소가 같은 값인지
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool AllEqual<T>(this IEnumerable<T> enumerable)
        {
            //if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (enumerable == null) throw new ArgumentNullException("enumerable");

            return enumerable.Distinct().OnlyOne();
        }

        /// <summary>
        /// 모든요소별로 작업
        ///  ** List<T>에 있는기능
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            //if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            //if (action == null) throw new ArgumentNullException(nameof(action));

            if (enumerable == null) throw new ArgumentNullException("enumerable");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var p in enumerable)
                action.Invoke(p);
        }

        /// <summary>
        ///  모든요소별로 작업 
        ///  ** List<T>에 있는기능
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            //if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            //if (action == null) throw new ArgumentNullException(nameof(action));

            if (enumerable == null) throw new ArgumentNullException("enumerable");
            if (action == null) throw new ArgumentNullException("action");

            var index = 0;
            foreach (var p in enumerable)
            {
                action.Invoke(p, index);
                index++;
            }
        }

        /// <summary>
        /// 페이징 옵션으로 IEnumerable<T> 를 반환한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> enumerable, int page, int pageSize)
        {
            //if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (enumerable == null) throw new ArgumentNullException("enumerable");

            return enumerable.Skip((page - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 시퀀스마다 같은조건인지 확인
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="second"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool SequenceEqual<T1, T2>(this IEnumerable<T1> enumerable, IEnumerable<T2> second, Func<T1, T2, bool> predicate)
        {
            //if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            //if (second == null) throw new ArgumentNullException(nameof(second));

            if (enumerable == null) throw new ArgumentNullException("enumerable");
            if (second == null) throw new ArgumentNullException("second");

            var iterator1 = enumerable.GetEnumerator();
            var iterator2 = second.GetEnumerator();

            while (iterator1.MoveNext())
            {
                if (!iterator2.MoveNext())
                    return false;

                if (!predicate.Invoke(iterator1.Current, iterator2.Current))
                    return false;
            }
            if (iterator2.MoveNext())
                return false;

            return true;
        }

        /// <summary>
        /// 해당아이템의 인덱스번호      
        /// </summary>
        /// <remarks>
        /// int, string등 기본 데이터 타입사용시 문제없음
        /// Custom 클래스 사용시 아래 참고
        /// Equals의 참조타입 구현시 참고 : https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1.equals?view=netframework-4.7.2
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T key)
        {
            //if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (enumerable == null) throw new ArgumentNullException("enumerable");
            var index = 0;
            foreach (T e in enumerable)
            {
                if (!e.Equals(key))
                    index++;
                else
                    return index;

            }

            //보통의 경우 못찾을경우 -1
            return -1;
            //throw new KeyNotFoundException();
        }
    }


}
