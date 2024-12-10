using System;
using System.Net;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SOMIODMiddleware.Helper
{
    public class ConnHelper
    {
        private readonly MqttClient client;
        private readonly string brokerIp = "127.0.0.1"; // Default broker IP (can be configured)

        public ConnHelper()
        {
            // Initialize MQTT client
            client = new MqttClient(IPAddress.Parse(brokerIp));
        }

        /// <summary>
        /// Emit a message to a specific MQTT topic.
        /// </summary>
        /// <param name="topicName">The topic to publish the message to.</param>
        /// <param name="content">The message content.</param>
        public void EmitMessageToTopic(string topicName, string content)
        {
            try
            {
                // Connect to the broker if not already connected
                if (!client.IsConnected)
                {
                    ConnectClient();
                }

                // Publish the message
                client.Publish(
                    topicName,
                    Encoding.UTF8.GetBytes(content),
                    MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                    false
                );

                // Log the emitted message
                LogBrokerMessage(topicName, content, "Broker", "System");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error emitting message to topic {topicName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Subscribe to one or more MQTT topics.
        /// </summary>
        /// <param name="topicNames">Array of topic names to subscribe to.</param>
        public void SubscribeToTopics(string[] topicNames)
        {
            try
            {
                // Connect to the broker if not already connected
                if (!client.IsConnected)
                {
                    ConnectClient();
                }

                // Subscribe to the provided topics
                client.Subscribe(
                    topicNames,
                    new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE }
                );

                // Assign callback for received messages
                client.MqttMsgPublishReceived += HandleNotificationEventsClientSide;

                Console.WriteLine($"Subscribed to topics: {string.Join(", ", topicNames)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error subscribing to topics: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles incoming messages from subscribed topics.
        /// </summary>
        private void HandleNotificationEventsClientSide(object sender, MqttMsgPublishEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Message);

            if (e.Topic.Contains("creation"))
            {
                Console.WriteLine($"Creation event received on topic {e.Topic}: {message}");
                // TODO: Add actions to handle creation events (e.g., notify clients, update state).
            }
            else if (e.Topic.Contains("deletion"))
            {
                Console.WriteLine($"Deletion event received on topic {e.Topic}: {message}");
                // TODO: Add actions to handle deletion events (e.g., notify clients, update state).
            }

            // Log received messages
            LogBrokerMessage(e.Topic, message, "Client", "Broker");
        }

        /// <summary>
        /// Logs messages sent/received through the MQTT broker.
        /// </summary>
        public void LogBrokerMessage(string topicName, string content, string receiver, string sender)
        {
            Console.WriteLine($"[LOG] Topic: {topicName}, Content: {content}, Receiver: {receiver}, Sender: {sender}");
            // Additional logging logic can be added here (e.g., save to a database or file).
        }

        /// <summary>
        /// Connects the MQTT client to the broker.
        /// </summary>
        private void ConnectClient()
        {
            try
            {
                client.Connect(Guid.NewGuid().ToString());
                Console.WriteLine("Connected to MQTT broker.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MQTT broker: {ex.Message}");
            }
        }

        /// <summary>
        /// Disconnects the MQTT client from the broker.
        /// </summary>
        public void DisconnectClient()
        {
            if (client.IsConnected)
            {
                client.Disconnect();
                Console.WriteLine("Disconnected from MQTT broker.");
            }
        }
    }
}