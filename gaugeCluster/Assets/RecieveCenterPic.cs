using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class RecieveCenterPic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        texture = new Texture2D(10, 10);
        //Thread a = new Thread(TestThis);
        //a.Start();
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
    }

    // Update is called once per frame
    void Update () {
       TestThis();

        if (texture != null)
        {
            texture.LoadImage(buffer);
            image.texture = texture;

            seatBelt.text = _seatBelt;
            leftSignal.text = _leftSignal;
            rightSignal.text = _rightSignal;
            emergencyBrake.text = _emergencyBrake;
            lightSymbol.text = _lightSymbol;
            reverseSymbol.text = _reverseSymbol;
            if(_brightnessUp ==  true)
            {
                Color temp = brightnessControllerImage.color;
                temp.a = 0.0f;
                brightnessControllerImage.color = temp;
            }
            else
            {
                Color temp = brightnessControllerImage.color;
                temp.a = 1.0f;
                brightnessControllerImage.color = temp;

            }


        }
    }

    TcpClient client;
    TcpListener listener;
    int port = 9997;
    public RawImage image;
    private Texture2D texture;
    public Text seatBelt;
    public Text leftSignal;
    public Text rightSignal;
    public Text emergencyBrake;
    public Text lightSymbol;
    public Text reverseSymbol;
    public Image brightnessControllerImage;
    string _seatBelt;
    string _leftSignal;
    string _rightSignal;
    string _emergencyBrake;
    string _lightSymbol;
    string _reverseSymbol;
    bool _brightnessUp = false;


    byte[] bytes;
    byte[] buffer;
    private void TestThis()
    {
      
        //while (true)
        //{
            //Debug.Log("Waiting for a connection... ");
            try
            {
                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Connected!");

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();


                buffer = new byte[999999];
                stream.Read(buffer, 0, buffer.Length);
            string[] message = Encoding.Default.GetString(buffer).Split(',');
            foreach (string a in message)
            {
                string[] result = a.Split('=');
                
                if (result[0].Contains("LightSymbol"))
                {
                    _lightSymbol = "LightSymbol= " + result[1];
                }
                else if (result[0].Contains("SeatBeltSymbol"))
                {
                    _seatBelt = "SeatBeltSymbol= " + result[1];
                }
                else if(result[0].Contains("ReverseSymbol"))
                {
                    _reverseSymbol = "ReverseSymbol= " + result[1];
                }
                else if(result[0].Contains("RightTurnSignal"))
                {
                    _rightSignal = "RightTurnSignal= " + result[1];
                }
                else if(result[0].Contains("LeftTurnSignal"))
                {
                    _leftSignal = "LeftTurnSignal = " + result[1];
                }
                else if(result[0].Contains("EmergencyBrake"))
                {
                   _emergencyBrake = "EmergencyBrake = " + result[1];
                }
                else if (result[0].Contains("BrightnessUp"))
                {
                    if (result[1].Contains("true"))
                    {
                        _brightnessUp = true;
                    }
                    else
                    {
                        _brightnessUp = false;
                    }
                }
            }
       
        //ms.Close();
                stream.Close();
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
            }
        //}
    }

    
}
