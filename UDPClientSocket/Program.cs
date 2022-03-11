using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UDPClientSocket
{
    class Program
    {
        static UdpClient udpClient = new UdpClient();
        static int HeaderSize = 1024;
        static bool debugBool = true;

        public static byte[] strToByteArray(string origString)
        {
            byte[] buff = new byte[origString.Length];
            buff = Encoding.UTF8.GetBytes(origString);
            //Debug 
            if (debugBool) Console.WriteLine("[DEBUG] Sending: "+Encoding.UTF8.GetString(buff));
            //End
            return buff;
        }

        public static byte[] addBuffer(byte[] msg)
        {
            byte[] msgCopy = msg;
            int buffToAdd = HeaderSize - msg.Length;
            for (var i = 0; i < buffToAdd; i++) {
                msgCopy.Append(Convert.ToByte(' '));
            }
            return msgCopy;
        }

        public static void sendClientMessage(string messageToSend)
        {
            var t_msgBytes = strToByteArray(messageToSend);
            //var msgBytes = addBuffer(t_msgBytes);
            udpClient.Client.Send(t_msgBytes);
        }
        static void Main(string[] args) {
            StartClient();
            Console.ReadKey();
        }

        static async void StartClient()
        {
            Console.Write("Enter (IP:Port) for UDP Server: ");
            string usrInput = Console.ReadLine();
            IPAddress serverIP = IPAddress.Parse(usrInput.Split(':')[0]);
            int port = int.Parse(usrInput.Split(':')[1]);
            udpClient.Client.Connect(serverIP, port);
            

            //Debug shit
            Console.WriteLine();
            Console.WriteLine("Bound: "+udpClient.Client.IsBound);
            Console.WriteLine("Connected: "+udpClient.Client.Connected);
            Console.WriteLine();
            //End


            Console.Write("Enter Message to send: ");
            sendClientMessage(Console.ReadLine());
            EndPoint endPoint= new IPEndPoint(serverIP, port);
            Console.Write("and again lmaooo this shi bussin: ");
            udpClient.Client.Send(addBuffer(strToByteArray(Console.ReadLine())));
            Console.ReadKey();
        }
    }
}
