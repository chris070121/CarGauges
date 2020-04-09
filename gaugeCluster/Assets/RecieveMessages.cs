using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class RecieveMessages : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        if(client == null || !client.Connected)
        {
            Connect();
        }

        if(client.Connected)
        {
            ReadStream();
        }
    }

    TcpClient client;
    TcpListener listener;
    int port = 9999;

    private void Connect()
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        try
        {
            client = listener.AcceptTcpClient();
            if (client.Connected)
            {
                Debug.Log("Client connected");
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }        
       
    }

    private void ReadStream()
    {
        client = listener.AcceptTcpClient();
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---read incoming stream---
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

        string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Debug.Log(dataReceived);
    }

}
