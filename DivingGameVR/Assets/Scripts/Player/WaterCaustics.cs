using UnityEngine;

[RequireComponent(typeof(Projector))]
public class WaterCaustics : MonoBehaviour
{
    [Header("Public vars")]
    [SerializeField] private float fps = 30f;
    [SerializeField] private Texture2D[] frames;

    [Header("Internal vars")]
    [SerializeField] private int frameIndex;
    [SerializeField] private Projector projector;

    private void Start()
    {
        projector = GetComponent<Projector>();
        InvokeRepeating(nameof(NextFrame), 1/fps, 1/fps);
    }

    private void NextFrame()
    {
        projector.material.SetTexture("_ShadowTex", frames[frameIndex]);
        frameIndex = (frameIndex + 1) % frames.Length;
    }
}
