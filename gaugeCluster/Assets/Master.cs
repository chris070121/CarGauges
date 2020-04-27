using UnityEngine;
using System.Collections;
using System.Threading;
using System.IO.Ports;
using System;
using UnityEngine.UI;

public class Master : MonoBehaviour {
    public NeedleTemp tachNeedleScript;
    public NeedleTemp speedNeedleScript;
    public NeedleTemp fuelNeedleScript;
    public NeedleTemp engTempNeedleScript;
    public NeedleTemp voltageTempNeedleScript;
    public Image brightnessControllerImage;
    public Image brightnessControllerImage2;
    public Button resteButton;
    private TimeSpan morning = new TimeSpan(6, 0, 0); //10 o'clock
    private TimeSpan night = new TimeSpan(17, 0, 0); //12 o'clock
    int morningTime;
    int eveningTime;
    bool _brightnessUp;
    private SerialPort stream = new SerialPort("COM4", 9600); //Set the port (com4) and the baud rate (9600, is standard on most devices)
    private string arduinoValue = "";
    Thread a;
    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 30;
        Display.displays[1].Activate();
        Display.displays[2].Activate();
        //resteButton.onClick +=  ;
        try
        {
            //stream.Open();
            a = new Thread(ParseArduinoValue);
            a.Start();
            GetSettings();
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    private void GetSettings()
    {
        string[] lines = System.IO.File.ReadAllLines(@"C:\Users\chris\Desktop\test\Car_SupportingProgram\webCamTest\webCamTest\bin\Debug\Settings.txt");
        foreach (string line in lines)
        {
            string[] result = line.Split('=');

            if (line.Contains("Morning"))
            {
                morningTime = Convert.ToInt32(result[1]);
            }
            else if (line.Contains("Evening"))
            {
                eveningTime = Convert.ToInt32(result[1]);
            }
        }
        morning = new TimeSpan(morningTime, 0, 0); //10 o'clock
        night = new TimeSpan(eveningTime, 0, 0); //12 o'clock
    }


    void OnDestroy()
    {
        a.Abort();
    }
    private void CheckTime()
    {
        TimeSpan now = DateTime.Now.TimeOfDay;

        if ((now > morning) && (now < night))
        {
            _brightnessUp = true;
        }
        else
        {
            _brightnessUp = false;
        }
    }
    // Update is called once per frame
    void Update () {
        if (stream.IsOpen)
        {
            GetRPM();
            GetSpeed();
            GetFuel();
            GetEngTemp();
            GetVoltageTemp();
            CheckTime();
            Debug.Log(_brightnessUp);
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
        else
        {
            try
            {
                stream = new SerialPort("COM4", 9600);
                stream.Open();
            }
            catch (Exception ex)
            { }
        }
    }

  
    public void GetRPM()
    {
        tachNeedleScript.speed = RPM;
        tachNeedleScript.numberForTextLabel.text = (Convert.ToInt32(RPM)).ToString();
    }

    public void GetSpeed()
    {
        speedNeedleScript.speed = MPH;
        speedNeedleScript.numberForTextLabel.text = (Convert.ToInt32(MPH)).ToString();
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

    public void GetVoltageTemp()
    {
        voltageTempNeedleScript.speed = VOLTAGE_TEMP + .5f;
        voltageTempNeedleScript.numberForTextLabel.text = (VOLTAGE_TEMP+.5f).ToString();

    }


    private float RPM;
    private float MPH;
    private float ENG_TEMP;
    private float VOLTAGE_TEMP;
    private float FUEL;
    private void ParseArduinoValue()
    {
        while (true)
        {
            if (stream.IsOpen)
            {
                arduinoValue = stream.ReadLine();
            }
            if (arduinoValue != "")
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
                    if (arduinoValue.Contains("voltage"))
                    {
                        VOLTAGE_TEMP = value;
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

}
