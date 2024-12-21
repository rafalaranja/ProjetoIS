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

                if (response1 == null)
                {
                    richTextBox1.AppendText("No data found\n");
                    return;
                }

                foreach (var name in response1)
                {
                    richTextBox1.AppendText(name + "\n");
                }

                return;
            }

            //caso do get "normal" devolve todas as applications
            request.RequestFormat = DataFormat.Xml;

            var response = client.Execute<List<Application>>(request).Data;

            richTextBox1.Clear();

            if (response == null)
            {
                richTextBox1.AppendText("No data found\n");
                return;
            }

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

        private void btnLocateContainer_Click(object sender, EventArgs e)
        {
            if(textBoxAppName.Text == "")
            {
                MessageBox.Show("Please insert an application name");
                return;
            }

            RestRequest request = new RestRequest("api/somiod/" + textBoxAppName.Text , Method.Get);

            if (comboBox2.SelectedIndex != -1 && comboBox2.Text != "") //caso tenha selecionado um somiod-locate
            {
                switch (comboBox2.SelectedIndex)
                {
                    case 0:
                        request.AddHeader("somiod-locate", "container");
                        break;
                    case 1:
                        request.AddHeader("somiod-locate", "record");
                        break;
                    case 2:
                        request.AddHeader("somiod-locate", "notification");
                        break;
                    default:
                        break;
                }

                request.RequestFormat = DataFormat.Xml;

                var response1 = client.Execute<List<String>>(request).Data;

                richTextBox2.Clear();

                if (response1 == null)
                {
                    richTextBox2.AppendText("No data found\n");
                    return;
                }
                foreach (var name in response1)
                {
                    richTextBox2.AppendText(name + "\n");
                }

                return;
            }

            //caso do get "normal", devolve so os atributos da application
            request.RequestFormat = DataFormat.Xml;

            var response = client.Execute<List<Application>>(request).Data;

            richTextBox2.Clear();

            if (response == null)
            {
                richTextBox2.AppendText("No data found\n");
                return;
            }

            foreach (var Application in response)
            {
                richTextBox2.AppendText($"Id: {Application.id} : {Application.name} \t {Application.creation_datetime} \n");
            }

            return;
        }


        private void btnPostContainer_Click(object sender, EventArgs e)
        {
            if (textBoxAppName.Text == "")
            {
                MessageBox.Show("Please insert an application name");
                return;
            }

            RestRequest request = new RestRequest("api/somiod/" + textBoxAppName.Text, Method.Post);

            request.RequestFormat = DataFormat.Xml;

            request.AddBody("<Container>\r\n    <name>" + textPostContainer.Text + "</name>\r\n</Container>");

            var response = client.Execute(request);

            richTextBox2.Clear();

            // Verifica se a requisição foi bem-sucedida
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Adiciona o conteúdo da resposta no richTextBox
                richTextBox2.AppendText(response.Content);
            }
            else
            {
                // Exibe a mensagem de erro no richTextBox
                richTextBox2.AppendText($"Error: {response.StatusDescription}");
            }
        }

        private void btnPutContainer_Click(object sender, EventArgs e)
        {
            if (textBoxAppName.Text == "")
            {
                MessageBox.Show("Please insert an application name");
                return;
            }

            RestRequest request = new RestRequest("api/somiod/" + textBoxAppName.Text + "/" + textPut1Container.Text, Method.Put);

            request.RequestFormat = DataFormat.Xml;

            request.AddBody("<Container>\r\n    <name>" + textPut2Container.Text + "</name>\r\n</Container>");

            var response = client.Execute(request);

            richTextBox2.Clear();

            // Verifica se a requisição foi bem-sucedida
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Adiciona o conteúdo da resposta no richTextBox
                richTextBox2.AppendText(response.Content);
            }
            else
            {
                // Exibe a mensagem de erro no richTextBox
                richTextBox2.AppendText($"Error: {response.StatusDescription}");
            }
        }

        private void btnDeleteContainer_Click(object sender, EventArgs e)
        {
            if (textBoxAppName.Text == "")
            {
                MessageBox.Show("Please insert an application name");
                return;
            }

            RestRequest request = new RestRequest("api/somiod/" + textBoxAppName.Text + "/" + textDeleteContainer.Text, Method.Delete);

            request.RequestFormat = DataFormat.Xml;

            var response = client.Execute(request);

            richTextBox2.Clear();

            // Verifica se a requisição foi bem-sucedida
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Adiciona o conteúdo da resposta no richTextBox
                richTextBox2.AppendText(response.Content);
            }
            else
            {
                // Exibe a mensagem de erro no richTextBox
                richTextBox2.AppendText($"Error: {response.StatusDescription}");
            }
        }

        #endregion

        #region Record Methods

        private void btnLocateRecord_Click(object sender, EventArgs e)
        {
            if (textBoxAppName2.Text == "" || textBoxContainerName.Text == "")
            {
                MessageBox.Show("Please insert an application/container name");
                return;
            }

            RestRequest request = new RestRequest("api/somiod/" + textBoxAppName2.Text + "/" + textBoxContainerName.Text, Method.Get);

            if (comboBox3.SelectedIndex != -1 && comboBox3.Text != "") //caso tenha selecionado um somiod-locate
            {
                switch (comboBox3.SelectedIndex)
                {
                    case 0:
                        request.AddHeader("somiod-locate", "record");
                        break;
                    case 1:
                        request.AddHeader("somiod-locate", "notification");
                        break;
                    default:
                        break;
                }

                request.RequestFormat = DataFormat.Xml;

                var response1 = client.Execute<List<String>>(request).Data;

                richTextBox3.Clear();
                
                if(response1 == null) {
                    richTextBox3.AppendText("No data found\n");
                    return;
                }

                foreach (var name in response1)
                {
                    richTextBox3.AppendText(name + "\n");
                }

                return;
            }

            //caso do get "normal", devolve so os atributos da application
            request.RequestFormat = DataFormat.Xml;

            var response = client.Execute<List<Container>>(request).Data;

            richTextBox3.Clear();

            if (response == null)
            {
                richTextBox3.AppendText("No data found\n");
                return;
            }

            foreach (var Container in response)
            {
                richTextBox3.AppendText($"Id: {Container.id} : {Container.name} \t {Container.creation_datetime} \n");
            }

            return;
        }

        private void btnPostRecord_Click(object sender, EventArgs e) // por acabar
        {
            if (textBoxAppName2.Text == "" || textBoxContainerName.Text == "")
            {
                MessageBox.Show("Please insert an application/container name");
                return;
            }

            RestRequest request = new RestRequest("api/somiod/" + textBoxAppName2.Text + "/" + textBoxContainerName.Text + "/record", Method.Post);

            request.RequestFormat = DataFormat.Xml;

            request.AddBody("<Record>\r\n" +
                               "    <name>" + textPostRecord.Text + "</name>\r\n" +
                                "    <content>" + textContentPostRecord.Text + "</content>\r\n" +
                                "</Record>");

            var response = client.Execute(request);

            richTextBox3.Clear();

            // Verifica se a requisição foi bem-sucedida
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Adiciona o conteúdo da resposta no richTextBox
                richTextBox3.AppendText(response.Content);
            }
            else
            {
                // Exibe a mensagem de erro no richTextBox
                richTextBox3.AppendText($"Error: {response.StatusDescription}");
            }
        }



        #endregion


    }
}
