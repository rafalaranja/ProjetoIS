using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;

namespace AppB
{
    public partial class Form1 : Form
    {
        private RestClient client;

        public Form1()
        {
            InitializeComponent();
            client = new RestClient("https://localhost:44354/"); // Base URI do SOMIOD
        }

        private void buttonOn_Click(object sender, EventArgs e)
        {

            SendLightCommand("on");

        }

        private void buttonOff_Click(object sender, EventArgs e)
        {
            SendLightCommand("off");

        }

        private void SendLightCommand(string command)
        {
            //Post aplication
            RestRequest request = new RestRequest("api/somiod", Method.Post);

            request.RequestFormat = DataFormat.Xml;

            string applicationName = "Switch";

            request.AddXmlBody($"<Application>\r\n    <name>{applicationName}</name>\r\n</Application>");

            var responseApp = client.Execute(request);

            //Post Record
            string appRota = "lighting";
            string containerRota = "light_bulb";

            RestRequest request1 = new RestRequest($"api/somiod/{appRota}/{containerRota}", Method.Post);

            request1.RequestFormat = DataFormat.Xml;

            if (command == "on")
                request1.AddXmlBody($"<Record>\r\n    <name>Ligar</name>\r\n    <content>{command}</content>\r\n</Record>");
            else
                request1.AddXmlBody($"<Record>\r\n    <name>Desligar</name>\r\n    <content>{command}</content>\r\n</Record>");

            var responseRecord = client.Execute(request1);



        }


    }
}
