using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using TMPro;

public class Arduino : MonoBehaviour
{
    SerialPort serial;
    [SerializeField] int baudrate = 9600;
    [SerializeField] const string buttonA = "D8";
    bool buttonAState = false;
    [SerializeField] const string buttonB = "D2";
    bool buttonBState = false;
    [SerializeField] const string weight = "SCALE";
    float weightState = 0;

    [SerializeField] private TextMeshProUGUI textMesh;

    private void Awake()
    {
        InitializeArduino();
    }

    private void Start()
    {
        StartCoroutine(GetDataFromSerial());
    }

    private void InitializeArduino()
    {
        Time.timeScale = 0;

        foreach (string portName in SerialPort.GetPortNames())
        {
            // Try to open the serial port
            try
            {
                serial = new SerialPort(portName, baudrate, Parity.None, 8, StopBits.One);
                serial.DtrEnable = true;
                serial.DataReceived += new SerialDataReceivedEventHandler(OnSerialDataReceived);
                serial.Open();

                Debug.Log($"Arduaaano found on port {portName}");
            }
            catch
            {
                // idk lmao, doesn't matter
            }
        }

        // We should probably check if the serialPort was opened at all, otherwise show some error the controller isn't conn0ected

        Time.timeScale = 1;
    }

    private void Update()
    {
        if (buttonAState)
        {
            Debug.Log("D1 was pressed");
        }
        textMesh.text = weightState.ToString();
    }

    private IEnumerator GetDataFromSerial()
    {
        while (true)
        {
            // Try to read serial
            try
            {
                if (serial.BytesToRead > 0)
                { 
                    string input = serial.ReadLine();
                    Debug.Log(input);
                    string[] splitInput = input.Split(",");

                    // Ignore invalid inputs
                    if (splitInput.Length == 2)
                    {
                        switch (splitInput[0])
                        {
                            case buttonA:
                                buttonAState = splitInput[1] == "1";
                                break;
                            case buttonB:
                                buttonBState = splitInput[1] == "1";
                                break;
                            case weight:
                                weightState = float.Parse(splitInput[1]);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch
            {
                // Do something if controller isn't found
                Debug.Log("Arduino not found, pls fix");
                InitializeArduino();
            }

            yield return new WaitForSecondsRealtime(0.05f); 
        }
    }

    private void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        //SerialPort serialPort = (SerialPort)sender;
        //byte[] buffer = new byte[serialPort.BytesToRead];

        //serialPort.Read(buffer, 0, buffer.Length);

        //// Do something with the data here

        //foreach (byte b in buffer)
        //{
 
        //}
        Debug.Log(serial.ReadExisting());
        //Debug.Log(buffer);
    }

    private void OnDestroy()
    {
        serial.Close();
    }
}
