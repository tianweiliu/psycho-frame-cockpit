using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

namespace ProjectPsychoFrame
{
    public class RelativeRotation : MonoBehaviour
    {
        public PointmanCalibrator pointMan;

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

        Dictionary<Kinect.JointType, Vector3> rotations = new Dictionary<Kinect.JointType, Vector3>();

        // Use this for initialization
        void Start()
        {
            for (Kinect.JointType jt = Kinect.JointType.SpineMid; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                if (JointToGameObject(jt))
                {
                    rotations[jt] = JointToGameObject(jt).transform.up;
                }
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (pointMan)
            {
                for (Kinect.JointType jt = Kinect.JointType.SpineMid; jt <= Kinect.JointType.ThumbRight; jt++)
                {
                    if (JointToGameObject(jt) && JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]))
                    {
                        if (JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).transform.root != JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).transform)
                        {
                            //Debug.Log(pointMan.RelativeFusedBody.JointOrientations[jt].Orientation.eulerAngles);
                            Vector3 dir = pointMan.RelativeFusedBody.Joints[jt].Position;
                            Debug.DrawRay(JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).transform.position, JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).transform.rotation * JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).transform.InverseTransformDirection(dir) * 100f, Color.red);
                            JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).transform.localRotation = Quaternion.LookRotation(JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).transform.InverseTransformDirection(dir), Vector3.up);
                        }
                        else
                            JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).transform.rotation = pointMan.RelativeFusedBody.JointOrientations[jt].Orientation;
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
    }
}
