using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public partial class RelayExclusion : UserControl
    {
        private object MyActiveObj;
        private int DeviceType = 0;//设备类型
        private MHRCU oMHRCU = null;
        private Relay oRelay = null;
        private string strNameHead;
        private byte bytSubID;
        private byte bytDevID;
        private bool isSet = false;
        public RelayExclusion(int devicetype, object obj, string strName)
        {
            InitializeComponent();
            this.DeviceType = devicetype;
            this.MyActiveObj = obj;
            this.strNameHead = strName;
            this.bytSubID = Convert.ToByte(strName.Split('\\')[0].ToString().Split('-')[0]);
            this.bytDevID = Convert.ToByte(strName.Split('\\')[0].ToString().Split('-')[1]);
            #region
            oMHRCU = null;
            oRelay = null;
            if (MyActiveObj is MHRCU)
            {
                if (CsConst.myRcuMixModules != null)
                {
                    foreach (MHRCU oTmp in CsConst.myRcuMixModules)
                    {
                        if (oTmp.DIndex == (MyActiveObj as MHRCU).DIndex)
                        {
                            oMHRCU = oTmp;
                            break;
                        }
                    }
                }
            }
            else if (MyActiveObj is Relay)
            {
                if (CsConst.myRelays != null)
                {
                    foreach (Relay oTmp in CsConst.myRelays)
                    {
                        if (oTmp.DIndex == (MyActiveObj as Relay).DIndex)
                        {
                            oRelay = oTmp;
                            break;
                        }
                    }
                }
            }
            #endregion
            if (CsConst.iLanguageId == 1)
            {
                cl1.HeaderText = CsConst.mstrINIDefault.IniReadValue("RelayExclusion", "00000", "");
                cl2.HeaderText = CsConst.mstrINIDefault.IniReadValue("RelayExclusion", "00001", "");
                cl3.HeaderText = CsConst.mstrINIDefault.IniReadValue("RelayExclusion", "00002", "");
                cl4.HeaderText = CsConst.mstrINIDefault.IniReadValue("RelayExclusion", "00003", "");
                cl5.HeaderText = CsConst.mstrINIDefault.IniReadValue("RelayExclusion", "00004", "");
                lbHint.Text = CsConst.mstrINIDefault.IniReadValue("RelayExclusion", "00005", "");
            }
            if (RelayDeviceTypeList.RelayThreeChnBaseWithACFunction.Contains(DeviceType))
            {
                lbHint.Visible = false;
                chbBoxList.Visible = false;
            }
            loadInfoMation();
        }

        private void loadInfoMation()
        {
            isSet = true;
            DgvChns.Rows.Clear();
            chbBoxList.Items.Clear();
            if (oMHRCU != null)
            {
                string strRelay = CsConst.mstrINIDefault.IniReadValue("Public", "99601", "");
                for (int i = 0; i < oMHRCU.ChnList.Count; i++)
                {
                    if (i < 16)
                    {
                        object[] obj = new object[] { oMHRCU.ChnList[i].ID + "-" + strRelay ,oMHRCU.ChnList[i].Remark,
                                                      oMHRCU.ChnList[i].stairsEnable,oMHRCU.ChnList[i].AutoClose.ToString(),false};
                        DgvChns.Rows.Add(obj);
                    }
                }
                string strChn = CsConst.mstrINIDefault.IniReadValue("Public", "00000", "");
                string strexclusion = CsConst.mstrINIDefault.IniReadValue("Public", "99591", "");
                for (int i = 1; i <= 8; i++)
                {
                    if (oMHRCU.bytAryExclusion[i-1] == 1)
                        chbBoxList.Items.Add(strChn + " " + ((i * 2) - 1).ToString() + "-" + (i * 2).ToString() + strexclusion, true);
                    else
                        chbBoxList.Items.Add(strChn + " " + ((i * 2) - 1).ToString() + "-" + (i * 2).ToString() + strexclusion, false);
                }
            }
            else if (oRelay != null)
            {
                for (int i = 0; i < oRelay.Chans.Count; i++)
                {
                    object[] obj = new object[] { oRelay.Chans[i].ID,oRelay.Chans[i].Remark,
                                                      (oRelay.Chans[i].bytOnStair==1),oRelay.Chans[i].intONTime.ToString(),false};
                    DgvChns.Rows.Add(obj);
                }
                string strChn = CsConst.mstrINIDefault.IniReadValue("Public", "00000", "");
                string strexclusion = CsConst.mstrINIDefault.IniReadValue("Public", "99591", "");
                if (chbBoxList.Visible)
                {
                    for (int i = 1; i <= oRelay.bytAryExclusion.Length; i++)
                    {
                        if (oRelay.bytAryExclusion[i - 1] == 1)
                            chbBoxList.Items.Add(strChn + " " + ((i * 2) - 1).ToString() + "-" + (i * 2).ToString() + strexclusion, true);
                        else
                            chbBoxList.Items.Add(strChn + " " + ((i * 2) - 1).ToString() + "-" + (i * 2).ToString() + strexclusion, false);
                    }
                }
            }
            isSet = false;
        }

        private void DgChns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (oMHRCU != null)
                {
                    if (oMHRCU.ChnList == null) return;
                    if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                    if (DgvChns[e.ColumnIndex, e.RowIndex].Value == null) DgvChns[e.ColumnIndex, e.RowIndex].Value = "";

                    for (int i = 0; i < DgvChns.SelectedRows.Count; i++)
                    {
                        DgvChns.SelectedRows[i].Cells[e.ColumnIndex].Value = DgvChns[e.ColumnIndex, e.RowIndex].Value.ToString();

                        string strTmp = "";
                        switch (e.ColumnIndex)
                        {
                            case 1:
                                strTmp = DgvChns[1, DgvChns.SelectedRows[i].Index].Value.ToString();
                                DgvChns[1, DgvChns.SelectedRows[i].Index].Value = HDLPF.IsRightStringMode(strTmp);
                                oMHRCU.ChnList[DgvChns.SelectedRows[i].Index].Remark = DgvChns[1, DgvChns.SelectedRows[i].Index].Value.ToString();
                                break;
                            case 2:
                                if (DgvChns[2, DgvChns.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                    oMHRCU.ChnList[DgvChns.SelectedRows[i].Index].stairsEnable = true;
                                else oMHRCU.ChnList[DgvChns.SelectedRows[i].Index].stairsEnable = false;
                                break;
                            case 3:
                                strTmp = DgvChns[3, DgvChns.SelectedRows[i].Index].Value.ToString();
                                DgvChns[3, DgvChns.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 60);
                                oMHRCU.ChnList[DgvChns.SelectedRows[i].Index].AutoClose = Convert.ToInt32(HDLPF.IsNumStringMode(DgvChns[3, DgvChns.SelectedRows[i].Index].Value.ToString(), 0, 3600));
                                break;
                            case 4:
                                string strName = strNameHead.Split('\\')[0].ToString();
                                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                                byte bytDevID = byte.Parse(strName.Split('-')[1]);

                                byte[] bytTmp = new byte[4];
                                bytTmp[0] = (byte)(DgvChns.SelectedRows[i].Index + 1);
                                bytTmp[2] = 0;
                                bytTmp[3] = 0;

                                if (DgvChns[4, DgvChns.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                    bytTmp[1] = 100;
                                else bytTmp[1] = 0;

                                Cursor.Current = Cursors.WaitCursor;
                                if (CsConst.mySends.AddBufToSndList(bytTmp, 0x0031, bytSubID, bytDevID, false, false, false, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                Cursor.Current = Cursors.Default;
                                break;
                        }
                    }
                    if (e.ColumnIndex == 1)
                    {
                        string strTmp = DgvChns[1, e.RowIndex].Value.ToString();
                        oMHRCU.ChnList[e.RowIndex].Remark = DgvChns[1, e.RowIndex].Value.ToString();
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        if (DgvChns[2, e.RowIndex].Value.ToString().ToLower() == "true")
                            oMHRCU.ChnList[e.RowIndex].stairsEnable = true;
                        else oMHRCU.ChnList[e.RowIndex].stairsEnable = false;
                    }
                    else if (e.ColumnIndex == 3)
                    {
                        string strTmp = DgvChns[3, e.RowIndex].Value.ToString();
                        DgvChns[3, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 60);
                        oMHRCU.ChnList[e.RowIndex].AutoClose = Convert.ToInt32(HDLPF.IsNumStringMode(DgvChns[3, e.RowIndex].Value.ToString(), 0, 3600));
                    }
                }
                else if (oRelay != null)
                {
                    if (oRelay.Chans == null) return;
                    if ((e.RowIndex == -1) || (e.ColumnIndex == -1)) return;
                    if (DgvChns[e.ColumnIndex, e.RowIndex].Value == null) DgvChns[e.ColumnIndex, e.RowIndex].Value = "";

                    for (int i = 0; i < DgvChns.SelectedRows.Count; i++)
                    {
                        DgvChns.SelectedRows[i].Cells[e.ColumnIndex].Value = DgvChns[e.ColumnIndex, e.RowIndex].Value.ToString();

                        string strTmp = "";
                        switch (e.ColumnIndex)
                        {
                            case 1:
                                strTmp = DgvChns[1, DgvChns.SelectedRows[i].Index].Value.ToString();
                                DgvChns[1, DgvChns.SelectedRows[i].Index].Value = HDLPF.IsRightStringMode(strTmp);
                                oRelay.Chans[DgvChns.SelectedRows[i].Index].Remark = DgvChns[1, DgvChns.SelectedRows[i].Index].Value.ToString();
                                break;
                            case 2:
                                if (DgvChns[2, DgvChns.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                    oRelay.Chans[DgvChns.SelectedRows[i].Index].bytOnStair = 1;
                                else oRelay.Chans[DgvChns.SelectedRows[i].Index].bytOnStair = 0;
                                break;
                            case 3:
                                strTmp = DgvChns[3, DgvChns.SelectedRows[i].Index].Value.ToString();
                                DgvChns[3, DgvChns.SelectedRows[i].Index].Value = HDLPF.IsNumStringMode(strTmp, 0, 60);
                                oRelay.Chans[DgvChns.SelectedRows[i].Index].intONTime = Convert.ToInt32(HDLPF.IsNumStringMode(DgvChns[3, DgvChns.SelectedRows[i].Index].Value.ToString(), 0, 3600));
                                break;
                            case 4:
                                string strName = strNameHead.Split('\\')[0].ToString();
                                byte bytSubID = byte.Parse(strName.Split('-')[0]);
                                byte bytDevID = byte.Parse(strName.Split('-')[1]);

                                byte[] bytTmp = new byte[4];
                                bytTmp[0] = (byte)(DgvChns.SelectedRows[i].Index + 1);
                                bytTmp[2] = 0;
                                bytTmp[3] = 0;

                                if (DgvChns[4, DgvChns.SelectedRows[i].Index].Value.ToString().ToLower() == "true")
                                    bytTmp[1] = 100;
                                else bytTmp[1] = 0;

                                Cursor.Current = Cursors.WaitCursor;
                                if (CsConst.mySends.AddBufToSndList(bytTmp, 0x0031, bytSubID, bytDevID, false, true, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                                {
                                    
                                    HDLUDP.TimeBetwnNext(20);
                                }
                                Cursor.Current = Cursors.Default;
                                break;
                        }
                    }
                    if (e.ColumnIndex == 1)
                    {
                        string strTmp = DgvChns[1, e.RowIndex].Value.ToString();
                        oRelay.Chans[e.RowIndex].Remark = DgvChns[1, e.RowIndex].Value.ToString();
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        if (DgvChns[2, e.RowIndex].Value.ToString().ToLower() == "true")
                            oRelay.Chans[e.RowIndex].bytOnStair = 1;
                        else oRelay.Chans[e.RowIndex].bytOnStair = 0;
                    }
                    else if (e.ColumnIndex == 3)
                    {
                        string strTmp = DgvChns[3, e.RowIndex].Value.ToString();
                        DgvChns[3, e.RowIndex].Value = HDLPF.IsNumStringMode(strTmp, 0, 60);
                        oRelay.Chans[e.RowIndex].intONTime = Convert.ToInt32(HDLPF.IsNumStringMode(DgvChns[3, e.RowIndex].Value.ToString(), 0, 3600));
                    }
                }
            }
            catch
            {
            }
        }

        private void DgChns_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DgvChns.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void chbBoxList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isSet) return;
            if (oMHRCU != null)
            {
                oMHRCU.bytAryExclusion[e.Index] = Convert.ToByte(e.NewValue);
            }
            else if (oRelay != null)
            {
                oRelay.bytAryExclusion[e.Index] = Convert.ToByte(e.NewValue);
            }
        }

        private void DgvChns_SizeChanged(object sender, EventArgs e)
        {
            HDLSysPF.setDataGridViewColumnsWidth(DgvChns);
        }
    }
}
