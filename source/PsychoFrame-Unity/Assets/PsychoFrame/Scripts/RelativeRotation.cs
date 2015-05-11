using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

namespace ProjectPsychoFrame
{
    public class RelativeRotation : MonoBehaviour
    {
        public Vector3 CoordinateMatch_Origin_To_Model;
        public Vector3 CoordinateMathc_Flip;

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

        Dictionary<Kinect.JointType, Vector3> initDirections = new Dictionary<Kinect.JointType, Vector3>();
        Dictionary<Kinect.JointType, Quaternion> initRotations = new Dictionary<Kinect.JointType, Quaternion>();

        // Use this for initialization
        void Start()
        {
            for (Kinect.JointType jt = Kinect.JointType.SpineMid; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                if (JointToGameObject(jt))
                {
                    initRotations[jt] = JointToGameObject(jt).transform.rotation;
                    if (Kinect.JointMap._RadialBoneMap.ContainsKey(jt))
                    {
                        if (JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]))
                        {
                            initDirections[jt] = (JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).transform.position - JointToGameObject(jt).transform.position).normalized;
                        }
                    }
                }
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {

            /* Modified by Xin
             * Issue: Kinect Bone has opposite direction of assigning bone direction
             * Which makes any branch bones a real trouble
             * Potential Solution - Invert bone direction assignment
             */
            if (pointMan)
            {
                for (Kinect.JointType jt = Kinect.JointType.SpineMid; jt <= Kinect.JointType.ThumbRight; jt++)
                {
                    Quaternion localRotation = pointMan.JointToGameObject(jt).transform.rotation;
                    Vector3 rotationVec = localRotation.eulerAngles;
                    Vector3 matchRotation = Vector3.zero;

                    if (CoordinateMatch_Origin_To_Model.x == 0)
                    { matchRotation.x = rotationVec.x; }
                    else if (CoordinateMatch_Origin_To_Model.x == 1)
                    { matchRotation.x = rotationVec.y; }
                    else
                    { matchRotation.x = rotationVec.z; }

                    if (CoordinateMatch_Origin_To_Model.y == 0)
                    { matchRotation.y = rotationVec.x; }
                    else if (CoordinateMatch_Origin_To_Model.y == 1)
                    { matchRotation.y = rotationVec.y; }
                    else
                    { matchRotation.y = rotationVec.z; }

                    if (CoordinateMatch_Origin_To_Model.z == 0)
                    { matchRotation.z = rotationVec.x; }
                    else if (CoordinateMatch_Origin_To_Model.z == 1)
                    { matchRotation.z = rotationVec.y; }
                    else
                    { matchRotation.z = rotationVec.z; }


                    if (CoordinateMathc_Flip.x < 0) {
                        matchRotation.x += 180;
                    }
                    if (CoordinateMathc_Flip.y < 0) {
                        matchRotation.y += 180;
                    }
                    if (CoordinateMathc_Flip.z < 0) {
                        matchRotation.z += 180;
                    }

                    localRotation = Quaternion.Euler(matchRotation);

                    JointToGameObject(jt).transform.rotation = localRotation;
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
