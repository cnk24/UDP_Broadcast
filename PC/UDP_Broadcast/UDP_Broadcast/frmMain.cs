using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP_Broadcast
{
    public partial class frmMain : Form
    {
        public enum COMM_STEP
        {
            JOIN,
            JOININFO,
            PHONEINFO,
            ALBUMNAME,
            IMAGEINFO,
            IMAGEFILE,
            VIDEOINFO,
            VIDEOFILE,
            END
        }
        private COMM_STEP eCommStep;

        private const int PORT_NUMBER = 20003;

        private volatile bool _shouldStopUDP;
        private volatile bool _shouldStopTCP;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            InitTreeView();

            Thread udp = new Thread(new ThreadStart(UDPThreadProc));
            udp.Start();

            Thread tcp = new Thread(new ThreadStart(TCPThreadProc));
            tcp.Start();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _shouldStopUDP = true;
            _shouldStopTCP = true;
        }

        private void InitTreeView()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo di in allDrives)
            {
                // 드라이브가 준비되었는지 여부 판단. 이 조건으로 CD R/W 드라이브 등을 제외시킴
                if (di.IsReady)
                {
                    TreeNode rootNode = new TreeNode(di.Name);
                    rootNode.ImageKey = "disk";
                    rootNode.SelectedImageKey = "disk";
                    treeView1.Nodes.Add(rootNode);
                    Fill(rootNode);
                }
            }

            treeView1.Nodes[0].Expand();
        }

        // 트리뷰에서 +버튼으로 항목을 펼치기 직전에 수행할 작업
        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text.Equals("*"))
            {
                e.Node.Nodes.Clear();
                Fill(e.Node);
            }
        }

        // 트리뷰에서 +버튼으로 항목을 펼친 후에 수행할 작업
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectTreeView(e.Node);
        }

        // 트리노드에 디렉토리 정보 채우기
        private void Fill(TreeNode dirNode)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(dirNode.FullPath);
                foreach (DirectoryInfo dirItem in dir.GetDirectories())
                {
                    if ((dirItem.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        // Add node for the directory
                        TreeNode newNode = new TreeNode(dirItem.Name);
                        newNode.ImageKey = "folder";
                        newNode.SelectedImageKey = "folder";
                        dirNode.Nodes.Add(newNode);
                        newNode.Nodes.Add("*");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error :" + e.Message);
            }
        }

        private void SelectTreeView(TreeNode node)
        {
            if (node.FullPath == null)
            {
                Console.WriteLine("empth node.FullPath");
                return;
            }

            string path = node.FullPath;

            AddListViewLargeImageItem(path);
        }

        /// <summary>
        /// 해당 폴더 경로의 아이콘들을 ListView에 추가한다.
        /// </summary>
        /// <param name="p"></param>
        private void AddListViewLargeImageItem(string folderPath)
        {
            DirectoryInfo info = new DirectoryInfo(folderPath);
            lb_path.Text = info.FullName;

            listView1.Items.Clear();

            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(64, 64);
            imageList = imageList_64x64;

            AddFolderIcon(ref imageList, folderPath);
            AddFileIcon(ref imageList, folderPath);

            listView1.LargeImageList = imageList;
        }

        /// <summary>
        /// 폴더의 아이콘을 추가합니다.
        /// </summary>
        /// <param name="imageList"></param>
        /// <param name="folderPath"></param>
        private void AddFolderIcon(ref ImageList imageList, string folderPath)
        {
            string[] folders = Directory.GetDirectories(folderPath);

            foreach (string folder in folders)
            {
                DirectoryInfo info = new DirectoryInfo(folder);

                if ((info.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {                    
                    listView1.Items.Add(info.Name, imageList.Images.IndexOfKey("folder"));
                }
            }
        }

        /// <summary>
        /// 파일의 아이콘을 추가합니다.
        /// </summary>
        /// <param name="imageList"></param>
        /// <param name="folderPath"></param>
        private void AddFileIcon(ref ImageList imageList, string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);

                if (CheckImageFile(info.Extension))
                {
                    Image image = GetThumbnail(info.FullName);
                    imageList.Images.Add(image);
                }
                else
                {
                    Icon icon = IconHelper.GetIcon(info.FullName, true);
                    imageList.Images.Add(icon);
                }
                
                listView1.Items.Add(info.Name, imageList.Images.Count - 1);
            }
        }

        
        private bool CheckImageFile(string ext)
        {
            if (ext.Equals(".png") || ext.Equals(".jpg") || ext.Equals(".gif") || ext.Equals(".bmp") || ext.Equals(".tif"))
            {
                return true;
            }

            return false;
        }

        private Image GetThumbnail(string filePath)
        {
            Image image = Image.FromFile(filePath);
            int nRate = 0;

            if (image.Width > image.Height)
            {
                nRate = image.Width / 64;
            }
            else
            {
                nRate = image.Height / 64;
            }

            int nWidth = image.Width / nRate;
            int nHeight = image.Height / nRate;

            Image thumb = image.GetThumbnailImage(nWidth, nHeight, () => false, IntPtr.Zero);
            image.Dispose();

            return thumb;
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


        private void UDPThreadProc()
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
            UdpClient socket = new UdpClient(remoteEP);
            socket.Client.ReceiveTimeout = 3000;

            while (!_shouldStopUDP)
            {
                try
                {
                    byte[] data = socket.Receive(ref remoteEP);

                    string sRecvData = Encoding.ASCII.GetString(data);

                    if (sRecvData.Equals("JOIN"))
                    {
                        this.Invoke(new Action(delegate ()
                        {
                            lb_state_value.Text = string.Empty;
                            lb_info_value.Text = string.Empty;
                        }));

                        byte[] buf = Encoding.UTF8.GetBytes("JOIN_OK");
                        socket.Send(buf, buf.Length, remoteEP);

                        eCommStep = COMM_STEP.JOININFO;

                        _shouldStopUDP = true;
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(string.Format("UDP Error [{0}]", e.Message));
                }
            }

            socket.Close();
        }


        private void TCPThreadProc()
        {
            BinaryWriter writer = null;
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
            server.Bind(ipep);
            server.Listen(1);

            Socket client = server.Accept();

            try
            {
                while (!_shouldStopTCP)
                {
                    byte[] data = new byte[1024];
                    client.Receive(data);

                    string sRecvData = Encoding.ASCII.GetString(data);

                    switch (eCommStep)
                    {
                        case COMM_STEP.JOININFO:
                            {
                                this.Invoke(new Action(delegate ()
                                {
                                    lb_state_value.Text = sRecvData;
                                }));

                                byte[] buf = Encoding.UTF8.GetBytes("OK");
                                client.Send(buf);

                                eCommStep = COMM_STEP.PHONEINFO;
                            }
                            break;
                        case COMM_STEP.PHONEINFO:
                            {
                                this.Invoke(new Action(delegate ()
                                {
                                    lb_info_value.Text = sRecvData;
                                }));

                                byte[] buf = Encoding.UTF8.GetBytes("OK");
                                client.Send(buf);

                                eCommStep = COMM_STEP.ALBUMNAME;
                            }
                            break;
                        case COMM_STEP.ALBUMNAME:
                            {

                            }
                            break;
                        case COMM_STEP.IMAGEINFO:
                            {

                            }
                            break;
                        case COMM_STEP.IMAGEFILE:
                            {
                                // 1. 파일명 받기
                                // 2. 파일 전송 받기

                                string filePath = "" + sRecvData;

                                FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                                writer = new BinaryWriter(fileStream);

                                while (client.Receive(data) > 0)
                                {
                                    writer.Write(data, 0, data.Length);
                                }


                            }
                            break;
                        case COMM_STEP.VIDEOINFO:
                            {

                            }
                            break;
                        case COMM_STEP.VIDEOFILE:
                            {

                            }
                            break;
                        case COMM_STEP.END:
                            {

                            }
                            break;
                    }
                }
            }
            catch (SocketException e)
            {
                Trace.WriteLine(string.Format("TCP Socket Error [{0}]", e.Message));
            }
            catch (Exception e)
            {
                Trace.WriteLine(string.Format("TCP Error [{0}]", e.Message));
            }
            finally
            {
                if (writer != null) writer.Close();
                if (client != null) client.Close();
                if (server != null) server.Close();
            }

        }



    }
}
