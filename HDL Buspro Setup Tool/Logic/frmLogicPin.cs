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
    public partial class frmLogicPin : Form
    {
        private List<Byte[]> publicPinTempltesArray = null;
        public frmLogicPin()
        {
            InitializeComponent();
        }

        public frmLogicPin(List<Byte[]> LogicPinByteArray)
        {
            InitializeComponent();
            publicPinTempltesArray = LogicPinByteArray;

            HDLSysPF.DisplayLogicTemplates(LogicPinByteArray, lvSensors,true);
        }

        void InitialFormCtrlsTextOrItems()
        {
            cboW1.Items.Clear();
            cboW2.Items.Clear();
            cboW21.Items.Clear();
            cboW1.Items.AddRange(CsConst.Weekdays);
            cboW2.Items.AddRange(CsConst.Weekdays);
            cboW21.Items.AddRange(CsConst.Weekdays);

            cboUV1.Items.Clear();
            cboUV1.Items.AddRange(CsConst.Status);
        }

        private void frmLogicPin_Load(object sender, EventArgs e)
        {
            InitialFormCtrlsTextOrItems();
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tp1.Visible = (cboType.SelectedIndex == 0);
            tp2.Visible = (cboType.SelectedIndex == 1);
            tp3.Visible = (cboType.SelectedIndex == 2);
            tp4.Visible = (cboType.SelectedIndex == 3);
            tp5.Visible = (cboType.SelectedIndex == 4);
            tp6.Visible = (cboType.SelectedIndex >= 5);
            chbAuto.Visible = (tp5.Visible || tp6.Visible);

            if (tp6.Visible)
            {
                label5.Visible = (cboType.SelectedIndex + 2 !=13);
                cboDev2.Visible = label5.Visible;
                switch (cboType.SelectedIndex + 2)
                {
                    
                    case 7: //外部输出值 温度范围
                        #region
                        cboDev3.Visible = true;
                        cboDev31.Visible = true;
                        cboDev3.Items.Clear();
                        cboDev31.Items.Clear();
                        for (int i = -127; i <= 127; i++)
                        {
                            cboDev3.Items.Add(i.ToString());
                            cboDev31.Items.Add(i.ToString());
                        }
                        #endregion
                        break;
                    case 8: //场景序列
                    case 9:
                        cboDev31.Visible = false;
                        cboDev3.Items.Clear();
                        for (int i = 0; i < 49; i++) cboDev3.Items.Add(i.ToString());
                        break;
                    case 10: //通用开关
                        cboDev31.Visible = false;
                        cboDev3.Items.Clear();
                        cboDev3.Items.AddRange(CsConst.Status);
                        break;
                    case 11: //回路亮度值
                        cboDev31.Visible = false;
                        cboDev3.Items.Clear();
                        for (int i = 0; i < 101; i++) cboDev3.Items.Add(i.ToString());
                        cboDev3.Items.Add(">0");
                        break;
                    case 12: //窗帘状态
                        cboDev31.Visible = false;
                        cboDev3.Items.Clear();
                        cboDev3.Items.AddRange(CsConst.CurtainModes.ToArray());
                        for (int i = 1; i <= 100; i++) cboDev3.Items.Add(i.ToString() + " %");
                        break;
                    case 13: //面板控制类型
                        cboDev31.Visible = false;
                        cboDev3.Items.Clear();
                        cboDev3.Items.AddRange(new string[] { "Invalid", "IR function", "Lock key of panel", "AC Power", "Cooling Temp", "FAN Speed", "AC Mode", 
                                                              "AC Heating Temp", "AC Auto Temp", "AC Rise Temp", "AC Decrese Temp", "LCD Backlight Status", "Lock AC", 
                                                              "Backlight", "Status Lights" });
                        break;
                    case 14: //安防命令
                        cboDev31.Visible = false;
                        cboDev3.Items.Clear();
                        cboDev3.Items.AddRange(CsConst.security.ToArray());
                        break;
                }
            }

            HDLSysPF.DisplayDevicesListAccordingly((byte)(cboType.SelectedIndex + 2), cboDev1);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (cboType.SelectedIndex == -1) return;
            if (lvSensors.SelectedItems.Count == 0) return;

           
        }

        private void rbY1_CheckedChanged(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((RadioButton)sender).Tag.ToString());
            if (((RadioButton)sender).Checked == true)
            {
                numY1.Enabled = (bytTag == 0);
                numY2.Enabled = (bytTag == 1);
                numY3.Enabled = (bytTag == 2);
                numY31.Enabled = (bytTag == 2);

                numY4.Enabled = (bytTag == 3);
                numY41.Enabled = (bytTag == 3);
            }
            
        }

        private void rbD1_CheckedChanged(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((RadioButton)sender).Tag.ToString());
            if (((RadioButton)sender).Checked == true)
            {
                dateMD1.Enabled = (bytTag == 0);
                dateMD2.Enabled = (bytTag == 1);
                dateMD21.Enabled = (bytTag == 1);
                dateMD3.Enabled = (bytTag == 2);
                dateMD31.Enabled = (bytTag == 2);
            }
        }

        private void rbT1_CheckedChanged(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((RadioButton)sender).Tag.ToString());
            if (((RadioButton)sender).Checked == true)
            {
                numT1.Enabled = (bytTag == 0);
                numT2.Enabled = (bytTag == 1);
                numT11.Enabled = (bytTag == 0);
                numT21.Enabled = (bytTag == 1);
            }
        }

        private void lvSensors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSensors.Items.Count == 0) return;
            if (lvSensors.SelectedItems == null || lvSensors.SelectedItems.Count == 0) return;
            int intPinID = lvSensors.SelectedItems[0].Index;
            panel2.Enabled = true;
            Byte[] TmpPin = publicPinTempltesArray[intPinID];

            DisplayToFormAccordinglyID(TmpPin);
        }

        void DisplayToFormAccordinglyID(Byte[] oTmpLogicPins)
        {
            if (oTmpLogicPins == null) return;

            Byte bytStart = 0;
            if (oTmpLogicPins[bytStart] > 14 || oTmpLogicPins[bytStart] == 0) oTmpLogicPins[bytStart] = 15;
            cboType.SelectedIndex = oTmpLogicPins[bytStart] - 2;
            cboType_SelectedIndexChanged(cboType, null);

            switch (oTmpLogicPins[bytStart])
            {
                case 2:  //年类型
                    #region
                    if (oTmpLogicPins[bytStart + 1] == 1) //指定年
                    {
                        rbY1.Checked = true;
                        numY1.Value = 2000 + oTmpLogicPins[bytStart + 2];
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 2) //指定某天某年
                    {
                        rbY2.Checked = true;
                        numY2.Text = (2000 + oTmpLogicPins[bytStart + 2]).ToString() + "/"
                                    + oTmpLogicPins[bytStart + 3].ToString() + "/"
                                    + oTmpLogicPins[bytStart + 4].ToString();
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 3) //指定哪年到哪年
                    {
                        rbY3.Checked = true;
                        numY3.Value = 2000 + oTmpLogicPins[bytStart + 2];
                        numY31.Value =2000 + oTmpLogicPins[bytStart + 3];
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 4) //指定某天某年
                    {
                        rbY4.Checked = true;
                        numY4.Text = (2000 + oTmpLogicPins[bytStart + 2]).ToString() + "/"
                                                    + oTmpLogicPins[bytStart + 3].ToString() + "/"
                                                    + oTmpLogicPins[bytStart + 4].ToString();

                        numY41.Text = (2000 + oTmpLogicPins[bytStart + 5]).ToString() + "/"
                                                    + oTmpLogicPins[bytStart + 6].ToString() + "/"
                                                    + oTmpLogicPins[bytStart + 7].ToString();
                    }
                    break;
                    #endregion

                case 3: //日期类型
                    #region
                    if (oTmpLogicPins[bytStart + 1] == 1) //指定哪天
                    {
                        rbD1.Checked = true;
                        dateMD1.Text =  (oTmpLogicPins[bytStart + 2]).ToString() + "/"
                                            + oTmpLogicPins[bytStart + 3].ToString();
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 2) //从什么时候到什么时候
                    {
                        rbD2.Checked = true;
                        dateMD2.Text = (oTmpLogicPins[bytStart + 2]).ToString() + "/"
                                    + oTmpLogicPins[bytStart + 3].ToString();

                        dateMD21.Text = oTmpLogicPins[bytStart + 4].ToString() + "/"
                                        + oTmpLogicPins[bytStart + 5].ToString();
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 3) //每月
                    {
                        rbD3.Checked = true;
                        dateMD3.Text = (oTmpLogicPins[bytStart + 2]).ToString() + "/"
                                    + oTmpLogicPins[bytStart + 3].ToString();

                        dateMD31.Text = oTmpLogicPins[bytStart + 4].ToString() + "/"
                                        + oTmpLogicPins[bytStart + 5].ToString();

                    }
                    break;
                    #endregion

                case 4: //星期类型
                    #region
                    if (oTmpLogicPins[bytStart + 1] == 1) //指定星期几
                    {
                        RW1.Checked = true;
                        cboW1.SelectedIndex = oTmpLogicPins[bytStart + 2];
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 2) //星期几到几
                    {
                        RW2.Checked = true;
                        cboW2.SelectedIndex = oTmpLogicPins[bytStart + 2];
                        cboW21.SelectedIndex = oTmpLogicPins[bytStart + 3];
                    }
                    break;
                    #endregion

                case 5: //时间类型
                    #region
                    if (oTmpLogicPins[bytStart + 1] == 1) //指定时间
                    {
                        rbT1.Checked = true;
                        if (oTmpLogicPins[bytStart + 2] == 1) // 时间点
                        {
                            numT1.SelectedIndex = 0;
                            numT11.Text = oTmpLogicPins[bytStart + 3].ToString() + ":"
                                            + oTmpLogicPins[bytStart + 4].ToString();
                        }
                        else if (oTmpLogicPins[bytStart + 2] == 2) // 日出
                        {
                            if (oTmpLogicPins[bytStart + 3] >= 0x80) //之后
                            {
                                numT1.SelectedIndex = 4;
                                numT11.Text = ((oTmpLogicPins[bytStart + 3] - 0x80) * 60  
                                                + oTmpLogicPins[bytStart + 4]).ToString();
                            }
                            else if (oTmpLogicPins[bytStart + 3] >= 0x40) //之前
                            {
                                numT1.SelectedIndex = 3;
                                numT11.Text = ((oTmpLogicPins[bytStart + 3] - 0x40) * 60 
                                                        + oTmpLogicPins[bytStart + 4]).ToString();
                            }
                            else numT1.SelectedIndex = 1;
                        }
                        else if (oTmpLogicPins[bytStart + 2] == 3) // 日落
                        {
                            if (oTmpLogicPins[bytStart + 3] >= 0x80) //之后
                            {
                                numT1.SelectedIndex = 6;
                                numT11.Text = ((oTmpLogicPins[bytStart + 3] - 0x80) * 60
                                                        + oTmpLogicPins[bytStart + 4]).ToString() ;
                            }
                            else if (oTmpLogicPins[bytStart + 3] >= 0x40) //之后
                            {
                                numT1.SelectedIndex = 5;
                                numT11.Text = ((oTmpLogicPins[bytStart + 3] - 0x40) * 60
                                                        + oTmpLogicPins[bytStart + 4]).ToString();
                            }
                            else numT1.SelectedIndex = 2;
                        }
                    }
                    else if (oTmpLogicPins[bytStart + 1] == 2) //从什么时候到什么时候
                    {
                        rbT2.Checked = true;
                        numT2.Text = (oTmpLogicPins[bytStart + 3] * 60
                                        + oTmpLogicPins[bytStart + 4]).ToString();

                        numT21.Text = (oTmpLogicPins[bytStart + 6] * 60
                                        + oTmpLogicPins[bytStart + 7]).ToString();
                    }
                    break;
                    #endregion
                case 6:  //自己的通用开关
                    #region
                    numUV1.Value = oTmpLogicPins[bytStart + 3];
                    cboUV1.SelectedIndex = oTmpLogicPins[bytStart + 4] / 255; break;
                    #endregion
                case 7:
                    #region
                    cboDev1.Text = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString();
                    cboDev2.Value = oTmpLogicPins[bytStart + 3];

                    if (oTmpLogicPins[bytStart + 4] > 0x80) cboDev3.Text = "-" + (oTmpLogicPins[bytStart + 4] - 0x80).ToString();
                    else if (oTmpLogicPins[bytStart + 4] == 0) cboDev3.Text = "0";
                    else cboDev3.Text = oTmpLogicPins[bytStart + 4].ToString();

                    if (oTmpLogicPins[bytStart + 5] > 0x80) cboDev31.Text = "-" + (oTmpLogicPins[bytStart + 5] - 0x80).ToString();
                    else if (oTmpLogicPins[bytStart + 5] == 0) cboDev31.Text = "0";
                    else cboDev31.Text = oTmpLogicPins[bytStart + 5].ToString(); break;
                    #endregion

                case 8://场景序列
                case 9:
                    #region
                    cboDev1.Text = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString();
                    cboDev2.Value = oTmpLogicPins[bytStart + 3];
                    cboDev3.Text = oTmpLogicPins[bytStart + 4].ToString();
                    break;
                    #endregion

                case 10:  //外部通用开关
                    #region
                    cboDev1.Text = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString();
                    cboDev2.Value = oTmpLogicPins[bytStart + 3];
                    cboDev3.SelectedIndex = oTmpLogicPins[bytStart + 4] / 255;
                    #endregion
                    break;

                case 11:  //回路状态
                    #region
                    cboDev1.Text = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString();
                    cboDev2.Value = oTmpLogicPins[bytStart + 3];
                    if (oTmpLogicPins[bytStart + 4] == 255) cboDev3.SelectedIndex = 101;
                    else cboDev3.SelectedIndex = oTmpLogicPins[bytStart + 4];
                    #endregion
                    break;

                case 12:  //窗帘状态
                    #region
                    cboDev1.Text = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString();
                    cboDev2.Value = oTmpLogicPins[bytStart + 3];
                    cboDev3.SelectedIndex = oTmpLogicPins[bytStart + 4];
                    #endregion
                    break;

                case 13: //面板控制指令
                    #region
                    cboDev1.Text = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString();
                    cboDev3.SelectedIndex = oTmpLogicPins[bytStart + 3];
                    cboDev3_SelectedIndexChanged(cboDev3, null);
                    if (oTmpLogicPins[bytStart + 4] > cboDev31.Items.Count) oTmpLogicPins[bytStart + 4] = 0;
                    cboDev31.SelectedIndex = oTmpLogicPins[bytStart + 4];
                    #endregion
                    break;
                case 14:
                    #region
                    cboDev1.Text = oTmpLogicPins[bytStart + 1].ToString() + "-" + oTmpLogicPins[bytStart + 2].ToString();
                    cboDev2.Value = oTmpLogicPins[bytStart + 3];
                    cboDev3.SelectedIndex = oTmpLogicPins[bytStart + 4] - 1;
                    #endregion
                    break;

            }
        }

        void UpdateBufferFormAccordinglyID(ref Byte[] oTmpLogicPins)
        {
            if (oTmpLogicPins == null) oTmpLogicPins = new byte[8];

            byte bytStart = 0;
            oTmpLogicPins[bytStart] = (byte)(cboType.SelectedIndex + 2);
                    
            switch (oTmpLogicPins[bytStart])
            {
                case 2:  //年类型
                    #region
                    if (rbY1.Checked) //指定年
                    {
                        oTmpLogicPins[bytStart + 1] = 1;
                        oTmpLogicPins[bytStart + 2] = (byte)(numY1.Value - 2000 );
                    }
                    else if ( rbY2.Checked == true) //指定某天某年
                    {
                        oTmpLogicPins[bytStart + 1] = 2;
                        oTmpLogicPins[bytStart + 2] = (byte)(numY2.Value.Year - 2000);
                        oTmpLogicPins[bytStart + 3] = (byte)(numY2.Value.Month);
                        oTmpLogicPins[bytStart + 4] = (byte)(numY2.Value.Day);
                    }
                    else if (rbY3.Checked == true) //指定哪年到哪年
                    {
                        oTmpLogicPins[bytStart + 1] = 3;
                        oTmpLogicPins[bytStart + 2] = (byte)(numY3.Value - 2000);
                        oTmpLogicPins[bytStart + 3] = (byte)(numY31.Value - 2000);
                    }
                    else if (rbY4.Checked == true) //指定某天某年
                    {
                        oTmpLogicPins[bytStart + 1] = 4;
                        oTmpLogicPins[bytStart + 2] = (byte)(numY4.Value.Year - 2000);
                        oTmpLogicPins[bytStart + 3] = (byte)(numY4.Value.Month);
                        oTmpLogicPins[bytStart + 4] = (byte)(numY4.Value.Day);

                        oTmpLogicPins[bytStart + 5] = (byte)(numY41.Value.Year - 2000);
                        oTmpLogicPins[bytStart + 6] = (byte)(numY41.Value.Month);
                        oTmpLogicPins[bytStart + 7] = (byte)(numY41.Value.Day);
                    }
                    break;
                    #endregion

                case 3: //日期类型
                    #region
                    if (rbD1.Checked == true) //指定哪天
                    {
                        oTmpLogicPins[bytStart + 1] = 1;
                        oTmpLogicPins[bytStart + 2] =Convert.ToByte(dateMD1.Text.Split('/')[0].ToString());
                        oTmpLogicPins[bytStart + 3] = Convert.ToByte(dateMD1.Text.Split('/')[1].ToString());
                    }
                    else if (rbD2.Checked == true) //从什么时候到什么时候
                    {
                        oTmpLogicPins[bytStart + 1] = 2;
                        oTmpLogicPins[bytStart + 2] = Convert.ToByte(dateMD2.Text.Split('/')[0].ToString());
                        oTmpLogicPins[bytStart + 3] = Convert.ToByte(dateMD2.Text.Split('/')[1].ToString());

                        oTmpLogicPins[bytStart + 4] = Convert.ToByte(dateMD21.Text.Split('/')[0].ToString());
                        oTmpLogicPins[bytStart + 5] = Convert.ToByte(dateMD21.Text.Split('/')[1].ToString());
                    }
                    else if (rbD3.Checked == true) //每月
                    {
                        oTmpLogicPins[bytStart + 1] = 3;
                        oTmpLogicPins[bytStart + 2] = Convert.ToByte(dateMD3.Text.Split('/')[0].ToString());
                        oTmpLogicPins[bytStart + 3] = Convert.ToByte(dateMD3.Text.Split('/')[1].ToString());

                        oTmpLogicPins[bytStart + 4] = Convert.ToByte(dateMD31.Text.Split('/')[0].ToString());
                        oTmpLogicPins[bytStart + 5] = Convert.ToByte(dateMD31.Text.Split('/')[1].ToString());
                    }
                    break;
                    #endregion

                case 4: //星期类型
                    #region
                    if (RW1.Checked == true)  //指定星期几
                    {
                        oTmpLogicPins[bytStart + 1] = 1;
                        oTmpLogicPins[bytStart + 2] = (byte)cboW1.SelectedIndex;
                    }
                    else if (RW2.Checked == true)//星期几到几
                    {
                        oTmpLogicPins[bytStart + 1] = 2; 
                        oTmpLogicPins[bytStart + 2] = (byte) cboW2.SelectedIndex;
                        oTmpLogicPins[bytStart + 3] = (byte) cboW21.SelectedIndex;
                    }
                    break;
                    #endregion

                case 5: //时间类型
                    #region
                    if (rbT1.Checked == true) //指定时间
                    {
                        oTmpLogicPins[bytStart + 1] = 1; 
                        if ( numT1.SelectedIndex == 0) // 时间点
                        {
                            oTmpLogicPins[bytStart + 2] = 1;
                            oTmpLogicPins[bytStart + 3] = (byte)(Convert.ToInt32(numT11.Text.ToString()) / 60);
                            oTmpLogicPins[bytStart + 4] = (byte)(Convert.ToInt32(numT11.Text.ToString()) % 60); 
                        }
                        else if (numT1.SelectedIndex == 1) // 日出
                        {
                            oTmpLogicPins[bytStart + 2] = 2;
                        }
                        else if (numT1.SelectedIndex == 2) // 日落
                        {
                            oTmpLogicPins[bytStart + 2] = 3;
                        }
                        else if (numT1.SelectedIndex == 3) //日出之前
                        {
                            oTmpLogicPins[bytStart + 2] = 2;
                            oTmpLogicPins[bytStart + 3] = (byte)(Convert.ToInt32(numT11.Text.ToString()) / 60 + 0x40);
                            oTmpLogicPins[bytStart + 4]=(byte)(Convert.ToInt32(numT11.Text.ToString()) % 60);
                        }
                        else if (numT1.SelectedIndex == 4) //日出之后
                        {
                            oTmpLogicPins[bytStart + 2] = 2;
                            oTmpLogicPins[bytStart + 3] = (byte)(Convert.ToInt32(numT11.Text.ToString()) / 60 + 0x80);
                            oTmpLogicPins[bytStart + 4] = (byte)(Convert.ToInt32(numT11.Text.ToString()) % 60);
                        }
                        else if (numT1.SelectedIndex == 5) // 日落之前
                        {
                            oTmpLogicPins[bytStart + 2] = 3;
                            oTmpLogicPins[bytStart + 3] = (byte)(Convert.ToInt32(numT11.Text.ToString()) / 60 + 0x40);
                            oTmpLogicPins[bytStart + 4] = (byte)(Convert.ToInt32(numT11.Text.ToString()) % 60);
                        }
                        else if (numT1.SelectedIndex == 6) // 日落之后
                        {
                            oTmpLogicPins[bytStart + 2] = 3;
                            oTmpLogicPins[bytStart + 3] = (byte)(Convert.ToInt32(numT11.Text.ToString()) / 60 + 0x80);
                            oTmpLogicPins[bytStart + 4] = (byte)(Convert.ToInt32(numT11.Text.ToString()) % 60);
                        } 
                    }
                    else if (rbT2.Checked == true)//从什么时候到什么时候
                    {
                        oTmpLogicPins[bytStart + 1] = 2;
                        oTmpLogicPins[bytStart + 2] = 1;
                        oTmpLogicPins[bytStart + 3] = (byte)(Convert.ToInt32(numT2.Text.ToString()) / 60);
                        oTmpLogicPins[bytStart + 4] = (byte)(Convert.ToInt32(numT2.Text.ToString()) % 60);
                        oTmpLogicPins[bytStart + 5] = 1;
                        oTmpLogicPins[bytStart + 6] = (byte)(Convert.ToInt32(numT21.Text.ToString()) / 60);
                        oTmpLogicPins[bytStart + 7] = (byte)(Convert.ToInt32(numT21.Text.ToString()) % 60);
                    }
                    break;
                    #endregion
                case 6:  //通用开关
                    #region
                    oTmpLogicPins[bytStart + 3] = (byte)(numUV1.Value);
                    oTmpLogicPins[bytStart + 4]  =(byte)(cboUV1.SelectedIndex  * 255); break;
                    #endregion

                case 7: // 温度范围
                    #region
                    if (cboDev1.Text != null) 
                    {
                        DeviceInfo oTmp = new DeviceInfo(cboDev1.Text);
                        oTmpLogicPins[bytStart + 1] = oTmp.SubnetID;
                        oTmpLogicPins[bytStart + 2] = oTmp.DeviceID;
                        oTmpLogicPins[bytStart + 3] = (byte)cboDev2.Value;

                        if (cboDev3.SelectedIndex < 128) oTmpLogicPins[bytStart + 4] = (byte)(0x80 + Math.Abs(Convert.ToDecimal(cboDev3.Text)));
                        else if (cboDev3.SelectedIndex > 128) oTmpLogicPins[bytStart + 4] = (byte)Math.Abs(Convert.ToDecimal(cboDev3.Text));

                        if (cboDev31.SelectedIndex < 128) oTmpLogicPins[bytStart + 5] = (byte)(0x80 + Math.Abs(Convert.ToDecimal(cboDev31.Text)));
                        else if (cboDev31.SelectedIndex > 128) oTmpLogicPins[bytStart + 5] = (byte)Math.Abs(Convert.ToDecimal(cboDev31.Text));
                    }
                    break;
                    #endregion

                case 8: //场景序列
                case 9:
                    #region
                    if (cboDev1.Text != null)
                    {
                        DeviceInfo oTmp = new DeviceInfo(cboDev1.Text);
                        oTmpLogicPins[bytStart + 1] = oTmp.SubnetID;
                        oTmpLogicPins[bytStart + 2] = oTmp.DeviceID;
                        oTmpLogicPins[bytStart + 3] = (byte)cboDev2.Value;
                        oTmpLogicPins[bytStart + 4] = Convert.ToByte(cboDev3.Text);
                    }
                    break;
                    #endregion
                case 10:  // 外部通用开关
                    #region
                    if (cboDev1.Text != null)
                    {
                        DeviceInfo oTmp = new DeviceInfo(cboDev1.Text);
                        oTmpLogicPins[bytStart + 1] = oTmp.SubnetID;
                        oTmpLogicPins[bytStart + 2] = oTmp.DeviceID;
                        oTmpLogicPins[bytStart + 3] = (byte)cboDev2.Value;
                        oTmpLogicPins[bytStart + 4] = (byte)(cboDev3.SelectedIndex * 255);
                    }
                    #endregion
                    break;

                case 11:  //回路亮度
                    #region
                    if (cboDev1.Text != null)
                    {
                        DeviceInfo oTmp = new DeviceInfo(cboDev1.Text);
                        oTmpLogicPins[bytStart + 1] = oTmp.SubnetID;
                        oTmpLogicPins[bytStart + 2] = oTmp.DeviceID;
                        oTmpLogicPins[bytStart + 3] = (byte)cboDev2.Value;
                        if (cboDev3.SelectedIndex == 101) oTmpLogicPins[bytStart + 4] = 255;
                        else oTmpLogicPins[bytStart + 4] = (byte)(cboDev3.SelectedIndex);
                    }
                    #endregion
                    break;
                case 12: //窗帘进度
                    #region
                    if (cboDev1.Text != null)
                    {
                        DeviceInfo oTmp = new DeviceInfo(cboDev1.Text);
                        oTmpLogicPins[bytStart + 1] = oTmp.SubnetID;
                        oTmpLogicPins[bytStart + 2] = oTmp.DeviceID;
                        oTmpLogicPins[bytStart + 3] = (byte)cboDev2.Value;
                        oTmpLogicPins[bytStart + 4] = (byte)(cboDev3.SelectedIndex);
                    }
                    #endregion
                    break;
                case 13:
                    #region
                    if (cboDev1.Text != null)
                    {
                        DeviceInfo oTmp = new DeviceInfo(cboDev1.Text);
                        oTmpLogicPins[bytStart + 1] = oTmp.SubnetID;
                        oTmpLogicPins[bytStart + 2] = oTmp.DeviceID;
                        oTmpLogicPins[bytStart + 3] = (byte)cboDev3.SelectedIndex;
                        oTmpLogicPins[bytStart + 4] = (byte)(cboDev31.SelectedIndex);
                    }
                    #endregion
                    break;

                case 14:
                    #region
                    if (cboDev1.Text != null)
                    {
                        DeviceInfo oTmp = new DeviceInfo(cboDev1.Text);
                        oTmpLogicPins[bytStart + 1] = oTmp.SubnetID;
                        oTmpLogicPins[bytStart + 2] = oTmp.DeviceID;
                        oTmpLogicPins[bytStart + 3] = Convert.ToByte(cboDev2.Value.ToString());
                        oTmpLogicPins[bytStart + 4] = (byte)(cboDev3.SelectedIndex + 1);
                    }
                    #endregion
                    break;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvSensors.SelectedItems == null) return;
            if (lvSensors.SelectedItems.Count == 0) return;

            if (publicPinTempltesArray == null || publicPinTempltesArray.Count == 0) return;

            Int32 selectedIndex = lvSensors.SelectedItems[0].Index;
            publicPinTempltesArray.RemoveAt(selectedIndex);

            for (Int32 intI = selectedIndex; intI < lvSensors.Items.Count; intI++)
            {
                lvSensors.Items[intI].SubItems[1].Text = (intI + 1).ToString(); 
            }
        }


        private void RW1_CheckedChanged(object sender, EventArgs e)
        {
            byte bytTag = Convert.ToByte(((RadioButton)sender).Tag.ToString());
            if (((RadioButton)sender).Checked == true)
            {
                cboW1.Enabled = (bytTag == 0);
                cboW2.Enabled = (bytTag == 1);
                cboW21.Enabled = (bytTag == 1);
            }
        }

        private void numT11_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboDev3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.SelectedIndex != 11) return; // 面板控制类型
            panelparam(cboDev3.SelectedIndex); 

        }

        private void panelparam(int bytSelectID)
        {
            cboDev31.Visible = false;
            cboDev31.Items.Clear();

            switch (bytSelectID)
            {
                case 0:break;
                case 1: //面板锁 空调开关  红外使能
                case 2:
                case 3:
                case 11:
                case 12:
                case 15:
                case 16:
                    cboDev31.Visible = true;
                    cboDev31.Items.AddRange(CsConst.Status);break;

                case 7: 
                case 8:
                case 4: // z制冷温度
                    cboDev31.Visible = true;
                    for (int i = 0; i < 87; i++)
                    {
                        if (i <= 31)
                        {
                            cboDev31.Items.Add(i.ToString() + "C");
                        }
                        else if (i > 31)
                        {
                            cboDev31.Items.Add(i.ToString() + "F");
                        }
                    }
                    break;

                case 5: //风速
                    cboDev31.Visible = true;
                    cboDev31.Items.AddRange(CsConst.StatusFAN);break;

                case 6: //模式
                    cboDev31.Visible = true;
                    cboDev31.Items.AddRange(CsConst.StatusAC);break;
                case 9:
                case 10:
                    cboDev31.Visible = true;
                    cboDev31.Items.AddRange(new string[] {"1","2"});break;
                case 13:
                case 14:
                    cboDev31.Visible = true;
                    for (int i = 0; i < 101; i++)
                    {
                        cboDev31.Items.Add(i.ToString());
                    }
                    break;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Button Tmp = ((Button)sender);
            if (Tmp.Tag == null) return;
            Byte ButtonTag = Convert.ToByte(Tmp.Tag.ToString());
            Byte[] TmpPin = null;
            Int32 TmpIndex = -1;
            if (ButtonTag == 0) // refresh current select pin
            {
                if (lvSensors.SelectedItems == null || lvSensors.SelectedItems.Count == 0) return;
                TmpIndex = lvSensors.SelectedItems[0].Index;
                TmpPin = publicPinTempltesArray[TmpIndex];
            }
            else
            {
                publicPinTempltesArray.Add(TmpPin);
                TmpIndex = publicPinTempltesArray.Count - 1;

                ListViewItem tmp = new ListViewItem();
                tmp.SubItems.Add((lvSensors.Items.Count + 1).ToString());
                tmp.SubItems.Add("");
                lvSensors.Items.Add(tmp);
            }
            UpdateBufferFormAccordinglyID(ref TmpPin);

            //更新显示
            lvSensors.Items[TmpIndex].SubItems[2].Text = HDLSysPF.GetDescriptionsFromBuffer(TmpPin, true)[0];
            publicPinTempltesArray[TmpIndex] = TmpPin;
        }

    }
}