using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using System.Xml;

public class PointmanScript : MonoBehaviour
{

    public BodySourceManager _BodyManager;

    public int BodyIndex;
    public bool EnableRotation;

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

    public bool OnRecord;
    public string SaveLocation;

    public bool calibrateLocation;
    public Transform calibrateOrigin;
    public Transform pointManRoot;
    public Transform superParent;

    public bool localMotion;
    public bool hideLocal;


    public int[] TrackNumber;
    public int debugIndex;

    public List<Kinect.Body> _AvaliableBody;


    public Vector3 ToCameraDistance;
    public Vector3 FacingDirection;

    // Use this for initialization
    void Start()
    {
        _AvaliableBody = new List<Kinect.Body>();
        FacingDirection = Vector3.zero;
        ToCameraDistance = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {


            Kinect.Body[] data = _BodyManager.GetData();
            if (data == null)
            {
                return;
            }

            _AvaliableBody.Clear();
            foreach (Kinect.Body body in data)
            {
                if (body == null)
                {
                    continue;
                }
                if (body.IsTracked)
                {
                    _AvaliableBody.Add(body);
                }
            }

            // Update Bone Position
            for (int index = 0; index < _AvaliableBody.Count; index++)
            {
                if (_AvaliableBody[index] == null)
                {
                    continue;
                }

                if (index == BodyIndex && _AvaliableBody[index].IsTracked)
                {
                    CalibrateRoot(_AvaliableBody[index]);
                    RefreshBodyObject(_AvaliableBody[index]);
                }
                else
                {
                    continue;
                }
            }
    }

    private void CalibrateRoot(Kinect.Body body)
    {
        // use two shoulder as calibration reference
        Kinect.Joint leftShouder = body.Joints[Kinect.JointType.ShoulderLeft];
        Kinect.Joint rightShouder = body.Joints[Kinect.JointType.ShoulderRight];

        Vector3 localLeft = GetVector3FromJoint(leftShouder);
        Vector3 localRight = GetVector3FromJoint(rightShouder);

        Vector3 deltaVec = localRight - localLeft;
        deltaVec.y = 0;

        float rotAngle = Vector3.Angle(deltaVec, calibrateOrigin.right);
        float rotDir = Vector3.Dot(deltaVec, calibrateOrigin.forward) > 0 ? 1 : -1;

        if (calibrateLocation)
        {
            pointManRoot.localRotation = Quaternion.Euler(0, rotAngle * rotDir, 0);
        }
        FacingDirection.y = rotAngle * rotDir;
    }

    private void RefreshBodyObject(Kinect.Body body)
    {
        TrackNumber[BodyIndex] = 0;

        Vector3 localDelta = Vector3.zero;
        Vector3 targetPosition = Vector3.zero;
        if (localMotion)
        {
            Kinect.Joint rootJoint = body.Joints[Kinect.JointType.SpineBase];
            Vector3 rootPosition = GetVector3FromJoint(rootJoint);
            localDelta = rootPosition - pointManRoot.position;
            
        }

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint targetJoint = null;

            GameObject pointObj = JointToGameObject(jt);
            GameObject targetJointObj = null;
            LineRenderer lr = pointObj.GetComponent<LineRenderer>();

            if (sourceJoint.TrackingState != Kinect.TrackingState.Tracked)
            {
                pointObj.renderer.enabled = false;
                lr.enabled = false;
            }
            else
            {
                pointObj.renderer.enabled = true;
                lr.enabled = true;
                TrackNumber[BodyIndex]++;
            }

            if (hideLocal)
            {
                pointObj.renderer.enabled = false;
                lr.enabled = false;
            }
            

            pointObj.transform.localPosition = GetVector3FromJoint(sourceJoint);
            if (jt == Kinect.JointType.SpineBase) {
                ToCameraDistance = pointObj.transform.localPosition;
            }


            if (Kinect.JointMap._BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[Kinect.JointMap._BoneMap[jt]];
                targetJointObj = JointToGameObject(Kinect.JointMap._BoneMap[jt]);
            }
            

            if (localMotion)
            {
                pointObj.transform.localPosition -= localDelta;
            }


            
            if (EnableRotation && targetJoint != null)
            {
                pointObj.transform.rotation = Quaternion.LookRotation((pointObj.transform.localPosition - GetVector3FromJoint(targetJoint)).normalized);
            }
            
           
            
            if (targetJoint != null)
            {
                lr.SetPosition(0, pointObj.transform.position);
                lr.SetPosition(1, targetJointObj.transform.position);
                //lr.SetColors(GetColorForState(sourceJoint.TrackingState), GetColorForState(targetJoint.TrackingState));
            }
            else
            {
                lr.enabled = false;
            }

            if (calibrateLocation)
            {
                GameObject left = JointToGameObject(Kinect.JointType.ShoulderLeft);
                GameObject right = JointToGameObject(Kinect.JointType.ShoulderRight);

                Debug.DrawLine(left.transform.position, right.transform.position);
            }

        }
    }


    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.x * 10, joint.Position.y * 10, joint.Position.z * 10);
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

    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
            case Kinect.TrackingState.Tracked:
                return Color.green;

            case Kinect.TrackingState.Inferred:
                return Color.red;

            default:
                return Color.black;
        }
    }

    private Vector3 GetVector3FromString(string x, string y, string z)
    {
        float xF, yF, zF;
        xF = float.Parse(x);
        yF = float.Parse(y);
        zF = float.Parse(z);
        return new Vector3(xF, yF, zF);
    }

    void OnGUI(){
        GUI.Label(new Rect(10, 10 + 50 * debugIndex, 200,40), "Body "+debugIndex+" Valid Count:" + TrackNumber[BodyIndex]);
    }

    public void RotateBack() {
        superParent.transform.rotation = Quaternion.Euler(0,180,0);
    }

    public void RotateFront()
    {
        superParent.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
