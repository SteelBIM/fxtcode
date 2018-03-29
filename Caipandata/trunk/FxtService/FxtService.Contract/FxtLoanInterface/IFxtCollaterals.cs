
using Model = FxtNHibernate.FxtLoanDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

/**
 * 作者:李晓东
 * 摘要:2014.01.22 新建
 *      2014.03.24 修改人:李晓东
 *                 修改:GetDetials 中的条件
  *      2014.03.26 修改人:贺黎亮
 *                 修改:GetCollateralCountByPCA 
 *                 type参数类型为string
 * **/
namespace FxtService.Contract.FxtLoanInterface
{
    [ServiceContract()]
    public interface IFxtCollaterals
    {
        /// <summary>
        /// 贷后 临时楼盘新增
        /// </summary>
        /// <param name="dataProject">楼盘JSON对象</param>
        /// <returns></returns>
        string DATAProjectAdd(string dataProject);

        /// <summary>
        /// 贷后 临时楼栋新增
        /// </summary>
        /// <param name="dataBuilding">楼栋JSON对象</param>
        /// <returns></returns>
        string DATABuildingAdd(string dataBuilding);

        /// <summary>
        /// 贷后 临时房号新增
        /// </summary>
        /// <param name="dataHouse">房号JSON对象</param>
        /// <returns></returns>
        string DATAHouseAdd(string dataHouse);

        /// <summary>
        /// 押品拆分保存
        /// </summary>
        /// <param name="dataCollateral">押品对象</param>
        /// <param name="uploadFileId">文件ID</param>
        /// <returns></returns>
        string DataCollateralAdd(string data);

        /// <summary>
        /// 押品拆分修改
        /// </summary>
        /// <param name="dataCollateral">押品对象</param>
        /// <returns></returns>
        string DataCollateralUpdate(string dataCollateral);

        /// <summary>
        /// 获得已标准化列表信息
        /// </summary>
        /// <param name="pageSize">一页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="orderProperty">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="orderType">文件Id</param>
        /// <returns></returns>
        string GetDataCollateral(int pageSize, int pageIndex, string orderProperty, string orderType, string cityarrid, string itemarrid, int uploadfileid, int customerid, int customertype);

        /// <summary>
        /// 根据条件,获得已有押品信息
        /// </summary>
        /// <param name="dataCollateral">押品对象</param>
        /// <returns></returns>
        string GetDataCollateralByMoreWhere(string dataCollateral);

        /// <summary>
        /// 获得已有押品全部信息
        /// </summary>
        /// <returns></returns>
        string GetAllDataCollateral(string cityarrid, string itemarrid);

        /// <summary>
        /// 获得已有押品全部信息根据某个文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="pageSize">一页记录</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns></returns>
        string GetAllDataCollateralByFileId(int fileId, int pageSize, int pageIndex);

        /// <summary>
        /// 获得属于某个文件的所有押品总量
        /// </summary>
        /// <param name="fileId">文件Id</param>
        /// <returns></returns>
        string GetCountCollateralByFileId(int fileId);

        /// <summary>
        /// 获得指定列值
        /// </summary>
        /// <param name="cId">城市ID</param>
        /// <param name="projectId">楼盘ID</param>
        /// <param name="columnName">指定列</param>
        /// <returns></returns>
        string GetCustomColumnsValue(int cId, int projectId, string columnName);
        /// <summary>
        /// 修改指定列信息
        /// </summary>
        /// <param name="data">对象</param>
        /// <param name="cid">城市</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        string UpdateCustomColumnsValue(string data, int cid, int type);
        /// <summary>
        /// 根据省份或者城市或者行政区得到楼盘总量信息
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="houseType">物业类型(押品)</param>
        /// <param name="buildingType">建筑类型(Fxt库)</param>
        /// <param name="buildingDate">建筑年代(Fxt库)</param>
        /// <param name="loanAmount">贷款额度(押品)</param>
        /// <param name="buildingArea">押品面积(押品)</param>
        /// <param name="age">年龄(押品)</param>
        /// <param name="type">统计类型</param>
        /// <returns></returns>
        string GetCollateralCountByPCA(int pId, int cId, int aId,
            string houseType, string buildingType, string buildingDate,
            string loanAmount, string buildingArea, string age, string type, string itemarrid);

        /// <summary>
        /// 押品监测地图导出
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="houseType">物业类型(押品)</param>
        /// <param name="buildingType">建筑类型(Fxt库)</param>
        /// <param name="buildingDate">建筑年代(Fxt库)</param>
        /// <param name="loanAmount">贷款额度(押品)</param>
        /// <param name="buildingArea">押品面积(押品)</param>
        /// <param name="age">年龄(押品)</param>
        /// <returns></returns>
        string GetExportByPCA(int pId, int cId, int aId,
            string houseType, string buildingType, string buildingDate,
            string loanAmount, string buildingArea, string age);

        /// <summary>
        /// 押品分类统计
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="requirement">条件</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束</param>
        /// <param name="type">返回类型(0:押品监测,1:押品资产价值动态监测)</param>
        /// <param name="cityarrid">选择城市</param>
        /// <param name="itemarrid">选择项目</param>
        /// <returns></returns>
        string GetCollateralClassification(int pId, int cId, int aId,
            string requirement, string start, string end, int type, string cityarrid, string itemarrid);

        /// <summary>
        /// 押品明细查询
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="houseType">物业类型(押品)</param>
        /// <param name="buildingType">建筑类型(Fxt库)</param>
        /// <param name="buildingDate">建筑年代(Fxt库)</param>
        /// <param name="loanAmount">贷款额度(押品)</param>
        /// <param name="buildingArea">押品面积(押品)</param>
        /// <param name="age">年龄(押品)</param>
        /// <param name="projectid">楼盘</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        string GetDetials(int pId, int cId, int aId,
            string houseType, string buildingType, string buildingDate,
            string loanAmount, string buildingArea, string age,
            int projectid, int companyid, string start, string end,
            int pageIndex, int pageSize, string cityarrid, string itemarrid);

        /// <summary>
        /// 模糊搜索根据押品中已匹配的楼盘信息
        /// </summary>
        /// <param name="cId">城市</param>
        /// <param name="name">模糊信息</param>
        /// <returns></returns>
        string GetProjectByDataCollateral(int cId, string name);

        /// <summary>
        /// 模糊获得开发商
        /// </summary>
        /// <param name="cId">城市</param>
        /// <param name="name">模糊信息</param>
        /// <returns></returns>
        string GetCompanyByDataCollateral(int cId, string name);

        /// <summary>
        /// 获得已有押品,根据押品编号,且状态为2(未匹配)的
        /// </summary>
        /// <param name="collNumber">押品编号</param>
        /// <returns></returns>
        string GetDataCollateralByNumber(string collNumber);

        #region 押品复估

        /// <summary>
        /// 复估管理
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">县区</param>
        /// <returns></returns>
        string GetCollateralsByReassessment(int pId, int cId, int aId, int pageSize, int pageIndex, string cityarrid, string itemarrid);

        /// <summary>
        /// 得到指定押品的估价值
        /// </summary>
        /// <param name="id">押品ID</param>
        /// <returns></returns>
        string ReassessmentCalculation(int id);

        /// <summary>
        /// 获取某个押品的复估列表
        /// </summary>
        /// <param name="id">押品ID</param>
        /// <param name="nMonths">月数</param>
        /// <returns></returns>
        string GetReassessment(int id, int nMonths);

        /// <summary>
        /// 人工复估
        /// </summary>
        /// <param name="id">复估ID</param>
        /// <param name="price">价格</param>
        /// <param name="oper">操作人</param>
        /// <returns></returns>
        string UpdateReassessment(int id, int price, int oper);

        #endregion

        #region 压力测试
        /// <summary>
        /// 押品价格走势分析
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="type">走势分析类型</param>
        /// <param name="ptwhere">走势分析类型条件</param>
        /// <returns></returns>
        string CollateralPriceTrend(int pId, int cId, int aId, int type, string ptwhere, string itemarrid);

        /// <summary>
        /// 压力测试
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="houseType">物业类型(押品)</param>
        /// <param name="buildingType">建筑类型(Fxt库)</param>
        /// <param name="buildingDate">建筑年代(Fxt库)</param>
        /// <param name="loanAmount">贷款额度(押品)</param>
        /// <param name="buildingArea">押品面积(押品)</param>
        /// <param name="age">年龄(押品)</param>
        /// <param name="twhere">测试条件</param>
        /// <returns></returns>
        string StressTest(int pId, int cId, int aId,
            string houseType, string buildingType, string buildingDate,
            string loanAmount, string buildingArea, string age,
            string start, string end, string twhere, string cityarrid, string itemarrid);

        /// <summary>
        /// 风险预警
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="type">统计类型</param>
        /// <returns></returns>
        string RiskWarning(int pId, int cId, int aId, int type, string itemarrid);
        
        /// <summary>
        /// 风险预警 获取危险值列表
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="itemarrid">项目列表ID</param>
        /// <returns></returns>
        string RikWarningToDanger(int pId, int cId, int aId, int type, string itemarrid);
        #endregion

        #region 任务

        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="task">任务模型</param>
        /// <returns></returns>
        string AddTask(string task);

        #endregion



        #region 文件项目
        /// <summary>
        /// 添加修改所属银行项目
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <returns></returns>
        string AddEditProjects(string data);

        /// <summary>
        /// 获取文件项目列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="id"></param>
        /// <param name="orderProperty"></param>
        /// <returns></returns>
        string GetSysBankProjectList(int pageIndex, int pageSize, int id, string orderProperty, string key, int bankid, int customerid, int customertype);


        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="id"></param>
        /// <param name="orderProperty"></param>
        /// <returns></returns>
        string GetTaskList(int pageIndex, int pageSize, int id, string orderProperty, string key, int bankid, int status, int customerid, int customertype);

        /// <summary>
        /// 获取指定任务日志
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="taskid"></param>
        /// <param name="orderProperty"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetTaskLogList(int pageIndex, int pageSize, int taskid
            , string orderProperty, string key);

       /// <summary>
        /// 获取指定城市列表
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        string GetAppointCity(int customerId);

        #endregion


        #region 文件

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="otype"></param>
        /// <returns></returns>
        bool Uploads(string model, string otype);

        /// <summary>
        /// 获得文件列表
        /// </summary>
        /// <param name="uploadfile"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        string GetUploads(string uploadfile, int pageSize, int pageIndex, int bankid, int proid, string key, int customerid, int customertype);

        /// <summary>
        /// 导出复估押品
        /// </summary>
        /// <param name="uploadfileid"></param>
        /// <param name="customerid"></param>
        /// <returns></returns>
        string TaskExport(int uploadfileid, int customerid);

           /// <summary>
        /// 导入复估押品
        /// </summary>
        /// <returns></returns>
        string TaskExcelUp(string objResolve, int uploadfileid, int rows, int cols);
        
        #endregion
    }
}
