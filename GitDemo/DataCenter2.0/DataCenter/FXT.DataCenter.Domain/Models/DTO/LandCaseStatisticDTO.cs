namespace FXT.DataCenter.Domain.Models.DTO
{
  public class LandCaseStatisticDTO
    {

      /// <summary>
      /// 行政区名
      /// </summary>
      public string AreaName { get; set; }
      /// <summary>
      /// 成交宗数
      /// </summary>
      public decimal CJZS { get; set; }

      /// <summary>
      /// 成交面积
      /// </summary>
      public decimal CJMJ { get; set; }
      
      /// <summary>
      /// 成交均价
      /// </summary>
      public decimal CJJJ { get; set; }

      /// <summary>
      /// 楼面价
      /// </summary>
      public decimal LMJ { get; set; }
    }
}
