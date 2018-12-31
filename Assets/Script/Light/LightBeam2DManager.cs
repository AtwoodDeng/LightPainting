#define _VERSION_III_
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AtStudio.LightPainting
{
    [ExecuteInEditMode]
    public class LightBeam2DManager : MonoBehaviour
    {

        public static LightBeam2DManager _instance;
        public static LightBeam2DManager instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<LightBeam2DManager>();
                return _instance;
            }
        }

        public LayerMask layerMask;
        [Range(1,10)]
        public int updateRound = 1;

        private Light _light;
        public Light light
        {
            get
            {
                if (_light == null)
                    _light = gameObject.GetComponent<Light>();
                if (_light == null)
                    _light = FindObjectOfType<Light>();
                return _light;
            }
        }

#if _VERSION_I_
        public float lightAngle
        {
            get
            {
                var forward = light.transform.forward;
                return - Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
            }
        }


#endif
#if _VERSION_II_ || _VERSION_III_

        private Transform _lightTrans;
        public Transform lightTrans
        {
            get
            {
                if (_lightTrans == null)
                    _lightTrans = light?.transform;
                return _lightTrans;
            }
        }

        public float lightAngle;

        public float widthWithAngle;

#endif 

        public float lightBeamAngle
        {
            get
            {
                return -lightAngle;
            }
        }

        public void Update()
        {
#if _VERSION_II_ || _VERSION_III_
            var forward = lightTrans.forward;
            lightAngle = -Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;

            widthWithAngle = width / BeamCount * Mathf.Sin(lightAngle * Mathf.Deg2Rad);
#endif

#if _VERSION_III_

            
            int rand2 = Random.RandomRange(0, 9999);
            for ( int i = 0; i < beams.Count; ++ i )
            {
                if ( ( (i + rand2) % updateRound == 0) )
                {
                    beams[i].UpdateAngle();
                    beams[i].UpdateScale();
                }
            }
#endif
        }

        public List<LightBeam2D> beams = new List<LightBeam2D>();

        /// <summary>
        ///      Beam Creation Related
        /// </summary>
        [BoxGroup("Beam Creation")]
        public int BeamCount = 10;
        [BoxGroup("Beam Creation")]
        public float offsetX = 0f;
        [BoxGroup("Beam Creation")]
        public float height = 5f;
        [BoxGroup("Beam Creation")]
        public float width = 5f;

        [BoxGroup("Beam Creation")]
        public GameObject beamPrefab;

        [BoxGroup("Beam Creation")]
        [Button("SetUp" , ButtonSizes.Large)]
        public void SetUp()
        {
#if UNITY_EDITOR
            if (beamPrefab == null)
                return;
            // clean up
            var children = transform.GetComponentsInChildren<Transform>();
            for( int i = 0; i < children.Length; ++ i )
            {
                if ( children[i] != transform )
                    DestroyImmediate(children[i].gameObject);
            }

            beams.Clear();
            for( int i = 0; i < BeamCount; ++ i )
            {
                var position = new Vector3((i - (BeamCount - 1) * 0.5f) * width / BeamCount + offsetX, height, 0);

                var prefab = Instantiate(beamPrefab) as GameObject;

                prefab.transform.position = position;
                prefab.transform.parent = transform;

                var beam = prefab.GetComponent<LightBeam2D>();
                beam.widthBase = width / BeamCount;
                beams.Add(beam);
            }
        }
#endif


        [BoxGroup("Beam Creation")]
        [Button("Refresh", ButtonSizes.Large)]
        public void Refresh()
        {
            for (int i = 0; i < beams.Count; ++i)
            {
                if (beams[i] != null)
                    beams[i].OnEnable();
            }
        }

        private void OnDrawGizmos()
        {

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(-width * 0.5f, height, 0), new Vector3(width * 0.5f, height, 0));

            for (int i = 0; i < BeamCount; ++i)
            {
                var position = new Vector3((i - (BeamCount - 1) * 0.5f) * width / BeamCount + offsetX, height, 0);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(position, Mathf.Min(0.1f,width / BeamCount));
            }

        }

    }

}