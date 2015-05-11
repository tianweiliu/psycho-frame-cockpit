using UnityEngine;
using System.Collections;
using Kinect = Windows.Kinect;

namespace ProjectPsychoFrame
{
    [RequireComponent(typeof(Animator))]
    public class IKController : MonoBehaviour
    {

        protected Animator _animator;
        public RelativePosition fk;

        [Header("Root Transform")]
        public Transform root;
        public float rootTransformScale;
        
        public Kinect.JointType LeftHand = Kinect.JointType.WristLeft;
        public Kinect.JointType RightHand = Kinect.JointType.WristRight;
        public Kinect.JointType LeftFoot = Kinect.JointType.AnkleLeft;
        public Kinect.JointType RightFoot = Kinect.JointType.AnkleRight;

        public Vector3 footOffset;

        public Transform leftHand, rightHand, leftFoot, rightFoot;

        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        void OnAnimatorIK()
        {
            if (fk)
            {
                _animator.SetIKPosition(AvatarIKGoal.LeftHand, RelativeJointPostion(LeftHand));
                _animator.SetIKPosition(AvatarIKGoal.RightHand, RelativeJointPostion(RightHand));
                _animator.SetIKPosition(AvatarIKGoal.LeftFoot, RelativeJointPostion(LeftFoot) + footOffset);
                _animator.SetIKPosition(AvatarIKGoal.RightFoot, RelativeJointPostion(RightFoot) + footOffset);
                //_animator.SetLookAtPosition(RelativeJointPostion(Root) + fk.FusedAbsoluteModelBody.JointOrientations[Root].Orientation * Vector3.forward);

                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
                _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            }
        }

        // Update is called once per frame
        void Update()
        {
            leftHand.position = RelativeJointPostion(LeftHand);
            rightHand.position = RelativeJointPostion(RightHand);
            leftFoot.position = RelativeJointPostion(LeftFoot) + footOffset;
            rightFoot.position = RelativeJointPostion(RightFoot) + footOffset;

            if (root != null)
            {
                transform.root.position = root.transform.position * rootTransformScale;
                transform.root.rotation = root.transform.rotation;
            }
        }

        /// <summary>
        /// Return joint transform relative to the gameObject that this script is attached to, 
        /// so the IK will be independent from parents' position and rotation.
        /// </summary>
        /// <param name="jointType">Joint type</param>
        /// <returns>Independent IK joint position</returns>
        Vector3 RelativeJointPostion(Kinect.JointType jointType)
        {
            return transform.TransformPoint(Quaternion.Inverse(transform.rotation) * (fk.FusedAbsoluteModelBody.Joints[jointType].Position - transform.position));
        }
    }
}
