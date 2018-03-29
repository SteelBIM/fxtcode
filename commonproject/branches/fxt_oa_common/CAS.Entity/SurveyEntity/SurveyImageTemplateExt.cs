using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;
namespace CAS.Entity.SurveyEntity
{
    /// <summary>
    /// 查勘照片模板实体
    /// </summary>
    public class SurveyImageTemplateExt:SYSSurveyImageTemplate
    {
        [SQLReadOnly]
        public float bottommargin { get; set; }
        [SQLReadOnly]
        public float footermargin { get; set; }
        [SQLReadOnly]
        public float headermargin { get; set; }
        [SQLReadOnly]
        public float leftmargin { get; set; }
        [SQLReadOnly]
        public List<SurveyImageTemplatePage> pagelist { get; set; }
        [SQLReadOnly]
        public float rightmargin { get; set; }
        [SQLReadOnly]
        public float topmargin { get; set; }
        [SQLReadOnly]
        public string surveytypecodename { get; set; }
        [SQLReadOnly]
        public string createtruename { get; set; }
    }

    [Serializable]
    public class SurveyImageTemplatePage
    {
        public SurveyImageTemplatePage(){}

        public int fontsize { get; set; }
        public float height { get; set; }
        public int imagecount { get; set; }
        public int index { get; set; }
        public float marginleft { get; set; }
        public float margintop { get; set; }
        public List<SurveyImageTemplatePhoto> photos { get; set; }
        public string phototypes { get; set; }
        public float starttop { get; set; }
        public float width { get; set; }
        /// <summary>
        /// 照片旋转角度（小于0左旋转 0不旋转 大于0右旋转） caoq 2014-02-27
        /// </summary>
        public float turnnumber { get; set; }
    }

    [Serializable]
    public class SurveyImageTemplatePhoto
    {
        public SurveyImageTemplatePhoto(){}
        public int index { get; set; }
        public string phototype { get; set; }
    }
}
