using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PipeMaterialColor
{
    Magenta = 1,
    Blue = 2,
    Yellow = 3,
    Green = 4
}

public class PipeManipulation : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _magentaMaterial;
    [SerializeField] private Material _blueMaterial;
    [SerializeField] private Material _yellowMaterial;
    [SerializeField] private Material _greenMaterial;

    // default material
    private Material _defaultMaterial;

    // current color
    [HideInInspector] public PipeMaterialColor pipeColor;
    [HideInInspector] public float pipeSize;

    // Start is called before the first frame update
    void Start()
    {
        _defaultMaterial = _renderer.material;

        pipeColor = PipeMaterialColor.Green;

        pipeSize = 0.5f;

        SetMaterial(pipeColor);
        SetSize(pipeSize);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMaterial(PipeMaterialColor color)
    {
        Material material = null;
        switch (color)
        {
            case PipeMaterialColor.Magenta:
                material = _magentaMaterial;
                break;
            case PipeMaterialColor.Blue:
                material = _blueMaterial;
                break;
            case PipeMaterialColor.Yellow:
                material = _yellowMaterial;
                break;
            case PipeMaterialColor.Green:
                material = _greenMaterial;
                break;
        }

        if (material != null)
        {
            _renderer.material = material;
        }
    }

    void RestoreMaterial()
    {
        _renderer.material = _defaultMaterial;
    }

    public void SetSize(float size)
    {
        // TODO: size mapping
        _renderer.gameObject.transform.localScale = new Vector3(pipeSize, pipeSize, pipeSize);
    }
}
