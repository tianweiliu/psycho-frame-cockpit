using UnityEngine;
using System.Collections.Generic;

namespace Windows.Kinect
{
    public class JointMap
    {
        static Dictionary<JointType, JointType> _boneMap = new Dictionary<JointType, JointType>()
        {
            { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
            { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
            { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
            { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
        
            { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
            { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
            { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
            { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
        
            { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
            { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
            { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
            { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
            { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
            { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
        
            { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
            { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
            { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
            { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
            { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
            { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
        
            { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
            { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
            { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
            { Kinect.JointType.Neck, Kinect.JointType.Head },
        };
        public static Dictionary<JointType, JointType> _BoneMap { get { return _boneMap; } } //So it is static and read only

        static Dictionary<JointType, JointType> _radialBoneMap = new Dictionary<JointType, JointType>()
        {
            { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
            { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
            { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
            { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
        
            { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
            { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
            { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
            { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
        
            { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
            { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
            { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
            { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
            { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
            { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
        
            { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
            { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
            { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
            { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
            { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
            { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
        
            { Kinect.JointType.Head, Kinect.JointType.Neck },
            { Kinect.JointType.Neck, Kinect.JointType.SpineShoulder },

            { Kinect.JointType.SpineShoulder, Kinect.JointType.SpineMid },
            { Kinect.JointType.SpineMid, Kinect.JointType.SpineBase }
        };
        public static Dictionary<JointType, JointType> _RadialBoneMap { get { return _radialBoneMap; } } //So it is static and read only

        static Dictionary<Kinect.JointType, Kinect.JointType> _mirrorBoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
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
            { Kinect.JointType.ShoulderRight, Kinect.JointType.ShoulderLeft },
        
            { Kinect.JointType.SpineBase, Kinect.JointType.SpineBase },
            { Kinect.JointType.SpineMid, Kinect.JointType.SpineMid },
            { Kinect.JointType.SpineShoulder, Kinect.JointType.SpineShoulder },
            { Kinect.JointType.Neck, Kinect.JointType.Neck },
            { Kinect.JointType.Head, Kinect.JointType.Head }
        };
        public static Dictionary<JointType, JointType> _MirrorBoneMap { get { return _mirrorBoneMap; } }
    }
}
