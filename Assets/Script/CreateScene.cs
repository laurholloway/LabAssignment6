using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class CreateScene : MonoBehaviour
{
    //ground position
    private UnityEngine.Vector3 groundPosition = new UnityEngine.Vector3(0, 0, 0);
    private float groundSizeX = 100f;
    private float groundSizeZ = 100f;

    // inspector controls

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateGround();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateGround()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";

        ground.transform.position = groundPosition;
        ground.transform.localScale = new UnityEngine.Vector3(groundSizeX, 0.1f, groundSizeZ);

        Renderer rend = ground.GetComponent<Renderer>();
        rend.material = new Material(Shader.Find("Universal Render Pipeline/Simple Lit"));
        
        Color groundColor;
        ColorUtility.TryParseHtmlString("#E87971", out groundColor);
        rend.material.color = groundColor;
    }
}
