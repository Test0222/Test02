using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace HDL_Buspro_Setup_Tool
{
    public partial class frmTirdPart : Form
    {
        public frmTirdPart()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cboSelect.SelectedIndex == -1) return;
            if (ipAddressNew1.Text == null || ipAddressNew1.Text == " . . . .") return;
            String sIpAddress = ipAddressNew1.Text;
            try
            {
               String sContent =  HDLSysPF.SimpleListConvertToJsonString();               
               String strURL = String.Format("http://{0}:8290/goform/Services?type=set", sIpAddress);
               if (sContent == null) sContent = string.Empty;
               string callback = PostMoths(strURL, sContent);

               String sGlobleScene = HDLSysPF.GlobleSceneConvertToJsonString();
               if (sGlobleScene == null) sGlobleScene = string.Empty;
               strURL = String.Format("http://{0}:8290/goform/SceneConfig?type=set", sIpAddress);
               callback = PostMoths(strURL, sGlobleScene);

               MessageBox.Show(callback);
                
            }
            catch
            { }
        }

        public static string PostMoths(string url, string param)
        {
            string strValue = "";
            if (param == "") return "";
            try
            {
                string strURL = url;
                WebRequest request = WebRequest.Create(new Uri(strURL));

                request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
                request.Method = "POST";
                request.ContentType = "application/json";
                string paraUrlCoded = param;
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                System.IO.Stream s;
                s = response.GetResponseStream();
                string StrDate = "";
                
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate + "\r\n";
                }
                request.Abort();
                return strValue;
            }
            catch(Exception ex)
            {
                return ex.ToString(); 
            }
            return strValue;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
