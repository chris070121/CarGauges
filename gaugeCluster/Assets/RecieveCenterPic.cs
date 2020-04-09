using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class RecieveCenterPic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        texture = new Texture2D(10, 10);
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
    }

    // Update is called once per frame
    void Update () {
        if (client == null || !client.Connected)
        {
            Connect();

        }

        if (client.Connected)
        {
            ReadStream();
        }

        if (texture != null)
        {
            image.texture = texture;
        }
    }

    TcpClient client;
    TcpListener listener;
    int port = 9997;
    public RawImage image;
    private Texture2D texture;

    private void Connect()
    {
      
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
        try {
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[999999];
            using (MemoryStream ms = new MemoryStream())
            {
                //---read incoming stream---
                int bytesRead = nwStream.Read(buffer, 0, buffer.Length);
                ms.Write(buffer, 0, bytesRead);

                bool a = texture.LoadImage(buffer);
                Debug.Log("Image loaded = " + a);
            }
            nwStream.Close();

        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

}
