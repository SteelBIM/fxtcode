using System.Collections.Generic;

namespace CAS.Common
{
    /// <summary>
    /// 云查勘Helper类
    /// </summary>
    public class SurveyHelper
    {
        /// <summary>
        /// 用户登录请求URL
        /// </summary>
        public static string surveyLogin = "login";
        /// <summary>
        /// 用户退出云查勘接口
        /// </summary>
        public static string surveyLoginOut = "logout";
        /// <summary>
        /// 云查勘用户验证
        /// </summary>
        public static string surveyVaild = "login/valid";
        /// <summary>
        /// 获取用户工作任务情况统计
        /// </summary>
        public static string surveyGetUserWorkCount = "survey/user_work_info";
        /// <summary>
        /// 获取查勘任务详情
        /// </summary>
        public static string surveyGetSurveyDetail = "survey/get_survey_detail";
        /// <summary>
        /// 获取查勘子表详情列表
        /// </summary>
        public static string surveyGetSubSurveyDetailList = "survey/sub_survey_list";
        /// <summary>
        /// 新增查勘
        /// </summary>
        public static string surveyAdd = "survey/new";
        /// <summary>
        /// 撤销查勘
        /// </summary>
        public static string surveyRevoke = "survey/revoke";
        /// <summary>
        /// 修改查勘
        /// </summary>
        public static string surveyUpdate = "survey/update";
        /// <summary>
        /// 批量更新分配节点
        /// </summary>
        public static string surveyUpdateAllocatedNode = "survey/batch_update_allocatedNode";
        /// <summary>
        /// 业务查勘对象,更新委托查勘信息
        /// </summary>
        public static string surveyUpdateEntrust = "survey/get_entrust_survey";
        /// <summary>
        /// 删除查勘
        /// </summary>
        public static string surveyDelete = "survey/delete";
        /// <summary>
        /// 重新分配查勘
        /// </summary>
        public static string surveyAllocate = "survey/allocate";
        /// <summary>
        /// 批量分配查勘
        /// </summary>
        public static string surveyMoreAllocate = "survey/batch_allocate";
        /// <summary>
        /// 获取查勘跟进记录
        /// </summary>
        public static string surveyGetTrack = "survey/get_track";
        /// <summary>
        /// 获取字段跟进记录
        /// </summary>
        public static string surveyGetFieldTrack = "survey/get_field_track";

        /// <summary>
        /// 获取沟通反馈信息
        /// </summary>
        public static string surveyGetMsg = "msg/chat_list";
        /// <summary>
        /// 根据查勘状态查询用户信息
        /// </summary>
        public static string surveyGetSurvorsInfo = "survey/survors_statistic";

        #region 字典操作
        /// <summary>
        ///获取字典code列表
        /// </summary>
        public static string surveyGetCodeList = "template/get_code_list";
        /// <summary>
        ///获取子字典code列表
        /// </summary>
        public static string surveyGetSubCodeList = "template/get_childcode_list";
        /// <summary>
        /// 新增字典code(可批量)
        /// </summary>
        public static string surveyAddCode = "template/add_code";
        /// <summary>
        /// 修改字典code(可批量) 包含增删改操作
        /// </summary>
        public static string surveyModifyCode = "template/modify_code";
        /// <summary>
        /// 更新字典code(可批量)
        /// </summary>
        public static string surveyUpdateCode = "template/update_code";
        /// <summary>
        /// 删除字典code(可批量)
        /// </summary>
        public static string surveyDeleteCode = "template/delete_code"; 

        #endregion

        /// <summary>
        /// 获取模版列表
        /// </summary>
        public static string surveyGetTemplateList = "template/get_tpl_list";
        /// <summary>
        /// 获取模版
        /// </summary>
        public static string surveyGetTemplate = "template/get_tpl";
        /// <summary>
        /// 获取模版可用字段列表
        /// </summary>
        public static string surveyGetTemplateFieldsList = "template/get_tplfields";
        /// <summary>
        /// 获取可选字段列表--字典设置
        /// </summary>
        public static string surveyGetTemplateFieldsCodeList = "template/get_field_codes";
        /// <summary>
        /// 更新模版
        /// </summary>
        public static string surveyUpdateTemplate = "template/update";

        /// <summary>
        /// 获取照片模版列表
        /// </summary>
        public static string surveyGetPhotoModelList = "phototpl/get_list";
        /// <summary>
        /// 获取照片模版详情
        /// </summary>
        public static string surveyGetPhotoModeDetails = "phototpl/get_details";
        /// <summary>
        /// 新增照片模版
        /// </summary>
        public static string surveyAddPhotoTemplate = "phototpl/add";
        /// <summary>
        /// 修改照片模版
        /// </summary>
        public static string surveyEditPhotoTemplate = "phototpl/update";
        /// <summary>
        /// 删除照片模版
        /// </summary>
        public static string surveyDelPhotoTemplate = "phototpl/delete";


        /// <summary>
        /// 修改系统字段
        /// </summary>
        public static string surveySysField = "template/update_sys_field";

        /// <summary>
        /// 添加可选字段
        /// </summary>
        public static string surveyAddField = "template/add_field";
        /// <summary>
        /// 修改可选字段
        /// </summary>
        public static string surveyModifyField = "template/update_field";
        /// <summary>
        /// 删除可选字段
        /// </summary>
        public static string surveyDelField = "template/delete_field";
        /// <summary>
        /// 验证可选字段
        /// </summary>
        public static string surveyCheckField = "template/check_field";
        /// <summary>
        /// 数据绑定设置--可选字段
        /// </summary>
        public static string surveyBindDataField = "template/data_bind";

        /// <summary>
        /// 获取查勘列表
        /// </summary>
        public static string surveyGetSurveyList = "survey/query_list";
        /// <summary>
        /// 获取查勘图片列表 (文件列表)
        /// </summary>
        public static string surveyGetSurveyPhotoList = "file/get_files";
        /// <summary>
        /// 删除查勘图片 (文件删除)
        /// </summary>
        public static string surveyDeletePhoto = "file/delete";
        /// <summary>
        /// 上传照片 (不保存数据库 只做临时保存,上传文件) 
        /// </summary>
        public static string surveyFileSimpleUpload = "file/simple_upload";
        /// <summary>
        ///  上传照片 并保存于数据库中(上传文件)
        /// </summary>
        public static string surveyFileUpload = "file/save_files_info";
        /// <summary>
        /// 批量修改照片类型 (修改文件类型)
        /// </summary>
        public static string surveyUpdatePhotosType = "file/update_type";
        /// <summary>
        /// 复制文件
        /// </summary>
        public static string surveyFlieCopy = "file/file_copy";
        /// <summary>
        /// 标记或重新标记地图
        /// </summary>
        public static string surveyMarkMap = "survey/markmap";
        /// <summary>
        /// 查勘照片一键下载
        /// </summary>
        public static string surveyDownLoadPics = "file/download_pics";
        /// <summary>
        /// 一键下载Excel文件
        /// </summary>
        public static string surveyDownLoadXls = "file/download";
        /// <summary>
        /// 获取一键下载公司设置
        /// </summary>
        public static string surveyGetDownLoadConfig = "config/get_config";
        /// <summary>
        /// 更新一键下载公司设置
        /// </summary>
        public static string surveyUpdateDownLoadConfig = "config/update";
        /// <summary>
        /// 查勘转询价后 委估对象ID回写查勘表
        /// </summary>
        public static string surveyEditObjectID = "survey/update_obj";
        /// <summary>
        /// 获取查勘详情列表--基础数据
        /// </summary>
        public static string surveyGetBaseDataList = "survey/query_detail_list";
        /// <summary>
        /// 获取查勘字段列表--基础数据
        /// </summary>
        public static string surveyGetBaseDataExportField = "survey/get_template_data";
        /// <summary>
        /// 导出数据--基础数据
        /// </summary>
        public static string surveyExportBaseData = "survey/export_data_to_local";
        /// <summary>
        /// 腾讯地图静态图密钥key
        /// </summary>
        public static string tencentMapKey = "BSJBZ-Q47KF-DGLJI-JPPFW-JTGFE-LHBQ6";
        /// <summary>
        /// 百度地图静态图密钥key
        /// </summary>
        public static string baiduMapKey = "V5HC6omBGEYCYTAPKsj55ugG";
        /// <summary>
        /// 发送消息（个推）给查勘 Alex 2016-05-18
        /// </summary>
        public static string surveySend_Msg = "msg/send_msg";
        /// <summary>
        /// 获取按天统计查勘量
        /// </summary>
        public static string surveyComplsurveyDailyCount = "survey/complsurvey_dailycount";
        /// <summary>
        /// 获取按委估对象统计查勘量
        /// </summary>
        public static string surveyComplsurveyObjectTypeCount = "survey/complsurvey_typecount";
        /// <summary>
        /// 激活查勘
        /// </summary>
        public static string surveyActivate = "survey/activate";
        /// <summary>
        /// 清空查勘业务编号和委估对象编号 Alex 2017-01-19
        /// </summary>
        public static string surveyCleanObjIdEntrustId = "survey/clean_ObjAndEnt_by_sid";

        /// <summary>
        /// 获取查勘总量
        /// </summary>
        public static string surveyTotalStatistics = "survey/fxtCompanyIds_surveyCount";

        #region 云查勘返回实体类
        /// <summary>
        /// 返回实体类
        /// </summary>
        public class SurveyReturnData<T>
        {
            /// <summary>
            /// body响应体
            /// </summary>
            public T body { get; set; }
            /// <summary>
            /// 返回码
            /// </summary>
            protected int _code = -1;
            public int code
            {
                get { return _code; }
                set { _code = value; }
            }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }
            /// <summary>
            /// 服务端签名
            /// </summary>
            public string signature { get; set; }
            /// <summary>
            /// 时间戳
            /// </summary>
            public string timestamp { get; set; }
            /// <summary>
            /// 总数
            /// </summary>
            public int totalSize { get; set; }
            /// <summary>
            /// 请求标识--登录时返回来的
            /// </summary>
            public string token { get; set; }
        } 
        #endregion

        #region 用户请求接口实体参数类
        /// <summary>
        /// syscode
        /// </summary>
        public  class survey_sysCode
        {
            public int id { get; set; }

            public int fxtCompanyId { get; set; }

            public int codeId { get; set; }

            public int? dicType { get; set; }

            public int? orderId { get; set; }

            public bool canEdit { get; set; }

            public string codeName { get; set; }

            public string codeType { get; set; }
        }
        #endregion

        #region 腾讯坐标转换返回类
        /// <summary>
        /// 腾讯坐标转换返回类
        /// </summary>
        public class TencentMapLocation
        {
            /// <summary>
            /// 状态码
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 状态信息
            /// </summary>
            public string message { get; set; }

            public List<Location> locations { get; set; }
        }
        /// <summary>
        /// 坐标值
        /// </summary>
        public class Location
        {
            /// <summary>
            /// 经度
            /// </summary>
            public decimal lng { get; set; }
            /// <summary>
            /// 纬度
            /// </summary>
            public decimal lat { get; set; }
        } 
        #endregion

        #region 分配节点 
        /// <summary>
        /// 分配节点-预评
        /// </summary>
        public static string AssignNode_YP = "预评";
        /// <summary>
        /// 分配节点-报告
        /// </summary>
        public static string AssignNode_Report = "报告";
        /// <summary>
        /// 分配节点-询价
        /// </summary>
        public static string AssignNode_Query = "询价";
        /// <summary>
        /// 分配节点-手机端
        /// </summary>
        public static string AssignNode_Mobile = "手机端";
        #endregion


    }
}
