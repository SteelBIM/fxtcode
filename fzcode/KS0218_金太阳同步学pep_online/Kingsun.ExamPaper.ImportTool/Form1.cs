using System;
using System.IO;
using System.Windows.Forms;
using Kingsun.ExamPaper.Common;

namespace Kingsun.ExamPaper.ImportTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (AppSetting.OnlyCheck)
            {
                btnImport.Text = "开始校验";
                btnClear.Visible = false;
            }
            else
            {
                btnImport.Text = "开始导入";
                btnClear.Visible = true;
            }
        }
        /// <summary>
        /// 1.选择路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChoosePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "请选择文件夹";
            DialogResult result = folderDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderDialog.SelectedPath;
                if (folderName != "")
                {
                    txtFileUrl.Text = folderName;
                    //自动读取单元
                    btnLoadPath_Click(null, null);
                }
            }
        }
        /// <summary>
        /// 2.读取单元
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadPath_Click(object sender, EventArgs e)
        {
            //清空显示信息
            listUnit.Items.Clear();
            rtbMessage.Clear();

            string ObjDirPath = txtFileUrl.Text;

            if (ObjDirPath.Length == 0)
            {
                MessageBox.Show("请选择一个目录");
                return;
            }
            try
            {
                DirectoryInfo SourceDir = new DirectoryInfo(ObjDirPath);
                listUnit.Items.Clear();
                listUnit.DisplayMember = "Value";
                int i = 0;
                foreach (FileSystemInfo FSI in SourceDir.GetFileSystemInfos())
                {
                    if (FSI is DirectoryInfo)
                    {
                        listUnit.Items.Add(new ListItem(i.ToString(), FSI.Name));
                        i++;
                    }
                }
            }
            catch
            {
                MessageBox.Show("读选择有效的目录！");
                return;
            }
        }
        /// <summary>
        /// 上移单元
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            if (listUnit.SelectedItems.Count == 0)
            {
                return;
            }
            if (listUnit.SelectedItems.Count > 1)
            {
                object lastSelItem = listUnit.SelectedItems[listUnit.SelectedItems.Count - 1];
                listUnit.ClearSelected();
                listUnit.SelectedItem = lastSelItem;
            }
            int selIndex = listUnit.SelectedIndex;
            if (selIndex > 0)
            {
                listUnit.ClearSelected();
                object selstr = listUnit.Items[selIndex];
                listUnit.Items[selIndex] = listUnit.Items[selIndex - 1];
                listUnit.Items[selIndex - 1] = selstr;
                listUnit.SetSelected(selIndex - 1, true);
            }
        }
        /// <summary>
        /// 下移单元
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, EventArgs e)
        {
            if (listUnit.SelectedItems.Count == 0)
            {
                return;
            }
            if (listUnit.SelectedItems.Count > 1)
            {
                object firstSelItem = listUnit.SelectedItems[0];
                listUnit.ClearSelected();
                listUnit.SelectedItem = firstSelItem;
            }
            int selIndex = listUnit.SelectedIndex;
            if (selIndex < listUnit.Items.Count - 1)
            {
                listUnit.ClearSelected();
                object selstr = listUnit.Items[selIndex];
                listUnit.Items[selIndex] = listUnit.Items[selIndex+1];
                listUnit.Items[selIndex + 1] = selstr;
                listUnit.SetSelected(selIndex + 1, true);
            }
        }
        /// <summary>
        /// 全选单元
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChooseAll_Click(object sender, EventArgs e)
        {
            listUnit.SelectionMode = SelectionMode.MultiExtended;
            for (int i = 0; i < listUnit.Items.Count; i++)
            {
                listUnit.SetSelected(i, true);                
            }
        }
        /// <summary>
        /// 开始导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            rtbMessage.Clear();
            if (string.IsNullOrEmpty(txtFileUrl.Text))
            {
                MessageBox.Show("请选择一个目录！");
                return;
            }
            if (listUnit.SelectedItems.Count == 0)
            {
                MessageBox.Show("没有选择单元！");
                return;
            }
            if (AppSetting.OnlyCheck)
            {
                Check();
            }
            else
            {
                Import();
            }
            
        }
        /// <summary>
        /// 校验
        /// </summary>
        private void Check()
        {
            if (MessageBox.Show("确定开始校验？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //开始校验
                ChangeBtnStatus();
                #region 校验操作
                string errorMsg = "";//存入失败的信息
                foreach (ListItem item in listUnit.SelectedItems)
                {
                    if (item != null)
                    {
                        errorMsg = "";
                        string filePath = GetExcelPath(txtFileUrl.Text + "/" + item.Value);
                        //调用导入方法
                        new ImportQuestion().LoadExcelByPath(filePath);
                        //读取文件有误
                        if (!string.IsNullOrEmpty(CacheHelper.Instance["ReadErrorMsg"]))
                        {
                            errorMsg += "读取Excel有误：\n";
                            errorMsg += CacheHelper.Instance["ReadErrorMsg"] + "\n";
                            CacheHelper.Instance.Remove("ReadErrorMsg");
                        }
                        //表格填写有误
                        else if (!string.IsNullOrEmpty(CacheHelper.Instance["DataErrorMsg"]))
                        {
                            errorMsg += "填写Excel有误：\n";
                            errorMsg += CacheHelper.Instance["DataErrorMsg"] + "\n";
                            CacheHelper.Instance.Remove("DataErrorMsg");
                        }
                        //资源读取有误
                        else if (!string.IsNullOrEmpty(CacheHelper.Instance["UploadErrorMsg"]))
                        {
                            errorMsg += "资源读取有误：\n";
                            errorMsg += string.IsNullOrEmpty(CacheHelper.Instance["UploadErrorMsg"]) ? "" : ("失败的资源：" + CacheHelper.Instance["UploadErrorMsg"] + "\n");
                            CacheHelper.Instance.Remove("UploadErrorMsg");
                        }
                        //校验失败
                        if (!string.IsNullOrEmpty(errorMsg))
                        {
                            rtbMessage.Text += item.Value + "：\n";
                            rtbMessage.Text += filePath + "\n";
                            rtbMessage.Text += errorMsg;
                            rtbMessage.Text += "*************************************************\n\n";
                        }
                        else
                        {
                            rtbMessage.Text += item.Value + "：校验完成！\n\n";
                            rtbMessage.Text += "*************************************************\n\n";

                        }
                    }
                }
                MessageBox.Show("校验结束！");
                #endregion
                //结束校验
                ChangeBtnStatus();
            }
        }
        /// <summary>
        /// 导入
        /// </summary>
        private void Import()
        {
            if (MessageBox.Show("确定开始导入？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //开始导入
                ChangeBtnStatus();
                #region 导入操作
                string errorMsg = "";//存入失败的信息
                foreach (ListItem item in listUnit.SelectedItems)
                {
                    if (item != null)
                    {
                        errorMsg = "";
                        string filePath = GetExcelPath(txtFileUrl.Text + "/" + item.Value);
                        //调用导入方法
                        new ImportQuestion().LoadExcelByPath(filePath);
                        //读取文件有误
                        if (!string.IsNullOrEmpty(CacheHelper.Instance["ReadErrorMsg"]))
                        {
                            errorMsg += "读取Excel有误，未开始导入：\n";
                            errorMsg += CacheHelper.Instance["ReadErrorMsg"] + "\n";
                            CacheHelper.Instance.Remove("ReadErrorMsg");
                        }
                        //表格填写有误
                        else if (!string.IsNullOrEmpty(CacheHelper.Instance["DataErrorMsg"]))
                        {
                            errorMsg += "填写Excel有误，未开始导入：\n";
                            errorMsg += CacheHelper.Instance["DataErrorMsg"] + "\n";
                            CacheHelper.Instance.Remove("DataErrorMsg");
                        }
                        //单元导入失败
                        else if (!string.IsNullOrEmpty(CacheHelper.Instance["UnitErrorMsg"]))
                        {
                            errorMsg += "单元导入失败，请将错误信息发送给开发人员：\n";
                            errorMsg += string.IsNullOrEmpty(CacheHelper.Instance["UnitErrorMsg"]) ? "" : ("报错信息：" + CacheHelper.Instance["UnitErrorMsg"] + "\n");
                            CacheHelper.Instance.Remove("UnitErrorMsg");
                        }
                        //题目导入失败
                        else if (!string.IsNullOrEmpty(CacheHelper.Instance["QuestionErrorMsg"]))
                        {
                            errorMsg += "题目导入失败，请将错误信息发送给开发人员：\n";
                            errorMsg += string.IsNullOrEmpty(CacheHelper.Instance["QuestionErrorMsg"]) ? "" : ("报错信息：" + CacheHelper.Instance["QuestionErrorMsg"] + "\n");
                            CacheHelper.Instance.Remove("QuestionErrorMsg");
                        }
                        //题目导入成功，资源上传有误
                        else if (!string.IsNullOrEmpty(CacheHelper.Instance["UploadErrorMsg"]))
                        {
                            errorMsg += "题目已导入成功，部分资源上传有误：\n";
                            errorMsg += string.IsNullOrEmpty(CacheHelper.Instance["UploadErrorMsg"]) ? "" : ("上传失败的资源：" + CacheHelper.Instance["UploadErrorMsg"] + "\n");
                            CacheHelper.Instance.Remove("UploadErrorMsg");
                        }
                        //导入失败 显示上传失败的资源
                        if (!string.IsNullOrEmpty(errorMsg))
                        {
                            rtbMessage.Text += item.Value + "：\n";
                            rtbMessage.Text += filePath + "\n";
                            rtbMessage.Text += errorMsg;
                            rtbMessage.Text += "*************************************************\n\n";
                        }
                        else
                        {
                            rtbMessage.Text += item.Value + "：导入完成！\n\n";
                            rtbMessage.Text += "*************************************************\n\n";

                        }
                    }
                }
                MessageBox.Show("导入结束！");
                #endregion
                //结束导入
                ChangeBtnStatus();
            }
        }

        /// <summary>
        /// 获取题库导入版本文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetExcelPath(string path)
        {
            string fullpath = "";
            DirectoryInfo SourceDir = new DirectoryInfo(path);
            foreach (FileSystemInfo FSI in SourceDir.GetFileSystemInfos())
            {
                if (FSI is FileInfo)
                {
                    if (FSI.FullName.Contains("题目导入模板"))
                    {
                        fullpath = FSI.FullName;
                    }

                }
            }
            return fullpath;
        }

        /// <summary>
        /// 改变按钮状态及文本，防止重复操作
        /// </summary>
        private void ChangeBtnStatus()
        {
            if (btnImport.Enabled)
            {
                btnImport.Text = "进行中...";
                btnImport.Enabled = false;
            }
            else
            {
                btnImport.Enabled = true;
                if (AppSetting.OnlyCheck)
                {
                    btnImport.Text = "开始校验";
                }
                else
                {
                    btnImport.Text = "开始导入";
                }
            }
        }
        /// <summary>
        /// 清空库中的所有导入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(AppSetting.EditionName))
            {
                MessageBox.Show("请先配置版本名称！");
            }
            else if (MessageBox.Show("确定要清空数据库中的数据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (new BaseManagement().ExcuteSqlWithTran("truncate table QTb_Edition;truncate table QTb_Book;truncate table QTb_Catalog;truncate table QTb_Resource;"
                    + "truncate table Tb_BlankAnswer;truncate table Tb_SelectItem;truncate table Tb_QuestionInfo;truncate table Tb_StuAnswer;truncate table Tb_StuCatalog;"
                    + "insert into QTb_Edition(EditionName,ParentID,IsRemove,MOD_ED) values('" + AppSetting.EditionName + "',0,0," + AppSetting.EditionID.ToString() + ")"))
                {
                    MessageBox.Show("已清空！");
                }
                else
                {
                    MessageBox.Show("清空失败！");
                }
            }
        }
    }
}
