using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Diagnostics;
using System;
using System.Runtime.InteropServices;

public class CameraInfo : MonoBehaviour {

    [DllImport("USER32.DLL")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    public Text ebrakeLbl;
    public Text leftTurnSignalLbl;
    public Text rightTurnSignalLbl;
    public Text shiftStatusLbl;
    public Text lightSymbolLbl;
    public Text seatBeltSymbolLbl;

    private string ebrakeString;
    private string leftTurnSignalString;
    private string rightTurnSignalString;
    private string shiftStatus = "Shift Status = ";
    private string shiftStatusResult;
    private string lightSymbolString;
    private string seatBeltSymbolString;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        ebrakeLbl.text = ebrakeString;
        leftTurnSignalLbl.text = leftTurnSignalString;
        rightTurnSignalLbl.text = rightTurnSignalString;
        shiftStatusLbl.text = shiftStatus + shiftStatusResult;
        lightSymbolLbl.text = lightSymbolString;
        seatBeltSymbolLbl.text = seatBeltSymbolString;
    }

    private void GetInfoFromCamera(string message)
    {
        string[] result = message.Split('=');
        bool answer;
        if (result.Length > 1)
        {
            if (result[0].Contains("ebrake"))
            {
                bool.TryParse(result[1], out answer);
                ebrakeString = "E-Brake = " + answer;
            }
            if (result[0].Contains("leftTurnSignal"))
            {
                bool.TryParse(result[1], out answer);
                leftTurnSignalString = "LEFT-TurnSignal = " + answer;
            }
            if (result[0].Contains("rightTurnSignal"))
            {
                bool.TryParse(result[1], out answer);
                rightTurnSignalString = "RIGHT-TurnSignal = " + answer;
            }
            if (result[0].Contains("parkSymbol"))
            {
                bool.TryParse(result[1], out answer);
                if (answer == true)
                {
                    shiftStatusResult = "PARK";
                }
            }
            if (result[0].Contains("reverseSymbol"))
            {
                bool.TryParse(result[1], out answer);
                if (answer == true)
                {
                    shiftStatusResult = "REVERSE";
                    //Find the reverse Camera Program and bring it to the front
                    Process[] localAll1 = Process.GetProcesses();
                    IntPtr x = new IntPtr();
                    foreach (Process proc in localAll1)
                    {
                        if (proc.MainWindowTitle == "Aforge.exe")
                        {
                            x = proc.MainWindowHandle;
                            SetForegroundWindow(x);
                            break;
                        }
                    }
                }
            }
            if (result[0].Contains("neutralSymbol"))
            {
                bool.TryParse(result[1], out answer);
                if (answer == true)
                {
                    shiftStatusResult = "NEUTRAL";
                }
            }
            if (result[0].Contains("driveSymbol"))
            {
                bool.TryParse(result[1], out answer);
                if (answer == true)
                {
                    shiftStatusResult = "DRIVE";
                }
            }
            if (result[0].Contains("lightSymbol"))
            {
                bool.TryParse(result[1], out answer);
                leftTurnSignalString = "LightSymbol = " + answer;
            }
            if (result[0].Contains("seatBeltSymbol"))
            {
                bool.TryParse(result[1], out answer);
                leftTurnSignalString = "Seat Belt = " + answer;
            }
        }
    }
}

