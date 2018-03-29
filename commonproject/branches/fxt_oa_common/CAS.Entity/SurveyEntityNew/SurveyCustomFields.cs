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
        public string n { get; set; }
        public string c { get; set; }
        public int r { get; set; }
        public int f { get; set; }
        public string t { get; set; }
        public string s { get; set; }
        public string u { get; set; }
        public string v { get; set; }
        //kevin 2013-8-20
        public string g { get; set; }
        //caoq 2014-2-19 自定义户型多层时，当前为第几层
        public string i { get; set; }
    }

}
