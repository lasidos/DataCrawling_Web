using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace DataCrawling_Web.BSL.Common
{
    public class Utility
    {
        public static readonly Random rd = new Random();

        // 타이머관련
        public static bool stopProcess;

        public static List<string> ReadAllText(string filepath)
        {
            List<string> list = new List<string>();
            using (StreamReader reader = new StreamReader(filepath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line); // Add to list.
                }
            }
            return list;
        }

        #region 대기시간 (초)

        // 대기시간
        public static void SecondsWait(int seconds)
        {
            DateTime startTime = DateTime.Now;
            while (true)
            {
                TimeSpan diff = DateTime.Now - startTime;
                int tempSpan = Convert.ToInt32(diff.TotalSeconds);

                if (stopProcess) return;
                if (seconds - tempSpan > 0)
                {
                    Thread.Sleep(10);
                    Application.DoEvents();
                }
                else break;
            }
        }

        // 지정 딜레이
        public static void Sleep(int delay)
        {
            stopProcess = false;
            SecondsWait(delay);
            if (stopProcess) return;
        }

        // 랜덤 딜레이
        public static void Sleep(int min, int max)
        {
            Random rand = new Random();
            stopProcess = false;
            int randomDelay = rand.Next(min, max);
            SecondsWait(randomDelay);
            if (stopProcess) return;
        }

        #endregion

        #region 대기시간 (마이크로초)

        // 대기시간
        public static void SecondsWaitM(int seconds)
        {
            DateTime startTime = DateTime.Now;
            while (true)
            {
                TimeSpan diff = DateTime.Now - startTime;
                int tempSpan = Convert.ToInt32(diff.TotalMilliseconds);

                if (stopProcess) return;
                if (seconds - tempSpan > 0)
                {
                    Thread.Sleep(10);
                    Application.DoEvents();
                }
                else break;
            }
        }

        // 지정 딜레이
        public static void SleepM(int delay)
        {
            stopProcess = false;
            SecondsWaitM(delay);
            if (stopProcess) return;
        }

        // 랜덤 딜레이
        public static void SleepM(int min, int max)
        {
            Random rand = new Random();

            stopProcess = false;
            int randomDelay = rand.Next(min, max);
            SecondsWaitM(randomDelay);
            if (stopProcess) return;
        }

        #endregion

        #region 랜덤함수

        public static IList<T> Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        // startNum부터 endNum사이의 랜덤한 숫자를 발생한다.
        public static int GetRandomNumber(int count)
        {
            int[] randomSequence = new int[count];

            try
            {
                int rdIndex = 0;

                List<int> li = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    li.Add(i);
                }

                for (int i = 0; i < count; i++)
                {
                    rdIndex = rd.Next(0, li.Count);
                    randomSequence[i] = li[rdIndex];

                    li.RemoveAt(rdIndex);
                }

                return randomSequence[0];
            }
            catch
            {
                return 0;
            }
        }

        // startNum부터 endNum사이의 랜덤한 숫자를 발생한다.
        public static int GetRandomNumber(int startNum, int endNum)
        {
            int[] randomSequence = new int[endNum - startNum];

            int rdIndex = 0;

            List<int> li = new List<int>();
            for (int i = startNum; i < endNum; i++)
            {
                li.Add(i);
            }

            for (int i = 0; i < randomSequence.Length; i++)
            {
                rdIndex = rd.Next(0, li.Count);
                randomSequence[i] = li[rdIndex];

                li.RemoveAt(rdIndex);
            }

            SleepM(1);
            return randomSequence[0];
        }

        // 0부터 count만큼의 랜덤한 배열을 생성한다.
        public static int[] GetRandomArrangement_StartZero(int count)
        {
            int[] randomSequence = new int[count];

            int rdIndex = 0;

            List<int> li = new List<int>();
            for (int i = 0; i < count; i++)
            {
                li.Add(i);
            }

            for (int i = 0; i < count; i++)
            {
                rdIndex = rd.Next(0, li.Count);
                randomSequence[i] = li[rdIndex];

                li.RemoveAt(rdIndex);
            }

            return randomSequence;
        }

        // 1부터 count만큼의 랜덤한 배열을 생성한다.
        public static int[] GetRandomArrangement_StartOne(int count)
        {
            int[] randomSequence = new int[count];

            int rdIndex = 0;

            List<int> li = new List<int>();
            for (int i = 1; i <= count; i++)
            {
                li.Add(i);
            }

            for (int i = 0; i < count; i++)
            {
                rdIndex = rd.Next(0, li.Count);
                randomSequence[i] = li[rdIndex];

                li.RemoveAt(rdIndex);
            }

            return randomSequence;
        }

        public static string GetRand_DigitNum(int Digit)
        {
            string str = string.Empty;
            for (int i = 0; i < Digit; i++)
            {
                str += rd.Next(1, 9).ToString();
            }
            return str;
        }

        #endregion

        #region [랜덤 정수]
        /// <summary>
        /// 랜덤 정수 리턴
        /// </summary>
        public static int RandomNumber(int minValue, int maxValue)
        {
            return rd.Next(minValue, maxValue);
        }
        #endregion

        #region [랜덤 5자리 정수]
        /// <summary>
        /// 랜덤 5자리 정수 리턴
        /// </summary>
        public static string RndString5()
        {
            return rd.Next(0, 99999).ToString("D5");

        }
        #endregion

        #region [랜덤 8자리 정수]
        /// <summary>
        /// 랜덤 5자리 정수 리턴
        /// </summary>
        public static string RndString8()
        {
            return rd.Next(0, 99999999).ToString("D5");

        }
        #endregion

        #region 핸드폰번호 Dash
        /// <summary>
        /// 핸드폰 번호에 -(dash)를 붙여준다.
        /// </summary>
        /// <param name="handPhone"></param>
        /// <returns></returns>
        public static string AddDashHandPhoneNo(string handPhone)
        {
            handPhone = handPhone.Replace("-", "");
            if (handPhone.Length == 10)
            {
                handPhone = string.Format("{0}-{1}-{2}", handPhone.Substring(0, 3), handPhone.Substring(3, 3), handPhone.Substring(6, 4));
            }
            else if (handPhone.Length == 11)
            {
                if (handPhone.StartsWith("01"))
                {
                    handPhone = string.Format("{0}-{1}-{2}", handPhone.Substring(0, 3), handPhone.Substring(3, 4), handPhone.Substring(7, 4));
                }
                else
                {
                    handPhone = string.Format("{0}-{1}-{2}", handPhone.Substring(0, 4), handPhone.Substring(4, 3), handPhone.Substring(7, 4));
                }
            }
            else if (handPhone.Length == 12)
            {
                handPhone = string.Format("{0}-{1}-{2}", handPhone.Substring(0, 4), handPhone.Substring(4, 4), handPhone.Substring(8, 4));
            }
            else
            {
                handPhone = string.Empty;
            }

            return handPhone;
        }
        #endregion

        #region Encrypt_SHA : SHA256Bit 암호화 함수
        // 용도 : 비밀번호용. 복호화가 필요 없이 분실시 새로 생성해야 하는 경우 사용
        public static string Encrypt_SHA(string Data)
        {
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
        #endregion

        #region Encrypt_AES : Decrypt_AES : AES 256Bit 암복호화 함수
        // 용도 : 주민번호, 신용카드번호 등 복호화 해야 하는 경우 처리
        public static string Encrypt_AES(string InputText)
        {
            return Encrypt_AES(InputText, "gtm56km412#$%inb5040sr!@#$%&^&(I");
        }

        public static string Encrypt_AES(string Input, string key)
        {
            string Output = "";

            if (Input != "")
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] xBuff = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Encoding.UTF8.GetBytes(Input);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }

                Output = Convert.ToBase64String(xBuff);
            }

            return Output;
        }

        public static string Decrypt_AES(string InputText)
        {
            return Decrypt_AES(InputText, "gtm56km412#$%inb5040sr!@#$%&^&(I");
        }

        public static string Decrypt_AES(string Input, string key)
        {
            string Output = "";

            if (Input != "")
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var decrypt = aes.CreateDecryptor();
                byte[] xBuff = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Convert.FromBase64String(Input);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }

                Output = Encoding.UTF8.GetString(xBuff);
            }

            return patchnull(Output);
        }

        #endregion

        #region patchnull : null 문자 패치 ( 통신데이터에서 ASCII 코드가 00 인것을 공백으로 치환 )
        public static string patchnull(string tmp)
        {
            string retVal = "";
            retVal = HttpUtility.UrlDecode(HttpUtility.UrlEncode(tmp).Replace("%00", ""));

            return retVal;
        }
        #endregion
    }
}