using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;
using TMPro;

public class Arduino : MonoBehaviour
{
    private SerialPort serial;
    [SerializeField] private int baudrate = 9600;
    private const string buttonA = "D8";
    [HideInInspector] public bool buttonAState = false;
    private const string buttonB = "D7";
    [HideInInspector] public bool buttonBState = false;
    private const string weight = "SCALE";
    [HideInInspector] public float weightState = 0;

    [SerializeField] private CountdownTimer countdownTimer;
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

        // We should probably check if the serialPort was opened at all, otherwise show some error the controller isn't connected

        Time.timeScale = 1;
    }

    private void Update()
    {
        if (!countdownTimer.gameEnded)
        {
            if (buttonAState)
            {
                countdownTimer.gameEnded = true;
                Debug.Log("D8 was pressed");
            }
            if (buttonBState)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Debug.Log("D7 was pressed");
            }
            textMesh.text = Mathf.CeilToInt(weightState) + " Grams";
        }
    }

    private IEnumerator GetDataFromSerial()
    {
        while (!countdownTimer.gameEnded)
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
