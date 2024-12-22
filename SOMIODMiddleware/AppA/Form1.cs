using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using RestSharp;
using System.Xml;

namespace AppA
{
    public partial class Form1 : Form
    {
        string baseURI = @"https://localhost:44354/";
        RestClient client = null;

        MqttClient m_cClient = new MqttClient("127.0.0.1");
        public Form1()
        {
            InitializeComponent();
            client = new RestClient(baseURI);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private string[] ExtractMsgFromXml(string str_xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(str_xml);
            XmlNode rootNode = doc.SelectSingleNode("/notification");
            String strType = rootNode["type"].InnerText;
            String strResource = rootNode["resource"].InnerText;

            string[] arrReturn = new string[2];

            arrReturn[0] = strType;
            arrReturn[1] = strResource;

            return arrReturn;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Post aplication
            RestRequest request = new RestRequest("api/somiod", Method.Post);

            request.RequestFormat = DataFormat.Xml;

            string applicationName = "Lighting";

            request.AddXmlBody($"<Application>\r\n    <name>{applicationName}</name>\r\n</Application>");

            var responseApp = client.Execute(request);

            if(responseApp.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Error creating application...");
                return;
            }

            //Post Container

            RestRequest request2 = new RestRequest($"api/somiod/{applicationName}", Method.Post);

            request2.RequestFormat = DataFormat.Xml;

            string containerName = "light_bulb";

            request2.AddXmlBody($"<Container>\r\n    <name>{containerName}</name> \r\n</Container>");

            var responseContainer = client.Execute(request2);

            if (responseContainer.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Error creating container...");
                return;
            }


            //Post da notificação

            RestRequest request3 = new RestRequest($"api/somiod/{applicationName}/{containerName}", Method.Post);

            request3.RequestFormat = DataFormat.Xml;

            string notificationName = "sub1";

            string @event = "creation";

            string endpoint = "127.0.0.1";

            request3.AddXmlBody($"<Notification>\r\n    <name>{notificationName}</name>\r\n    <event>{@event}</event>\r\n    <endpoint>{endpoint}</endpoint>\r\n</Notification>");

            var responseNotification = client.Execute(request3);

            if (responseNotification.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Error creating notification...");
                return;
            }

            //Post de subscrever Mqqt

            m_cClient.Connect(Guid.NewGuid().ToString());
            if (!m_cClient.IsConnected)
            {
                MessageBox.Show("Error connecting to message broker...");
                return;
            }

            //subscribe channel

            string topic = "light_bulb";
            m_cClient.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });


            //receive message

            m_cClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            /*


            m_cClient.MqttMsgPublishReceived += (s, ev) =>
            {
                string receivedMessage = Encoding.UTF8.GetString(ev.Message);

                string[] arrMsg = ExtractMsgFromXml(receivedMessage);

                if (arrMsg[1] == "on")
                {
                    textBoxEstado.Text = "Light is on";
                }
                else if (arrMsg[1] == "off")
                {
                    textBoxEstado.Text = "Light is off";
                }

            };

            */


        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string receivedMessage = Encoding.UTF8.GetString(e.Message);
            string[] arrMsg = ExtractMsgFromXml(receivedMessage);
            if (arrMsg[1] == "on")
            {
                textBoxEstado.Text = "Light is on";
            }
            else if (arrMsg[1] == "off")
            {
                textBoxEstado.Text = "Light is off";
            }
        }

    }
}
