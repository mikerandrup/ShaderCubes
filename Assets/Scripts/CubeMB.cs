using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMB : MonoBehaviour {

    public Material[] MaterialsSortedByWeight;

    private MeshRenderer _meshRenderer;

    public void Start() {
        _meshRenderer = GetComponent<MeshRenderer>();
    }


    public void SetCellWeight(float value) {

        // scale the height accordingly

        // pick the appropriate material and assign it
        // inline material array stuff can be found in config driven runSquish code

    }


}
