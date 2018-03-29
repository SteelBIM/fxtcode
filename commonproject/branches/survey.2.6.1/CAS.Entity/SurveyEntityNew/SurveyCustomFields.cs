using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyEntityNew
{
    /// <summary>
    /// 查勘库扩展类（重复类，为了保证命名空间一致，OA统一查勘的时候需要修改） caoq 2013-11-11
    /// </summary>

    //n:ID或名称 name
    //c:显示的名称 caption
    //l:logo url logo
    //d:描述 desc
    //t:类型 type
    //v:值或列表值 value
    //s:结果值 show
    //r:必填项 required
    //f:系统字段 field
    //u:单位 unit
    //"t":文本 text
    //"n":数字 number
    //"s":下拉框 select
    //"r":单选框 radio
    //"b":bool选择框
    //"c":多选框 checkbox
    //"m":通过测距仪输入数据框   measurement
    /// <summary>
    /// 自定义字段组 kevin
    /// </summary>
    //返回的分类
    public class mobileFields
    {
        public string n { get; set; }
        public string c { get; set; }
        public string d { get; set; }
        public string l { get; set; }
        public object v { get; set; }
    }
    //返回的字段
    public class mobileFieldValues
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string n { get; set; }
        /// <summary>
        /// 字段标题
        /// </summary>
        public string c { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public int? r { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? f { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string t { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string s { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string u { get; set; }
        /// <summary>
        /// 保存结果
        /// </summary>
        public string v { get; set; }
        //kevin 2013-8-20
        /// <summary>
        /// 选项
        /// </summary>
        public string g { get; set; }
        //caoq 2014-2-19 自定义户型多层时，当前为第几层
        public string i { get; set; }
        /// <summary>
        /// 最大长度
        /// </summary>
        public int? m { get; set; }
    }

}
