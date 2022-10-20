using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;

public class OutPutCameraAspect : MonoBehaviour
{
    int matID;
    // Start is called before the first frame update
    void Start()
    {
        // var camera = this.GetComponent<Camera>();
        // Debug.Log(camera.aspect);
        // Debug.Log(camera.projectionMatrix);
        // Debug.Log(GL.GetGPUProjectionMatrix(camera.projectionMatrix, false));

    }

    public void NewMethod(Camera camera)
    {
        float aspect = camera.aspect;


        float Near = 0.3f;
        float Far = 1000f;
        float FOV = 40f;

        float cot = 1 / Mathf.Tan(FOV / 2);
        float t1 = cot / aspect;
        float t2 = cot;
        float t3 = (Far + Near) / (Far - Near) * -1;
        float t4 = (2 * Near * Far) / (Far - Near) * -1;

        // FOV = camera.fieldOfView;

        var matrix = Matrix4x4.Perspective(FOV, aspect, Near, Far);
        matrix = GL.GetGPUProjectionMatrix(matrix, false);


        matID = Shader.PropertyToID("_perspectiveVector");
        var renders = this.GetComponentsInChildren<CanvasRenderer>(false);
        foreach (var render in renders)
        {
            Material material = render.GetMaterial();
            if(material!=null)
            material.SetVector(matID, new UnityEngine.Vector4(
                matrix[0, 0], matrix[1, 1],
            matrix[2, 2], matrix[2, 3]));
        }

        Debug.Log(matrix);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
