using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class PointmanCalibrator : MonoBehaviour {
    public PointmanScript[] m_Cameras;
    public float[] m_CameraAngles; 
    public int m_MainCameraIndex;


    public GameObject FootLeft;
    public GameObject AnkleLeft;
    public GameObject KneeLeft;
    public GameObject HipLeft;

    public GameObject FootRight;
    public GameObject AnkleRight;
    public GameObject KneeRight;
    public GameObject HipRight;

    public GameObject HandTipLeft;
    public GameObject ThumbLeft;
    public GameObject HandLeft;
    public GameObject WristLeft;
    public GameObject ElbowLeft;
    public GameObject ShoulderLeft;

    public GameObject HandTipRight;
    public GameObject ThumbRight;
    public GameObject HandRight;
    public GameObject WristRight;
    public GameObject ElbowRight;
    public GameObject ShoulderRight;

    public GameObject SpineBase;
    public GameObject SpineMid;
    public GameObject SpineShoulder;
    public GameObject Neck;
    public GameObject Head;


    Kinect.Body[] m_Bodies;

	// Use this for initialization
	void Start () {
        m_Bodies = new Kinect.Body[m_Cameras.Length];
	}

    public Dictionary<Kinect.JointType, Kinect.JointType> _MirrorBoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.FootRight },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.AnkleRight },
        { Kinect.JointType.KneeLeft, Kinect.JointType.KneeRight },
        { Kinect.JointType.HipLeft, Kinect.JointType.HipRight },
        
        { Kinect.JointType.FootRight, Kinect.JointType.FootLeft },
        { Kinect.JointType.AnkleRight, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.KneeRight, Kinect.JointType.KneeLeft },
        { Kinect.JointType.HipRight, Kinect.JointType.HipLeft },
        
        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandTipRight },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.ThumbRight },
        { Kinect.JointType.HandLeft, Kinect.JointType.HandRight },
        { Kinect.JointType.WristLeft, Kinect.JointType.WristRight },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.ShoulderRight },
        
        { Kinect.JointType.HandTipRight, Kinect.JointType.HandTipLeft },
        { Kinect.JointType.ThumbRight, Kinect.JointType.ThumbLeft },
        { Kinect.JointType.HandRight, Kinect.JointType.HandLeft },
        { Kinect.JointType.WristRight, Kinect.JointType.WristLeft },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.ShoulderRight },
        
        { Kinect.JointType.SpineBase, Kinect.JointType.SpineBase },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.Neck, Kinect.JointType.Neck },
        { Kinect.JointType.Head, Kinect.JointType.Head}
    };

	// Update is called once per frame
	void Update () {
	    // Check main camera skelenton to check if any bone is lost track
        int maxTrack = 0;

        for(int n = 0; n < m_Cameras.Length; ++n){
            if (maxTrack < m_Cameras[n].TrackNumber[0]) {
                maxTrack = m_Cameras[n].TrackNumber[0];
                m_MainCameraIndex = n;
            }
            
            //m_Cameras[n].RotateBack();
            m_Bodies[n] = m_Cameras[n]._AvaliableBody[0];
        }
        //m_Cameras[m_MainCameraIndex].RotateFront();

        CheckMainBody(m_Bodies);
	}

    void CheckMainBody(Kinect.Body[] bodies) {
        // Zero is always the main body

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = bodies[m_MainCameraIndex].Joints[jt];
            GameObject jtObj = JointToGameObject(jt);

            if (sourceJoint.TrackingState != Kinect.TrackingState.Tracked)
            {
                // Try replace it with tracked backups

                for (int n = 0; n < bodies.Length; n++)
                {
                    if(n == m_MainCameraIndex){
                        continue;
                    }
                    Kinect.Joint backupJoint = bodies[n].Joints[_MirrorBoneMap[jt]];
                    if (backupJoint.TrackingState == Kinect.TrackingState.Tracked)
                    {
                        // We can try this
                        jtObj.transform.position = m_Cameras[n].JointToGameObject(_MirrorBoneMap[jt]).transform.position;
                        jtObj.transform.rotation = m_Cameras[n].JointToGameObject(_MirrorBoneMap[jt]).transform.rotation;
                    }
                }
            }
            else {
                
                jtObj.transform.position = m_Cameras[m_MainCameraIndex].JointToGameObject(jt).transform.position;
                jtObj.transform.rotation = m_Cameras[m_MainCameraIndex].JointToGameObject(jt).transform.rotation;
            }


            if (m_Cameras[0]._BoneMap.ContainsKey(jt))
            {
                LineRenderer lr = jtObj.GetComponent<LineRenderer>();
                if (lr)
                {
                    lr.SetPosition(0, jtObj.transform.position);
                    lr.SetPosition(1, JointToGameObject(m_Cameras[0]._BoneMap[jt]).transform.position);
                }
            }
             
        }
    }

    public GameObject JointToGameObject(Kinect.JointType joint)
    {
        switch (joint)
        {
            case Kinect.JointType.SpineBase:
                return SpineBase;
            case Kinect.JointType.SpineMid:
                return SpineMid;
            case Kinect.JointType.SpineShoulder:
                return SpineShoulder;
            case Kinect.JointType.Neck:
                return Neck;
            case Kinect.JointType.Head:
                return Head;

            case Kinect.JointType.FootLeft:
                return FootLeft;
            case Kinect.JointType.AnkleLeft:
                return AnkleLeft;
            case Kinect.JointType.KneeLeft:
                return KneeLeft;
            case Kinect.JointType.HipLeft:
                return HipLeft;

            case Kinect.JointType.FootRight:
                return FootRight;
            case Kinect.JointType.AnkleRight:
                return AnkleRight;
            case Kinect.JointType.KneeRight:
                return KneeRight;
            case Kinect.JointType.HipRight:
                return HipRight;

            case Kinect.JointType.HandTipLeft:
                return HandTipLeft;
            case Kinect.JointType.ThumbLeft:
                return ThumbLeft;
            case Kinect.JointType.HandLeft:
                return HandLeft;
            case Kinect.JointType.WristLeft:
                return WristLeft;
            case Kinect.JointType.ElbowLeft:
                return ElbowLeft;
            case Kinect.JointType.ShoulderLeft:
                return ShoulderLeft;

            case Kinect.JointType.HandTipRight:
                return HandTipRight;
            case Kinect.JointType.ThumbRight:
                return ThumbRight;
            case Kinect.JointType.HandRight:
                return HandRight;
            case Kinect.JointType.WristRight:
                return WristRight;
            case Kinect.JointType.ElbowRight:
                return ElbowRight;
            case Kinect.JointType.ShoulderRight:
                return ShoulderRight;

            default:
                return SpineBase;
        }
    }

    void OnGUI() {
        //GUI.Button(new Rect(10 + Screen.width * 0.7f * m_MainCameraIndex, 10, Screen.width * 0.2f, Screen.width * 0.2f), "Main");
    }
}
