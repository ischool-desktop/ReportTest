using Aspose.Words;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ReportTest
{
    public partial class MergeNameExprotForm :FISCA.Presentation.Controls.BaseForm
    {
        List<string> _ColumnNameList;
        public MergeNameExprotForm()
        {
            InitializeComponent();
            _ColumnNameList = new List<string>();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        public void SetDefaultColumnsName(List<string> data)
        {
            _ColumnNameList = data;
        }

        private bool ChkSave()
        {
            bool retVal = true;

            foreach (DataGridViewRow dr in dgData.Rows)
            {
                if (dr.IsNewRow)
                    continue;

                if (dr.Cells[col01.Index].Value == null || dr.Cells[col02.Index].Value == null)
                {
                    retVal = false;
                    break;
                }
            }

            return retVal;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (ChkSave() == false)
            {
                FISCA.Presentation.Controls.MsgBox.Show("顯示名稱或核病名稱不允取空白。");
                return;
            }

            btnExport.Enabled = false;

            Document doc = new Document(new MemoryStream(Properties.Resources.temp));
            DocumentBuilder db = new DocumentBuilder(doc);
            List<DataGridViewRow> lsda = new List<DataGridViewRow>();
            foreach(DataGridViewRow dr in dgData.SelectedRows)
            {
                if(dr.IsNewRow)
                    continue;
                lsda.Add(dr);
            }
            lsda.Reverse();

            db.StartTable();
            // 使用群組
            if (chkGroup.Checked)
            {
                int cot = iptAdd.Value;
                for (int i = 1; i <= cot; i++)
                {
                    string ii="";
                    if(cot>1)
                    {
                        ii="_"+i;
                    }
                    foreach (DataGridViewRow dr in lsda)
                    {

                        db.InsertCell();
                        string insertStrD = "«" + dr.Cells[col01.Index].Value.ToString() + "»";

                        string bfStr = "";
                        if (dr.Cells[col03.Index].Value != null)
                        {
                            string bf = dr.Cells[col03.Index].Value.ToString();
                            if (!string.IsNullOrEmpty(bf))
                                bfStr = @" \b " + bf;
                        }

                        string afStr = "";
                        if (dr.Cells[col04.Index].Value != null)
                        {
                            string af = dr.Cells[col04.Index].Value.ToString();
                            if (!string.IsNullOrEmpty(af))
                                afStr = @" \f "+af;
                        }

                        string insertStrF = "MERGEFIELD " + dr.Cells[col02.Index].Value.ToString()+ii+bfStr+afStr+ @" \* MERGEFORMAT";
                        db.InsertField(insertStrF, insertStrD);
                    }

                    db.EndRow();
                }            
            }
            else
            {
                
                // 非群組
                foreach (DataGridViewRow dr in lsda)
                {                 

                    string bfStr = "";
                    if (dr.Cells[col03.Index].Value != null)
                    {
                        string bf = dr.Cells[col03.Index].Value.ToString();
                        if (!string.IsNullOrEmpty(bf))
                            bfStr = @" \b " + bf;
                    }

                    string afStr = "";
                    if (dr.Cells[col04.Index].Value != null)
                    {
                        string af = dr.Cells[col04.Index].Value.ToString();
                        if (!string.IsNullOrEmpty(af))
                            afStr = @" \f " + af;
                    }

                    db.InsertCell();
                    string insertStrD = "«" + dr.Cells[col01.Index].Value.ToString() + "»";
                    string insertStrF = "MERGEFIELD " + dr.Cells[col02.Index].Value.ToString() + bfStr+afStr+@" \* MERGEFORMAT";
                    db.InsertField(insertStrF, insertStrD);
                    db.EndRow();
                }                
            }

            db.EndTable();

            Utility.ExportDoc("合併欄位", doc);
            btnExport.Enabled = true;
        }

        private void chkGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGroup.Checked)
                iptAdd.Enabled = true;
            else
                iptAdd.Enabled = false;
        }

        private void MergeNameExprotForm_Load(object sender, EventArgs e)
        {
            LoadDefault();    
        }

        private void LoadDefault()
        {
            if (_ColumnNameList.Count > 0)
            {
                foreach (string name in _ColumnNameList)
                {
                    int idx = dgData.Rows.Add();
                    dgData.Rows[idx].Cells[col01.Index].Value = name;
                    dgData.Rows[idx].Cells[col02.Index].Value = name;
                    dgData.Rows[idx].Cells[col03.Index].Value = "";
                    dgData.Rows[idx].Cells[col04.Index].Value = "";
                }            
            }
        }

        private void btn01_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDisplayName.Text.Trim()))
            {
                string str = txtDisplayName.Text.Trim();
                foreach (DataGridViewRow dr in dgData.SelectedRows)
                {
                    if (dr.IsNewRow)
                        continue;

                    dr.Cells[col01.Index].Value = str;
                }
            }
        }
    }
}
