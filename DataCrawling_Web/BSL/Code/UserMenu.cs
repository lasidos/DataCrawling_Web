using Dapper;
using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.CaChe;
using DataCrawling_Web.Models.Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace DataCrawling_Web.BSL.Code
{
    public class UserMenu
    {
        private static DefaultCache cacheManager = new DefaultCache();
        /// <summary>
        /// 캐시 만료일 패턴 설명
        /// 1. * * * * * : 분, 시, 일, 월, 요일
        /// 2. 분 : 0-59
        /// 3. 시 : 0-23
        /// 4. 일 : 1-31
        /// 5. 월 : 1-12
        /// 6. 요일 : 0-6 (일요일 : 0)
        /// 7. Wildcards(*) : 매분, 매시, 매일, 매월, 매요일을 의미
        /// 8. 패턴 지정 예제
        ///    - * * * * *    : 매분 만료
        ///    - 5 * * * *    : 매시 5분 만료 
        ///    - * 21 * * *   : 매일 21시 매분 만료 
        ///    - 31 15 * * *  : 매일 15시 31분 만료
        ///    - 7 4 * * 6    : 토요일 4시 7분 만료
        ///    - 15 21 4 7 *  : 7월 4일 21시 15분 만료
        /// </summary>
        private const string defaultCacheExpire = "* 0 * * *"; //매일 0분 Expire(1일 Cache)

        /// <summary>
        /// 사이트 코드 목록 조회
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<W_MenuModel> GetSiteW_Menu(bool cacheUse = false)
        {
            string key = "CodeCoDBTalent";
            IEnumerable<W_MenuModel> entity;

            if (!cacheUse || !cacheManager.TryGetValue(key, out entity))
            {
                entity = GetSiteW_MenuDB();
                cacheManager.AddEx(key, entity, defaultCacheExpire);
            }

            return entity;
        }

        public static IEnumerable<W_MenuModel> GetSiteW_MenuDB()
        {
            IEnumerable<W_MenuModel> entity;
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mkapi"].ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@M_ID", AuthUser.M_ID);
                entity = conn.Query<W_MenuModel>(param: p, commandType: CommandType.StoredProcedure
                    , sql: "USP_USER_ACCESS_MENU_S");
            }

            return entity;
        }

        public static IEnumerable<W_MenuModel> GetMenu(int Parent_Id = -1, int Menu_Type = 1, bool cacheUse = false)
        {
            var menuInfo = GetSiteW_Menu(cacheUse);
            if (Parent_Id == -1) menuInfo = menuInfo.Where(p => p.Menu_Level == 0 && p.Menu_Type == Menu_Type).OrderBy(s => s.Order_No);
            else menuInfo = menuInfo.Where(p => p.Parent_Id == Parent_Id && p.Menu_Type == Menu_Type).OrderBy(s => s.Order_No);

            return menuInfo;
        }
    }
}