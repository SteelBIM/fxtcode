using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FXTExcelAddIn
{
    public  class UCBase:UserControl
    {
        /// <summary>
        /// 禁用/启用控件
        /// </summary>
        /// <param name="enable"></param>
        protected void EnableControls(bool enable)
        {
            EnableControls(this.Controls, enable);
        }

        private void EnableControls(ControlCollection ctrls, bool enable)
        {
            foreach (Control item in ctrls)
            {
                if (item.HasChildren)
                {
                    EnableControls(item.Controls, enable);
                }
                else { item.Enabled = enable; }
            }
        }

        protected void SetTimeStatus(RichTextBox txtStatus, string str,bool begin)
        {
            string status = string.Empty;
            if (txtStatus.TextLength > 0 && begin)
            {
                status += "---------------------\n";
            }
            status += str + (begin?"开始":"结束") + ":" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            SetLabelStatus(EnumHelper.LabelStatus.Normal,txtStatus, status);
        }

        protected void SetLabelStatus(EnumHelper.LabelStatus status,Label lblStatus, string str)
        {
            lblStatus.Text = str;
            switch (status)
            {
                case EnumHelper.LabelStatus.Faild:
                    lblStatus.ForeColor = Color.Red;
                    break;
                case EnumHelper.LabelStatus.Success:
                    lblStatus.ForeColor = Color.Green;
                    break;
                case EnumHelper.LabelStatus.Normal:
                default:
                    lblStatus.ForeColor = Color.Black;
                    break;
            }
        }

        protected void SetLabelStatus(EnumHelper.LabelStatus status, RichTextBox txtStatus, string str)
        {
            int start = txtStatus.TextLength;
            txtStatus.AppendText((txtStatus.TextLength==0?"": "\n") + str);
            txtStatus.Select(start, txtStatus.TextLength);
            switch (status)
            {
                case EnumHelper.LabelStatus.Faild:
                    txtStatus.SelectionColor = Color.Red;
                    break;
                case EnumHelper.LabelStatus.Success:
                    txtStatus.SelectionColor = Color.Green;
                    break;
                case EnumHelper.LabelStatus.Normal:
                default:
                    txtStatus.SelectionColor = Color.Black;
                    break;
            }
            
        }

       
    }
}
