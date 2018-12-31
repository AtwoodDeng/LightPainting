using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

namespace AtStudio.LightPainting
{
    [System.Serializable]
    [PostProcess(typeof(LightingPPRender) , PostProcessEvent.BeforeStack , "Custom/LightingPP")]
    public class LightingPP : PostProcessEffectSettings
    {
        //public ColorParameter color;
    }
     
    public class LightingPPRender : PostProcessEffectRenderer<LightingPP>
    {
        public override void Init()
        {
            base.Init();
        }
        public override void Render(PostProcessRenderContext context)
        {


        }
    }

}