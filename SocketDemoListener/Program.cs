using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketDemoListener
{
    // Socket Listener acts as a server and listens to the incoming   
    // messages on the specified port and protocol.  
    public class SocketListener
    {
        public static int Main(String[] args)
        {
            StartServer();
            return 0;
        }

        public static void StartServer()
        {
            try
            {
                // Get Host IP Address that is used to establish a connection                
                // (If a host has multiple addresses, you will get a list of addresses)            
                IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

                Socket listener = new Socket(
                    ipAddress.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp);

                listener.Bind(localEndPoint);

                // Specify how many requests a Socket can listen before it gives Server busy response.  
                listener.Listen(10);

                Console.WriteLine("Waiting for a connection...");
                Socket handler = listener.Accept();

                // Incoming data from the client.    
                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }

                Console.WriteLine("Text received : {0}", data);

                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
                
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

                listener.Dispose();
                handler.Dispose();
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();
        }
    }
}
