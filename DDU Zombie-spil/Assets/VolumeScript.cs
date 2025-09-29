using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeScript : MonoBehaviour
{
    public Volume volume;
    public Vignette vignette;

    [Range(0f, 1f)] public float vignetteInt;
    
    public bool playerHurt;



    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out vignette);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHurt)
        {
            if (!vignette.active)
            {
                vignette.active = true;
            }

            vignette.intensity.value = vignetteInt;
        }
        else if (vignette.active)
        {
            vignette.active = false;
        }
    }


}
