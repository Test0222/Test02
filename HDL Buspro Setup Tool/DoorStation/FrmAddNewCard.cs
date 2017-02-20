using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDL_Buspro_Setup_Tool
{
    public partial class FrmAddNewCard : Form
    {
        private int DeviceType;
        private NewDS oNewDS;
        private DS oDS;
        private byte[] arayUID;
        private string strRemark;
        private byte SubnetID;
        private byte DeviceID;
        private int CaseNum;
        private int ID;
        private object MyActiveObj;
        public FrmAddNewCard()
        {
            InitializeComponent();
        }

        public FrmAddNewCard(byte[] arayuid,object obj, int devicetype,string strname,int casenum,int index)
        {
            InitializeComponent();
            string strDevName = strname.Split('\\')[0].ToString();
            strRemark = strname.Split('\\')[1].ToString();
            SubnetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            DeviceType = devicetype;
            MyActiveObj = obj;
            oDS = null;
            oNewDS = null;
            if (MyActiveObj is DS)
            {
                if (CsConst.myDS != null)
                {
                    foreach (DS oTmp in CsConst.myDS)
                    {
                        if (oTmp.DIndex == (MyActiveObj as DS).DIndex)
                        {
                            oDS = oTmp;
                            break;
                        }
                    }
                }
            }
            else if (MyActiveObj is NewDS)
            {
                if (CsConst.myNewDS != null)
                {
                    foreach (NewDS oTmp in CsConst.myNewDS)
                    {
                        if (oTmp.DIndex == (MyActiveObj as NewDS).DIndex)
                        {
                            oNewDS = oTmp;
                            break;
                        }
                    }
                }
            }
            
            lbSubValue.Text = SubnetID.ToString();
            lbDevValue.Text = DeviceID.ToString();
            lbRemarkValue.Text = strRemark;
            arayUID = arayuid;
            CaseNum = casenum;
            ID = index;
        }

        private void FrmAddNewCard_Load(object sender, EventArgs e)
        {
            btnModify.Visible = (CaseNum == 1);
            btnAdd.Visible = (CaseNum == 0);
            cbType.Items.Clear();
            for (int i = 0; i < 3; i++)
                cbType.Items.Add(CsConst.mstrINIDefault.IniReadValue("Public", "0037" + i.ToString(), ""));
            switch (CaseNum)
            {
                case 0:
                    AddCard();
                    break;
                case 1:
                    ModifyCard();
                    break;
            }
        }

        private void ModifyCard()
        {
            try
            {
                NewDS.CardInfo temp = null;
                if (oNewDS != null)
                    temp = oNewDS.MyCardInfo[ID];
                else if(oDS!=null)
                    temp = oDS.MyCardInfo[ID];
                lbUIDLV.Text = temp.UIDL.ToString();
                string strUID = "";
                for (int i = 0; i < temp.UIDL; i++)
                    strUID = strUID + GlobalClass.AddLeftZero(temp.UID[i].ToString("X"), 2) + " ";
                lbUIDDV.Text = strUID.Trim();
                cbType.SelectedIndex = temp.CardType - 1;
                txtBuilding.Text = temp.BuildingNO.ToString("X");
                txtUnit.Text = temp.UnitNO.ToString("X");
                txtRoom.Text = temp.RoomNO.ToString("X");
                txtName.Text = HDLPF.Byte2String(temp.arayName);
                txtPhone.Text = HDLPF.Byte2String(temp.arayPhone);
                txtRemark.Text = temp.Remark;
                if (temp.arayDate[3] > 23) temp.arayDate[3] = 23;
                if (temp.arayDate[4] > 59) temp.arayDate[4] = 59;
                numTime1.Value = Convert.ToDecimal(temp.arayDate[3]);
                numTime2.Value = Convert.ToDecimal(temp.arayDate[4]);
                if (temp.arayDate[1] > 12) temp.arayDate[1] = 12;
                if (temp.arayDate[2] > 31) temp.arayDate[2] = 31;
                DateTime d2 = new DateTime(temp.arayDate[0] + 2000, temp.arayDate[1], temp.arayDate[2]);
                TimePicker.Text = d2.ToString();
                TimePicker.Value = d2;
            }
            catch
            {
            }
        }

        private void AddCard()
        {
            try
            {
                lbUIDLV.Text = arayUID[0].ToString();
                string strUID = "";
                for (int i = 1; i < arayUID.Length; i++)
                    strUID = strUID + GlobalClass.AddLeftZero(arayUID[i].ToString("X"), 2) + " ";
                lbUIDDV.Text = strUID.Trim();
                txtBuilding.Text = "0";
                txtUnit.Text = "0";
                txtRoom.Text = "0";
                txtName.Text = "";
                txtPhone.Text = "";
                txtRemark.Text = "";
                DateTime d1;
                d1 = DateTime.Now;
                numTime1.Value = Convert.ToDecimal(d1.Hour);
                numTime2.Value = Convert.ToDecimal(d1.Minute);
                DateTime d2 = new DateTime(d1.Year + 100, d1.Month, d1.Day);
                TimePicker.Text = d2.ToString();
                TimePicker.Value = d2;
                cbType.SelectedIndex = 0;
            }
            catch
            {
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (CaseNum == 0) btnAdd_Click(null, null);
            else if (CaseNum == 1) btnModify_Click(null, null);
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                int Count = 0;
                if (oDS != null)
                    Count = oDS.MyCardInfo.Count;
                else if (oNewDS != null)
                    Count = oNewDS.MyCardInfo.Count;
                for (int i = 0; i < Count; i++)
                {
                    NewDS.CardInfo temp = null;
                    if (oNewDS != null)
                        temp = oNewDS.MyCardInfo[i];
                    else if (oDS != null)
                        temp = oDS.MyCardInfo[i];
                    if (temp != null)
                    {
                        if (temp.CardType == 0)
                        {
                            NewDS.CardInfo tmp = new NewDS.CardInfo();
                            string str1 = txtBuilding.Text;
                            string str2 = txtUnit.Text;
                            string str3 = txtRoom.Text;
                            tmp.CardNum = temp.CardNum;
                            tmp.CardType = Convert.ToByte(cbType.SelectedIndex + 1);
                            tmp.BuildingNO = Convert.ToInt32(str1, 16);
                            tmp.UnitNO = Convert.ToInt32(str2, 16);
                            tmp.RoomNO = Convert.ToInt32(str3, 16);
                            tmp.UIDL = Convert.ToByte(lbUIDLV.Text);
                            tmp.UID = new byte[10];
                            byte[] arayUIDTmp = GlobalClass.HexToByte(lbUIDDV.Text);
                            if (arayUIDTmp.Length > 10)
                            {
                                Array.Copy(arayUIDTmp, 0, tmp.UID, 0, 10);
                            }
                            else
                            {
                                Array.Copy(arayUIDTmp, 0, tmp.UID, 0, arayUIDTmp.Length);
                            }
                            tmp.arayDate = new byte[5];
                            tmp.arayDate[0] = Convert.ToByte(TimePicker.Value.Year - 2000);
                            tmp.arayDate[1] = Convert.ToByte(TimePicker.Value.Month);
                            tmp.arayDate[2] = Convert.ToByte(TimePicker.Value.Day);
                            tmp.arayDate[3] = Convert.ToByte(numTime1.Value);
                            tmp.arayDate[4] = Convert.ToByte(numTime2.Value);
                            tmp.arayPhone = new byte[11];
                            byte[] arayTmpRemark = HDLUDP.StringToByte(txtPhone.Text);
                            if (arayTmpRemark.Length > 11)
                            {
                                Array.Copy(arayTmpRemark, 0, tmp.arayPhone, 0, 11);
                            }
                            else
                            {
                                Array.Copy(arayTmpRemark, 0, tmp.arayPhone, 0, arayTmpRemark.Length);
                            }

                            tmp.arayName = new byte[10];
                            arayTmpRemark = HDLUDP.StringToByte(txtName.Text);
                            if (arayTmpRemark.Length > 10)
                            {
                                Array.Copy(arayTmpRemark, 0, tmp.arayName, 0, 10);
                            }
                            else
                            {
                                Array.Copy(arayTmpRemark, 0, tmp.arayName, 0, arayTmpRemark.Length);
                            }

                            tmp.Remark = txtRemark.Text.Trim();
                            byte[] arayTmp = new byte[64];
                            arayTmp[0] = tmp.UIDL;
                            Array.Copy(tmp.UID, 0, arayTmp, 1, tmp.UIDL);
                            arayTmp[11] = tmp.CardType;
                            arayTmp[12] = Convert.ToByte(tmp.CardNum / 256);
                            arayTmp[13] = Convert.ToByte(tmp.CardNum % 256);
                            arayTmp[14] = Convert.ToByte(tmp.BuildingNO / 256);
                            arayTmp[15] = Convert.ToByte(tmp.BuildingNO % 256);
                            arayTmp[16] = Convert.ToByte(tmp.UnitNO / 256);
                            arayTmp[17] = Convert.ToByte(tmp.UnitNO % 256);
                            arayTmp[18] = Convert.ToByte(tmp.RoomNO / 256);
                            arayTmp[19] = Convert.ToByte(tmp.RoomNO % 256);
                            Array.Copy(tmp.arayDate, 0, arayTmp, 20, tmp.arayDate.Length);
                            Array.Copy(tmp.arayName, 0, arayTmp, 25, tmp.arayName.Length);
                            Array.Copy(tmp.arayPhone, 0, arayTmp, 35, tmp.arayPhone.Length);

                            arayTmpRemark = HDLUDP.StringToByte(tmp.Remark);
                            if (arayTmpRemark.Length > 18)
                            {
                                Array.Copy(arayTmpRemark, 0, arayTmp, 46, 18);
                            }
                            else
                            {
                                Array.Copy(arayTmpRemark, 0, arayTmp, 46, arayTmpRemark.Length);
                            }

                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x352B, SubnetID, DeviceID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {

                                temp.CardType = tmp.CardType;
                            }
                            else
                            {
                                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99778", ""), ""
                                                       , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                return;
                            }
                            HDLUDP.TimeBetwnNext(20);
                            if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3518, SubnetID, DeviceID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                            {
                                temp.arayDate = tmp.arayDate;
                                temp.arayName = tmp.arayName;
                                temp.arayPhone = tmp.arayPhone;
                                temp.BuildingNO = tmp.BuildingNO;
                                temp.RoomNO = tmp.RoomNO;
                                temp.CardType = tmp.CardType;
                                temp.Remark = tmp.Remark;
                                temp.UID = tmp.UID;
                                temp.UIDL = tmp.UIDL;
                                temp.UnitNO = tmp.UnitNO;


                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99777", ""), ""
                                                       , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                return;
                            }
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtBuilding_KeyPress(object sender, KeyPressEventArgs e)
        {
            string str = "abcdefABCDEF1234567890";
            if (e.KeyChar != 8)
            {
                if (str.IndexOf(e.KeyChar.ToString()) < 0)
                {
                    e.Handled = true;
                }
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                NewDS.CardInfo tmp = new NewDS.CardInfo();
                string str1 = txtBuilding.Text;
                string str2 = txtUnit.Text;
                string str3 = txtRoom.Text;
                if (oDS != null)
                    tmp.CardNum = oDS.MyCardInfo[ID].CardNum;
                else if (oNewDS != null)
                    tmp.CardNum = oNewDS.MyCardInfo[ID].CardNum;
                tmp.CardType = Convert.ToByte(cbType.SelectedIndex + 1);
                tmp.BuildingNO = Convert.ToInt32(str1, 16);
                tmp.UnitNO = Convert.ToInt32(str2, 16);
                tmp.RoomNO = Convert.ToInt32(str3, 16);
                tmp.UIDL = Convert.ToByte(lbUIDLV.Text);
                tmp.UID = new byte[10];
                byte[] arayUIDTmp = GlobalClass.HexToByte(lbUIDDV.Text);
                if (arayUIDTmp.Length > 10)
                {
                    Array.Copy(arayUIDTmp, 0, tmp.UID, 0, 10);
                }
                else
                {
                    Array.Copy(arayUIDTmp, 0, tmp.UID, 0, arayUIDTmp.Length);
                }
                tmp.arayDate = new byte[5];
                tmp.arayDate[0] = Convert.ToByte(TimePicker.Value.Year - 2000);
                tmp.arayDate[1] = Convert.ToByte(TimePicker.Value.Month);
                tmp.arayDate[2] = Convert.ToByte(TimePicker.Value.Day);
                tmp.arayDate[3] = Convert.ToByte(numTime1.Value);
                tmp.arayDate[4] = Convert.ToByte(numTime2.Value);
                tmp.arayPhone = new byte[11];
                byte[] arayTmpRemark = HDLUDP.StringToByte(txtPhone.Text);
                if (arayTmpRemark.Length > 11)
                {
                    Array.Copy(arayTmpRemark, 0, tmp.arayPhone, 0, 11);
                }
                else
                {
                    Array.Copy(arayTmpRemark, 0, tmp.arayPhone, 0, arayTmpRemark.Length);
                }

                tmp.arayName = new byte[10];
                arayTmpRemark = HDLUDP.StringToByte(txtName.Text);
                if (arayTmpRemark.Length > 10)
                {
                    Array.Copy(arayTmpRemark, 0, tmp.arayName, 0, 10);
                }
                else
                {
                    Array.Copy(arayTmpRemark, 0, tmp.arayName, 0, arayTmpRemark.Length);
                }

                tmp.Remark = txtRemark.Text.Trim();
                byte[] arayTmp = new byte[64];
                arayTmp[0] = tmp.UIDL;
                Array.Copy(tmp.UID, 0, arayTmp, 1, tmp.UIDL);
                arayTmp[11] = tmp.CardType;
                arayTmp[12] = Convert.ToByte(tmp.CardNum / 256);
                arayTmp[13] = Convert.ToByte(tmp.CardNum % 256);
                arayTmp[14] = Convert.ToByte(tmp.BuildingNO / 256);
                arayTmp[15] = Convert.ToByte(tmp.BuildingNO % 256);
                arayTmp[16] = Convert.ToByte(tmp.UnitNO / 256);
                arayTmp[17] = Convert.ToByte(tmp.UnitNO % 256);
                arayTmp[18] = Convert.ToByte(tmp.RoomNO / 256);
                arayTmp[19] = Convert.ToByte(tmp.RoomNO % 256);
                Array.Copy(tmp.arayDate, 0, arayTmp, 20, tmp.arayDate.Length);
                Array.Copy(tmp.arayName, 0, arayTmp, 25, tmp.arayName.Length);
                Array.Copy(tmp.arayPhone, 0, arayTmp, 35, tmp.arayPhone.Length);

                arayTmpRemark = HDLUDP.StringToByte(tmp.Remark);
                if (arayTmpRemark.Length > 18)
                {
                    Array.Copy(arayTmpRemark, 0, arayTmp, 46, 18);
                }
                else
                {
                    Array.Copy(arayTmpRemark, 0, arayTmp, 46, arayTmpRemark.Length);
                }

                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3518, SubnetID, DeviceID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {
                    if (oNewDS != null)
                        oNewDS.MyCardInfo[ID] = tmp;
                    else if (oDS != null)
                        oDS.MyCardInfo[ID] = tmp;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(CsConst.mstrINIDefault.IniReadValue("Public", "99777", ""), ""
                                           , MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
            }
            catch
            {
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnPC_Click(object sender, EventArgs e)
        {
            DateTime d1;
            d1 = DateTime.Now;
            numTime1.Value = Convert.ToDecimal(d1.Hour);
            numTime2.Value = Convert.ToDecimal(d1.Minute);
            TimePicker.Text = d1.ToString();
        }
    }
}
