using System;
using System.Threading.Tasks;
using System.Text;
using MQTTnet;
using MQTTnet.Server;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using MQTTnet.Client.Unsubscribing;
using System.Diagnostics;
using Czytnik123.DAL;
using System.Collections.Generic;
using Czytnik.Models;

namespace Czytnik123.Mqtt
{
    class MqttProgram
    {

        public static CardsContext xd;


        // MQTT  Server side 
        public static IMqttServer mqttServer;

        // MQTT  client 
        public static IMqttClient mqttClient;

        public static async void StartServer()
        {
            Debug.WriteLine("pfjiepfifsfhodfhohuofguifguifguisfguiasfguasfguia");
            try
            {
                //// 1.  establish  MQTT  Connection verification , Used for connection authentication 
                //MqttServerConnectionValidatorDelegate connectionValidatorDelegate = new MqttServerConnectionValidatorDelegate(
                //    p =>
                //    {
                //        // p  Represents the context of a link being initiated 

                //        //  client  id  verification 
                //        //  In most cases , We should use the equipment identification number to verify 
                //        if (p.ClientId == "twle_client")
                //        {
                //            //  User name and password authentication 
                //            //  In most cases , We should use client-side encryption  token  verification , That is, the client can  ID  After the corresponding key is encrypted  token
                //            if (p.Username != "yufei" && p.Password != "123456")
                //            {
                //                //  Validation failed , Tell the client , Authentication failure 
                //                p.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.BadUserNameOrPassword;
                //            }
                //        }
                //    }
                //);


                // 2.  establish  MqttServerOptions  Example , For customization  MQTT  All kinds of parameters 
                MqttServerOptions options = new MqttServerOptions();

                // 3.  Set various options 
                //  Client authentication 

                //  Set the server address and port number 
                options.DefaultEndpointOptions.Port = 1883;

                // 4.  establish  MqttServer  example 
                mqttServer = new MqttFactory().CreateMqttServer();

                // 5.  Set up  MqttServer  Properties of 
                //  Set message subscription notification 
                mqttServer.ClientSubscribedTopicHandler = new MqttServerClientSubscribedTopicHandlerDelegate(SubScribedTopic);
                //  Set message unsubscribe notification 
                mqttServer.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(UnScribedTopic);
                //  Set the message handler 
                mqttServer.UseApplicationMessageReceivedHandler(MessageReceived);
                //  Set the handler after the client connection is successful 
                mqttServer.UseClientConnectedHandler(ClientConnected);
                //  Set the handler after the client is disconnected 
                mqttServer.UseClientDisconnectedHandler(ClientDisConnected);

                //  Start the server 
                await mqttServer.StartAsync(options);
  


                Console.WriteLine(" Server started successfully ！ Press enter directly to stop the service ");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.Write($" Server start failed :{ex}");
            }
        }


        //  The client initiates a subscription to the topic notification 
        private static void SubScribedTopic(MqttServerClientSubscribedTopicEventArgs args)
        {
            //  Get client id 
            string clientId = args.ClientId;
            //  Get the subscription topic initiated by the client 
            string topic = args.TopicFilter.Topic;

            Debug.WriteLine($" client [{clientId}] Subscribed topics :{topic}");
        }

        //  The client unsubscribes from the topic notification 
        private static void UnScribedTopic(MqttServerClientUnsubscribedTopicEventArgs args)
        {
            //  Get client id 
            string clientId = args.ClientId;
            //  Get the subscription topic initiated by the client 
            string topic = args.TopicFilter;

            Debug.WriteLine($" client [{clientId}] Unsubscribed from topic :{topic}");
        }

        //  Receive messages from clients 
        private static void MessageReceived(MqttApplicationMessageReceivedEventArgs args)
        {
            //  Get the client ID of the message 
            string clientId = args.ClientId;
            //  Get the subject of the message 
            string topic = args.ApplicationMessage.Topic;
            //  Get the sent message content 
            string payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
            //  Get the sending level of the message (Qos)
            var qos = args.ApplicationMessage.QualityOfServiceLevel;
            //  Get the hold form of the message 
            bool retain = args.ApplicationMessage.Retain;



            Console.WriteLine($" client [{clientId}] >>  The theme : [{topic}]  Content : [{payload}] Qos: [{qos}] Retain:[{retain}]");

        }

        //  Processing notification after successful client connection 
        private static void ClientConnected(MqttServerClientConnectedEventArgs args)
        {
            //  Get client id 
            string clientId = args.ClientId;

            Console.WriteLine($" New client [{clientId}]  Join in ");
        }

        //  Client disconnect notification 
        private static void ClientDisConnected(MqttServerClientDisconnectedEventArgs args)
        {
            //  Get client id 
            string clientId = args.ClientId;

            Console.WriteLine($" New client [{clientId}]  Has left ");
        }




        public static void StartClient()
        {
            ConnectToServer();
            SendMessage();
        }


        private static void ConnectToServer()
        {
            try
            {
                // 1.  establish  MQTT  client 
                mqttClient = new MqttFactory().CreateMqttClient();


                // 2 .  Set up  MQTT  Client options 
                MqttClientOptionsBuilder optionsBuilder = new MqttClientOptionsBuilder();
                //  Set the server-side address 
                optionsBuilder.WithTcpServer("192.168.1.3", 1883);

                //  Set authentication parameters 

                //  Set the client serial number 

                //  Create options 
                IMqttClientOptions options = optionsBuilder.Build();


                //  Set the message receiving handler 
                mqttClient.UseApplicationMessageReceivedHandler(args => {
                    Console.WriteLine("###  Received a message from the server  ###");
                    //  Received message subject 
                    string topic = args.ApplicationMessage.Topic;
                    //  The content of the message received 
                    string payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
                    //  Send level received (Qos)
                    var qos = args.ApplicationMessage.QualityOfServiceLevel;
                    //  The received message remains in the form 
                    bool retain = args.ApplicationMessage.Retain;

                    Console.WriteLine($" The theme : [{topic}]  Content : [{payload}] Qos: [{qos}] Retain:[{retain}]");
                });

                //  Reconnection mechanism 
                mqttClient.UseDisconnectedHandler(async e =>
                {
                    Console.WriteLine(" The connection to the server was lost , Trying to reconnect ");
                    //  wait for  5s  Time 
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    try
                    {
                        //  Reconnect the 
                        await mqttClient.ConnectAsync(options);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Reconnection to the server failed :{ex}");
                    }
                });

                //  Connect to the server 
                mqttClient.ConnectAsync(options);
                Console.WriteLine(" Successfully connected to the server ！ Please enter any content and press enter to enter the menu interface ");


            }
            catch (Exception ex)
            {
                Console.Write($" Failed to connect to server : {ex}");
            }
        }

        public static void SendMessage()
        {
            Console.ReadLine();

            //  Exit or not 
            bool isExit = false;

            while (!isExit)
            {
                Console.WriteLine(@" Please enter 
                    1.  Subscribe to topics 
                    2.  Unsubscribe 
                    3.  Send a message 
                    4.  sign out ");

                string input = Console.ReadLine();
                string topic = "";

                switch (input)
                {
                    case "1":
                        Console.WriteLine(@" Please enter a topic name ：");
                        topic = Console.ReadLine();
                        ClientSubscribeTopic(topic);
                        break;
                    case "2":
                        Console.WriteLine(@" Please enter the subject name to unsubscribe from ：");
                        topic = Console.ReadLine();
                        ClientUnsubscribeTopic(topic);
                        break;
                    case "3":
                        Console.WriteLine(@" Please enter the subject name to be sent : ");
                        topic = Console.ReadLine();
                        Console.WriteLine(@" Please enter the content of the message to be sent ：");
                        string message = Console.ReadLine();
                        ClientPublish(topic, message);
                        break;
                    case "4":
                        isExit = true;
                        break;
                    default:
                        Console.WriteLine(" Please enter the correct command ");
                        break;
                }
            }
        }

        private static async void ClientSubscribeTopic(string topic)
        {
            topic = topic.Trim();
            if (string.IsNullOrEmpty(topic))
            {
                Console.Write(" The subscription topic cannot be empty ！");
                return;
            }

            //  Determine whether the client is connected 
            if (!mqttClient.IsConnected)
            {
                Console.WriteLine("MQTT  The client is not connected yet !");
                return;
            }

            //  Set subscription parameters 
            var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(topic)
                .Build();

            //  subscribe 
            await mqttClient.SubscribeAsync(
                    subscribeOptions,
                    System.Threading.CancellationToken.None);
        }

        private static async void ClientUnsubscribeTopic(string topic)
        {
            topic = topic.Trim();
            if (string.IsNullOrEmpty(topic))
            {
                Console.Write(" Unsubscribe subject cannot be empty ！");
                return;
            }

            //  Determine whether the client is connected 
            if (!mqttClient.IsConnected)
            {
                Console.WriteLine("MQTT  The client is not connected yet !");
                return;
            }

            //  Set subscription parameters 
            var subscribeOptions = new MqttClientUnsubscribeOptionsBuilder()
                .WithTopicFilter(topic)
                .Build();


            //  unsubscribe 
            await mqttClient.UnsubscribeAsync(
                    subscribeOptions,
                    System.Threading.CancellationToken.None);
        }


        private async static void ClientPublish(string topic, string message)
        {
            topic = topic.Trim();
            message = message.Trim();

            if (string.IsNullOrEmpty(topic))
            {
                Console.Write(" Unsubscribe subject cannot be empty ！");
                return;
            }

            //  Determine whether the client is connected 
            if (!mqttClient.IsConnected)
            {
                Console.WriteLine("MQTT  The client is not connected yet !");
                return;
            }

            //  Fill in the message 
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)       //  The theme 
                .WithPayload(message)   //  news 
                .WithExactlyOnceQoS()   // qos
                .WithRetainFlag()       // retain
                .Build();

            await mqttClient.PublishAsync(applicationMessage);
        }
    }
}