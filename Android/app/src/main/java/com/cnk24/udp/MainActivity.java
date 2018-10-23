package com.cnk24.udp;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import java.net.DatagramPacket;
import java.net.DatagramSocket;

public class MainActivity extends AppCompatActivity {

    public static final int PORT = 20003;

    public Receiver mReceiver = null;
    public TextView txtView = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Button btnHello = (Button) findViewById(R.id.Hello);
        txtView = (TextView) findViewById(R.id.textView);

        btnHello.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mReceiver = new Receiver();
                mReceiver.start();
            }
        });

    }

    class Receiver extends Thread {
        public void run() {
            String text;
            byte[] message = new byte[1024];
            try{
                DatagramPacket p = new DatagramPacket(message, message.length);
                DatagramSocket s = new DatagramSocket(PORT);
                s.receive(p);
                text = new String(message, 0, p.getLength());
                Log.d("test","message:" + text);
                s.close();
            }catch(Exception e){
                Log.d("test","error  " + e.toString());
            }

        }
    }

}
