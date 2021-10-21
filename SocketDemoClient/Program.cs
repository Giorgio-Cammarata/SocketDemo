using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketDemoClient
{
    // Client app is the one sending messages to a Server/listener.   
    // Both listener and client can send messages back and forth once a   
    // communication is established.  
    public class SocketClient
    {
        public static int Main(String[] args)
        {
            StartClient();
            return 0;
        }

        public static void StartClient()
        {
            byte[] bytes = new byte[1024];

            try
            {
                // Get Host IP Address that is used to establish a connection  
                // (If a host has multiple addresses, you will get a list of addresses)  
                IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                Socket sender = new Socket(
                    ipAddress.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp);


                // Connect to Remote server  
                sender.Connect(remoteEP);

                Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                int bytesSent = sender.Send(msg);

                int bytesRec = sender.Receive(bytes);

                Console.WriteLine("Echoed test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

                sender.Dispose();
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
        }
    }
}
