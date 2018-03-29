
namespace OpenPlatform.Framework.Data.SQL
{
   public struct CustomerSql
   {
       /// <summary>
       /// 根据授权用户名获取授权对象
       /// </summary>
       public const string GetCustomerByUserId = @"select CompanyName,UserId,Password from Customer where UserId= @userId ";


   }
}
