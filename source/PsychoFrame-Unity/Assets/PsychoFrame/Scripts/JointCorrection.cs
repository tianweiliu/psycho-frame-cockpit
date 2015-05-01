using UnityEngine;
using System.Collections;

public class JointCorrection : MonoBehaviour {

    public Transform joint;
    Vector3 positionOffset;

	// Use this for initialization
	void Start () {
        positionOffset = joint.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        joint.position = transform.position + positionOffset;
	}
}
