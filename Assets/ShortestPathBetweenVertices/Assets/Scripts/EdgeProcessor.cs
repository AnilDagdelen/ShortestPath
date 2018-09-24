using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EdgeProcessor  {
    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.sortingOrder = 1;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.material.color = Color.red;
        lr.transform.tag = "Line";
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
    public static Edge[] BuildManifoldEdges(Mesh mesh)
    {
        Edge[] edges = BuildEdges(mesh.vertexCount, mesh.triangles);

        ArrayList culledEdges = new ArrayList();
        foreach (Edge edge in edges)
        {
            if (edge.faceIndex[0] == edge.faceIndex[1])
            {
                culledEdges.Add(edge);
            }
        }

        return culledEdges.ToArray(typeof(Edge)) as Edge[];
    }
    public static Edge[] BuildEdges(int vertexCount, int[] triangleArray)
    {
        int maxEdgeCount = triangleArray.Length;
        int[] firstEdge = new int[vertexCount + maxEdgeCount];
        int nextEdge = vertexCount;
        int triangleCount = triangleArray.Length / 3;

        for (int a = 0; a < vertexCount; a++)
            firstEdge[a] = -1;
        Edge[] edgeArray = new Edge[maxEdgeCount];

        int edgeCount = 0;
        for (int a = 0; a < triangleCount; a++)
        {
            int i1 = triangleArray[a * 3 + 2];
            for (int b = 0; b < 3; b++)
            {
                int i2 = triangleArray[a * 3 + b];
                if (i1 < i2)
                {
                    Edge newEdge = new Edge();
                    newEdge.vertexIndex[0] = i1;
                    newEdge.vertexIndex[1] = i2;
                    newEdge.faceIndex[0] = a;
                    newEdge.faceIndex[1] = a;
                    edgeArray[edgeCount] = newEdge;

                    int edgeIndex = firstEdge[i1];
                    if (edgeIndex == -1)
                    {
                        firstEdge[i1] = edgeCount;
                    }
                    else
                    {
                        while (true)
                        {
                            int index = firstEdge[nextEdge + edgeIndex];
                            if (index == -1)
                            {
                                firstEdge[nextEdge + edgeIndex] = edgeCount;
                                break;
                            }

                            edgeIndex = index;
                        }
                    }

                    firstEdge[nextEdge + edgeCount] = -1;
                    edgeCount++;
                }

                i1 = i2;
            }
        }

        for (int a = 0; a < triangleCount; a++)
        {
            int i1 = triangleArray[a * 3 + 2];
            for (int b = 0; b < 3; b++)
            {
                int i2 = triangleArray[a * 3 + b];
                if (i1 > i2)
                {
                    bool foundEdge = false;
                    for (int edgeIndex = firstEdge[i2]; edgeIndex != -1; edgeIndex = firstEdge[nextEdge + edgeIndex])
                    {
                        Edge edge = edgeArray[edgeIndex];
                        if ((edge.vertexIndex[1] == i1) && (edge.faceIndex[0] == edge.faceIndex[1]))
                        {
                            edgeArray[edgeIndex].faceIndex[1] = a;
                            foundEdge = true;
                            break;
                        }
                    }

                    if (foundEdge)
                    {
                        Edge newEdge = new Edge();
                        newEdge.vertexIndex[0] = i1;
                        newEdge.vertexIndex[1] = i2;
                        newEdge.faceIndex[0] = a;
                        newEdge.faceIndex[1] = a;
                        edgeArray[edgeCount] = newEdge;
                        edgeCount++;
                    }
                }

                i1 = i2;
            }
        }

        Edge[] compactedEdges = new Edge[edgeCount];
        for (int e = 0; e < edgeCount; e++)
            compactedEdges[e] = edgeArray[e];

        return compactedEdges;
    }

}
