using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CubeRootMB : MonoBehaviour {

    public GameObject CubePrefab;
    public GameObject Effector;

    [Header("Needs to Correspond with Preview Texture")]
    public Vector2 CubeOffset = new Vector2(1, 1);
    public Texture2D DensityTexture;
    public Vector2 TextureScale = new Vector2(1, 1);
    public Vector2 TextureSize = new Vector2(64, 64);


    float[] _densities01;

    // Start is called before the first frame update
    void Start() {




    }

    // Update is called once per frame
    void Update() {

    }

    private float GetWeightAtPosition(Vector2 position) {

        return 0.5f;

    }


    //private CubeParams[] CalculateCubes(Bounds ) {



    //}


    private void LoadDensityTexture() {

        Color[] colors = DensityTexture.GetPixels();

        _densities01 = new float[colors.Length];
        for (int i = 0; i < colors.Length; i++) {
            _densities01[i] = colors[i].grayscale;
        }

    }
}

public struct CubeParams {
    float xPos;
    float zPos;
    float weight;
}