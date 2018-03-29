using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FXTExcelAddIn
{
    public partial class UC_CitySelect : UCBase
    {
        public UC_CitySelect()
        {
            InitializeComponent();
        }


        protected void SetStatus(EnumHelper.LabelStatus status, string str)
        {
            SetLabelStatus(status, lblStatus, str);
        }

        private void UC_CitySelect_Load(object sender, EventArgs e)
        {
            btnGetCity.Visible = false;
            btnGetProvine.Visible = false;
            SetStatus(EnumHelper.LabelStatus.Normal, "获取省份城市中...");
            EnableControls(false);
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 500;
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Enabled = true;
        }

        private System.Windows.Forms.Timer timer;
        private void Timer_Tick(object obj, EventArgs args)
        {
            timer.Enabled = false;
            Init();
        }


        public void Init()
        {
            GetProvinces();
        }

        private void GetProvinces()
        {
            EnableControls(false);
            btnGetProvine.Visible = false;
            //初始化省份列表
            cbProvince.DropDownStyle = ComboBoxStyle.DropDownList;
            System.Net.WebClient wc = new System.Net.WebClient();
            JSONHelper.JsonData data = new JSONHelper.JsonData();
            data.sinfo = new SecurityInfo(FxtCommon.SignName, "1003104", "108746028", "855190548");
            data.sinfo.functionname = "provincelist";
            data.info.funinfo = new
            {
                fxtcompanyid = 25,
                typecode = 1003302
            };
            string str = data.GetJsonString();
            try
            {
                string provincelist = FxtCommon.APIPostBack(FxtCommon.API_Datacenter, str);
                JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(provincelist);
                if (rtn.returntype > 0)
                {
                    EnableControls(true);
                    List<Province> provinces = JSONHelper.JSONStringToList<Province>(rtn.data.ToString());
                    foreach (Province item in provinces)
                    {
                        item.provincename = FxtCommon.GetPinyinFirst(item.provincename.Replace("省", "").Replace("自治区", "").Replace("壮族", "")) + "_" + item.provincename;
                    }
                    provinces.Sort((x, y) => x.provincename.CompareTo(y.provincename));
                    cbProvince.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbProvince.DataSource = provinces;
                    cbProvince.ValueMember = "provinceid";
                    cbProvince.DisplayMember = "provincename";
                    SetStatus(EnumHelper.LabelStatus.Success, "获取省份城市成功");
                }
                else
                {
                    SetStatus(EnumHelper.LabelStatus.Faild, rtn.returntext.ToString());
                    btnGetProvine.Visible = true;
                    btnGetProvine.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                SetStatus(EnumHelper.LabelStatus.Faild, ex.Message);
                btnGetProvine.Visible = true;
                btnGetProvine.Enabled = true;
            }
        }

        /// <summary>
        /// 绑定城市列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCitys();
        }

        private void GetCitys()
        {
            EnableControls(false);
            btnGetCity.Visible = false;
            System.Net.WebClient wc = new System.Net.WebClient();
            JSONHelper.JsonData data = new JSONHelper.JsonData();
            data.sinfo = new SecurityInfo(FxtCommon.SignName, "1003104", "108746028", "855190548");
            data.sinfo.functionname = "citylist";
            data.info.funinfo = new
            {
                fxtcompanyid = 25,
                typecode = 1003302,
                provinceid = cbProvince.SelectedValue
            };
            string str = data.GetJsonString();
            try
            {
                string citylist = FxtCommon.APIPostBack(FxtCommon.API_Datacenter, str);
                JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(citylist);
                if (rtn.returntype > 0)
                {
                    EnableControls(true);
                    List<City> citys = JSONHelper.JSONStringToList<City>(rtn.data.ToString());
                    foreach (City item in citys)
                    {
                        item.cityname = FxtCommon.GetPinyinFirst(item.cityname.Replace("市", "").Replace("直辖", "")) + "_" + item.cityname;
                    }
                    citys.Sort((x, y) => x.cityname.CompareTo(y.cityname));
                    cbCity.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbCity.DataSource = citys;
                    cbCity.ValueMember = "cityid";
                    cbCity.DisplayMember = "cityname";
                    SetStatus(EnumHelper.LabelStatus.Success, "获取城市成功");
                }
                else
                {
                    SetStatus(EnumHelper.LabelStatus.Faild, "获取城市失败");
                    btnGetCity.Visible = true;
                    btnGetCity.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                SetStatus(EnumHelper.LabelStatus.Faild, ex.Message);
                btnGetCity.Visible = true;
                btnGetCity.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FxtAddIn.CityID = Convert.ToInt32(cbCity.SelectedValue);
            FxtAddIn.CityName = ((City)cbCity.SelectedItem).cityname.Split(new string[] { "_" },StringSplitOptions.RemoveEmptyEntries)[1].ToString();
            Globals.Ribbons.FxtRibbon.btnSelCity.Label = FxtAddIn.CityName + "\n";
            Globals.Ribbons.FxtRibbon.CityPane.Visible = false;
            
        }

        private void btnGetProvine_Click(object sender, EventArgs e)
        {
            GetProvinces();
        }

        private void btnGetCity_Click(object sender, EventArgs e)
        {
            GetCitys();
        }

        private void UC_CitySelect_DragDrop(object sender, DragEventArgs e)
        {
           
        }

        private void UC_CitySelect_StyleChanged(object sender, EventArgs e)
        {
            
        }

        private void UC_CitySelect_AutoSizeChanged(object sender, EventArgs e)
        {
           
        }

        private void UC_CitySelect_DockChanged(object sender, EventArgs e)
        {
            
        }

        private void UC_CitySelect_Move(object sender, EventArgs e)
        {
            
        }
    }
}
