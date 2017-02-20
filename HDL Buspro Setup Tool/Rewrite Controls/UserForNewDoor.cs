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
    public partial class UserForNewDoor : UserControl
    {
        public int ID;
        private NewDS oNewDS;
        private int DeviceType;
        private int ShowType;
        public bool isSelect = false;
        private string StrName;
        private System.Windows.Forms.Panel Pnl;
        private byte SubNetID;
        private byte DeviceID;
        private int intTag;
        private Form Frm;
        public UserForNewDoor()
        {
            InitializeComponent();
        }

        public UserForNewDoor(NewDS newds, int id, int devicetype, int showtype, string strname, System.Windows.Forms.Panel pnl, int tag, Form frm)
        {
            InitializeComponent();
            ID = id;
            oNewDS = newds;
            DeviceType = devicetype;
            ShowType = showtype;
            StrName = strname;
            Pnl = pnl;
            string strDevName = strname.Split('\\')[0].ToString();
            SubNetID = Convert.ToByte(strDevName.Split('-')[0]);
            DeviceID = Convert.ToByte(strDevName.Split('-')[1]);
            intTag = tag;
            Frm = frm;
        }

        private void UserForNewDoor_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (oNewDS == null) return;
            if (oNewDS.MyCardInfo == null) return;
            picSelect.Visible = true;
            isSelect = true;
            byte[] arayTmp = new byte[0];
            FrmAddNewCard frmTmp = new FrmAddNewCard(arayTmp, oNewDS, DeviceType, StrName, 1, ID);
            frmTmp.ShowDialog();
            int Width = Pnl.Width;
            int Heigh = Pnl.Height;

            Pnl.Controls.Clear();
            int WCount = Width / 150;
            int num = 0;
            for (int i = 0; i < oNewDS.MyCardInfo.Count; i++)
            {
                NewDS.CardInfo temp = oNewDS.MyCardInfo[i];
                if (temp.CardType == Convert.ToByte(intTag))
                {
                    UserForNewDoor tmp = new UserForNewDoor(oNewDS, i, DeviceType, ShowType, StrName, Pnl, intTag, Frm);
                    tmp.Name = "Card" + num.ToString();
                    tmp.Left = (num % WCount) * 140 + 10;
                    tmp.Top = (num / WCount) * 120 + 10;
                    Pnl.Controls.Add(tmp);
                    num = num + 1;
                }
            }
            if (Frm is FrmNewDS)
                (Frm as FrmNewDS).SetCount();
            else if (Frm is frmDS)
                (Frm as frmDS).SetCount();
        }


        private void UserForNewDoor_Load(object sender, EventArgs e)
        {
            isSelect = false;
            picSelect.Visible = false;
            LoadUser(ShowType);
        }

        public void LoadUser(int Type)
        {
            NewDS.CardInfo temp = oNewDS.MyCardInfo[ID];
            switch (Type)
            {
                case 0: lb.Text = temp.BuildingNO.ToString("X"); break;
                case 1: lb.Text = temp.UnitNO.ToString("X"); break;
                case 2: lb.Text = temp.RoomNO.ToString("X"); break;
                case 3: lb.Text = (temp.arayDate[0] + 2000).ToString() + "/" + temp.arayDate[1].ToString() + "/" + temp.arayDate[2].ToString() + "  " +
                        temp.arayDate[3].ToString() + ":" + temp.arayDate[4].ToString(); break;
                case 4: lb.Text = HDLPF.Byte2String(temp.arayName); break;
                case 5: lb.Text = HDLPF.Byte2String(temp.arayPhone); break;
                case 6: lb.Text = temp.Remark; break;
            }
        }

        private void SetSelect()
        {
            isSelect = false;
            picSelect.Visible = false;
        }

        public void ModifyInterFace()
        {
            picSelect.Visible = true;
            isSelect = true;
            byte[] arayTmp = new byte[0];
            FrmAddNewCard frmTmp = new FrmAddNewCard(arayTmp, oNewDS, DeviceType, StrName, 1, ID);
            frmTmp.ShowDialog();
        }

        public bool DeleteCard()
        {
            bool result = true;
            try
            {
                NewDS.CardInfo temp = oNewDS.MyCardInfo[ID];
                temp.CardType = 0;
                temp.BuildingNO = 0;
                temp.UnitNO = 0;
                temp.RoomNO = 0;
                temp.UIDL = 0;
                temp.UID = new byte[10];
                temp.arayDate = new byte[5];
                temp.arayPhone = new byte[11];
                temp.arayName = new byte[10];
                temp.Remark = "";
                byte[] arayTmp = new byte[64];
                arayTmp[0] = temp.UIDL;
                Array.Copy(temp.UID, 0, arayTmp, 1, temp.UIDL);
                arayTmp[11] = temp.CardType;
                arayTmp[12] = Convert.ToByte(temp.CardNum / 256);
                arayTmp[13] = Convert.ToByte(temp.CardNum % 256);
                arayTmp[14] = Convert.ToByte(temp.BuildingNO / 256);
                arayTmp[15] = Convert.ToByte(temp.BuildingNO % 256);
                arayTmp[16] = Convert.ToByte(temp.UnitNO / 256);
                arayTmp[17] = Convert.ToByte(temp.UnitNO % 256);
                arayTmp[18] = Convert.ToByte(temp.RoomNO / 256);
                arayTmp[19] = Convert.ToByte(temp.RoomNO % 256);
                Array.Copy(temp.arayDate, 0, arayTmp, 20, temp.arayDate.Length);
                Array.Copy(temp.arayName, 0, arayTmp, 25, temp.arayName.Length);
                Array.Copy(temp.arayPhone, 0, arayTmp, 35, temp.arayPhone.Length);

                byte[] arayTmpRemark = HDLUDP.StringToByte(temp.Remark);
                byte[] ArayMain = new byte[18];
                if (arayTmpRemark.Length > 18)
                {
                    Array.Copy(arayTmpRemark, 0, ArayMain, 0, 18);
                }
                else
                {
                    Array.Copy(arayTmpRemark, 0, ArayMain, 0, arayTmpRemark.Length);
                }
                Array.Copy(ArayMain, 0, arayTmp, 46, ArayMain.Length);
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0x3518, SubNetID, DeviceID, false, false, true, CsConst.minAllWirelessDeviceType.Contains(DeviceType)) == true)
                {

                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        private void UserForNewDoor_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < Pnl.Controls.Count; i++)
            {
                UserForNewDoor temp = (UserForNewDoor)Pnl.Controls[i];
                temp.SetSelect();
            }
            picSelect.Visible = true;
            isSelect = true;
        }

        private void PIC_MouseHover(object sender, EventArgs e)
        {
            string str = "";
            NewDS.CardInfo temp = oNewDS.MyCardInfo[ID];
            str = CsConst.mstrINIDefault.IniReadValue("Public", "00360", "") + ":" + GlobalClass.AddLeftZero(temp.BuildingNO.ToString("X"), 2) + "\r\n" +
                  CsConst.mstrINIDefault.IniReadValue("Public", "00361", "") + ":" + GlobalClass.AddLeftZero(temp.UnitNO.ToString("X"), 2) + "\r\n" +
                  CsConst.mstrINIDefault.IniReadValue("Public", "00362", "") + ":" + GlobalClass.AddLeftZero(temp.RoomNO.ToString("X"), 2) + "\r\n" +
                  CsConst.mstrINIDefault.IniReadValue("Public", "00363", "") + ":" + (temp.arayDate[0] + 2000).ToString() + "/" + temp.arayDate[1].ToString() + "/" + temp.arayDate[2].ToString() + "  " + temp.arayDate[3].ToString() + ":" + temp.arayDate[4].ToString() + "\r\n" +
                  CsConst.mstrINIDefault.IniReadValue("Public", "00364", "") + ":" + HDLPF.Byte2String(temp.arayName) + "\r\n" +
                  CsConst.mstrINIDefault.IniReadValue("Public", "00365", "") + ":" + HDLPF.Byte2String(temp.arayPhone) + "\r\n" +
                  CsConst.mstrINIDefault.IniReadValue("Public", "00366", "") + ":" + temp.Remark.ToString();
            toolTip1.Show(str, PIC);
        }

        private void PIC_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Show("", PIC);
        }
    }
}

