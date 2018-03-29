using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;
namespace CAS.Entity.GJBEntity
{
   public class Dat_ObjectBONAFinancial :DatObjectBONAFinancial
    {
       /// <summary>
       /// 产权人信息列表
       /// </summary>
       [SQLReadOnly]
       public string  fullname { get; set; }
       /// <summary>
       /// 省份
       /// </summary>
       [SQLReadOnly]
       public int? provinceid { get; set; }


    }
}
