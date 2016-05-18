using UnityEngine;
using System.Collections;

namespace Cameo {
	public class IKController : MonoBehaviour {

		public Transform LookTarget;
        public Transform LookCamera;
		public Transform IKObject;
		public Animator Controller;
		public bool IsActive = true;

		// Use this for initialization
		void Start () {
	
		}

		void OnAnimatorIK()
		{
			if(Controller) {

				//if the IK is active, set the position and rotation directly to the goal. 
				if(IsActive) {

					// Set the look target position, if one has been assigned
					if(LookTarget != null) {
						Controller.SetLookAtWeight(1);
						Controller.SetLookAtPosition(LookTarget.position);
					}    

                    if(LookCamera != null)
                    {
                        Controller.SetLookAtWeight(1);
                        Controller.SetLookAtPosition(LookCamera.position + LookCamera.forward * 100);
                    }

					// Set the right hand target position and rotation, if one has been assigned
					if(IKObject != null) {
						Controller.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
						Controller.SetIKRotationWeight(AvatarIKGoal.RightHand,1);  
						Controller.SetIKPosition(AvatarIKGoal.RightHand,IKObject.position);
						Controller.SetIKRotation(AvatarIKGoal.RightHand,IKObject.rotation);
					}        

				}

				//if the IK is not active, set the position and rotation of the hand and head back to the original position
				else {          
					Controller.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
					Controller.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 
					Controller.SetLookAtWeight(0);
				}
			}
		} 
	}
}