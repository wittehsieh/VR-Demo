using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

namespace Cameo
{
    public class PlatformSwitcher : MonoBehaviour
    {
        public GameObject NonVRCamera;
        public GameObject VRCamera;
        public IKController IKController;

        // Use this for initialization
        void Start()
        {
            NonVRCamera.SetActive(!GameSystem.Instance.UseVR);
            VRCamera.SetActive(GameSystem.Instance.UseVR);

            if(IKController != null)
            {
                IKController.LookCamera = (GameSystem.Instance.UseVR) ? VRCamera.transform : NonVRCamera.transform;
            }
        }
    }
}

