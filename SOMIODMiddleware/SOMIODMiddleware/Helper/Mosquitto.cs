using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SOMIODMiddleware.Helper
{
    public class Mosquitto
    {
        private readonly MqttClient mqttClient;
        private readonly string brokerAddress;
        public bool IsConnected => mqttClient.IsConnected;

        public Mosquitto(string brokerAddress = "127.0.0.1")
        {
            this.brokerAddress = brokerAddress;

            // Inicializar o cliente MQTT
            mqttClient = new MqttClient(brokerAddress);
            mqttClient.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
        }

        public void Connect()
        {
            try
            {
                mqttClient.Connect(Guid.NewGuid().ToString());
                if (!mqttClient.IsConnected)
                    throw new Exception("Error connecting to broker MQTT.");
                Console.WriteLine("Connected to MQTT.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to broker MQTT: {ex.Message}");
                throw;
            }
        }

        public void Disconnect()
        {
            if (mqttClient.IsConnected)
            {
                mqttClient.Disconnect();
                Console.WriteLine("Disconnected from broker MQTT.");
            }
        }

        public void Subscribe(string topic)
        {
            if (!mqttClient.IsConnected)
                throw new Exception("Client MQTT not connected.");

            mqttClient.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            Console.WriteLine($"Subscribed topic: {topic}");
        }

        public void Publish(string topic, string message)
        {
            if (!mqttClient.IsConnected)
                throw new Exception("Client MQTT not connected.");

            mqttClient.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            Console.WriteLine($"Message published on topic {topic}: {message}");
        }

        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Message);
            Console.WriteLine($"Messaged received on topic {e.Topic}: {message}");
            // Aqui pode ser feita a lógica para tratar mensagens recebidas
        }
    }
}
