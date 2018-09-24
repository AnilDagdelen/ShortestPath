using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicProgrammingPathFinder : MonoBehaviour
{

    public static List<int> FindPath(Vector3 Starting, Vector3 Target, List<Edge> Edges, List<Vector3> vertices)
    {
        float min = int.MaxValue;
        int startpoint = 0;
        for (int i = 0; i < vertices.Count; i++)
        {
            if ((Vector3.Distance(Starting, vertices[i]) < min))
            {
                startpoint = i;
                min = Vector3.Distance(Starting, vertices[i]);
            }
        }
        min = int.MaxValue;
        int targetpoint = 0;
        for (int i = 0; i < vertices.Count; i++)
        {
            if ((Vector3.Distance(Target, vertices[i]) < min))
            {
                targetpoint = i;
                min = Vector3.Distance(Target, vertices[i]);
            }
        }

        List<List<int>> Possibilities = new List<List<int>>();
        Possibilities.Add(new List<int>() { startpoint }); 
        while (true)
        { 
                for (int i1 = 0; i1 < Edges.Count; i1++)
                {
                    if (Edges[i1].vertexIndex[0] == Possibilities[0][Possibilities[0].Count - 1])
                    {
                        List<int> possibility = new List<int>();
                        possibility.AddRange(Possibilities[0]);
                        possibility.Add(Edges[i1].vertexIndex[1]);
                        Possibilities.Add(possibility);
                        Edges.RemoveAt(i1);
                        i1--;
                    }
                    else if (Edges[i1].vertexIndex[1] == Possibilities[0][Possibilities[0].Count - 1])
                    {
                        List<int> possibility = new List<int>();
                        possibility.AddRange(Possibilities[0]);
                        possibility.Add(Edges[i1].vertexIndex[0]);
                        Possibilities.Add(possibility);
                        Edges.RemoveAt(i1);
                        i1--;
                    }
                }
                for(int i=0; i<Possibilities.Count;i++)
                if (Possibilities[i][Possibilities[i].Count - 1] == targetpoint) 
                    return Possibilities[i]; 
                Possibilities.RemoveAt(0);  
            
        }
    } 
}
