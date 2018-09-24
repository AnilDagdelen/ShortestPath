using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour {
    static GameObject Character = GameObject.Find("Character");
    static GameObject NodeAndDurationText = GameObject.Find("NodeAndDurationText");
    static Vector3[] v = Character.GetComponentsInChildren<MeshFilter>().SelectMany(mf => mf.mesh.vertices).ToArray();
    static Edge[] Edges = EdgeProcessor.BuildManifoldEdges(Character.GetComponentsInChildren<MeshFilter>().First().mesh);

    public static void find(GameObject P1,GameObject P2)
    {
        ClearPaths();
        var watch = System.Diagnostics.Stopwatch.StartNew();
        List<int> path = DynamicProgrammingPathFinder.FindPath(P1.transform.position, P2.transform.position, Edges.ToList(),v.ToList());
        watch.Stop();
        EdgeProcessor.DrawLine(v[path[0]], P1.transform.localPosition, Color.red);//draw line from first vertice to starting point
        EdgeProcessor.DrawLine(v[path[path.Count-1]], P2.transform.localPosition, Color.red);//draw line from last vertice to target point
        for (int i = 0; i < path.Count-1; i++)
            EdgeProcessor.DrawLine(v[path[i]], v[path[i + 1]], Color.red);
        var ElapsedMs = watch.ElapsedMilliseconds;
        NodeAndDurationText.GetComponent<TextMesh>().text = "STATS\nNode Count : " + (path.Count-1) + "\nCalc Duration : " + ElapsedMs+" ms";
    }

    private static void ClearPaths()
    {
        GameObject[] paths;

        paths = GameObject.FindGameObjectsWithTag("Line");

        foreach (GameObject path in paths)
        {
            Destroy(path);
        }
    }
}