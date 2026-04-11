using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Blog.Core.Utils
{
    public static class DbTimeUtil
    {
        public static ISqlSugarClient _db;
        public static void InitDb(ISqlSugarClient db)
        {
            _db = db;
        }


        public static DateTime GetDbTime()
        {
            return _db.GetDate();
        }
    }
}
