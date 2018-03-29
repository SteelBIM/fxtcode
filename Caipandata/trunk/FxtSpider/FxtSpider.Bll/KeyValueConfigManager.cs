using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.DAL.DB;
using System.Data.SqlClient;
using System.Data;

namespace FxtSpider.Bll
{
    public static class KeyValueConfigManager
    {
        public static bool SetKeyValueConfig(int cityId, int webId, string key, string value, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            Dat_KeyValueConfig obj = GetKeyValueConfig(cityId, webId, key, db);
            if (obj != null)
            {
                Update(cityId, webId, key, value, db);
            }
            else
            {
                Insert(cityId, webId, key, value, db);
            }
            return true;
        }
        public static bool Insert(int cityId, int webId, string key, string value, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            string sql = @"insert Dat_KeyValueConfig with(rowlock)(KeyName,KeyValue,CityId,WebId)  select '{0}','{1}',{2},{3}";
            sql = string.Format(sql, key, value, cityId, webId);
            db.DB.ExecuteCommand(sql);
            db.Connection_Close();
            return true;
        }
        public static bool Update(int cityId, int webId, string key, string value, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            string sql = @" Update Dat_KeyValueConfig with(rowlock) set KeyValue='{0}' where KeyName='{1}' and CityId={2} and WebId={3} ";
            sql = string.Format(sql, value, key, cityId, webId);
            db.DB.ExecuteCommand(sql);
            db.Connection_Close();
            return false;
        }
        public static Dat_KeyValueConfig GetKeyValueConfig(int cityId, int webId, string key, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            string sql = @"select * from Dat_KeyValueConfig with(nolock) where KeyName='{0}' and CityId={1} and WebId={2} ";
            sql = string.Format(sql, key, cityId, webId);
            Dat_KeyValueConfig obj = db.DB.ExecuteQuery<Dat_KeyValueConfig>(sql).FirstOrDefault();
            return obj;
        }


        public static Dat_KeyValueConfig Insert(Dat_KeyValueConfig obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                long nowID = 0;
                db.DB.Dat_KeyValueConfig_Insert(obj.KeyName,obj.KeyValue, obj.WebId, obj.CityId,  out nowID);
                obj.ID = nowID;
            }
            return obj;
        }
        public static Dat_KeyValueConfig Update(Dat_KeyValueConfig obj, DataClass _db = null)
        {
            DataClass db = new DataClass(_db);
            if (obj != null)
            {
                db.DB.Dat_KeyValueConfig_Update(obj.KeyName, obj.KeyValue, obj.WebId, obj.CityId, obj.ID);
            }
            return obj;
        }
    }
}
