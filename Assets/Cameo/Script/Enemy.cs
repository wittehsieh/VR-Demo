using UnityEngine;
using System.Collections;

namespace Cameo
{
    public class Enemy : MonoBehaviour
    {
        public int Level = 0;
        public VREnemyHitTrigger HitTrigger;

        private Transform _target;

        void Awake()
        {
            GameSystem.Instance.RegisterEnemy(Level, this);
            HitTrigger.OnHitCallback += OnHit;

            GameObject targetObj = GameObject.Find("VRRayCaster");
            if (targetObj != null)
            {
                _target = targetObj.transform;
            }

            gameObject.SetActive(false);
        }

        void Update()
        {
            if (_target != null)
            {
                transform.LookAt(_target);
            }
        }

        private void OnHit()
        {
            StartCoroutine(playHitEffect());
        }

        public void OnStart()
        {
            gameObject.SetActive(true);
        }

        private IEnumerator playHitEffect()
        {
            Debug.Log("play fly effect");
            yield return new WaitForSeconds(1);
            Debug.Log("play explosion effect");
            yield return new WaitForSeconds(1.5f);

            Destroy(gameObject);
        }
    }
}
