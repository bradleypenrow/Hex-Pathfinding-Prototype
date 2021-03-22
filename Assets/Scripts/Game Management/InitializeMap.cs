using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Diagnostics;

//Uncomment below to generate the map while in Edit mode
//[ExecuteInEditMode]

/// <summary>
/// Class that controls the initial creation of the map
/// </summary>
public class InitializeMap : MonoBehaviour
{
    
    public Terrain[] possibleTerrain;
    public Resource[] possibleResource;
    public int rows;
    public int cols;
    public GameObject hexPrefab;
    public int shoreRiverThreshold = 10;
    public int numRivers;
    public GameObject[,] hexes;
    public List<Province> allHexes = new List<Province>();
    public List<Province> mountainHexes = new List<Province>();
    public List<Province> shore = new List<Province>();
    public List<Army> armies = new List<Army>();

    private List<Terrain> weightedTerrain = new List<Terrain>();

    // Start is called before the first frame update
    [ContextMenu("Generate Hex Map")]
    void Start()
    {
        hexes = new GameObject[cols, rows];
        DestroyHexes();
        GenerateWeightedTerrain();
        SpawnHexes();
        AssignNeighbors();
        //   BuildRivers();
        AssignTestUnitsToProvinces();

        //Change the size of the hexes
        transform.localScale = new Vector3(8, 8, 8);
    }

    private void AssignTestUnitsToProvinces()
    {
        armies[0].currentProvince = allHexes[0];
        armies[1].currentProvince = allHexes[3];

    }

    private void AssignNeighbors()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        foreach (Province p in allHexes)
        {
            foreach (Province n in allHexes)
            {
                //Direct Left
                if (n.cubicCoord.x == p.cubicCoord.x - 1 && n.cubicCoord.y == p.cubicCoord.y + 1 && n.cubicCoord.z == p.cubicCoord.z)
                    p.neighbors[0] = n;
                //Bottom Left
                if (n.cubicCoord.x == p.cubicCoord.x && n.cubicCoord.y == p.cubicCoord.y + 1 && n.cubicCoord.z == p.cubicCoord.z - 1)
                    p.neighbors[1] = n;
                //Bottom Right
                if (n.cubicCoord.x == p.cubicCoord.x + 1 && n.cubicCoord.y == p.cubicCoord.y && n.cubicCoord.z == p.cubicCoord.z - 1)
                    p.neighbors[2] = n;
                //Direct Right
                if (n.cubicCoord.x == p.cubicCoord.x + 1 && n.cubicCoord.y == p.cubicCoord.y - 1 && n.cubicCoord.z == p.cubicCoord.z)
                    p.neighbors[3] = n;
                //Top Right
                if (n.cubicCoord.x == p.cubicCoord.x && n.cubicCoord.y == p.cubicCoord.y - 1 && n.cubicCoord.z == p.cubicCoord.z + 1)
                    p.neighbors[4] = n;
                //Top Left
                if (n.cubicCoord.x == p.cubicCoord.x - 1 && n.cubicCoord.y == p.cubicCoord.y && n.cubicCoord.z == p.cubicCoord.z + 1)
                    p.neighbors[5] = n;
            }
        }

        sw.Stop();
        UnityEngine.Debug.Log("Time to assign neighbors " + sw.ElapsedMilliseconds + " ms");
    }

    //Waited list to choose terrain based on the spawnchance of each terrain type
    private void GenerateWeightedTerrain()
    {
        foreach (Terrain t in possibleTerrain)
        {
            for (int i = 0; i < t.spawnChance; i++)
            {
                weightedTerrain.Add(t);
            }
        }
    }

    private void SpawnHexes()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        float curX = 0;
        float curZ = 0;

        for (int z = 0; z < rows; z++)
        {
            if (z % 2 > 0 && z > 0)
                curX += .5f; //Offset next row up to match hexagons correctly


            for (int x = 0; x < cols; x++)
            {
                GameObject hex = Instantiate(hexPrefab, new Vector3(curX, 0, curZ), Quaternion.identity, this.transform);

                hexes[x, z] = hex;

                Province hexProvince = hex.GetComponent<Province>();

                allHexes.Add(hexProvince);


                hexProvince.resource = possibleResource[UnityEngine.Random.Range(0, possibleResource.Length)];
                hexProvince.terrain = weightedTerrain[UnityEngine.Random.Range(0, weightedTerrain.Count)];

                //Ensure 2 test armies start on Plains. If they start on Mountains, the logic breaks
                if (z == 0 && (x == 0 || x == 3))
                {
                    hexProvince.terrain = possibleTerrain[1];
                    UnityEngine.Debug.Log(hexProvince.terrain);
                }

                if (hexProvince.terrain.name == "Mountains")
                {
                    mountainHexes.Add(hex.GetComponent<Province>());
                    hex.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
                }
                if (hexProvince.terrain.name != "Mountains" && (x == 0 || x == cols - 1 || z == 0 || z == rows - 1))
                    shore.Add(hexProvince);

                hexProvince.coord = new Vector2(x,z);
                int cubeX = x - (z - (z & 1)) / 2;
                int cubeZ = z;
                int cubeY = -cubeX - cubeZ;
                hexProvince.cubicCoord = new Vector3(cubeX,cubeY,cubeZ);
                hex.GetComponentInChildren<TextMeshProUGUI>().text = hex.GetComponent<Province>().terrain.name + "\n Cost: " + hexProvince.terrain.travelTime;
               
                curX += 1f;
            }
            curZ += .867f;
            curX = 0;
        }
        sw.Stop();
        UnityEngine.Debug.Log("Time to spawn Hexes " + sw.ElapsedMilliseconds + " ms");
    }

    //Unused future enhancement
    private void BuildRivers()
    {
        //Choose River Mouths
        for (int i = 0; i < numRivers; i++)
        {
            Province p = mountainHexes[UnityEngine.Random.Range(0, mountainHexes.Count)];
            if (p.riverMouth == false)
            {
                p.gameObject.GetComponentInChildren<TextMeshProUGUI>().text += " River Start";
                p.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.blue;
                p.riverMouth = true;
            }
            else
            {
                if (numRivers < mountainHexes.Count)
                    i--;
            }
        }             
    }

    [ContextMenu("Delete Hex Map")]
    private void DestroyHexes()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        foreach (Province o in allHexes)
        {
            GameObject.DestroyImmediate(o.gameObject);
        }

        mountainHexes.Clear();
        shore.Clear();
        weightedTerrain.Clear();
        allHexes.Clear();
        transform.localScale = new Vector3(1, 1, 1);

        sw.Stop();
        UnityEngine.Debug.Log("Time to destroy Hexes " + sw.ElapsedMilliseconds + " ms");
    }
    
}
