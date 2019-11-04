using UnityEngine;
using System.Collections;

// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

public class ExamplePerlinMB : MonoBehaviour {
    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float Scale = 1.0F;
    public float Octave2Scale = 5.0f;
    public float Octave3Scale = 13.0f;
    public float Octave2Blend = 0.3f;
    public float Octave3Blend = 0.3f;

    public AnimationCurve WeightCurve;

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;

    void Start() {
        rend = GetComponent<Renderer>();

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];

        rend.material.SetTexture("_BaseMap", noiseTex);

        CalcNoise();
    }

    void CalcNoise() {
        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height) {
            float x = 0.0F;
            while (x < noiseTex.width) {
                float octave1 = GetPerlinOctave(y, x, 1);
                float octave2 = GetPerlinOctave(y, x, Octave2Scale);
                float octave3 = GetPerlinOctave(y, x, Octave3Scale);

                float blendedOctaves = WeightCurve.Evaluate(
                    Mathf.Clamp01(
                        (octave1 + octave2 * Octave2Blend + octave3 + Octave3Blend) / (1 + Octave2Blend + Octave3Blend)
                    )
                );

                pix[(int)y * noiseTex.width + (int)x] = new Color(blendedOctaves, blendedOctaves, blendedOctaves);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    private float GetPerlinOctave(float y, float x, float octaveScale) {
        float xCoord = xOrg + x / noiseTex.width * octaveScale * Scale;
        float yCoord = yOrg + y / noiseTex.height * octaveScale * Scale;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);

        return sample;
    }

    void Update() {
        CalcNoise();
    }
}

