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
                _animator.SetIKPosition(AvatarIKGoal.LeftHand, fk.FusedModelBody.Joints[LeftHand].Position);
                _animator.SetIKPosition(AvatarIKGoal.RightHand, fk.FusedModelBody.Joints[RightHand].Position);
                _animator.SetIKPosition(AvatarIKGoal.LeftFoot, fk.FusedModelBody.Joints[LeftFoot].Position + footOffset);
                _animator.SetIKPosition(AvatarIKGoal.RightFoot, fk.FusedModelBody.Joints[RightFoot].Position + footOffset);
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
                _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            }
        }

        // Update is called once per frame
        void Update()
        {
            leftHand.position = fk.FusedModelBody.Joints[LeftHand].Position;
            rightHand.position = fk.FusedModelBody.Joints[RightHand].Position;
            leftFoot.position = fk.FusedModelBody.Joints[LeftFoot].Position + footOffset;
            rightFoot.position = fk.FusedModelBody.Joints[RightFoot].Position + footOffset;
        }
    }
}
