using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; //Stream
using System.Linq;
using System.Net; //HttpWebRequest
using System.Text;
using System.Threading.Tasks;
//using System.Web.Script.Serialization;
using System.Windows.Forms;
using RestSharp;
using RestSharp.Serializers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MiddlwareTester
{
    public partial class Form1 : Form
    {
        string baseURI = @"https://localhost:44354/";
        RestClient client = null;
        public Form1()
        {
            InitializeComponent();
            client = new RestClient(baseURI);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region Application Methods

        private void button1_Click(object sender, EventArgs e) //butao do Get/Locate App
        {
            RestRequest request = new RestRequest("api/somiod", Method.Get);

            if (comboBox1.SelectedIndex != -1 && comboBox1.Text != "") //caso tenha selecionado um somiod-locate
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        request.AddHeader("somiod-locate", "application");
                        break;
                    case 1:
                        request.AddHeader("somiod-locate", "container");
                        break;
                    case 2:
                        request.AddHeader("somiod-locate", "record");
                        break;
                    case 3:
                        request.AddHeader("somiod-locate", "notification");
                        break;
                    default:
                        break;
                }

                request.RequestFormat = DataFormat.Xml;

                var response1 = client.Execute<List<String>>(request).Data;

                richTextBox1.Clear();
                foreach (var name in response1)
                {
                    richTextBox1.AppendText(name + "\n");
                }

                return;
            }

            //caso do get "normal"
            request.RequestFormat = DataFormat.Xml;

            var response = client.Execute<List<Application>>(request).Data;

            richTextBox1.Clear();
            foreach (var Application in response)
            {
                richTextBox1.AppendText($"Id: {Application.id} : {Application.name} \t {Application.creation_datetime} \n");
            }

            return;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnPostApp_Click(object sender, EventArgs e)
        {
            RestRequest request = new RestRequest("api/somiod", Method.Post);

            request.RequestFormat = DataFormat.Xml;

            request.AddBody("<Application>\r\n    <name>" + textPostApp.Text + "</name>\r\n</Application>");

            var response = client.Execute(request);

            richTextBox1.Clear();

            // Verifica se a requisição foi bem-sucedida
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Adiciona o conteúdo da resposta no richTextBox
                richTextBox1.AppendText(response.Content);
            }
            else
            {
                // Exibe a mensagem de erro no richTextBox
                richTextBox1.AppendText($"Error: {response.StatusDescription}");
            }

        }

        private void btnPutApp_Click(object sender, EventArgs e)
        {
            RestRequest request = new RestRequest("api/somiod/" + textPut1App.Text, Method.Put);

            request.RequestFormat = DataFormat.Xml;

            request.AddBody("<Application>\r\n    <name>" + textPut2App.Text + "</name>\r\n</Application>");

            var response = client.Execute(request);

            richTextBox1.Clear();

            // Verifica se a requisição foi bem-sucedida
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Adiciona o conteúdo da resposta no richTextBox
                richTextBox1.AppendText(response.Content);
            }
            else
            {
                // Exibe a mensagem de erro no richTextBox
                richTextBox1.AppendText($"Error: {response.StatusDescription}");
            }
        }

        private void btnDeleteApp_Click(object sender, EventArgs e)
        {
            RestRequest request = new RestRequest("api/somiod/" + textDeleteApp.Text, Method.Delete);

            request.RequestFormat = DataFormat.Xml;

            var response = client.Execute(request);

            richTextBox1.Clear();

            // Verifica se a requisição foi bem-sucedida
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Adiciona o conteúdo da resposta no richTextBox
                richTextBox1.AppendText(response.Content);
            }
            else
            {
                // Exibe a mensagem de erro no richTextBox
                richTextBox1.AppendText($"Error: {response.StatusDescription}");
            }
        }
        #endregion

        #region Container Methods



        #endregion
    }
}
