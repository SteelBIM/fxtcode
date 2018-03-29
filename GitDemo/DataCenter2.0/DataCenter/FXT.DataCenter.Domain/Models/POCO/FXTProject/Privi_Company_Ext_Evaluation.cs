using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_Company_Ext_Evaluation
    {
        private int _fk_companyid;
        public int fk_companyid
        {
            get { return _fk_companyid; }
            set { _fk_companyid = value; }
        }
        private int? _houselevel;
        /// <summary>
        /// 房地产资质证等级（1013）
        /// </summary>
        public int? houselevel
        {
            get { return _houselevel; }
            set { _houselevel = value; }
        }
        private string _housecode;
        /// <summary>
        /// 房地产资质证编号
        /// </summary>
        public string housecode
        {
            get { return _housecode; }
            set { _housecode = value; }
        }
        private int? _houseassociationid;
        /// <summary>
        /// 房地产所属协会
        /// </summary>
        public int? houseassociationid
        {
            get { return _houseassociationid; }
            set { _houseassociationid = value; }
        }
        private int? _landlevel;
        /// <summary>
        /// 土地资质等级（1014）
        /// </summary>
        public int? landlevel
        {
            get { return _landlevel; }
            set { _landlevel = value; }
        }
        private string _landcode;
        /// <summary>
        /// 土地资质编号
        /// </summary>
        public string landcode
        {
            get { return _landcode; }
            set { _landcode = value; }
        }
        private int? _landassociationid;
        /// <summary>
        /// 土地资质所属协会
        /// </summary>
        public int? landassociationid
        {
            get { return _landassociationid; }
            set { _landassociationid = value; }
        }
        private int? _assetslevel;
        /// <summary>
        /// 资产评估等级（1021）
        /// </summary>
        public int? assetslevel
        {
            get { return _assetslevel; }
            set { _assetslevel = value; }
        }
        private string _assetscode;
        /// <summary>
        /// 资产评估编号
        /// </summary>
        public string assetscode
        {
            get { return _assetscode; }
            set { _assetscode = value; }
        }
        private int? _assetsassociationid;
        /// <summary>
        /// 资产评估所属协会
        /// </summary>
        public int? assetsassociationid
        {
            get { return _assetsassociationid; }
            set { _assetsassociationid = value; }
        }

    }
}
