#define _VERSION_III_
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AtStudio.LightPainting
{
    [ExecuteInEditMode]
    public class LightBeam2D : MonoBehaviour
    {
        public float widthBase = 1f;
#if _VERSION_I_
        public float angle
        {
            get
            {
                return LightBeam2DManager.instance.lightAngle;
            }
        }
        public float width
        {
            get
            {
                return widthBase * Mathf.Sin( angle * Mathf.Deg2Rad) * pixel2World ;
            }
        }
        Vector3 size = new Vector3(1f, 1f, 1f);
#endif

#if _VERSION_II_ || _VERSION_III_
        private LightBeam2DManager manager;
        private Vector3 position;
        private Vector3 right;
        private int maskValue;
        public float width
        {
            get
            {
                return manager.widthWithAngle * pixel2World;
            }
        }

        [SerializeField , ReadOnly] float _angle = 0;
        float angle
        {
            get
            {
                return _angle;
            }

            set
            {
                if (!value.Equals(_angle)) // change the scal only if the size is changed
                {
                    _angle = value;
                    transform.eulerAngles = new Vector3(0, 0, angle);
                }
            }
        }


        [SerializeField, ReadOnly]  Vector3 _size = Vector3.one;
        Vector3 size
        {
            get
            {
                return _size;
            }

            set
            {
                if ( !value.Equals(_size ) ) // change the scal only if the size is changed
                {
                    _size = value;
                    transform.localScale = _size;
                }
            }
        }
#endif
        public float pixel2World = 10f;

#if _VERSION_I_ || _VERSION_II_
        private void Update()
        {
            UpdateAngle();
            UpdateScale();
        }
#endif
        public void OnEnable()
        {

#if _VERSION_II_ || _VERSION_III_

            manager = LightBeam2DManager.instance;
            position = transform.position;
            right = transform.right;
            maskValue = manager.layerMask.value;
#endif
        }

        public void UpdateAngle()
        {

#if _VERSION_I_
            transform.eulerAngles = new Vector3(0, 0, LightBeam2DManager.instance.lightBeamAngle);
#endif
#if _VERSION_II_ || _VERSION_III_
            angle = manager.lightBeamAngle;
#endif
        }

        public void UpdateScale()
        {
#if _VERSION_I_
            RaycastHit hitInfo;
            if (Physics.Raycast( transform.position , transform.right ,  out hitInfo , 100f , LightBeam2DManager.instance.layerMask.value ,  QueryTriggerInteraction.Collide ) )
            {
                var length = hitInfo.distance * pixel2World;
                size.x = length * 2f;
                size.y = width;
                size.z = 1f;
            }else
            {
                size.x = 100f * pixel2World;
                size.y = width;
                size.z = 1f;
            }

            transform.localScale = size;
#endif
#if _VERSION_II_ || _VERSION_III_
            RaycastHit hitInfo;
            var temSize = size;
            if (Physics.Raycast( position , right ,  out hitInfo , 100f , maskValue,  QueryTriggerInteraction.Collide ) )
            {
                var length = hitInfo.distance * pixel2World;
                temSize.x = length * 2f;
                temSize.y = width;
                temSize.z = 1f;
            }else
            {
                temSize.x = 100f * pixel2World;
                temSize.y = width;
                temSize.z = 1f;
            }
            size = temSize;

#endif
        }
    }
}