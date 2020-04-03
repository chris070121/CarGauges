using UnityEngine;
using System.Collections;
using System.Threading;
using System.IO.Ports;
using System;

public class Master : MonoBehaviour {
    public NeedleTemp tachNeedleScript;
    public NeedleTemp speedNeedleScript;
    public NeedleTemp fuelNeedleScript;
    public NeedleTemp engTempNeedleScript;
    public NeedleTemp oilTempNeedleScript;
    private SerialPort stream = new SerialPort("COM6", 9600); //Set the port (com4) and the baud rate (9600, is standard on most devices)
    private string arduinoValue = "";
    // Use this for initialization
    void Start () {
        try
        {
            //stream.Open();
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    
	// Update is called once per frame
	void Update () {
     /*   if (stream.IsOpen)
        {
            arduinoValue = stream.ReadLine();
            ParseArduinoValue(arduinoValue);
            GetRPM();
            GetSpeed();
            GetFuel();
            GetEngTemp();
            //GetOilTemp();
        }
        else
        {
            stream.Open();
        }*/
    }

  
    public void GetRPM()
    {
        tachNeedleScript.speed = RPM;
        tachNeedleScript.numberForTextLabel.text = RPM.ToString();
    }

    public void GetSpeed()
    {
        speedNeedleScript.speed = MPH;
        speedNeedleScript.numberForTextLabel.text = MPH.ToString();
    }
    public void GetFuel()
    {
        fuelNeedleScript.speed = FUEL;
        float a = FUEL / 100;
        float b = 19 * a;
        fuelNeedleScript.numberForTextLabel.text = (b).ToString();
    }
    public void GetEngTemp()
    {
        engTempNeedleScript.speed = ENG_TEMP;
        engTempNeedleScript.numberForTextLabel.text = ENG_TEMP.ToString();
    }

    public void GetOilTemp()
    {
        oilTempNeedleScript.speed = OIL_TEMP;
    }


    private float RPM;
    private float MPH;
    private float ENG_TEMP;
    private float OIL_TEMP;
    private float FUEL;
    private void ParseArduinoValue(string arduinoValue)
    {
        if(arduinoValue != "")
        {
            // Debug.Log(arduinoValue);
            string[] split = arduinoValue.Split('=');
            if (split.Length > 1)
            {
                float value = 0;
                float.TryParse(split[1], out value);
                if (arduinoValue.Contains("RPM"))
                {
                    RPM = value;
                }
                if (arduinoValue.Contains("speed"))
                {
                    //have to convert from kph to mph
                    float valueMPH = value / 1.609f;
                    MPH = valueMPH;
                }
                if (arduinoValue.Contains("OilTemp"))
                {
                    OIL_TEMP = value;
                }
                if (arduinoValue.Contains("coolant"))
                {
                    //convert from celseius to fahrenheit
                    float valueF = (value * 9 / 5) + 32;
                    ENG_TEMP = valueF;
                }
                if (arduinoValue.Contains("Fuel"))
                {
                    //value is a percentage
                    FUEL = value;
                }
            }
        }
    }

}
