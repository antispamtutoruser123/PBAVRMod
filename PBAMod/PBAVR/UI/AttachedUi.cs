using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PBAVR
{
    class AttachedUi : MonoBehaviour
    {
        private Transform targetTransform;

        public static void Create<TAttachedUi>(Canvas canvas, float scale = 0)
            where TAttachedUi : AttachedUi
        {
            var instance = canvas.gameObject.AddComponent<TAttachedUi>();
            if (scale > 0) canvas.transform.localScale = new Vector3(scale, scale, scale); ;
            canvas.renderMode = RenderMode.WorldSpace;
            
        }
        protected virtual void Start()
        {
     
        }
        protected virtual void Update()
        {
            UpdateTransform();
        }

        public void SetTargetTransform(Transform target)
        {
            targetTransform = target;
        }

        private void UpdateTransform()
        {
          //  transform.position = Camera.main.transform.position + Camera.main.transform.forward;
         //   transform.rotation = Camera.main.transform.rotation;
        }
    }
}
