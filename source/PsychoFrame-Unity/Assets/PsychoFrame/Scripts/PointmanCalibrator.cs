using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class PointmanCalibrator : MonoBehaviour
{

    public bool flipX;
    public bool flipZ;

    public bool hideLocal;

    public PointmanScript[] m_Cameras;
    public float[] m_CameraAngles;
    public int m_MainCameraIndex;

    public Kinect.Body FusedBody;
    public Kinect.Body RelativeFusedBody;

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


    List<Kinect.Body> m_Bodies;

    // Use this for initialization
    void Start()
    {
        m_Bodies = new List<Kinect.Body>();
        FusedBody = new Kinect.Body();
        RelativeFusedBody = new Kinect.Body();
    }

    // Update is called once per frame
    void Update()
    {
        // Check main camera skelenton to check if any bone is lost track
        int maxTrack = 0;

        m_Bodies.Clear();
        for (int n = 0; n < m_Cameras.Length; ++n)
        {
            if (maxTrack < m_Cameras[n].TrackNumber[0])
            {
                maxTrack = m_Cameras[n].TrackNumber[0];
                m_MainCameraIndex = n;
            }

            //m_Cameras[n].RotateBack();
            if (m_Cameras[n]._AvaliableBody.Count > 0)
                m_Bodies.Add(m_Cameras[n]._AvaliableBody[0]);
        }
        //m_Cameras[m_MainCameraIndex].RotateFront();

        CheckMainBody(m_Bodies.ToArray());
    }

    void CheckMainBody(Kinect.Body[] bodies)
    {
        // There is a chance that there will be no body avaliable
        if (bodies.Length <= m_MainCameraIndex)
        {
            return;
        }

        // Zero is always the main body

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.TrackingState trackingState = bodies[m_MainCameraIndex].Joints[jt].TrackingState;
            Kinect.Joint sourceJoint = bodies[m_MainCameraIndex].Joints[jt];
            GameObject jtObj = JointToGameObject(jt);
            if (jtObj)
            {
                if (jtObj.GetComponent<Renderer>())
                    jtObj.GetComponent<Renderer>().enabled = !hideLocal;
                if (jtObj.GetComponent<LineRenderer>())
                jtObj.GetComponent<LineRenderer>().enabled = !hideLocal;
            }
            if (sourceJoint.TrackingState != Kinect.TrackingState.Tracked && bodies.Length > 1) // When there is only one body avalible (aka. only one camera tracking), do not try to use only tracked data
            {
                // Try replace it with tracked backups

                for (int n = 0; n < bodies.Length; n++)
                {
                    if (n == m_MainCameraIndex)
                    {
                        continue;
                    }
                    Kinect.Joint backupJoint = bodies[n].Joints[Kinect.JointMap._MirrorBoneMap[jt]];
                    if (backupJoint.TrackingState == Kinect.TrackingState.Tracked)
                    {
                        // We can try this
                        trackingState = backupJoint.TrackingState;
                        Vector3 jointPosition = getPosition(m_Cameras[n].JointToGameObject(Kinect.JointMap._MirrorBoneMap[jt]).transform.position);
                        Quaternion jointRotation = m_Cameras[n].JointToGameObject(Kinect.JointMap._MirrorBoneMap[jt]).transform.rotation;
                        FusedBody.UpdateJoint(jt, jointPosition, jointRotation, trackingState);
                        if (jtObj)
                        {
                            jtObj.transform.localPosition = getPosition(m_Cameras[n].JointToGameObject(Kinect.JointMap._MirrorBoneMap[jt]).transform.position);
                            jtObj.transform.rotation = m_Cameras[n].JointToGameObject(Kinect.JointMap._MirrorBoneMap[jt]).transform.rotation;
                        }
                    }
                }
            }
            else
            {
                Vector3 jointPosition = getPosition(m_Cameras[m_MainCameraIndex].JointToGameObject(jt).transform.position);
                Quaternion jointRotation = m_Cameras[m_MainCameraIndex].JointToGameObject(jt).transform.rotation;
                FusedBody.UpdateJoint(jt, jointPosition, jointRotation, trackingState);
                if (jtObj)
                {
                    jtObj.transform.localPosition = getPosition(m_Cameras[m_MainCameraIndex].JointToGameObject(jt).transform.position);
                    jtObj.transform.rotation = m_Cameras[m_MainCameraIndex].JointToGameObject(jt).transform.rotation;
                }
            }


            if (Kinect.JointMap._BoneMap.ContainsKey(jt) && jtObj)
            {
                LineRenderer lr = jtObj.GetComponent<LineRenderer>();
                if (lr)
                {
                    lr.SetPosition(0, jtObj.transform.position);
                    lr.SetPosition(1, JointToGameObject(Kinect.JointMap._BoneMap[jt]).transform.position);
                }
            }
        }

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            if (jt == Kinect.JointType.SpineBase)
                RelativeFusedBody.UpdateJoint(jt, FusedBody.Joints[jt].Position, FusedBody.JointOrientations[jt].Orientation, FusedBody.Joints[jt].TrackingState);
            else
            {
                RelativeFusedBody.UpdateJoint(jt, FusedBody.Joints[jt].Position - FusedBody.Joints[Kinect.JointMap._RadialBoneMap[jt]].Position, Quaternion.identity, FusedBody.Joints[jt].TrackingState);
            }
        }
    }

    Vector3 getPosition(Vector3 position)
    {
        return new Vector3(flipX ? -position.x : position.x, position.y, flipZ ? -position.z : position.z);
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

    void OnGUI()
    {
        //GUI.Button(new Rect(10 + Screen.width * 0.7f * m_MainCameraIndex, 10, Screen.width * 0.2f, Screen.width * 0.2f), "Main");
    }
}
