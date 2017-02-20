using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace HDL_Buspro_Setup_Tool
{
    public class SenceDesign
    {
        public List<ChnSenceInfomation> Scene;
        private int intWidth = 0;
        private int intHeight = 0;
        private System.Windows.Forms.Panel Con;
        private ComboBox cb1 = new ComboBox();
        private object MyActiveObj;
        private int ZoneIndex = 0;
        private int SceneIndex = 0;
        private string strNameHead;
        private int MyDeviceType = 0;
        DataGridView DGV = new DataGridView();
        private bool isSet = false;
        private bool isMultiChange = false;
        private int AddControlIndex = 0;

        public SenceDesign(System.Windows.Forms.Panel pnl, List<ChnSenceInfomation> scene, int DeviceType,int zoneindex,int sceneindex,object obj,string strname)
        {
            Con = pnl;
            this.Scene = scene;
            this.MyDeviceType = DeviceType;
            intWidth = Con.Width;
            intHeight = Con.Height;
            this.MyActiveObj = obj;
            this.ZoneIndex = zoneindex;
            this.SceneIndex = sceneindex;
            this.strNameHead = strname;
            loadScene();
        }

        private void loadScene()
        {
            isSet = true;
            if (Scene != null && Scene.Count > 0)
            {
                DGV = new DataGridView();
                cb1.DropDownStyle = ComboBoxStyle.DropDownList;
                cb1.SelectedIndexChanged+=cb1_SelectedIndexChanged;
                DGV.Controls.Add(cb1);
                DGV.ReadOnly = true;
                DGV.Dock = DockStyle.Fill;
                cb1.Visible = false;
                DGV.AllowUserToAddRows = false;
                DGV.AllowUserToDeleteRows = false;
                DGV.AllowUserToResizeRows = false;
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 12F);
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                DGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                DGV.EnableHeadersVisualStyles = false;
                DGV.RowHeadersVisible = false;
                DGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                System.Windows.Forms.DataGridViewTextBoxColumn cl1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cl1.HeaderText = CsConst.WholeTextsList[934].sDisplayName;
                cl1.Name = "cl1";
                cl1.ReadOnly = true;
                cl1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cl1.Width = 150;
                System.Windows.Forms.DataGridViewTextBoxColumn cl2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cl2.HeaderText = CsConst.mstrINIDefault.IniReadValue("Public", "00366", "");

                cl2.Name = "cl2";
                cl2.ReadOnly = true;
                cl2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cl2.Width = 300;
                System.Windows.Forms.DataGridViewTextBoxColumn cl3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cl3.HeaderText = CsConst.WholeTextsList[2524].sDisplayName;

                cl3.Name = "cl3";
                cl3.ReadOnly = true;
                cl3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cl3.Width = 100;
                System.Windows.Forms.DataGridViewTextBoxColumn cl4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cl4.HeaderText = CsConst.mstrINIDefault.IniReadValue("Public", "99599", "");
                cl4.Name = "cl4";
                cl4.ReadOnly = true;
                cl4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cl4.Width = 100;
                cl4.Visible = false;
                DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cl1, cl2, cl3, cl4 });
                DGV.CellClick += DGV_CellClick;
                DGV.CellValueChanged += DGV_CellValueChanged;
                DGV.Scroll += DGV_Scroll;
                Con.Controls.Add(DGV);
                HDLSysPF.setDataGridViewColumnsWidth(DGV);
                DGV.Rows.Clear();
                for (int i = 0; i < Scene.Count; i++)
                {
                    ChnSenceInfomation temp = Scene[i];
                    string strChanel = temp.ID.ToString();
                    string str1 = CsConst.mstrINIDefault.IniReadValue("Public", "99598", "");
                    string str2 = CsConst.mstrINIDefault.IniReadValue("Public", "99597", "");
                    if (temp.type == 1)
                        strChanel = strChanel + "-" + str1;
                    else if (temp.type == 2)
                        strChanel = strChanel + "-" + str2;
                    string strLight = temp.light.ToString();
                    int Type = 0;
                    if (temp.type == 2)
                    {
                        if (temp.isCurtainChannel)
                        {
                            if (temp.light == 1) strLight = CsConst.CurtainRelayState[2];
                            else if (temp.light == 100) strLight = CsConst.CurtainRelayState[1];
                            else strLight = CsConst.CurtainRelayState[0];
                            
                            Type = 3;
                        }
                        else
                        {
                            if (temp.light >= 100) strLight = CsConst.Status[1];
                            else strLight = CsConst.Status[0];
                            Type = 2;
                        }
                    }
                    else if (temp.type == 1)
                    {
                        Type = 1;
                    }
                    object[] obj = new object[] { strChanel, temp.Remark, strLight, Type.ToString() };
                    DGV.Rows.Add(obj);
                }
            }
            isSet = false;
        }

        void DGV_Scroll(object sender, ScrollEventArgs e)
        {
            cb1.Visible = false;
        }

        void DGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isSet) return;
            if (MyActiveObj is Relay)
            {
                Relay oRe = null;
                if (CsConst.myRelays != null)
                {
                    foreach (Relay oTmp in CsConst.myRelays)
                    {
                        if (oTmp.DIndex == (MyActiveObj as Relay).DIndex)
                        {
                            oRe = oTmp;
                            int Tag = Convert.ToInt32(DGV[0, e.RowIndex].Value.ToString().Split('-')[0]);
                            if (oRe.Areas == null || oRe.Areas.Count == 0) return;
                            if (DGV[2, e.RowIndex].Value.ToString() == cb1.Items[0].ToString())
                                oRe.Areas[ZoneIndex].Scen[SceneIndex].light[Tag - 1] = 0;
                            else if (DGV[2, e.RowIndex].Value.ToString() == cb1.Items[1].ToString())
                                oRe.Areas[ZoneIndex].Scen[SceneIndex].light[Tag - 1] = 100;
                            if (oTmp.isOutput && !isMultiChange)
                            {
                                if (DGV.SelectedRows.Count > 1 && e.RowIndex == AddControlIndex)
                                {
                                    return;
                                }
                                byte bytAreaID = Convert.ToByte(ZoneIndex);
                                byte bytSceID = Convert.ToByte(SceneIndex);
                                string strName = strNameHead.Split('\\')[0].ToString();
                                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                                byte bytDevID = byte.Parse(strName.Split('-')[1]);
                                int OutputCount = 0;
                                for (int i = 0; i < oTmp.Chans.Count; i++)
                                {
                                    if (oTmp.Chans[i].intBelongs == bytAreaID + 1)
                                    {
                                        OutputCount++;
                                    }
                                }
                                byte[] bytTmp = new byte[2 + OutputCount];
                                bytTmp[0] = Convert.ToByte(bytAreaID + 1);
                                bytTmp[1] = bytSceID;
                                for (int i = 0; i < OutputCount; i++)
                                {
                                    bytTmp[i + 2] = oTmp.Areas[bytAreaID].Scen[bytSceID].light[Tag - 1];
                                }
                                CsConst.mySends.AddBufToSndList(bytTmp, 0xF074, bytSubID, bytDevID, false, false, false,false);
                            }
                            break;
                        }
                    }
                }
            }
        }

        void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cb1.Visible = false;
            if (e.RowIndex >= 0)
            {
                if (SceneIndex != 0)
                {
                    cb1.Items.Clear();
                    int Type = Convert.ToInt32((sender as DataGridView)[3, e.RowIndex].Value.ToString());
                    string strLight = (sender as DataGridView)[2, e.RowIndex].Value.ToString();
                    if (Type == 1)
                    {
                        cb1.Items.Clear();
                        for (int i = 0; i <= 100; i++) cb1.Items.Add(i.ToString());
                    }
                    else if (Type == 2)
                    {
                        cb1.Items.Clear();
                        cb1.Items.AddRange(CsConst.Status);
                    }
                    else if (Type == 3)
                    {
                        cb1.Items.Clear();
                        cb1.Items.AddRange(CsConst.CurtainRelayState);
                    }
                    addcontrols(2, e.RowIndex, cb1, (sender as DataGridView));
                    if (cb1.Visible && cb1.Items.Count > 0)
                    {
                        if (!cb1.Items.Contains(strLight))
                            cb1.SelectedIndex = 0;
                        else
                            cb1.Text = strLight;
                    }
                    if (cb1.Visible) cb1_SelectedIndexChanged(null, null);
                }
            }
        }

        private void addcontrols(int col, int row, Control con,DataGridView dgv)
        {
            con.Show();
            con.Visible = true;
            Rectangle rect = dgv.GetCellDisplayRectangle(col, row, true);
            con.Size = rect.Size;
            con.Top = rect.Top;
            con.Left = rect.Left;
        }

        void ModifyMultilinesIfNeeds(string strTmp, int ColumnIndex)
        {
            if (DGV.SelectedRows == null || DGV.SelectedRows.Count == 0) return;
            isMultiChange = false;
            if (strTmp == null) strTmp = "";
            if (DGV.SelectedRows.Count > 1)
            {
                isMultiChange = true;
                for (int i = 0; i < DGV.SelectedRows.Count; i++)
                {
                    if (i == (DGV.SelectedRows.Count - 1)) isMultiChange = false;
                    
                    DGV.SelectedRows[i].Cells[ColumnIndex].Value = strTmp;
                }
            }
        }

        void cb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DGV.CurrentRow.Index < 0) return;
            if (DGV.RowCount <= 0) return;
            int index = DGV.CurrentRow.Index;
            AddControlIndex = index;
            
            DGV[2, index].Value = cb1.Text;
            ModifyMultilinesIfNeeds(cb1.Text.ToString(), 2);
        }
            
    }
}
