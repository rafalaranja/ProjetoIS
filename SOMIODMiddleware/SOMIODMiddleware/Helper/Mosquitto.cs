using System;
using System.Net;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SOMIODMiddleware.Helper
{
    public class Mosquitto
    {
        //private MqttClient client = new MqttClient(IPAddress.Parse("127.0.0.1"));

        
        private MqttClient client;

        public void MosquittoIP(string brokerIp)
        {
            client = new MqttClient(brokerIp);
        }

    

        // Conecta ao broker MQTT
        private void Connect()
        {
            if (!client.IsConnected)
            {
                client.Connect(Guid.NewGuid().ToString());
            }
        }

        // Envia uma mensagem para o tópico especificado
        public void Publish(string topic, string message)
        {
            try
            {
                Connect();
                client.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                Console.WriteLine($"Mensagem publicada no tópico '{topic}': {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao publicar mensagem: {ex.Message}");
            }
        }

        // Inscreve-se em um tópico para receber notificações
        public void Subscribe(string topic)
        {
            try
            {
                Connect();
                client.MqttMsgPublishReceived += HandleReceivedMessage;
                client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                Console.WriteLine($"Inscrito no tópico: {topic}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inscrever-se no tópico: {ex.Message}");
            }
        }

        // Evento chamado ao receber uma mensagem
        private void HandleReceivedMessage(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                string topic = e.Topic;
                string message = Encoding.UTF8.GetString(e.Message);
                Console.WriteLine($"Mensagem recebida no tópico '{topic}': {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem recebida: {ex.Message}");
            }
        }
    }
}
