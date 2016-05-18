using UnityEngine;
using System.Collections;
using System;

namespace Cameo
{
    public class VREnemyHitTrigger : VRLookButton
    {
        public Action OnHitCallback = delegate { };

        protected override void onFinishLook()
        {
            OnHitCallback();
        }
    }
}
