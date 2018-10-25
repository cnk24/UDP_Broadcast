package com.cnk24.udp_broadcast;

import android.net.ConnectivityManager;
import android.net.DhcpInfo;
import android.net.NetworkInfo;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.os.Handler;
import android.os.Message;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.Socket;
import java.net.SocketTimeoutException;

public class MainActivity extends AppCompatActivity {

    public static final int COMM_SETP_JOININFO = 0;
    public enum COMM_STEP
    {
        JOIN,
        JOININFO,
        PHONEINFO,
        ALBUMNAME,
        IMAGE,
        VIDEO,
        END
    }
    public COMM_STEP eCommStep;

    public static final int PORT = 20003;

    public TextView txtView = null;
    public Button btnJoin = null;
    public Button btnReJoin = null;
    public Button btnStart = null;

    private volatile boolean _shouldStop;
    private InetAddress mServerIP;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        NetworkInfo networkState = getNetworkinfo();
        if (networkState != null && networkState.isConnected()) {
            if (networkState.getType() == ConnectivityManager.TYPE_WIFI) {
                // WIFI 네트워크 연결됨
            } else if (networkState.getType() == ConnectivityManager.TYPE_MOBILE) {
                // 모바일(3G/LTE) 네트워크에 연결됨
            }
        } else {
            // 네트워크가 연결되지 않았습니다.
        }

        txtView = (TextView) findViewById(R.id.textView);
        btnJoin = (Button) findViewById(R.id.Join);
        btnReJoin = (Button) findViewById(R.id.ReJoin);
        btnStart = (Button) findViewById(R.id.Start);

        btnJoin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                eCommStep = COMM_STEP.JOIN;

                BroadcastSend broadcast = new BroadcastSend();
                broadcast.start();
            }
        });

        btnReJoin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                eCommStep = COMM_STEP.JOIN;
            }
        });

        btnStart.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (eCommStep == COMM_STEP.ALBUMNAME) {
                    FileTrans trans = new FileTrans();
                    trans.start();
                }
            }
        });

    }

    @Override
    protected void onDestroy() {
        super.onDestroy();

        _shouldStop = true;
    }

    // 버튼 컨트롤 메세지 핸들러
    final Handler handler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
            switch (msg.what)
            {
                case 1:
                    btnJoin.setEnabled(false);
                    break;
                case 2:
                    btnJoin.setEnabled(true);
                    break;
            }
        }
    };



    public NetworkInfo getNetworkinfo() {
        ConnectivityManager connectivityManager = (ConnectivityManager)getSystemService(CONNECTIVITY_SERVICE);
        NetworkInfo networkInfo = connectivityManager.getActiveNetworkInfo();
        return networkInfo;
    }

    public String getIPAddress() {
        WifiManager mWifi = (WifiManager)getSystemService(WIFI_SERVICE);
        DhcpInfo dhcp = mWifi.getDhcpInfo();
        String ipAddress = String.format(
                "%d.%d.%d.%d",
                (dhcp.ipAddress & 0xff),
                (dhcp.ipAddress >> 8 & 0xff),
                (dhcp.ipAddress >> 16 & 0xff),
                (dhcp.ipAddress >> 24 & 0xff));

        //int broadcast = (dhcp.ipAddress & dhcp.netmask) | ~dhcp.netmask;
        //byte[] quads = new byte[4];
        //for(int k = 0; k < 4 ; k++) {
        //    quads[k] = (byte) ((broadcast >> k * 8) & 0xFF);
        //}
        //Log.d("UDP","main ipadd1 = : " + dhcp.ipAddress);
        //Log.d("UDP","main ipadd2 = : " + dhcp.netmask);
        //Log.d("UDP","main ipadd3 = : " + InetAddress.getByAddress(quads));

        return ipAddress;
    }

    public String getDeviceName() {
        String manufacturer = Build.MANUFACTURER;
        String model = Build.MODEL;
        if (model.startsWith(manufacturer)) {
            return capitalize(model);
        } else {
            return capitalize(manufacturer) + " " + model;
        }
    }

    private String capitalize(String s) {
        if (s == null || s.length() == 0) {
            return "";
        }
        char first = s.charAt(0);
        if (Character.isUpperCase(first)) {
            return s;
        } else {
            return Character.toUpperCase(first) + s.substring(1);
        }
    }



    class BroadcastSend extends Thread {
        public void run() {
            try {
                handler.sendEmptyMessage(1);

                DatagramSocket socket = new DatagramSocket();
                socket.setSoTimeout(3000);

                String sendData = "JOIN";
                DatagramPacket packet = new DatagramPacket(sendData.getBytes(), sendData.getBytes().length, InetAddress.getByName("255.255.255.255"), PORT);
                socket.send(packet);

                byte[] buf = new byte[20];
                packet.setData(buf);
                socket.receive(packet);

                String sRecvData = new String(packet.getData()).trim();
                Log.d("UDP","Return Data =>  " + sRecvData);

                if (sRecvData.equals("JOIN_OK")) {
                    eCommStep = COMM_STEP.JOININFO;
                    mServerIP = packet.getAddress();

                    InfoTrans trans = new InfoTrans();
                    trans.start();
                }

                socket.close();
            } catch(SocketTimeoutException e) {
                // 타임 아웃 처리
                handler.sendEmptyMessage(2);

                Log.d("UDP","SocketTimeout  " + e.toString());
            } catch(Exception e) {
                Log.d("UDP","error  " + e.toString());
            }
            finally {

            }
        }
    }

    class InfoTrans extends Thread {
        public void run() {
            try {
                _shouldStop = false;

                Socket socket = new Socket(mServerIP, PORT);
                BufferedReader socket_in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
                PrintWriter socket_out = new PrintWriter(socket.getOutputStream(), true);

                while (!_shouldStop)
                {
                    String sSendData = null;

                    if (eCommStep == COMM_STEP.JOININFO) {
                        sSendData = String.format("%s", getIPAddress());
                        //packet.setData(sSendData.getBytes());
                        //socket.send(packet);
                    }
                    else if (eCommStep == COMM_STEP.PHONEINFO) {
                        sSendData = String.format("%s", getDeviceName());
                        //packet.setData(sSendData.getBytes());
                        //socket.send(packet);
                    }

                    if (sSendData == null) continue;

                    socket_out.print(sSendData);
                    socket_out.flush();

                    char[] data = new char[100];
                    socket_in.read(data);

                    String sRecvData = new String(data).trim();
                    Log.d("UDP","Return Data =>  " + sRecvData);

                    if (sRecvData.equals("OK")) {
                        if (eCommStep == COMM_STEP.JOININFO) {
                            eCommStep = COMM_STEP.PHONEINFO;
                        }
                        else if(eCommStep == COMM_STEP.PHONEINFO) {
                            eCommStep = COMM_STEP.ALBUMNAME;
                            _shouldStop = true;
                        }
                    }

                }

                socket_in.close();
                socket_out.close();
                socket.close();
            } catch(IOException e) {
                Log.d("UDP","error  " + e.toString());
            } catch(Exception e) {
                Log.d("UDP","error  " + e.toString());
            }
            finally {

            }
        }
    }


    class FileTrans extends Thread {
        public void run() {
            try {
                _shouldStop = false;

                while (!_shouldStop)
                {
                    DatagramSocket socket = new DatagramSocket(PORT, mServerIP);

                    String sRecvData;
                    byte[] buf = new byte[1024];
                    DatagramPacket packet = new DatagramPacket(buf, buf.length);

                    if (eCommStep == COMM_STEP.ALBUMNAME) {

                    }
                    else if (eCommStep == COMM_STEP.IMAGE) {

                    }
                    else if (eCommStep == COMM_STEP.VIDEO) {

                    }
                    else if (eCommStep == COMM_STEP.END) {
                        _shouldStop = true;
                    }

                    socket.close();
                }

            } catch(Exception e) {
                Log.d("UDP","error  " + e.toString());
            }
        }
    }

}
