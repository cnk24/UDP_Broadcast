using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP_Broadcast
{
    public partial class frmMain : Form
    {
        private const int PORT_NUMBER = 20003;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Send("TEST");


            //Thread thread = new Thread(new ThreadStart(ThreadProc));
            //thread.Start();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }



        public void Send(string message)
        {
            //UdpClient client = new UdpClient();
            //IPEndPoint ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), PORT_NUMBER);
            //client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            //byte[] bytes = Encoding.UTF8.GetBytes(message);
            //client.Send(bytes, bytes.Length, ip);
            //client.Close();


            //UdpClient udp = new UdpClient();
            //udp.EnableBroadcast = true;

            //udp.Client.ReceiveTimeout = 3000;

            //IPEndPoint broad = new IPEndPoint(IPAddress.Broadcast, PORT_NUMBER); // IPAddress.Broadcast == 255.255.255.255
            //byte[] buf = Encoding.UTF8.GetBytes(message);
            //udp.Send(buf, buf.Length, broad);




            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.EnableBroadcast = true;

            byte[] sendbuf = Encoding.ASCII.GetBytes("HELLO");
            EndPoint targetEndPoint = new IPEndPoint(IPAddress.Broadcast, PORT_NUMBER);

            // UDP 브로드캐스팅 패킷을 네트워크에 전송
            s.SendTo(sendbuf, targetEndPoint);


        }



        static private void ThreadProc()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 20003);
            UdpClient srv = new UdpClient(ipep);







            byte[] ttt = new byte[3];
            ttt[0] = 0x31;
            ttt[1] = 0x32;
            ttt[2] = 0x33;
            srv.Send(ttt, ttt.Length);




            //while (true)
            //{
            //    byte[] dgram = srv.Receive(ref remoteEP);
            //    Trace.WriteLine(string.Format("[Receive] {0} 로부터 {1} 바이트 수신", remoteEP.ToString(), dgram.Length));

            //    srv.Send(dgram, dgram.Length, remoteEP);
            //    Trace.WriteLine(string.Format("[Send] {0} 로 {1} 바이트 송신", remoteEP.ToString(), dgram.Length));
            //}



        }



    }
}
