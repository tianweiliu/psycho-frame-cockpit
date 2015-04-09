using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class PointmanCalibrator : MonoBehaviour {
    public PointmanScript[] m_Cameras;
    public float[] m_CameraAngles; 
    public int m_MainCameraIndex;

    Kinect.Body[] m_Bodies;

	// Use this for initialization
	void Start () {
        m_Bodies = new Kinect.Body[m_Cameras.Length];
	}
	
	// Update is called once per frame
	void Update () {
	    // Check main camera skelenton to check if any bone is lost track

        
        for(int n = 0; n < m_Cameras.Length; ++n){
            //m_Bodies[n] = m_Cameras[(m_MainCameraIndex + n) % m_Cameras.Length]._AvaliableBody[0];
            if (n == m_MainCameraIndex)
            {
                m_Cameras[n].RotateFront();
            }
            else {
                m_Cameras[n].RotateBack();
            }
        }
        
	}

    void CheckMainBody(Kinect.Body[] bodies) {
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = bodies[0].Joints[jt];
            if (sourceJoint.TrackingState != Kinect.TrackingState.Tracked) { 
                // Try replace it with tracked backups

                for (int n = 1; n < bodies.Length; n++) {
                    Kinect.Joint backupJoint = bodies[n].Joints[jt];
                    if (backupJoint.TrackingState == Kinect.TrackingState.Tracked) { 
                        // We can try this


                        break;
                    }
                }

                
            }
        }
    }
}
