using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

namespace ProjectPsychoFrame
{
    public class RelativePosition : MonoBehaviour
    {
        [Header("Pointman Calibrator")]
        public PointmanCalibrator pointMan;

        [Header("3D Model Rig Joints")]
        public Kinect.JointSetting FootLeft;
        public Kinect.JointSetting AnkleLeft;
        public Kinect.JointSetting KneeLeft;
        public Kinect.JointSetting HipLeft;

        public Kinect.JointSetting FootRight;
        public Kinect.JointSetting AnkleRight;
        public Kinect.JointSetting KneeRight;
        public Kinect.JointSetting HipRight;

        public Kinect.JointSetting HandTipLeft;
        public Kinect.JointSetting ThumbLeft;
        public Kinect.JointSetting HandLeft;
        public Kinect.JointSetting WristLeft;
        public Kinect.JointSetting ElbowLeft;
        public Kinect.JointSetting ShoulderLeft;

        public Kinect.JointSetting HandTipRight;
        public Kinect.JointSetting ThumbRight;
        public Kinect.JointSetting HandRight;
        public Kinect.JointSetting WristRight;
        public Kinect.JointSetting ElbowRight;
        public Kinect.JointSetting ShoulderRight;

        public Kinect.JointSetting SpineBase;
        public Kinect.JointSetting SpineMid;
        public Kinect.JointSetting SpineShoulder;
        public Kinect.JointSetting Neck;
        public Kinect.JointSetting Head;

        public Kinect.Body FusedAbsoluteModelBody = new Kinect.Body(); //Absolute positions of scaled joints
        public Kinect.Body FusedRelativeModelBody = new Kinect.Body(); //Relative positions of scaled joints

        Dictionary<Kinect.JointType, float> boneSize = new Dictionary<Kinect.JointType, float>();

        // Use this for initialization
        void Start()
        {
            for (Kinect.JointType jt = Kinect.JointType.SpineMid; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                if (JointToGameObject(jt).jointGameObject && JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).jointGameObject)
                {
                    boneSize[jt] = Vector3.Distance(JointToGameObject(jt).jointGameObject.transform.position, JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).jointGameObject.transform.position);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (pointMan)
            {
                for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
                {
                    if (jt == Kinect.JointType.SpineBase) // Set spine base directly since it does not have relative scale
                    {
                        if (JointToGameObject(jt).jointGameObject)
                        {
                            if (JointToGameObject(jt).jointGameObject.transform.root != JointToGameObject(jt).jointGameObject.transform)
                            {
                                FusedAbsoluteModelBody.UpdateJoint(jt,
                                    JointToGameObject(jt).jointGameObject.transform.root.rotation * pointMan.FusedBody.Joints[jt].Position + JointToGameObject(jt).jointGameObject.transform.root.position,
                                    Quaternion.identity, pointMan.FusedBody.Joints[jt].TrackingState);

                                FusedRelativeModelBody.UpdateJoint(jt,
                                    JointToGameObject(jt).jointGameObject.transform.root.rotation * pointMan.FusedBody.Joints[jt].Position,
                                    Quaternion.identity, pointMan.FusedBody.Joints[jt].TrackingState);
                            }
                            else
                            {
                                FusedAbsoluteModelBody.UpdateJoint(jt, pointMan.FusedBody.Joints[jt].Position, Quaternion.identity, pointMan.FusedBody.Joints[jt].TrackingState);
                                FusedRelativeModelBody.UpdateJoint(jt, pointMan.FusedBody.Joints[jt].Position, Quaternion.identity, pointMan.FusedBody.Joints[jt].TrackingState);
                            }
                            if (JointToGameObject(jt).applyTransform)
                                JointToGameObject(jt).jointGameObject.transform.position = FusedAbsoluteModelBody.Joints[jt].Position;
                        }
                    }
                    else if (JointToGameObject(jt).jointGameObject && JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).jointGameObject && boneSize.ContainsKey(jt))
                    {
                        if (pointMan.RelativeFusedBody.Joints[jt].Position.magnitude != 0)
                        {
                            //Debug.Log(pointMan.RelativeFusedBody.Joints[jt].Position.magnitude + " -> " + boneSize[jt]);
                            FusedAbsoluteModelBody.UpdateJoint(jt,
                                JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).jointGameObject.transform.root.rotation *
                                (pointMan.RelativeFusedBody.Joints[jt].Position / pointMan.RelativeFusedBody.Joints[jt].Position.magnitude * boneSize[jt]) +
                                FusedAbsoluteModelBody.Joints[Kinect.JointMap._RadialBoneMap[jt]].Position,
                                Quaternion.identity, pointMan.FusedBody.Joints[jt].TrackingState);

                            FusedRelativeModelBody.UpdateJoint(jt,
                                JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).jointGameObject.transform.root.rotation *
                                (pointMan.RelativeFusedBody.Joints[jt].Position / pointMan.RelativeFusedBody.Joints[jt].Position.magnitude * boneSize[jt]),
                                Quaternion.identity, pointMan.FusedBody.Joints[jt].TrackingState);
                        }
                        else
                        {
                            FusedAbsoluteModelBody.UpdateJoint(jt,
                                JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).jointGameObject.transform.root.rotation *
                                Vector3.ClampMagnitude(pointMan.RelativeFusedBody.Joints[jt].Position, boneSize[jt]) +
                                FusedAbsoluteModelBody.Joints[Kinect.JointMap._RadialBoneMap[jt]].Position,
                                Quaternion.identity, pointMan.FusedBody.Joints[jt].TrackingState);

                            FusedRelativeModelBody.UpdateJoint(jt,
                                JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).jointGameObject.transform.root.rotation *
                                Vector3.ClampMagnitude(pointMan.RelativeFusedBody.Joints[jt].Position, boneSize[jt]),
                                Quaternion.identity, pointMan.FusedBody.Joints[jt].TrackingState);
                        }
                        if (JointToGameObject(jt).applyTransform)
                            JointToGameObject(jt).jointGameObject.transform.position = FusedAbsoluteModelBody.Joints[jt].Position;
                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                if (FusedAbsoluteModelBody.Joints.ContainsKey(jt))
                {
                    if (JointToGameObject(jt).jointGameObject)
                    {
                        switch (FusedAbsoluteModelBody.Joints[jt].TrackingState)
                        {
                            case Kinect.TrackingState.Tracked:
                                Gizmos.color = Color.green;
                                break;
                            case Kinect.TrackingState.Inferred:
                                Gizmos.color = Color.yellow;
                                break;
                            case Kinect.TrackingState.NotTracked:
                                Gizmos.color = Color.red;
                                break;
                        }
                        Gizmos.DrawSphere(FusedAbsoluteModelBody.Joints[jt].Position, 0.01f);
                        if (Kinect.JointMap._RadialBoneMap.ContainsKey(jt))
                        {
                            if (JointToGameObject(Kinect.JointMap._RadialBoneMap[jt]).jointGameObject)
                            {
                                Gizmos.color = Color.blue;
                                Gizmos.DrawLine(FusedAbsoluteModelBody.Joints[jt].Position, FusedAbsoluteModelBody.Joints[Kinect.JointMap._RadialBoneMap[jt]].Position);
                            }
                        }
                    }
                }
            }
        }

        public Kinect.JointSetting JointToGameObject(Kinect.JointType joint)
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
