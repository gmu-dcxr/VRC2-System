using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class texture_scaling : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject sideA;
    //public GameObject sideB;
    public bool cut = false;
    void Start()
    {

    }

    // Update is called once per frame
    // update on cut
    void Update()
    {
        if (cut)
        {
            scalePipeTexture(sideA);
            //scalePipeTexture(sideB);
        }
    }

    void scalePipeTexture(GameObject pipe) 
    {
        //Pipe with texture
        //Scale texture on given pipe based on x value of pipe scale.
        //check pipe type (water has multiple textures)
        float xValue = pipe.transform.localScale.x;
        float yValue = pipe.transform.localScale.y;
        Renderer r = pipe.GetComponent<Renderer>();
        //material of pipe, changes to this only effect this object. ONLY AT RUNTIME
        Material m = r.material; 
        m.SetTextureScale("_MainTex", new Vector2(xValue, yValue));
        //Having offset set between values 0.1-0.4, inclusive, seemed to properly align texture. 
        m.SetTextureOffset("_MainTex", new Vector2(0.1f, 0.0f));

        m.SetTextureScale("_BumpMap", new Vector2(xValue, yValue));
        m.SetTextureOffset("_BumpMap", new Vector2(0.1f, 0.0f));
        m.SetTextureScale("_ParallaxMap", new Vector2(xValue, yValue));
        m.SetTextureOffset("_ParallaxMap", new Vector2(0.1f, 0.0f));
    }
}

