using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Lauren Holloway

public class CreateScene : MonoBehaviour
{
    // private controls

    //ground position
    private Vector3 groundPosition = new Vector3(0, 0, 0);
    private float groundSizeX = 80f;
    private float groundSizeZ = 80f;

    // pyramid position and scale
    private Vector3 pyramidPosition = new Vector3(15,0,0);
    private float cubeSize = 2f;

    // forest position
    private Vector3 forestCenter = new Vector3(-15,0,0);

    // celestial object position and radius
    private Vector3 celestialOrbitCenter = new Vector3(0, -10, 10);
    private float celestialOrbitRadius = 50f;


    // inspector controls

    [Header("Pyramid Settings")]
    [Range(3,15)]
    public int pyramidBaseSize = 5;

    [Header("Forest Settings")]
    public int numberOfTrees = 20;
    public float forestSpread = 15f;

    [Header("Celestial Object Settings")]
    public float celestialOrbitSpeed = 10f;

    private GameObject celestialObject;
    private Light celestialLight;
    private float orbitAngle = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateGround();
        CreatePyramid();
        CreateForest();
        CreateCelestialObject();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCelestialObject();
    }

    void CreateGround()
    {
        // creates ground object and names it in hierarchy
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";

        // easily adjustable size and position
        ground.transform.position = groundPosition;
        ground.transform.localScale = new Vector3(groundSizeX, 0.1f, groundSizeZ);

        // ground color
        Renderer rend = ground.GetComponent<Renderer>();
        rend.material = new Material(Shader.Find("Universal Render Pipeline/Simple Lit"));
        rend.material.color = new Color(0.91f, 0.47f, 0.44f);
    }

    void CreatePyramid()
    {
        // sets min and max pyramid sizes
        pyramidBaseSize = Mathf.Clamp(pyramidBaseSize, 3, 15);

        int totalLevels = pyramidBaseSize;

        // creates parent object to organize hierarchy and keep pyramid grouped together
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
                    // creates, names, and parents each cube primitive
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "PyramidCube_L" + level + "_" + x + "_" + z;
                    cube.transform.parent = pyramidParent.transform;

                    // scales the cubes based on cubeSize value
                    cube.transform.localScale = Vector3.one * cubeSize;

                    // sets the cubes position, centers the row, and multiplys by cubeSize to prevent overlap
                    cube.transform.position = new Vector3(
                        pyramidPosition.x + (x - offset) * cubeSize,
                        yPosition,
                        pyramidPosition.z + (z - offset) * cubeSize
                    );

                    // sets color
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
            // picks random point inside a circle then scales by forestSpead, controls the spread of the trees
            Vector2 randomCircle = Random.insideUnitCircle * forestSpread;

            float treeX = forestCenter.x + randomCircle.x;
            float treeZ = forestCenter.z + randomCircle.y;

            // randomize tree height and radius
            float treeHeight = Random.Range(2f, 10f);
            float treeRadius = Random.Range(0.5f, 1f);

            // creates, names, and parents tree primitives
            GameObject tree = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tree.name = "Tree_" + i;
            tree.transform.parent = forestParent.transform;

            // the pivot point of the cylinders is at the center, so it's moves up half it's height so it's on the ground
            tree.transform.position = new Vector3(treeX, treeHeight / 2f, treeZ);
            // x and z control diameter, y controls height
            tree.transform.localScale = new Vector3(treeRadius * 2f, treeHeight / 2f, treeRadius * 2f);

            // gives the trees a random shade of green
            float greenShade = Random.Range(0.2f,0.8f);
            Renderer treeRend = tree.GetComponent<Renderer>();
            treeRend.material = new Material(Shader.Find("Universal Render Pipeline/Simple Lit"));
            treeRend.material.color = new Color(0.0f, greenShade, 0.0f);
        }
    }

    void CreateCelestialObject()
    {
        celestialObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        celestialObject.name = "CelestialObject";

        celestialObject.transform.localScale = new Vector3(5f, 5f, 5f);

        Renderer rend = celestialObject.GetComponent<Renderer>();
        // unlit shader ignores scene lighting
        rend.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        rend.material.SetColor("_BaseColor", Color.yellow);

        // point light attached to sphere
        celestialLight = celestialObject.AddComponent<Light>();
        celestialLight.type = LightType.Point;
        celestialLight.range = 150f;
        celestialLight.intensity = 3f;
        celestialLight.color = Color.white;

        celestialObject.transform.position = celestialOrbitCenter;

        orbitAngle = 0f;
    }

    void MoveCelestialObject()
    {
        if (celestialObject == null) return;

        // changes orbit angle each frame based on speed
        orbitAngle += celestialOrbitSpeed * Time.deltaTime;

        // converts degrees to radians and uses sin and cos to move in a circle
        float rad = orbitAngle * Mathf.Deg2Rad;
        float x = celestialOrbitCenter.x + Mathf.Cos(rad) * celestialOrbitRadius;
        float y = celestialOrbitCenter.y + Mathf.Sin(rad) * celestialOrbitRadius;

        celestialObject.transform.position = new Vector3(x, y, celestialOrbitCenter.z);

        // dayFactor is 0 at the bottom of the orbit and 1 at the top
        float dayFactor = Mathf.Clamp01(Mathf.Sin(rad));

        // blends the sphere color between night and day
        Color currentColor = Color.Lerp(Color.blue, Color.yellow, dayFactor);
        celestialLight.color = currentColor;
        celestialObject.GetComponent<Renderer>().material.SetColor("_BaseColor", currentColor);

        // makes light less intense at night
        // celestialLight.intensity = Mathf.Lerp(0.2f, 3f, dayFactor);
    }

}
