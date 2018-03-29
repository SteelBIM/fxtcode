using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtNHibernate.DATProjectDomain.Entities;

namespace FxtNHibernate.DTODomain.DATProjectDTO
{
    /**
     * 作者:曾智磊
     * 摘要:新建 2014.03.03(FxtNHibernate.DATProjectDomain.DiyEntities中转移过来,原文件已删除)
     * **/
    public class DATProjectView
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public virtual string ProjectName
        {
            get;
            set;
        }
        /// <summary>
        /// CityID
        /// </summary>
        public virtual int CityID
        {
            get;
            set;
        }
        /// <summary>
        /// CityName
        /// </summary>
        public virtual string CityName
        {
            get;
            set;
        }
        /// <summary>
        /// 行政区
        /// </summary>
        public virtual int AreaID
        {
            get;
            set;
        }
        /// <summary>
        /// 行政区名称
        /// </summary>
        public virtual string AreaName
        {
            get;
            set;
        }
        public DATProjectView()
        { }
        public DATProjectView(int projectId, string projectName, int areaID, string areaName,int cityId,string cityName)
        {
            this.ProjectId = projectId;
            this.ProjectName = ProjectName;
            this.AreaID = areaID;
            this.AreaName = areaName;
            this.CityID = cityId;
            this.CityName = cityName;
        }
        public DATProjectView(DATProject proj, IList<SYSCity> cityList, IList<SYSArea> areaList)
        {
            this.ProjectId = proj.ProjectId;
            this.ProjectName = proj.ProjectName; ;
            this.AreaID = proj.AreaID;
            SYSArea area = areaList.Where(p => p.AreaId == proj.AreaID).FirstOrDefault();
            if (area != null)
            {
                this.AreaName = area.AreaName;
            }
            this.CityID = proj.CityID;
            SYSCity city = cityList.Where(p => p.CityId == proj.CityID).FirstOrDefault();
            if (city != null)
            {
                this.CityName = city.CityName;
            }
        }
        public static IList<DATProjectView> ConvertToList(IList<DATProject> projList, IList<SYSCity> cityList, IList<SYSArea> areaList)
        {
            IList<DATProjectView> viewList = new List<DATProjectView>();
            if (projList == null || projList.Count < 1)
            {
                return viewList;
            }
            foreach (DATProject proj in projList)
            {
                DATProjectView view = new DATProjectView(proj, cityList, areaList);
                viewList.Add(view);
            }
            return viewList;
        }
    }
}
