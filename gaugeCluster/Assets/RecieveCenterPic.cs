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

public class RecieveCenterPic : MonoBehaviour
{

    // Use this for initialization
    void Start()
    { 
        texture = new Texture2D(10, 10);
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        a = new Thread(TestThis);
        a.Start();
    }
    Thread a;

    void OnDestroy()
    {
        a.Abort();
    }
    // Update is called once per frame
    void Update()
    {
        //TestThis();

        if (texture != null)
        {
            texture.LoadImage( bytes);
            image.texture = texture;

            seatBelt.text = _seatBelt;
            leftSignal.text = _leftSignal;
            rightSignal.text = _rightSignal;
            emergencyBrake.text = _emergencyBrake;
            lightSymbol.text = _lightSymbol;
            checkEngineSymbol.text = _checkEngineSymbol;
            reverseSymbol.text = _reverseSymbol;
            if (_brightnessUp == true)
            {
                Color temp = brightnessControllerImage.color;
                temp.a = 0.0f;
                brightnessControllerImage.color = temp;

                temp = brightnessControllerImage2.color;
                temp.a = 0.0f;
                brightnessControllerImage2.color = temp;
            }
            else
            {
                Color temp = brightnessControllerImage.color;
                temp.a = .5f;
                brightnessControllerImage.color = temp;

                temp = brightnessControllerImage2.color;
                temp.a = .5f;
                brightnessControllerImage2.color = temp;

            }


        }
    }

    TcpListener listener;
    TcpClient client;
    NetworkStream stream;
    int port = 9997;
    public RawImage image;
    private Texture2D texture;
    public Text seatBelt;
    public Text leftSignal;
    public Text rightSignal;
    public Text emergencyBrake;
    public Text lightSymbol;
    public Text reverseSymbol;
    public Image brightnessControllerImage2;
    public Image brightnessControllerImage;
    public Text checkEngineSymbol;

    string _seatBelt;
    string _leftSignal;
    string _rightSignal;
    string _emergencyBrake;
    string _lightSymbol;
    string _reverseSymbol;
    string _checkEngineSymbol;
    bool _brightnessUp = false;

    byte[] bytes;
    byte[] buffer;
    private void TestThis()
    {
        client = listener.AcceptTcpClient();
        stream = client.GetStream();

        while (true)
        {
            try
        {
                Thread.Sleep(100);
                Debug.Log("Connected!");

                // Get a stream object for reading and writing


                buffer = new byte[999999];
                stream.Read(buffer, 0, buffer.Length);
                bytes = (byte[])buffer.Clone();
                Debug.Log("MessageRead");
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
                    else if (result[0].Contains("ReverseSymbol"))
                    {
                        _reverseSymbol = "ReverseSymbol= " + result[1];
                    }
                    else if (result[0].Contains("RightTurnSignal"))
                    {
                        _rightSignal = "RightTurnSignal= " + result[1];
                    }
                    else if (result[0].Contains("LeftTurnSignal"))
                    {
                        _leftSignal = "LeftTurnSignal = " + result[1];
                    }
                    else if (result[0].Contains("EmergencyBrake"))
                    {
                        _emergencyBrake = "EmergencyBrake = " + result[1];
                    }
                    else if (result[0].Contains("CheckEngine"))
                    {
                        _checkEngineSymbol = "CheckEngine = " + result[1];
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

            }
            catch (Exception ex)
            {
                stream.Close();

                Debug.Log(ex.Message);
                client = listener.AcceptTcpClient();
                stream = client.GetStream();


            }
        }
    }

   
}

