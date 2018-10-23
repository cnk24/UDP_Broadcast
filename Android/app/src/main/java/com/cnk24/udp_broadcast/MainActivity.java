package com.cnk24.udp_broadcast;

import android.net.ConnectivityManager;
import android.net.DhcpInfo;
import android.net.NetworkInfo;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;

public class MainActivity extends AppCompatActivity {

    private static final int PORT = 20003;

    private Receiver mReceiver = null;
    private TextView txtView = null;
    private Button btnHello = null;

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




        btnHello = (Button) findViewById(R.id.Hello);
        txtView = (TextView) findViewById(R.id.textView);

        btnHello.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mReceiver = new Receiver();
                mReceiver.start();
            }
        });
    }

    private NetworkInfo getNetworkinfo() {
        ConnectivityManager connectivityManager = (ConnectivityManager)getSystemService(CONNECTIVITY_SERVICE);
        NetworkInfo networkInfo = connectivityManager.getActiveNetworkInfo();
        return networkInfo;
    }






    class Receiver extends Thread {
        public void run() {

            try {
                DatagramSocket socket = new DatagramSocket(PORT);
                socket.setBroadcast(true);

                byte[] buf = new byte[1024];
                DatagramPacket packet = new DatagramPacket(buf, buf.length);
                socket.receive(packet);

                String text = new String(buf, 0, packet.getLength());
                Log.d("test","message:" + text);

                InetAddress ia = packet.getAddress();
                String str = new String(packet.getData()).trim();
                System.out.println(ia.getHostName()+" ===> "+str);


                socket.close();

                //btnHello.setEnabled(true);

            } catch(Exception e) {
                Log.d("test","error  " + e.toString());
            }

        }
    }

}
