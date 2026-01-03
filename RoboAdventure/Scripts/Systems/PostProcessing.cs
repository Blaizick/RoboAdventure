using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

public class PostProcessing
{
    public PressureSystem pressureSystem;
    public Volume volume;
    
    private ChromaticAberration m_ChromaticAberration;
    private LensDistortion m_LensDistortion;
    private Vignette m_Vignette;

    private CmsPostProcessingComp m_CmsPostProcessingComp;
    
    [Inject(Id = InjectIds.PostProcessingCmsEntity)] public CmsEntity cmsEntity;
    
    public PostProcessing(PressureSystem pressureSystem, Volume volume)
    {
        this.pressureSystem = pressureSystem;
        this.volume = volume;
    }

    public void Init()
    {
        volume.profile.TryGet(out m_ChromaticAberration);
        volume.profile.TryGet(out m_LensDistortion);
        volume.profile.TryGet(out m_Vignette);
        
        cmsEntity.TryGetComponent(out m_CmsPostProcessingComp);
    }
    
    public void _Update()
    {
        m_ChromaticAberration.intensity.value = pressureSystem.Pressure * m_CmsPostProcessingComp.chromaticAbberationPerPressureUnit;
        m_LensDistortion.intensity.value = pressureSystem.Pressure * m_CmsPostProcessingComp.lensDistortionPerPressureUnit;
        m_Vignette.intensity.value = pressureSystem.Pressure * m_CmsPostProcessingComp.vignettePerPressureUnit;
    }
}