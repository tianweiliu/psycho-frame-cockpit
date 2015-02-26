using UnityEngine;
using RootSystem = System;
using System.Linq;
using System.Collections.Generic;
namespace Windows.Kinect
{
    //
    // Windows.Kinect.Joint
    //
    [RootSystem.Runtime.InteropServices.StructLayout(RootSystem.Runtime.InteropServices.LayoutKind.Sequential)]
    public class Joint
    {
        public Windows.Kinect.JointType JointType { get; set; }
        public Vector3 Position { get; set; }
        public Windows.Kinect.TrackingState TrackingState { get; set; }
    }

}
