using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRootMB : MonoBehaviour {

    public GameObject CubePrefab;
    public GameObject Effector;

    public Vector2 CubeOffset = new Vector2(1, 1);
    public Vector2 SimulationScale = new Vector2(1, 1);
    public int SimulationSizeX = 64;
    public int SimulationSizeZ = 64;

    [Header("Perlin Density Noise Parameters")]
    public float Scale = 1.0f;
    public float Octave2Scale = 5.0f;
    public float Octave3Scale = 13.0f;
    public float Octave2Blend = 0.3f;
    public float Octave3Blend = 0.3f;

    public AnimationCurve DensityWeightCurve = new AnimationCurve(
        new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) }
    );

    private void Start() {
        _effectorMB = Effector.GetComponent<EffectorPointMB>();
        SpawnCubes();
    }

    private void Update() {
        PerformReCompute();
    }

    private void PerformReCompute() {
        foreach (CubeMB cube in SpawnedCubes) {
            var pos = cube.transform.position;
            cube.SetCellWeight(
                Mathf.Clamp01(
                    Mathf.Sqrt(
                        GetDensityAt(pos) * GetEffectorWeightAt(pos)
                    )
                )
            );
        }
    }

    private List<CubeMB> SpawnedCubes;
    private void SpawnCubes() {
        SpawnedCubes = new List<CubeMB>();

        // for now, spawn the full grid of them
        for (float x = CubeOffset.x / 2; x < SimulationSizeX * SimulationScale.x; x += CubeOffset.x) {
            for (float z = CubeOffset.y / 2; z < SimulationSizeZ * SimulationScale.y; z += CubeOffset.y) {
                Spawn(x, z);
            }
        }

    }

    private void Spawn(float x, float z) {
        CubeMB cubeMB = Instantiate(CubePrefab, this.transform).GetComponent<CubeMB>();

        Vector3 pos = new Vector3(x, 0, z);

        cubeMB.transform.position = pos;

        SpawnedCubes.Add(cubeMB);
    }

    private Bounds _textureBounds;
    public float GetDensityAt(Vector3 worldPosition) {
        return CalcPerlinDensity(worldPosition);
    }

    private EffectorPointMB _effectorMB;
    public float GetEffectorWeightAt(Vector3 worldPosition) {

        // 2D? Or not 2D? For now, project onto XZ only
        Vector2 twoDpos = new Vector2(
            worldPosition.x,
            worldPosition.z
        );
        Vector2 twoDeffector = new Vector2(
            Effector.transform.position.x,
            Effector.transform.position.z
        );

        float rawDistance = Mathf.Abs((twoDpos - twoDeffector).magnitude);

        float effect01 = 1.0f - Mathf.Clamp01(rawDistance / _effectorMB.Radius);

        return _effectorMB.Weight.Evaluate(effect01);
    }

    private float CalcPerlinDensity(Vector3 worldPos) {

        float octave1 = GetPerlinOctave(worldPos.x, worldPos.z, 1);
        float octave2 = GetPerlinOctave(worldPos.x, worldPos.z, Octave2Scale);
        float octave3 = GetPerlinOctave(worldPos.x, worldPos.z, Octave3Scale);

        float blendedOctaves = DensityWeightCurve.Evaluate(
            Mathf.Clamp01(
                (octave1 + octave2 * Octave2Blend + octave3 * Octave3Blend) / (1 + Octave2Blend + Octave3Blend)
            )
        );

        return blendedOctaves;

    }

    private float GetPerlinOctave(float x, float y, float octaveScale) {
        float xCoord = x / octaveScale * Scale;
        float yCoord = y / octaveScale * Scale;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);

        return sample;
    }

}
