namespace CAS.Entity.BonaServiceEntity.Response
{
    /// <summary>
    /// 业务单审核详情信息
    /// </summary>
    public class RecordBaseEntity
    {
        /// <summary>
        /// 买房人身份证
        /// </summary>
        public string BUYERIDNUM { get; set; }

        /// <summary>
        /// 业务单ID
        /// </summary>
        public string SERIALNO { get; set; }

        /// <summary>
        /// 产权人姓名
        /// </summary>
        public string PERSONNAME { get; set; }

        /// <summary>
        /// 买房人联系方式
        /// </summary>
        public string BUYERPHONE { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string BUSINESSTYPE { get; set; }

        /// <summary>
        /// 业务类型名字
        /// </summary>
        public string BUSINESSTYPENNAME
        {

            get
            {
                var re = "";
                if (this.BUSINESSTYPE == "A")
                {
                    re = "按揭";
                }
                else if (this.BUSINESSTYPE == "D")
                {
                    re = "抵押";
                }
                return re;

            }
        }

        /// <summary>
        /// 产权人联系方式
        /// </summary>
        public string CONTRACTPHONE { get; set; }

        /// <summary>
        /// 房产地址
        /// </summary>
        public string ADDRESS { get; set; }

        /// <summary>
        /// 房产所在区
        /// </summary>
        public string AREACODE { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string PROJECTNAME { get; set; }

        /// <summary>
        /// 贷款银行
        /// </summary>
        public string LENDINGBANK { get; set; }


        /// <summary>
        /// 贷款银行
        /// </summary>
        public string OHERLENDINGBANK { get; set; }

        /// <summary>
        /// 拟贷金额
        /// </summary>
        public string AMOUNT { get; set; }

        /// <summary>
        /// 买房人姓名
        /// </summary>
        public string BUYERNAME { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public string INPUTDATE { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDNUM { get; set; }

        /// <summary>
        /// 拟货金额
        /// </summary>
        public string PREPARELOANAMOUNT { get; set; }

        /// <summary>
        /// 首付金额
        /// </summary>
        public string DOWNPAYMENT { get; set; }

        /// <summary>
        /// 贷款人角色
        /// </summary>
        public string LENDERROLE { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string PERSONCITY { get; set; }

        /// <summary>
        /// 买房人户籍
        /// </summary>
        public string BUYERCENSUSREGISTER { get; set; }
        /// <summary>
        /// 产权人户籍
        /// </summary>
        public string OWNERCENSUSREGISTER { get; set; }
        /// <summary>
        /// 按揭类型
        /// </summary>
        public string MORTGAGETYPE { get; set; }
        /// <summary>
        /// 开发商
        /// </summary>
        public string DEVELOPERS { get; set; }
        //{"BUYERIDNUM":"451111111111111112","SERIALNO":"BN20151210100019","PERSONNAME":"请补充","BUYERPHONE":"13800010002","BUSINESSTYPE":"A","CONTRACTPHONE":"请补充","ADDRESS":"华润城二期12栋3单元1801","PROJECTNAME":"华润城二期","LENDINGBANK":"招商银行","AMOUNT":"      20.00","BUYERNAME":"刘佳","INPUTDATE":1449742987000,"IDNUM":"请补充"
    }
}
