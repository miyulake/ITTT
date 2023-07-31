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
    [SerializeField] private TextMeshProUGUI weightTextMesh;
    [SerializeField] private TextMeshProUGUI arduinoTextMesh;
    [HideInInspector] public float resultWeight;
    private bool hasStoredWeight;

    private void Awake()
    {
        InitializeArduino();
    }

    private void Start()
    {
        StartCoroutine(GetDataFromSerial());
        hasStoredWeight = false;
    }


    private void Update()
    {
        // Button for resetting game
        if (buttonBState)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("D7 was pressed");
        }
        if (!countdownTimer.gameEnded)
        {
            // Button for ending game
            if (buttonAState)
            {
                countdownTimer.gameEnded = true;
                Debug.Log("D8 was pressed");
            }
            // For debugging purposes
            weightTextMesh.text = Mathf.CeilToInt(weightState) + " Grams";
        }
        else
        {
            // Store weight seperately to keep the Arduino running
            if (!hasStoredWeight)
            {
                resultWeight = weightState;
                hasStoredWeight = true;
            }
            weightTextMesh.text = Mathf.CeilToInt(resultWeight) + " Grams";
        }
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
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

                arduinoTextMesh.text = "Arduino connected!";
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
                arduinoTextMesh.text = "Connect Arduino first!";
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
