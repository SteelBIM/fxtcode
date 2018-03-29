using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity.DBEntity;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace FxtCenterTest
{
    public partial class _default : System.Web.UI.Page
    {
        //public T Clone<T>(T RealObject)
        //{
        //    using (Stream objectStream = new MemoryStream())
        //    {
        //        IFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(objectStream, RealObject);
        //        objectStream.Seek(0, SeekOrigin.Begin);
        //        return (T)formatter.Deserialize(objectStream);
        //    }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            SearchBase search = new SearchBase();
            search.CityId = 6;
            search.PageIndex = 1;
            search.PageRecords = 100;
            search.Page = true;
            search.FxtCompanyId = 27;
            string key = "l";
            //更新楼栋顺序
            List<DATBuildingOrderBy> result = DatBuildingBL.GetBuildingBaseInfoList(search, 45716, 1);
            List<DATBuildingOrderBy> dirct = new List<DATBuildingOrderBy>();
            dirct = OrderByHelper.OrderBy<DATBuildingOrderBy>(result, "BuildingName");
            //CityTable city = CityTableBL.GetCityTable(search.CityId);
            //for (int i = 0; i < dirct.Count; i++)
            //{
            //    //dirct[i].orderbyIndex = i + 1;
            //    DATBuilding model = dirct[i];
            //    model.SetAvailableFields(new string[] { "orderbyIndex" });
            //    DatBuildingBL.Update(model, city.BuildingTable);
            //}
            //var ne = dirct.Select(o => new { ob_starletter = o.ob_starletter, ob_number = o.ob_number, buildingid = o.buildingid, buildingname = o.buildingname + "----" + o.buildingname });

            //更新房号顺序
            //List<DATHouse> result = DatHouseBL.GetAutoHouseListList(search, 560361, 1, "");
            //List<DATHouse> dirct = new List<DATHouse>();
            //dirct = OrderByHelper.OrderBy<DATHouse>(result, "housename");

            //CityTable city = CityTableBL.GetCityTable(search.CityId);
            //for (int i = 0; i < dirct.Count; i++)
            //{
            //    dirct[i].orderbyIndex = i + 1;
            //    DATHouse model = dirct[i];
            //    model.SetAvailableFields(new string[] { "orderbyIndex" });
            //    DatHouseBL.Update(model, city.HouseTable);
            //}
            //var ne = dirct.Select(o => new { ob_starletter = o.ob_starletter, ob_number = o.ob_number, buildingid = o.buildingid, floorno = o.floorno + "----" + o.housename });

            //var ne = dirct.Select(o => new {othername= o.name +"---------"+ o.othername}); 
            Response.Write("va".ToJson());
        }

    }
    /*
    /// <summary>
    /// 实体类User，测试用
    /// </summary>
    [Serializable]
    public class User
    {
        protected string _name;
        protected int _age;
        protected string _address;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
    }
    [Serializable]
    public class Dirct
    {
        public int id { get; set; }
        public string name { get; set; }
        public string othername { get; set; }
        /// <summary>
        /// 开头数字
        /// </summary>
        public int startnum { get; set; }
        /// <summary>
        /// 开头字母
        /// </summary>
        public string starletter { get; set; }
        public string text { get; set; }
        public int number { get; set; }
    }

    /// <summary>
    /// 继承IComparer<T>接口，实现同一自定义类型　对象比较
    /// </summary>
    /// <typeparam name="T">T为泛用类型</typeparam>
    public class Reverser<T> : IComparer<T>
    {
        private Type type = null;
        private ReverserInfo info;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">进行比较的类类型</param>
        /// <param name="name">进行比较对象的属性名称</param>
        /// <param name="direction">比较方向(升序/降序)</param>
        public Reverser(Type type, string name, ReverserInfo.Direction direction)
        {
            this.type = type;
            this.info.name = name;
            if (direction != ReverserInfo.Direction.ASC)
                this.info.direction = direction;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="className">进行比较的类名称</param>
        /// <param name="name">进行比较对象的属性名称</param>
        /// <param name="direction">比较方向(升序/降序)</param>
        public Reverser(string className, string name, ReverserInfo.Direction direction)
        {
            try
            {
                this.type = Type.GetType(className, true);
                this.info.name = name;
                this.info.direction = direction;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="t">进行比较的类型的实例</param>
        /// <param name="name">进行比较对象的属性名称</param>
        /// <param name="direction">比较方向(升序/降序)</param>
        public Reverser(T t, string name, ReverserInfo.Direction direction)
        {
            this.type = t.GetType();
            this.info.name = name;
            this.info.direction = direction;
        }

        //必须！实现IComparer<T>的比较方法。
        int IComparer<T>.Compare(T t1, T t2)
        {
            object x = this.type.InvokeMember(this.info.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, t1, null);
            object y = this.type.InvokeMember(this.info.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, t2, null);
            if (this.info.direction != ReverserInfo.Direction.ASC)
                Swap(ref x, ref y);
            return (new CaseInsensitiveComparer()).Compare(x, y);
        }

        //交换操作数
        private void Swap(ref object x, ref object y)
        {
            object temp = null;
            temp = x;
            x = y;
            y = temp;
        }
    }

    /// <summary>
    /// 对象比较时使用的信息类
    /// </summary>
    public struct ReverserInfo
    {
        /// <summary>
        /// 比较的方向，如下：
        /// ASC：升序
        /// DESC：降序
        /// </summary>
        public enum Direction
        {
            ASC = 0,
            DESC,
        };

        public enum Target
        {
            CUSTOMER = 0,
            FORM,
            FIELD,
            SERVER,
        };

        public string name;
        public Direction direction;
        public Target target;
    }
    */
}