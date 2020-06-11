using UnityEngine;
using System.Collections;
using System.Threading;
using System.IO.Ports;
using System;
using UnityEngine.UI;
using Coding4Fun.Obd.ObdManager;

public class Master : MonoBehaviour {
    public NeedleTemp tachNeedleScript;
    public NeedleTemp speedNeedleScript;
    public NeedleTemp fuelNeedleScript;
    public NeedleTemp engTempNeedleScript;
    public NeedleTemp voltageTempNeedleScript;
    public Image brightnessControllerImage;
    public Image brightnessControllerImage2;
    public Button resteButton;
    public Text textBox;
    private TimeSpan morning = new TimeSpan(6, 0, 0); //10 o'clock
    private TimeSpan night = new TimeSpan(17, 0, 0); //12 o'clock
    int morningTime;
    int eveningTime;
    bool _brightnessUp;
    private int count = 0;
    public bool testingInUnity = false;
    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 30;
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
            Display.displays[2].Activate();
        }
        if (testingInUnity == false)
        {
            try
            {

                obd = new ObdDevice();
                obd.ObdChanged += obd_ObdChanged;
                obd.Connect("COM6", 115200, ObdDevice.UnknownProtocol, true);

                GetSettings();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
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

    private void OnDestroy()
    {
        obd.thread.Abort();

    }
    void OnApplicationQuit()
    {
        obd.thread.Abort();
       // a.Abort();
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
    private int testingCOunter = 0;
    void Update ()
    {
        if(testingInUnity == true)
        {
            testingCOunter++;
            UpdateGui();
        }
            GetRPM();
            GetSpeed();
        if (count < 20)
        {
            GetFuel();
            GetEngTemp();
            GetVoltageTemp();
            CheckTime();
            GetOilTemp();

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

            count = 0;
        }
        else
        {
            count++;
        }
           
    }

  
    public void GetRPM()
    {
        float x = RPM /1f;
        tachNeedleScript.speed = x;
        tachNeedleScript.numberForTextLabel.text = (Convert.ToInt32(RPM)).ToString();
    }

    public void GetSpeed()
    {
        speedNeedleScript.speed = MPH;
        speedNeedleScript.numberForTextLabel.text = (Convert.ToInt32(MPH)).ToString();
    }
    public void GetFuel()
    {
        float a = FUEL / 100;
        float b = 19 * a;
        fuelNeedleScript.numberForTextLabel.text = (b).ToString();
        fuelNeedleScript.speed = b;
    }
    public void GetEngTemp()
    {
        engTempNeedleScript.speed = ENG_TEMP;
        engTempNeedleScript.numberForTextLabel.text = ENG_TEMP.ToString();
    }

    public void GetVoltageTemp()
    {
        voltageTempNeedleScript.speed = VOLTAGE_TEMP;
        voltageTempNeedleScript.numberForTextLabel.text = (VOLTAGE_TEMP).ToString();

    }

    public void GetOilTemp()
    {
    }

    private int RPM;
    private float MPH;
    private float ENG_TEMP;
    private float VOLTAGE_TEMP;
    private float FUEL;
    static ObdDevice obd;
    private float OIL_TEMP;

    private void obd_ObdChanged(object sender, ObdChangedEventArgs e)
    {
        double gallons = ((double)e.ObdState.FuelLevel / 100) * 19;
    
        FUEL  = e.ObdState.FuelLevel;
        RPM = e.ObdState.Rpm;
        VOLTAGE_TEMP = (float)e.ObdState.BatteryVoltage;
        double y = e.ObdState.MilesPerHour;
        MPH= (float)y;
        bool temp = e.ObdState.MilLightOn;
        ENG_TEMP = e.ObdState.EngineCoolantTemperature;
        OIL_TEMP = e.ObdState.OilTemperature;
    }

    private void UpdateGui()
    {
        FUEL = testingCOunter;
        RPM = testingCOunter;
        VOLTAGE_TEMP = testingCOunter;
        MPH = testingCOunter;

        ENG_TEMP = testingCOunter;
        OIL_TEMP = testingCOunter;
    }
}
