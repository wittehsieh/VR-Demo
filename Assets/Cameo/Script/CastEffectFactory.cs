using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cameo
{
    public class CastEffectFactory : Singleton<CastEffectFactory>
    {
        public List<GameObject> EffectList;

        public static void CreateFlyEffect(int effectIndex, Vector3 from, Transform to, float during)
        {
            GameObject effect = GameObject.Instantiate(Instance.EffectList[effectIndex]);
            EffectHandler handler = effect.GetComponent<EffectHandler>();
            handler.InitEffect(from, to, during);
        }
    }

}
