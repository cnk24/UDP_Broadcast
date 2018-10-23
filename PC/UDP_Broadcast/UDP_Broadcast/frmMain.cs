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
            


            //Thread thread = new Thread(new ThreadStart(ThreadProc));
            //thread.Start();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        private string getIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }



        public void BroadcastFind(string message)
        {

            //string ipAddr = getIPAddress();





            UdpClient udp = new UdpClient();
            udp.EnableBroadcast = true;

            udp.Client.ReceiveTimeout = 3000;

            IPEndPoint broad = new IPEndPoint(IPAddress.Broadcast, PORT_NUMBER); // IPAddress.Broadcast == 255.255.255.255

            byte[] buf = Encoding.UTF8.GetBytes(message);
            udp.Send(buf, buf.Length, broad);




        }



        static private void ThreadProc()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
            UdpClient srv = new UdpClient(ipep);



            //while (true)
            //{
            //    byte[] dgram = srv.Receive(ref remoteEP);
            //    Trace.WriteLine(string.Format("[Receive] {0} 로부터 {1} 바이트 수신", remoteEP.ToString(), dgram.Length));

            //    srv.Send(dgram, dgram.Length, remoteEP);
            //    Trace.WriteLine(string.Format("[Send] {0} 로 {1} 바이트 송신", remoteEP.ToString(), dgram.Length));
            //}



        }



        private void btn_test_Click(object sender, EventArgs e)
        {
            BroadcastFind("TEST");
        }




    }
}
