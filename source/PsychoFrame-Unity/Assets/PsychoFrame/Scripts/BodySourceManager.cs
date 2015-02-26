using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceManager : MonoBehaviour
{

    public string identifier;

    private Dictionary<int, Kinect.Body> _AvaliableBody = new Dictionary<int, Kinect.Body>();

	// Use this for initialization
	void Start () {
        PsychoFrameListener.OnPsychoFrameDataReceived += onKinectDataReceived;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void onKinectDataReceived(PsychoFrameListener sender, string identifier, int bodyIndex, Kinect.JointType jointType, Vector3 position, Quaternion rotation, Kinect.TrackingState trackingState)
    {
        if (identifier != this.identifier && this.identifier != "")
            return;
        if (!_AvaliableBody.ContainsKey(bodyIndex))
            _AvaliableBody[bodyIndex] = new Kinect.Body();
        _AvaliableBody[bodyIndex].UpdateJoint(jointType, position, rotation, trackingState);
        //print(identifier + ": " + bodyIndex + ": " + jointType.ToString() + ": " + position.ToString() + " " + rotation.ToString() + " " + trackingState.ToString());
    }

    public Kinect.Body[] GetData()
    {
        Kinect.Body[] bodies = new Kinect.Body[_AvaliableBody.Count];
        _AvaliableBody.Values.CopyTo(bodies, 0);
        return bodies;
    }
}
