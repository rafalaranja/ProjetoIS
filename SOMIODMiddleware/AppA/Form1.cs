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

        private void button1_Click(object sender, EventArgs e)
        {
            //Post aplication
            RestRequest request = new RestRequest("api/somiod", Method.Post);

            request.RequestFormat = DataFormat.Xml;

            string applicationName = "Lighting";

            request.AddXmlBody($"<Application>\r\n    <name>{applicationName}</name>\r\n</Application>");

            var responseApp = client.Execute(request);

            //Post Container

            RestRequest request2 = new RestRequest($"api/somiod/{applicationName}", Method.Put);

            request2.RequestFormat = DataFormat.Xml;

            string containerName = "light_bulb";

            request2.AddXmlBody($"<Container>\r\n    <name>{containerName}</name>\r\n</Container>");

            var responseContainer = client.Execute(request2);

            //Post de subscrever Mqqt

            m_cClient.Connect(Guid.NewGuid().ToString());
            if (!m_cClient.IsConnected)
            {
                MessageBox.Show("Error connecting to message broker...");
                return;
            }

            //subscribe
            string res_type = "notification";

            //TODO AINDA





            /*

            // Verifica se a requisição foi bem-sucedida
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Adiciona o conteúdo da resposta numa MessageB
                richTextBox1.AppendText(response.Content);
            }
            else
            {
                // Exibe a mensagem de erro no richTextBox
                richTextBox1.AppendText($"Error: {response.StatusDescription}");
            }
            */


        }
    }
}
