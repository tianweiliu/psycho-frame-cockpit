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
                _animator.SetIKPosition(AvatarIKGoal.LeftFoot, RelativeJointPostion(LeftFoot));
                _animator.SetIKPosition(AvatarIKGoal.RightFoot, RelativeJointPostion(RightFoot));
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
            leftFoot.position = RelativeJointPostion(LeftFoot);
            rightFoot.position = RelativeJointPostion(RightFoot);
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
