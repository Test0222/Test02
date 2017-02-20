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
    public partial class frmGScenes : Form
    {
        int totalDimmers = 1;
        public frmGScenes()
        {
            InitializeComponent();
        }

        private void frmGScenes_Load(object sender, EventArgs e)
        {
            if (CsConst.myDimmers != null)
            {
               foreach (Dimmer OTmp in CsConst.myDimmers)
                {
                    //DisplayAllChannelsValues(OTmp);
                    DisplayAllChannelsValuesVerital(OTmp);
                   // break;
                }
            }

            if (CsConst.myRelays != null)
            {

               foreach (Relay OTmp in CsConst.myRelays)
               {
                   //DisplayAllChannelsValues(OTmp);
                   DisplayAllChannelsValuesVeritalRelay(OTmp);
                   // break;
               }
            }
        }

        void DisplayAllChannelsValuesVerital(Dimmer oDimmer)
        {
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;

            byte bytAreaID = 1;
            int DataGridHeight = 0;
            // get ready satble header and other basic information
            RowMergeView oDg = (RowMergeView)this.Controls.Find("Dg"+totalDimmers.ToString(),false)[0];
            oDg.ColumnHeadersHeight = 48;
            // oDg.Columns.Add("Clm1", "Remark");
            oDg.Columns.Add("Clm1", "Zone");
            oDg.Columns.Add("Clm2", "Channel");
            oDg.Columns.Add("Clm3", "Name");

            for (int i = 0; i <= 12; i++)
            {
                //显示所有列的信息 运行时间和回路数
                oDg.Columns.Add("Clm" + (i + 4).ToString(), "场景 " + i.ToString());
                oDg.Columns[3 + i].Width = oDg.Width / 20;
            }
            oDg.AddSpanHeader(0, 16, oDimmer.DeviceName);

            for (byte bytTmp = 1; bytTmp <= oDimmer.Areas.Count; bytTmp++)
            {
                bytAreaID = bytTmp;
                Dimmer.Area oArea = null;

                foreach (Dimmer.Area Tmp in oDimmer.Areas)
                {
                    if (Tmp.ID == bytAreaID)
                    {
                        oArea = Tmp;
                        break;
                    }
                }
                if (oArea.Scen == null) return;

                foreach (Dimmer.Channel oChn in oDimmer.Chans)
                {
                    if (oChn.intBelongs == bytAreaID)
                    {                         
                        object[] oTmp = new object[16];
                        oTmp[0] = "区" + bytAreaID.ToString();
                        oTmp[1] = oChn.ID;
                        oTmp[2] = oChn.Remark;
                        for (int i = 0; i < 13; i++)
                        {
                            oTmp[3 + i] = oArea.Scen[i].light[oChn.ID - 1].ToString();
                         }
                        oDg.Rows.Add(oTmp);
                        DataGridHeight += 30;
                    }
                }
            }

            oDg.MergeColumnNames.Add("Clm1");
            oDg.Height = DataGridHeight;
            totalDimmers++;
        }

        void DisplayAllChannelsValuesVeritalRelay(Relay oDimmer)
        {
            if (oDimmer == null) return;
            if (oDimmer.Areas == null) return;

            byte bytAreaID = 1;
            int DataGridHeight = 0;
            // get ready satble header and other basic information
            RowMergeView oDg = (RowMergeView)this.Controls.Find("Dg" + totalDimmers.ToString(), false)[0];
            oDg.ColumnHeadersHeight = 48;
            // oDg.Columns.Add("Clm1", "Remark");
            oDg.Columns.Add("Clm1", "Zone");
            oDg.Columns.Add("Clm2", "Channel");
            oDg.Columns.Add("Clm3", "Name");

            for (int i = 0; i <= 12; i++)
            {
                //显示所有列的信息 运行时间和回路数
                oDg.Columns.Add("Clm" + (i + 4).ToString(), "场景 " + i.ToString());
                oDg.Columns[3 + i].Width = oDg.Width / 20;
            }
            oDg.AddSpanHeader(0, 16, oDimmer.DeviceName);

            for (byte bytTmp = 1; bytTmp <= oDimmer.Areas.Count; bytTmp++)
            {
                bytAreaID = bytTmp;
                Relay.Area oArea = null;

                foreach (Relay.Area Tmp in oDimmer.Areas)
                {
                    if (Tmp.ID == bytAreaID)
                    {
                        oArea = Tmp;
                        break;
                    }
                }
                if (oArea.Scen == null) return;

                foreach (RelayChannel oChn in oDimmer.Chans)
                {
                    if (oChn.intBelongs == bytAreaID)
                    {
                        object[] oTmp = new object[16];
                        oTmp[0] = "区" + bytAreaID.ToString();
                        oTmp[1] = oChn.ID;
                        oTmp[2] = oChn.Remark;
                        for (int i = 0; i < 13; i++)
                        {
                            oTmp[3 + i] = oArea.Scen[i].light[oChn.ID - 1].ToString();
                        }
                        oDg.Rows.Add(oTmp);
                        DataGridHeight += 30;
                    }
                }
            }

            oDg.MergeColumnNames.Add("Clm1");
            oDg.Height = DataGridHeight;
            totalDimmers++;
        }

        private void Dg1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
