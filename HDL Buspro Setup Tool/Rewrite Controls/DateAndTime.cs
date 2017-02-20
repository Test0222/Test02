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
    public partial class DateAndTime : UserControl
    {
        private byte SubNetID=0;
        private byte DevID = 0;
        private int MyDeviceType = 0;
        public DateAndTime(byte subnetid, byte devid, int devtype)
        {
            this.SubNetID = subnetid;
            this.DevID = devid;
            this.MyDeviceType = devtype;
            InitializeComponent();
            if (CsConst.iLanguageId == 1)
            {
                lbDate.Text = "日期:";
                lbTime.Text = "时间:";
                btnPC.Text = "PC时间";
                btnRead.Text = "读取";
                btnSave.Text = "保存";
            }
            if (CsConst.iLanguageId == 1) btnRead_Click(null, null);
        }

        private void btnPC_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            cbDate.Value = dt;
            cbDate_ValueChanged(null, null);
            Num1.Value = Convert.ToDecimal(dt.Hour);
            Num2.Value = Convert.ToDecimal(dt.Minute);
            Num3.Value = Convert.ToDecimal(dt.Second);
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (MyDeviceType == 4000 || MyDeviceType == 38 || MyDeviceType == 33 || MyDeviceType == 4001)
            {
                byte[] arayTmp = new byte[0];
                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC04E, SubNetID, DevID, false, true, true, false) == true)
                {
                    string strTmp = "";
                    for (int i = 0; i < 16; i++)
                    {
                        if (CsConst.myRevBuf[25 + i] != 0 && CsConst.myRevBuf[25 + i] != 69 && CsConst.myRevBuf[25 + i] != 13)
                            strTmp = strTmp + Convert.ToChar(CsConst.myRevBuf[25 + i]);
                    }
                    txt1.Text = strTmp;
                    byte Year = CsConst.myRevBuf[42];
                    byte Month = CsConst.myRevBuf[43];
                    byte Day = CsConst.myRevBuf[44];
                    if (Month > 12 || Day > 31)
                    {
                        Month = 12;
                        Day = 1;
                    }
                    DateTime dt = new DateTime(Year + 2000, Month, Day);
                    cbDate.Value = dt;
                    byte bytH = CsConst.myRevBuf[46];
                    byte bytM = CsConst.myRevBuf[47];
                    byte bytS = CsConst.myRevBuf[48];
                    if (bytH > 23) bytH = 0;
                    if (bytM > 59) bytM = 0;
                    if (bytS > 59) bytS = 0;
                    Num1.Value = Convert.ToDecimal(bytH);
                    Num2.Value = Convert.ToDecimal(bytM);
                    Num3.Value = Convert.ToDecimal(bytS);
                    cbDate_ValueChanged(null, null);
                    
                }

                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC072, SubNetID, DevID, false, true, true, false) == true)
                {
                    string strTmp = "";
                    for (int i = 0; i < 6; i++)
                    {
                        if (CsConst.myRevBuf[25 + i] != 0 && CsConst.myRevBuf[25 + i] != 69 && CsConst.myRevBuf[25 + i] != 13)
                            strTmp = strTmp + Convert.ToChar(CsConst.myRevBuf[25 + i]);
                        txt2.Text = strTmp;
                    }
                    
                }

                if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC090, SubNetID, DevID, false, true, true, false) == true)
                {
                    string strTmp = "";
                    for (int i = 0; i < 15; i++)
                    {
                        if (CsConst.myRevBuf[25 + i] > 57 || CsConst.myRevBuf[25 + i] < 48) 
                            strTmp = strTmp + "0";
                        else
                            strTmp = strTmp + Convert.ToChar(CsConst.myRevBuf[25 + i]);
                        lbIMEIValue.Text = strTmp;
                    }
                    
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            byte[] arayTmp = new byte[7];
            DateTime dt = cbDate.Value;
            arayTmp[0] = Convert.ToByte(dt.Year - 2000);
            arayTmp[1] = Convert.ToByte(dt.Month);
            arayTmp[2] = Convert.ToByte(dt.Day);
            arayTmp[3] = Convert.ToByte(dt.DayOfWeek.ToString("D"));
            arayTmp[4] = Convert.ToByte(Num1.Value);
            arayTmp[5] = Convert.ToByte(Num2.Value);
            arayTmp[6] = Convert.ToByte(Num3.Value);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC04C, SubNetID, DevID, false, true, true, false) == true)
            {
                
                HDLUDP.TimeBetwnNext(20);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            System.Threading.Thread.Sleep(100);
            arayTmp = new byte[16];
            string strTmp = txt1.Text;
            for (int i = 0; i < 16; i++)
                arayTmp[i] = 69;
            if (strTmp != "")
                for (int i = 0; i < strTmp.Length; i++)
                    arayTmp[i] = Convert.ToByte(strTmp[i]);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC040, SubNetID, DevID, false, true, true, false) == true)
            {
                
                HDLUDP.TimeBetwnNext(20);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            System.Threading.Thread.Sleep(100);
            arayTmp = new byte[6];
            strTmp = txt2.Text;
            for (int i = 0; i < 6; i++)
                arayTmp[i] = 69;
            if (strTmp != "")
                for (int i = 0; i < strTmp.Length; i++)
                    arayTmp[i] = Convert.ToByte(strTmp[i]);
            if (CsConst.mySends.AddBufToSndList(arayTmp, 0xC074, SubNetID, DevID, false, true, true, false) == true)
            {
                
                HDLUDP.TimeBetwnNext(20);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void cbDate_ValueChanged(object sender, EventArgs e)
        {
            lbWeek.Text = cbDate.Value.DayOfWeek.ToString();
            if (CsConst.iLanguageId == 1)
                lbWeek.Text=new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", }[Convert.ToInt16(DateTime.Now.DayOfWeek.ToString("D"))];
        }
    }
}
