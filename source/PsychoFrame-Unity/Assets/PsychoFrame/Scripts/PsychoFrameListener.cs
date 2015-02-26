using UnityEngine;
using System.Collections;
using Ventuz.OSC;
using Windows.Kinect;
using System.Threading;
using System.Collections.Generic;
using System;

public class PsychoFrameListener : MonoBehaviour {

    public enum OscDataType {
        OscBundle,
        OscMessage
    }

    public delegate void PsychoFrameKinectEventHandler(PsychoFrameListener sender, string identifier, int bodyIndex, JointType jointType, Vector3 position, Quaternion rotation, TrackingState trackingState);
    public static event PsychoFrameKinectEventHandler OnPsychoFrameDataReceived;

    public OscDataType oscDataType = OscDataType.OscMessage;
    public int port = 12000;

    private UdpReader receiver = null;
    private List<OscElement> processQueue = new List<OscElement>();
    
    private Thread thread;
    private bool connected = false;

	// Use this for initialization
	void Start () {
        connect();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // FetchOscMessages();
        // FetchOscBundle();
        lock (processQueue)
        {
            foreach (OscElement element in processQueue)
                ParseElement(element);
            processQueue.Clear();
        }
	}

    public void OnApplicationQuit()
    {
        disconnect();
    }

    public void connect()
    {

        try
        {
            //print("connecting.");
            connected = true;
            receiver = new UdpReader(port);
            thread = new Thread(new ThreadStart(listen));
            thread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("failed to connect to port " + port);
            Debug.Log(e.Message);
        }
    }

    public void disconnect()
    {
        connected = false;
    }

    private void listen()
    {
        while (connected)
        {
            try
            {
                if (oscDataType == OscDataType.OscMessage)
                {
                    OscMessage packet = receiver.Receive();
                    if (packet != null)
                    {
                        lock (processQueue)
                        {

                            //Debug.Log( "adding  packets " + processQueue.Count );
                            processQueue.Add((OscElement)packet);
                        }
                    }
                    else Console.WriteLine("null packet");
                }
                else
                {
                    OscBundle packet = receiver.ReceiveBundle();
                    if (packet != null)
                    {
                        lock (processQueue)
                        {
                            foreach (OscElement element in packet.Elements)
                                //Debug.Log( "adding  packets " + processQueue.Count );
                                processQueue.Add(element);
                        }
                    }
                    else Console.WriteLine("null packet");
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Console.WriteLine(e.Message);
            }
        }
        if (receiver != null)
        {
            receiver.Dispose();
            receiver = null;
        }
    }

    void ParseElement(OscElement oscElement)
    {
        string[] addresses = oscElement.Address.Split('/');
        string identifier = addresses[1];
        int bodyIndex = (int)oscElement.Args[0];
        JointType jointType = (JointType)oscElement.Args[1];
        Vector3 position = new Vector3((float)oscElement.Args[2], (float)oscElement.Args[3], (float)oscElement.Args[4]);
        Quaternion rotation = new Quaternion((float)oscElement.Args[5], (float)oscElement.Args[6], (float)oscElement.Args[7], (float)oscElement.Args[8]);
        TrackingState trackingState = (TrackingState)oscElement.Args[9];
        if (OnPsychoFrameDataReceived != null)
            OnPsychoFrameDataReceived(this, identifier, bodyIndex, jointType, position, rotation, trackingState);
    }
}
