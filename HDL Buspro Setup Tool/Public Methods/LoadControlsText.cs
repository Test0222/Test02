using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Data.OleDb;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;



namespace HDL_Buspro_Setup_Tool
{
    public static class LoadControlsText
    {
        private static String Path = Application.StartupPath;

        /* public string[] allform = new string[] {"frmLogic", "frmWay","frmMain", "frmBacNet", "frmCtrls", "frmUpload", "frmDown", "frmPrint", "frmProject", "frmProperty", "frm12in1",
                                               "frm8in1","frmAudio","frmTargets","frmCurtain","frmDimmer","frmDLP","frmDS","frmHMIX","frmDMX","frmEIB","frmFH","frmGPRS",
                                               "frmHAI","frmHVAC","frmIPMod","frmIR","frmMC","frmMotion","frmImport","frmInitial","frmIRLibrary",
                                               "frmMS04","frmPanel","FrmDownloadShow","frmRelay","frmRemote","frmRS232","frmSecurity","frmSensorsINOne",
                                               "frmAdd","frmLoadType","frmEMain","frmNetWork","frmSPA","frm4Temp","frmDIY","frmICON","frmMAC","frmMutex","frmLED",
                                               "frmRemark","frmSetup","frmTool","frmUpgrade","frmAC","frmCountDown","frmFHT","frmHvI","frmList","frmLogicPin",
                                              "frmWIFI","frmMAC","frmTHVAC","frmTSensor","frmWizard","frmIRlearner","frmMHIOU","FrmAddress","frmModifAddress", "frmQC" };
        */
        public static string[] allform = new string[] { "frm12in1", "frm7in1", "frm8in1", "FrmMzBox", "frmBacNet", "frmCameraNvr", "FrmMHIC",
                                               "FrmTargetForCardReader","FrmCalculateTargetForColorDLP","FrmColorDLP","FrmColorDLPACSetup",
                                               "FrmColorDLPTargets","FrmColorDLPUI","FrmCoolMaster","frmCurtain","frmCamera",
                                               "FrmColorDLPImageType","FrmEditPanlePicture","frmHvacRelay","frmNetModule","FrmPage",
                                               "frmTSensor","frmDimmer","frmDLP","frmDMX","FrmAddNewCard","FrmColorForDS","frmDS","FrmNewDS",
                                               "frmEIB","frmFH","frmGScenes","frmGPRS","frmHAI","frmHVAC","frmIPMod","frmIRlearner","FrmNewIREmitor",
                                               "FrmMultiRemoteSender","FrmChnAndKey","frmLogic","frmLogicPin","FrmMiniSensor","frmMC","frmMS04",
                                               "FrmMSPU","frmEnviro","FrmEnviroUI","frmPanel","FrmDownloadShow","frmMain","FrmAbout",
                                               "frmTool","FrmSignal","frmSA","FrmRestrore","FrmRaw","frmQC","frmIRLibrary","FrmBackup",
                                               "frm4Temp","frmSocket","frmTirdPart","frmManualFunctionLists","frmFunctionLists","frmAdd",
                                               "FrmBusToRS232","frmRS232","FrmRS232BUS","frmRemote","frmRelay","frmUpgrade","FrmSlave",
                                               "frmSetup","frmRemark","FrmReceiverTargets","FrmPassWork","frmModifAddress","frmDimTemplates",
                                               "frmMAC","FrmInfrared","FrmHeatTargets","FrmEditClock","frmCmdSetup","FrmCalibrationLux",
                                               "frmButtonSetup","FrmBoundRate","FrmADVSearchDevice","FrmACSetup",
                                               "FrmCombinationWays","frmRefreshFlash"};


        //"FrmCombinationWays","frmRefreshFlash","frmCmdTemplates",
        // public string[] allform = new string[] { "FrmEditClock" };

        [Serializable]
        public class FormsInformation
        {
            public String sFormName;///窗体名字
            public List<ControlInfotmation> cControlsInForm; // 控件列表           
        }

        [Serializable]
        public class ControlInfotmation
        {
            public List<int> iIndexInWholeList; //整体列表中的排序
            public Type sControlType;///控件类型
            public String sControlName; //控件名字
        }

        [Serializable]
        public class FormDisplayTextList
        {
            public int iIndexInWholeList; //整体列表中的排序
            public String sDisplayName; //控件名字 
            public int sMaxLimit; //最大长度限制
        }

        public static bool CheckAndLoadControlsNew()
        {
            bool result = true;
            bool isadd = true;
            String sTest = "";
            try
            {
                CsConst.FormControlsIdinfo = new List<FormsInformation>();
                for (int i = 0; i < allform.Length; i++)
                {
                    string strnamespace = "HDL_Buspro_Setup_Tool";//根据你自己的命名空间来修改 

                    string strfrm = allform[i].ToString();

                    Form frm = (Form)Activator.CreateInstance(Type.GetType(strnamespace + "." + strfrm));
                    FormsInformation frmtemp = new FormsInformation();

                    //add to new struct
                    #region
                    frmtemp.sFormName = frm.Name.ToString();
                    frmtemp.cControlsInForm = new List<ControlInfotmation>();
                    CsConst.FormControlsIdinfo.Add(frmtemp);
                    #endregion
                    //read all ctrls in form, save the ones that needs to save struct
                   List<Control> list = GetAllControls(frm);//获取form1所有子控件

                    foreach (System.Windows.Forms.Control tmpControl in list)
                    {
                        // read all ctrls name to buffer
                        #region
                        ControlInfotmation temp = new ControlInfotmation();
                        if (temp != null)
                        {
                            if (tmpControl != null)
                            {
                                sTest = frm.Name.ToString() + "-" + tmpControl.Name;
                                temp.sControlName = tmpControl.Name;
                                temp.sControlType = tmpControl.GetType();

                                if (tmpControl is StatusStrip) // 工具栏
                                {
                                    #region
                                    StatusStrip oTmpStrip = (StatusStrip)tmpControl;
                                    temp.iIndexInWholeList = new List<int>();
                                    for (int intI = 0; intI < oTmpStrip.Items.Count; intI++)
                                    {
                                        String sName = oTmpStrip.Items[intI].Text;
                                        temp.iIndexInWholeList.Add(GetIfExistedTextAlready(sName, sName.Length + 4));
                                    }
                                    #endregion
                                }
                                else if (tmpControl is ToolStrip)
                                {
                                    #region
                                    ToolStrip oTmpStrip = (ToolStrip)tmpControl;
                                    temp.iIndexInWholeList = new List<int>();
                                    for (int intI = 0; intI < oTmpStrip.Items.Count; intI++)
                                    {
                                        String sName = oTmpStrip.Items[intI].Text;
                                        temp.iIndexInWholeList.Add(GetIfExistedTextAlready(sName, sName.Length + 4));
                                    }
                                    #endregion
                                }
                                else if (tmpControl is ContextMenuStrip)
                                {
                                    #region
                                    ContextMenuStrip oTmpStrip = (ContextMenuStrip)tmpControl;
                                    temp.iIndexInWholeList = new List<int>();
                                    for (int intI = 0; intI < oTmpStrip.Items.Count; intI++)
                                    {
                                        String sName = oTmpStrip.Items[intI].Text;
                                        temp.iIndexInWholeList.Add(GetIfExistedTextAlready(sName, sName.Length + 4));
                                    }
                                    #endregion
                                }
                                else if (tmpControl is CheckedListBox)
                                {
                                    #region
                                    CheckedListBox oTmpStrip = (CheckedListBox)tmpControl;
                                    temp.iIndexInWholeList = new List<int>();
                                    for (int intI = 0; intI < oTmpStrip.Items.Count; intI++)
                                    {
                                        String sName = oTmpStrip.Items[intI].ToString();
                                        temp.iIndexInWholeList.Add(GetIfExistedTextAlready(sName, sName.Length + 4));
                                    }
                                    #endregion
                                }
                                else if (tmpControl is DataGridView)
                                {
                                    #region
                                    DataGridView oTmpStrip = (DataGridView)tmpControl;
                                    temp.iIndexInWholeList = new List<int>();
                                    for (int intI = 0; intI < oTmpStrip.ColumnCount; intI++)
                                    {
                                        String sName = oTmpStrip.Columns[intI].HeaderText.ToString();
                                        temp.iIndexInWholeList.Add(GetIfExistedTextAlready(sName, sName.Length + 4));
                                    }
                                    #endregion
                                }
                                else
                                {
                                    String sName = "";
                                    if (tmpControl.Text.Trim() != "")
                                    {
                                        sName = tmpControl.Text;
                                        temp.iIndexInWholeList = new List<int>();
                                        temp.iIndexInWholeList.Add(GetIfExistedTextAlready(sName, sName.Length + 4));
                                    }
                                }
                                frmtemp.cControlsInForm.Add(temp);
                            }
                        }
                       #endregion
                    }
                    frm.Dispose();
                }
                SaveWholeStructToXmlFile();
                SaveDatatoDB();
                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(sTest + ex.ToString());
                result = false;
            }
            return result;
        }

        public static List<Control> GetAllControls(Control control)
        {
            var list = new List<Control>();
            foreach (Control con in control.Controls)
            {
                list.Add(con);
                if ( con.HasChildren && con.Controls.Count > 0)
                {
                    list.AddRange(GetAllControls(con));
                }
            }
            return list;
        }

        public static void SaveWholeStructToXmlFile()
        {
            try
            {
                if (CsConst.FormControlsIdinfo != null && CsConst.FormControlsIdinfo.Count != 0)
                {
                    FileStream fs = new FileStream(Path +@"\FormControls.dat", FileMode.Create);
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, CsConst.FormControlsIdinfo);
                    fs.Close();
                   
                }
            }
            catch
            { }
        }

        public static void LoadControlsTextIdListFromXML()
        {
            try
            {
                CsConst.FormControlsIdinfo = new List<FormsInformation>();
                if (File.Exists(Path + @"\FormControls.dat"))
                {
                    FileStream fs = new FileStream(Path + @"\FormControls.dat", FileMode.Open);
                    BinaryFormatter bf = new BinaryFormatter();
                    CsConst.FormControlsIdinfo = bf.Deserialize(fs) as List<FormsInformation>;
                    fs.Close();
                }
            }
            catch
            { }
        }

        public static bool SaveDatatoDB()
        {
            bool result = true;
            int test = -1;
            try
            {
                if (CsConst.WholeTextsList == null || CsConst.WholeTextsList.Count <= 0) return true;

                string strsql = "";
                foreach (FormDisplayTextList temp in CsConst.WholeTextsList)
                {
                    // check if it has already exist, if yes ,save to ini 
                    #region
                    test = temp.iIndexInWholeList;
                    strsql = string.Format("insert into dbLanguaText(TextIndex,TextLength,English) values ({0},{1},'{2}')",
                                         temp.iIndexInWholeList, temp.sMaxLimit, temp.sDisplayName);

                    DataModule.ExecuteSQLDatabase(strsql);
                    #endregion
                }
            }
            catch
            {
                MessageBox.Show(test.ToString());
                result = false;
            }
            return result;
        }

        public static int GetIfExistedTextAlready(String sName, int iMaxLimit)
        {
            int iResult = -1;
            try
            {
                Boolean bIsExisted = false;
                 // 如果不存在 直接添加
                if (CsConst.WholeTextsList == null || CsConst.WholeTextsList.Count == 0)
                {
                    CsConst.WholeTextsList = new List<FormDisplayTextList>();
                    iResult = 0;
                }
                else
                {
                    foreach (FormDisplayTextList tmp in CsConst.WholeTextsList)
                    {
                        if (tmp.sDisplayName.ToLower() == sName.ToLower())
                        {
                            return tmp.iIndexInWholeList;
                        }
                    }
                }

                if (bIsExisted == false)
                {
                    if (iResult == -1) iResult = CsConst.WholeTextsList.Count + 2;
                    //增加新的
                    FormDisplayTextList tmp = new FormDisplayTextList();
                    tmp.iIndexInWholeList = iResult;
                    tmp.sDisplayName = sName;
                    tmp.sMaxLimit = iMaxLimit;
                    CsConst.WholeTextsList.Add(tmp);
                }
                return iResult;
            }
            catch
            {
                return iResult;
            }
            return iResult;
        }

        public static void LoadButtonCoontrolTypeFromDatabaseToPublicClass()
        {
            CsConst.WholeTextsList = new List<FormDisplayTextList>();
            try
            {
                string strsql = string.Format("select * from dbLanguaText order by TextIndex");
                OleDbDataReader dr = DataModule.SearchAResultSQLDB(strsql, CsConst.mstrDefaultPath);

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        FormDisplayTextList TmpButtonMode = new FormDisplayTextList();
                        TmpButtonMode.iIndexInWholeList = dr.GetInt32(0);
                        TmpButtonMode.sMaxLimit = dr.GetInt32(1);
                        if (dr.GetString(CsConst.iLanguageId + 2) != null)
                            TmpButtonMode.sDisplayName = dr.GetString(CsConst.iLanguageId + 2);
                        CsConst.WholeTextsList.Add(TmpButtonMode);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void DisplayTextToFormWhenFirstShow(Control sForm)
        {
            if (sForm.Name== null || sForm.Name == "") return;
            if (CsConst.FormControlsIdinfo == null) return;
            if (CsConst.WholeTextsList == null) return;
            try
            {
                List<Control> tmpFormControls = GetAllControls(sForm);
                foreach (FormsInformation oTmp in CsConst.FormControlsIdinfo)
                {
                    if (oTmp.sFormName == sForm.Name) // 窗体名字
                    {
                        while (tmpFormControls.Count != 0)
                        {
                            Boolean bIsFound = false;
                            foreach (ControlInfotmation oTmpControl in oTmp.cControlsInForm)
                            {
                                if (tmpFormControls[0].Name == oTmpControl.sControlName)
                                {
                                    #region
                                    if (tmpFormControls[0] is StatusStrip) // 工具栏
                                    {
                                        #region
                                        StatusStrip oTmpStrip = (StatusStrip)tmpFormControls[0];
                                        if (oTmpControl.iIndexInWholeList != null)
                                        {
                                            for (int intI = 0; intI < oTmpStrip.Items.Count; intI++)
                                            {
                                                int iTmpIndex = oTmpControl.iIndexInWholeList[intI];
                                                if (iTmpIndex == -1 || iTmpIndex > CsConst.WholeTextsList.Count) iTmpIndex = 0;
                                                oTmpStrip.Items[intI].Text = CsConst.WholeTextsList[iTmpIndex].sDisplayName;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (tmpFormControls[0] is ToolStrip)
                                    {
                                        #region
                                        if (tmpFormControls[0].Name != "toolStrip3")
                                        {
                                            ToolStrip oTmpStrip = (ToolStrip)tmpFormControls[0];
                                            if (oTmpControl.iIndexInWholeList != null)
                                            {
                                                for (int intI = 0; intI < oTmpStrip.Items.Count; intI++)
                                                {
                                                    int iTmpIndex = oTmpControl.iIndexInWholeList[intI];
                                                    if (iTmpIndex == -1 || iTmpIndex > CsConst.WholeTextsList.Count) iTmpIndex = 0;
                                                    oTmpStrip.Items[intI].Text = CsConst.WholeTextsList[iTmpIndex].sDisplayName;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (tmpFormControls[0] is ContextMenuStrip)
                                    {
                                        #region
                                        ContextMenuStrip oTmpStrip = (ContextMenuStrip)tmpFormControls[0];
                                        if (oTmpControl.iIndexInWholeList != null)
                                        {
                                            for (int intI = 0; intI < oTmpStrip.Items.Count; intI++)
                                            {
                                                int iTmpIndex = oTmpControl.iIndexInWholeList[intI];
                                                if (iTmpIndex == -1 || iTmpIndex > CsConst.WholeTextsList.Count) iTmpIndex = 0;
                                                oTmpStrip.Items[intI].Text = CsConst.WholeTextsList[iTmpIndex].sDisplayName;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (tmpFormControls[0] is CheckedListBox)
                                    {
                                        #region
                                        CheckedListBox oTmpStrip = (CheckedListBox)tmpFormControls[0];
                                        if (oTmpControl.iIndexInWholeList != null)
                                        {
                                            for (int intI = 0; intI < oTmpStrip.Items.Count; intI++)
                                            {
                                                int iTmpIndex = oTmpControl.iIndexInWholeList[intI];
                                                if (iTmpIndex == -1 || iTmpIndex > CsConst.WholeTextsList.Count) iTmpIndex = 0;
                                                oTmpStrip.Items[intI] = CsConst.WholeTextsList[iTmpIndex].sDisplayName;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (tmpFormControls[0] is DataGridView)
                                    {
                                        #region
                                        DataGridView oTmpStrip = (DataGridView)tmpFormControls[0];
                                        if (oTmpControl.iIndexInWholeList != null)
                                        {
                                            for (int intI = 0; intI < oTmpStrip.ColumnCount; intI++)
                                            {
                                                int iTmpIndex = oTmpControl.iIndexInWholeList[intI];
                                                if (iTmpIndex == -1 || iTmpIndex > CsConst.WholeTextsList.Count) iTmpIndex = 0;
                                                oTmpStrip.Columns[intI].HeaderText = CsConst.WholeTextsList[iTmpIndex].sDisplayName;
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                       // String[] sTmpTest = new String[]{"label1","label4","lbC4","chbCardEnable"};

                                        if (oTmpControl.iIndexInWholeList != null)
                                        {
                                            int iTmpIndex = oTmpControl.iIndexInWholeList[0];
                                            if (iTmpIndex == -1 || iTmpIndex > CsConst.WholeTextsList.Count) iTmpIndex = 0;
                                            tmpFormControls[0].Text = CsConst.WholeTextsList[iTmpIndex].sDisplayName;
                                        }
                                    }
                                    #endregion
                                    bIsFound = true;
                                    tmpFormControls.RemoveAt(0);
                                    break;
                                }
                                else
                                {
                                   // tmpFormControls.RemoveAt(0);
                                }
                            }
                            if (bIsFound == false)
                            {
                                tmpFormControls.RemoveAt(0);
                            }
                        }
                        break;
                    }
                }
            }
            catch
            { }
        }
        
    }
}
