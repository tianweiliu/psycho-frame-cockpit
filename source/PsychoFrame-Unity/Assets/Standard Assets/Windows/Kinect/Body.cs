using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Windows
{
    namespace Kinect
    {
        //
        // Windows.Kinect.Body
        //
        public class Body
        {
            public bool IsTracked = false;
            public Dictionary<JointType, Joint> Joints;
            public Dictionary<JointType, JointOrientation> JointOrientations;

            public Body()
            {
                this.IsTracked = true;
                this.Joints = new Dictionary<JointType, Joint>();
                this.JointOrientations = new Dictionary<JointType, JointOrientation>();
                foreach (JointType jointType in Enum.GetValues(typeof(JointType)))
                {
                    Joints[jointType] = new Joint();
                    JointOrientations[jointType] = new JointOrientation();
                }
            }

            public void UpdateJoint(JointType jointType, Vector3 position, Quaternion rotation, TrackingState trackingState)
            {
                this.Joints[jointType].JointType = jointType;
                this.Joints[jointType].Position = position;
                this.Joints[jointType].TrackingState = trackingState;
                this.JointOrientations[jointType].Orientation = rotation;
            }
        }
    }
}