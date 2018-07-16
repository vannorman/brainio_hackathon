/*
 
    -----------------------
    UDP-Receive (send to)
    -----------------------
    // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]
   
   
    // > receive
    // 127.0.0.1 : 8051
   
    // send
    // nc -u 127.0.0.1 8051
 
*/
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceiveSO : MonoBehaviour
{

    // receiving Thread
    private Thread receiveThread;

    // udpclient object
    private UdpClient client;

    // public
    // public string IP = "127.0.0.1"; default local
    [SerializeField]
    private int port = 8051; // define > init

    [SerializeField]
    private SOCircularBufferDouble[] channelBuffers = new SOCircularBufferDouble[9];

    // 0 is bandPowerFilterAcrossLast6Channels (alpha wave)
    // 1-8 are raw channels
    /*
    [SerializeField]
    SOCircularBufferDouble alphaBandPower;
    [SerializeField]
    SOCircularBufferDouble chan1;
    [SerializeField]
    SOCircularBufferDouble chan2;
    [SerializeField]
    SOCircularBufferDouble chan3;
    [SerializeField]
    SOCircularBufferDouble chan4;
    [SerializeField]
    SOCircularBufferDouble chan5;
    [SerializeField]
    SOCircularBufferDouble chan6;
    [SerializeField]
    SOCircularBufferDouble chan7;
    [SerializeField]
    SOCircularBufferDouble chan8;
    */


    public string lastValueString { get; private set; }

    // start from shell
    private static void Main()
    {
        UDPReceive receiveObj = new UDPReceive();
        receiveObj.Start();

        string text = "";
        do
        {
            text = Console.ReadLine();
        }
        while (!text.Equals("exit"));
    }

    // start from unity3d
    public void Start()
    {
        init();
    }

    // OnGUI
    void OnGUI()
    {
        Rect rectObj = new Rect(40, 10, 200, 400);
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        GUI.Box(rectObj, "# UDPReceive\n127.0.0.1 " + port + " #\n"
            + "shell> nc -u 127.0.0.1 : " + port + " \n"
                + "\nLast Value: \n" + lastValueString
            , style);
    }

    // init
    private void init()
    {
        // creates the receive thread
        print("UDPReceiveSO.init()");

        // ----------------------------
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

    }

    // receive thread
    private void ReceiveData()
    {

        client = new UdpClient(port);
        while (true)
        {

            try
            {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                                   
                for (int chanId = 0; chanId < channelBuffers.Length; chanId++)
                {
                    // 0 is bandPowerFilterAcrossLast6Channels (alpha wave)
                    // 1-8 are raw channels
                    // get the Nth value into the Nth buffer
                    double value = System.BitConverter.ToDouble(data, chanId * sizeof(double));
                    channelBuffers[chanId].pushValue(value);

                    lastValueString = value.ToString();
                }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
}
