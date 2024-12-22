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

        public Mosquitto(string brokerAddress = "127.0.0.1")
        {
            this.brokerAddress = brokerAddress;
            mqttClient = new MqttClient(brokerAddress);
        }

        public void Connect()
        {
            try
            {
                mqttClient.Connect(Guid.NewGuid().ToString());
                if (!mqttClient.IsConnected)
                    throw new Exception("Failed to connect to MQTT broker.");
                Console.WriteLine("Connected to MQTT broker.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MQTT broker: {ex.Message}");
                throw;
            }
        }

        public void Publish(string topic, string message)
        {
            if (!mqttClient.IsConnected)
                throw new Exception("MQTT client is not connected.");

            mqttClient.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            Console.WriteLine($"Published message to topic {topic}: {message}");
        }

        public void Disconnect()
        {
            if (mqttClient.IsConnected)
            {
                mqttClient.Disconnect();
                Console.WriteLine("Disconnected from MQTT broker.");
            }
        }
    }
}
