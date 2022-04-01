using System;
using System.Threading.Tasks;
using System.Text;
using MQTTnet;
using MQTTnet.Server;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using MQTTnet.Client.Unsubscribing;

namespace Czytnik123.Mqtt
{
    class Program
    {
        // MQTT  Server side 
        public static IMqttServer mqttServer;

        // MQTT  client 
        public static IMqttClient mqttClient;

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("usage [dotnet run server] start server daemon");
                Console.WriteLine("usage [dotnet run client] start client daemon");
            }

            if (args[0] == "server")
            {
                StartServer();
            }

            if (args[0] == "client")
            {
                StartClient();
            }
        }

        private static async void StartServer()
        {
            try
            {
                // 1.  establish  MQTT  Connection verification , Used for connection authentication 
                MqttServerConnectionValidatorDelegate connectionValidatorDelegate = new MqttServerConnectionValidatorDelegate(
                    p =>
                    {
                        // p  Represents the context of a link being initiated 

                        //  client  id  verification 
                        //  In most cases , We should use the equipment identification number to verify 
                        if (p.ClientId == "twle_client")
                        {
                            //  User name and password authentication 
                            //  In most cases , We should use client-side encryption  token  verification , That is, the client can  ID  After the corresponding key is encrypted  token
                            if (p.Username != "yufei" && p.Password != "123456")
                            {
                                //  Validation failed , Tell the client , Authentication failure 
                                p.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.BadUserNameOrPassword;
                            }
                        }
                    }
                );


                // 2.  establish  MqttServerOptions  Example , For customization  MQTT  All kinds of parameters 
                MqttServerOptions options = new MqttServerOptions();

                // 3.  Set various options 
                //  Client authentication 
                options.ConnectionValidator = connectionValidatorDelegate;

                //  Set the server address and port number 
                options.DefaultEndpointOptions.Port = 8001;

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






        private static void StartClient()
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
                optionsBuilder.WithTcpServer("127.0.0.1", 8001);

                //  Set authentication parameters 
                optionsBuilder.WithCredentials("yufei", "123456");

                //  Set the client serial number 
                optionsBuilder.WithClientId(Guid.NewGuid().ToString());

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
    }
}