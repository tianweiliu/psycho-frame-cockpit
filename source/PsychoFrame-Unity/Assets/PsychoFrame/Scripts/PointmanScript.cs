using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using System.Xml;

public class PointmanScript : MonoBehaviour {

    public BodySourceManager _BodyManager;

    public int BodyIndex;
    public bool EnableRotation;
    public bool UseReplay;
    
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

    public bool OnRecord;
    public string SaveLocation;
    

    private List<Kinect.Body> _AvaliableBody;
    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
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

    private XmlWriter _RecordWriter;
    private XmlDocument _RecordReader;
    private int _RecordIndex;

	// Use this for initialization
	void Start () {
        if (!UseReplay)
        {
            if (_BodyManager == null)
            {
                Debug.LogError("Body Manager is Not Found");
                return;
            }

            _AvaliableBody = new List<Kinect.Body>();
        }
        else {
            _RecordReader = new XmlDocument();
            _RecordReader.Load(SaveLocation);
            _RecordIndex = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!UseReplay)
        {

            Kinect.Body[] data = _BodyManager.GetData();
            if (data == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.F10))
            {
                if (!OnRecord)
                {
					XmlWriterSettings setting = new XmlWriterSettings();
					setting.Indent = true;
					setting.IndentChars = "\t";
					setting.OmitXmlDeclaration = true;

                    OnRecord = true;
                    // Start Recording
                    Debug.Log("Start Recording");

                    _RecordIndex = 0;
                    _RecordWriter = XmlWriter.Create(SaveLocation,setting);
					_RecordWriter.Settings.Indent = true;
                    _RecordWriter.WriteStartDocument();
					_RecordWriter.WriteStartElement("Root");

                }
                else
                {
                    OnRecord = false;
                    // End Recording, Write Record Into File
                    _RecordWriter.WriteElementString("Length", _RecordIndex.ToString());
					_RecordWriter.WriteEndElement();
					_RecordWriter.WriteEndDocument();
                    _RecordWriter.Close();

                    Debug.Log("Finish Record");
                }
            }

            _AvaliableBody.Clear();
            foreach (Kinect.Body body in data)
            {
                if (body == null)
                {
                    continue;
                }
                if (body.IsTracked)
                {
                    _AvaliableBody.Add(body);
                }
            }

            if (OnRecord)
            {
                _RecordWriter.WriteStartElement("Frame"+_RecordIndex.ToString());
            }
            for (int index = 0; index < _AvaliableBody.Count; index++)
            {
                if (_AvaliableBody[index] == null)
                {
                    continue;
                }
                if (index == BodyIndex && _AvaliableBody[index].IsTracked)
                {
                    RefreshBodyObject(_AvaliableBody[index]);
                }
                else
                {
                    continue;
                }
            }
            if (OnRecord)
            {
                _RecordWriter.WriteEndElement();
				_RecordIndex++;
            }
        }
        else {
            ReplayBodyObject();
            _RecordIndex = (_RecordIndex + 1) % int.Parse(_RecordReader["Root"]["Length"].InnerText);
        }
	}

    private void RefreshBodyObject(Kinect.Body body)
    {

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            if (OnRecord) {
                _RecordWriter.WriteStartElement("jt"+((int)jt).ToString());
            }


            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint targetJoint = null;

            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }


            GameObject pointObj = JointToGameObject(jt);
            pointObj.transform.localPosition = GetVector3FromJoint(sourceJoint);

            if (OnRecord)
            {
                _RecordWriter.WriteElementString("PosX", GetVector3FromJoint(sourceJoint).x.ToString());
                _RecordWriter.WriteElementString("PosY", GetVector3FromJoint(sourceJoint).y.ToString());
                _RecordWriter.WriteElementString("PosZ", GetVector3FromJoint(sourceJoint).z.ToString());
                _RecordWriter.WriteElementString("TrackState", sourceJoint.TrackingState.ToString());
            }

            if (EnableRotation && targetJoint != null) {
                pointObj.transform.rotation = Quaternion.LookRotation((GetVector3FromJoint(targetJoint) - pointObj.transform.localPosition).normalized);
            }

            LineRenderer lr = pointObj.GetComponent<LineRenderer>();
            if (targetJoint != null)
            {
                lr.SetPosition(0, pointObj.transform.localPosition);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint));
                lr.SetColors(GetColorForState(sourceJoint.TrackingState), GetColorForState(targetJoint.TrackingState));
            }
            else
            {
                lr.enabled = false;
            }

            if (OnRecord) {
                _RecordWriter.WriteEndElement();
            }
            
        }
    }

    private void ReplayBodyObject() { 
		XmlNode root = (XmlNode)_RecordReader.DocumentElement;
        XmlNode currentFrame = root["Frame"+_RecordIndex.ToString()];
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {

            XmlNode currentJoint = currentFrame["jt"+((int)jt).ToString()];
            XmlNode targetJoint = null;
            
            if (_BoneMap.ContainsKey(jt))
            {
                Kinect.JointType targetJointType = _BoneMap[jt];
                targetJoint = currentFrame["jt"+((int)targetJointType).ToString()];
            }


			if(currentJoint != null){
				//print ("At Frame "+_RecordIndex +" Value "+currentJoint["PosX"].InnerText +", "+currentJoint["PosY"].InnerText+", "+currentJoint["PosZ"].InnerText);
            	GameObject pointObj = JointToGameObject(jt);
				Vector3 pointPosition = GetVector3FromString(currentJoint["PosX"].InnerText, currentJoint["PosY"].InnerText, currentJoint["PosZ"].InnerText);
				Vector3 targetPosition = targetJoint == null ? Vector3.zero : GetVector3FromString(targetJoint["PosX"].InnerText, targetJoint["PosY"].InnerText, targetJoint["PosZ"].InnerText);
	
    	        pointObj.transform.localPosition = pointPosition;
					
	            if (EnableRotation && targetJoint != null)
    	        {
        	        pointObj.transform.rotation = Quaternion.LookRotation((targetPosition - pointPosition).normalized);
            	}

	            LineRenderer lr = pointObj.GetComponent<LineRenderer>();
    	        if (targetJoint != null)
        	    {
            	    lr.SetPosition(0, pointObj.transform.localPosition);
                	lr.SetPosition(1, targetPosition);
	            }
    	        else
        	    {
            	    lr.enabled = false;
            	}
			}
			else{
				print ("Jt "+((int)jt).ToString() +" is Null at "+_RecordIndex.ToString());
			}
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.x * 10, joint.Position.y * 10, joint.Position.z * 10);
    }

    private GameObject JointToGameObject(Kinect.JointType joint) {
        switch (joint) { 
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

    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
            case Kinect.TrackingState.Tracked:
                return Color.green;

            case Kinect.TrackingState.Inferred:
                return Color.red;

            default:
                return Color.black;
        }
    }

    private Vector3 GetVector3FromString(string x, string y, string z) {
        float xF, yF, zF;
        xF = float.Parse(x);
        yF = float.Parse(y);
        zF = float.Parse(z);
        return new Vector3(xF, yF, zF);
    }
}
