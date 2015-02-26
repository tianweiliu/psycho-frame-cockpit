using UnityEngine;
using RootSystem = System;
using System.Linq;
using System.Collections.Generic;
namespace Windows.Kinect
{
    //
    // Windows.Kinect.JointOrientation
    //
    [RootSystem.Runtime.InteropServices.StructLayout(RootSystem.Runtime.InteropServices.LayoutKind.Sequential)]
    public class JointOrientation
    {
        public Windows.Kinect.JointType JointType { get; set; }
        public Quaternion Orientation { get; set; }
    }

}
