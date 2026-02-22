using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateScene : MonoBehaviour
{
    //ground position
    private Vector3 groundPosition = new Vector3(0, 0, 0);
    private float groundSizeX = 100f;
    private float groundSizeZ = 100f;

    // pyramid position and scale
    private Vector3 pyramidPosition = new Vector3(15,0,0);
    private float cubeSize = 2f;

    // forest position
    private Vector3 forestCenter = new Vector3(-15,0,0);

    // inspector controls

    [Header("Pyramid Settings")]
    [Range(3,15)]
    public int pyramidBaseSize = 5;

    [Header("Forest Settings")]
    public int numberOfTrees = 20;
    public float forestSpread = 15f;




    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateGround();
        CreatePyramid();
        CreateForest();
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

    void CreatePyramid()
    {
        // sets min and max pyramid sizes
        pyramidBaseSize = Mathf.Clamp(pyramidBaseSize, 3, 15);

        int totalLevels = pyramidBaseSize;

        GameObject pyramidParent = new GameObject("Pyramid");
        pyramidParent.transform.position = pyramidPosition;

        for(int level = 0; level < totalLevels; level++)
        {
            // makes it so each level grid gets smaller
            int cubesInThisRow = pyramidBaseSize - level;

            // changes color on each level
            float hue = (float)level / totalLevels;
            Color levelColor = Color.HSVToRGB(hue, 0.8f, 0.9f);

            // makes it so cubes are stacked correctly based on cube size
            float yPosition = pyramidPosition.y + level * cubeSize + cubeSize / 2f;

            //keeps the pyramid centered
            float offset = (cubesInThisRow - 1)/2f;

            // loops to create pyramid levels
            for(int x = 0; x < cubesInThisRow; x++)
            {
                for(int z = 0; z < cubesInThisRow; z++)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "PyramidCube_L" + level + "_" + x + "_" + z;
                    cube.transform.parent = pyramidParent.transform;

                    cube.transform.localScale = Vector3.one * cubeSize;

                    cube.transform.position = new UnityEngine.Vector3(
                        pyramidPosition.x + (x - offset) * cubeSize,
                        yPosition,
                        pyramidPosition.z + (z - offset) * cubeSize
                    );

                    Renderer rend = cube.GetComponent<Renderer>();
                    rend.material = new Material(Shader.Find("Universal Render Pipeline/Simple Lit"));
                    rend.material.color = levelColor;
                }
            }
        }
    }

    void CreateForest()
    {
        GameObject forestParent = new GameObject("Forest");

        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * forestSpread;

            float treeX = forestCenter.x + randomCircle.x;
            float treeZ = forestCenter.z + randomCircle.y;

            float treeHeight = Random.Range(2f, 10f);
            float treeRadius = Random.Range(0.5f, 1f);

            GameObject tree = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tree.name = "Tree_" + i;
            tree.transform.parent = forestParent.transform;

            tree.transform.position = new Vector3(treeX, treeHeight / 2f, treeZ);

            tree.transform.localScale = new UnityEngine.Vector3(treeRadius * 2f, treeHeight / 2f, treeRadius * 2f);

            float greenShade = Random.Range(0.2f,0.8f);
            Renderer treeRend = tree.GetComponent<Renderer>();
            treeRend.material = new Material(Shader.Find("Universal Render Pipeline/Simple Lit"));
            treeRend.material.color = new Color(0.0f, greenShade, 0.0f);
        }
    }
}
