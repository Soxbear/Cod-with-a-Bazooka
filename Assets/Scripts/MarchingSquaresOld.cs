using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MarchingSquaresOld
{    
    public Mesh GenerateMesh(float[,] voxels) { // 10 - 11 test ms
        Mesh mesh = new Mesh();

        List<Vector3> v = new List<Vector3>();
        List<int> t = new List<int>();

        int w = voxels.GetLength(0);
        int h = voxels.GetLength(1);

        int i = 0;

        for (int x = 1; x < w; x++) {
            for (int y = 1; y < h; y++) {

                v.Add(new Vector2(x - 0, h -y + 0 - 1));
                v.Add(new Vector2(x - 1, h -y + 0 - 1));
                v.Add(new Vector2(x - 0, h -y + 1 - 1));
                v.Add(new Vector2(x - 1, h -y + 1 - 1));
                v.Add(new Vector2(x - lint(voxels[y, x], voxels[y, x - 1]), h -y + 0 - 1));
                v.Add(new Vector2(x - 1, h -y + lint(voxels[y, x - 1], voxels[y - 1, x - 1]) - 1));
                v.Add(new Vector2(x - 0, h -y + lint(voxels[y, x], voxels[y - 1, x]) - 1));
                v.Add(new Vector2(x - lint(voxels[y - 1, x], voxels[y - 1, x - 1]), h -y + 1 - 1));

                t.AddRange(offsetTriangles(squareTriangles[((voxels[y, x] > 0) ? 1 : 0) + ((voxels[y, x - 1] > 0) ? 2 : 0) + ((voxels[y - 1, x] > 0) ? 4 : 0) + ((voxels[y - 1, x - 1] > 0) ? 8 : 0)], i));
                i += 8;
            }
        }

        mesh.vertices = v.ToArray();
        mesh.triangles = t.ToArray();

        return mesh;
    }

    public Mesh GenerateMeshMin(float[,] voxels) { // 6 - 7 test ms
        Mesh mesh = new Mesh();

        List<Vector3> v = new List<Vector3>();
        List<int> t = new List<int>();

        int w = voxels.GetLength(0);
        int h = voxels.GetLength(1);

        int i = 0;

        for (int x = 1; x < w; x++) {
            for (int y = 1; y < h; y++) {
                int sqNum = ((voxels[y, x] > 0) ? 1 : 0) + ((voxels[y, x - 1] > 0) ? 2 : 0) + ((voxels[y - 1, x] > 0) ? 4 : 0) + ((voxels[y - 1, x - 1] > 0) ? 8 : 0);

                if (squareVerticies[sqNum][0]) v.Add(new Vector2(x - 0, h -y + 0 - 1));
                if (squareVerticies[sqNum][1]) v.Add(new Vector2(x - 1, h -y + 0 - 1));
                if (squareVerticies[sqNum][2]) v.Add(new Vector2(x - 0, h -y + 1 - 1));
                if (squareVerticies[sqNum][3]) v.Add(new Vector2(x - 1, h -y + 1 - 1));
                if (squareVerticies[sqNum][4]) v.Add(new Vector2(x - lint(voxels[y, x], voxels[y, x - 1]), h -y + 0 - 1));
                if (squareVerticies[sqNum][5]) v.Add(new Vector2(x - 1, h -y + lint(voxels[y, x - 1], voxels[y - 1, x - 1]) - 1));
                if (squareVerticies[sqNum][6]) v.Add(new Vector2(x - 0, h -y + lint(voxels[y, x], voxels[y - 1, x]) - 1));
                if (squareVerticies[sqNum][7]) v.Add(new Vector2(x - lint(voxels[y - 1, x], voxels[y - 1, x - 1]), h -y + 1 - 1));

                t.AddRange(offsetTriangles(squareMinTriangles[sqNum], i));
                i += squareVertexCount[sqNum];
            }
        }

        mesh.vertices = v.ToArray();
        mesh.triangles = t.ToArray();

        return mesh;
    }

    public Mesh GenerateMeshMinMin(float[,] voxels) { // 6 - 8 test ms
        Mesh mesh = new Mesh();

        List<Vector3> v = new List<Vector3>();
        List<int> t = new List<int>();

        int w = voxels.GetLength(0);
        int h = voxels.GetLength(1);

        int[,][] i = new int[h, w][];

        for (int y = 1; y < h; y++) {
            for (int x = 1; x < w; x++) {
                // Debug.Log("x: " + x + ", y: " + y);

                int sqNum = ((voxels[y, x] > 0) ? 1 : 0) + ((voxels[y, x - 1] > 0) ? 2 : 0) + ((voxels[y - 1, x] > 0) ? 4 : 0) + ((voxels[y - 1, x - 1] > 0) ? 8 : 0);
                
                i[y, x] = new int[] {-1, -1, -1, -1, -1, -1, -1, -1, v.Count, sqNum};

                int[] verts = new int[8];

                if (squareVerticies[sqNum][0]) 
                    verts[0] = v.AddReturnIndex<Vector3>(new Vector2(x - 0, h -y + 0 - 1));
                
                if (squareVerticies[sqNum][1]) 
                    verts[1] = (i[y, x - 1] != null) ? i[y, x - 1][0] : v.AddReturnIndex<Vector3>(new Vector2(x - 1, h -y + 0 - 1));
                
                if (squareVerticies[sqNum][2]) 
                    verts[2] = (i[y - 1, x] != null) ? i[y - 1, x][0] : v.AddReturnIndex<Vector3>(new Vector2(x - 0, h -y + 1 - 1));
                
                if (squareVerticies[sqNum][3]) 
                    verts[3] = (i[y - 1, x - 1] != null) ? i[y - 1, x - 1][0] : v.AddReturnIndex<Vector3>(new Vector2(x - 1, h -y + 1 - 1));



                if (squareVerticies[sqNum][4]) 
                    verts[4] = v.AddReturnIndex<Vector3>(new Vector2(x - lint(voxels[y, x], voxels[y, x - 1]), h -y + 0 - 1));                    
                
                if (squareVerticies[sqNum][5]) 
                    verts[5] = (i[y, x - 1] != null) ? i[y, x - 1][6] : v.AddReturnIndex<Vector3>(new Vector2(x - 1, h -y + lint(voxels[y, x - 1], voxels[y - 1, x - 1]) - 1));
                
                if (squareVerticies[sqNum][6]) 
                    verts[6] = v.AddReturnIndex<Vector3>(new Vector2(x - 0, h -y + lint(voxels[y, x], voxels[y - 1, x]) - 1));
                
                if (squareVerticies[sqNum][7]) 
                    verts[7] = (i[y - 1, x] != null) ? i[y - 1, x][4] : v.AddReturnIndex<Vector3>(new Vector2(x - lint(voxels[y - 1, x], voxels[y - 1, x - 1]), h -y + 1 - 1));

                for (int o = 0; o < squareTriangles[sqNum].Length; o++) {
                    t.Add(verts[squareTriangles[sqNum][o]]);
                    i[y, x][squareTriangles[sqNum][o]] = verts[squareTriangles[sqNum][o]];
                }
            }
        }

        mesh.vertices = v.ToArray();
        mesh.triangles = t.ToArray();

        return mesh;
    }

    public Mesh GenerateMeshMinMinMin(float[,] voxels) { // 7 - 9 test ms
        Mesh mesh = new Mesh();

        List<Vector3> v = new List<Vector3>();
        List<int> t = new List<int>();

        int w = voxels.GetLength(0);
        int h = voxels.GetLength(1);

        int[,][] i = new int[h, w][];

        for (int y = 1; y < h; y++) {
            for (int x = 1; x < w; x++) {
                int sqNum = ((voxels[y, x] > 0) ? 1 : 0) + ((voxels[y, x - 1] > 0) ? 2 : 0) + ((voxels[y - 1, x] > 0) ? 4 : 0) + ((voxels[y - 1, x - 1] > 0) ? 8 : 0);

                if (sqNum == 15)
                    continue;
                
                i[y, x] = new int[] {-1, -1, -1, -1, -1, -1, -1, -1, v.Count, sqNum};

                int[] verts = new int[8];

                if (squareVerticies[sqNum][0]) 
                    verts[0] = v.AddReturnIndex<Vector3>(new Vector2(x - 0, h -y + 0 - 1));
                
                if (squareVerticies[sqNum][1]) 
                    verts[1] = (i[y, x - 1] != null) ? i[y, x - 1][0] : v.AddReturnIndex<Vector3>(new Vector2(x - 1, h -y + 0 - 1));
                
                if (squareVerticies[sqNum][2]) 
                    verts[2] = (i[y - 1, x] != null) ? i[y - 1, x][0] : v.AddReturnIndex<Vector3>(new Vector2(x - 0, h -y + 1 - 1));
                
                if (squareVerticies[sqNum][3]) 
                    verts[3] = (i[y - 1, x - 1] != null) ? i[y - 1, x - 1][0] : v.AddReturnIndex<Vector3>(new Vector2(x - 1, h -y + 1 - 1));



                if (squareVerticies[sqNum][4]) 
                    verts[4] = v.AddReturnIndex<Vector3>(new Vector2(x - lint(voxels[y, x], voxels[y, x - 1]), h -y + 0 - 1));                    
                
                if (squareVerticies[sqNum][5]) 
                    verts[5] = (i[y, x - 1] != null) ? i[y, x - 1][6] : v.AddReturnIndex<Vector3>(new Vector2(x - 1, h -y + lint(voxels[y, x - 1], voxels[y - 1, x - 1]) - 1));
                
                if (squareVerticies[sqNum][6]) 
                    verts[6] = v.AddReturnIndex<Vector3>(new Vector2(x - 0, h -y + lint(voxels[y, x], voxels[y - 1, x]) - 1));
                
                if (squareVerticies[sqNum][7]) 
                    verts[7] = (i[y - 1, x] != null) ? i[y - 1, x][4] : v.AddReturnIndex<Vector3>(new Vector2(x - lint(voxels[y - 1, x], voxels[y - 1, x - 1]), h -y + 1 - 1));

                for (int o = 0; o < squareTriangles[sqNum].Length; o++) {
                    t.Add(verts[squareTriangles[sqNum][o]]);
                    i[y, x][squareTriangles[sqNum][o]] = verts[squareTriangles[sqNum][o]];
                }
            }
        }

        bool[,] gen = new bool[h, w];

        for (int y = 1; y < h; y++) {
            for (int x = 1; x < w; x++) {
                if (i[y, x] != null || gen[y, x])
                    continue;
                
                List<int> vertical = new List<int>();
                
                Vector2Int size = new Vector2Int();

                for (int xx = x + 1; xx < w; xx++) {
                    if (i[y, xx] != null || gen[y, xx]) break;
                    size.x++;
                }

                for (int yy = y + 1; yy < h; yy++) {
                    bool b = false;

                    for (int xx = x; xx <= x + size.x; xx++) {
                        if (i[yy, xx] != null || gen[yy, xx]) {
                            b = true;
                            break;
                        }
                    }

                    if (b) break;

                    size.y++;
                }

                for (int yy = 0; yy < size.y + 1; yy++) {
                    for (int xx = 0; xx < size.x + 1; xx++) {
                        gen[y + yy, x + xx] = true;
                    }
                }

                int[] verts = new int[4];

                verts[0] = (y + size.y < h && x + size.x < w && i[y + size.y, x + size.x] != null) ? i[y + size.y, x + size.x][3] : v.AddReturnIndex<Vector3>(new Vector2(x + size.x, h - (y + size.y) - 1));

                verts[1] = (y + size.y < h && i[y + size.y, x - 1] != null) ? i[y + size.y, x - 1][0] : v.AddReturnIndex<Vector3>(new Vector2(x - 1, h - (y + size.y) - 1));

                verts[2] = (x + size.x < w && i[y - 1, x + size.x] != null) ? i[y - 1, x + size.x][0] : v.AddReturnIndex<Vector3>(new Vector2(x + size.x, h - (y - 1) - 1));

                verts[3] = (i[y - 1, x - 1] != null) ? i[y - 1, x - 1][0] : v.AddReturnIndex<Vector3>(new Vector2(x - 1, h - (y - 1) - 1));
                
                

                t.AddRange(new int[] {verts[0], verts[1], verts[3], verts[0], verts[3], verts[2]});
            }
        }

        mesh.vertices = v.ToArray();
        mesh.triangles = t.ToArray();

        return mesh;
    }

    public static int[] offsetTriangles(int[] triangles, int offset) {
        int[] ot = new int[triangles.Length];
        for (int i = 0; i < ot.Length; i++) {
            ot[i] = triangles[i] + offset;
        }
        return ot;
    }

    public static float lint(float a, float b) { //Doing the a + b = 0 to test if both are zero,because then an error would be thrown
        return (a + b == 0) ? 0.5f : Mathf.Abs(a) / (Mathf.Abs(a) + Mathf.Abs(b));
    }

    public readonly static bool[][] squareVerticies = {
        new[] {false, false, false, false, false, false, false, false}, //
        new[] {true, false, false, false, true, false, true, false}, //
        new[] {false, true, false, false, true, true, false, false}, //
        new[] {true, true, false, false, false, true, true, false}, //

        new[] {false, false, true, false, false, false, true, true}, //
        new[] {true, false, true, false, true, false, false, true}, //
        new[] {false, true, true, false, true, true, true, true}, //
        new[] {true, true, true, false, false, true, false, true}, //

        new[] {false, false, false, true, false, true, false, true}, //
        new[] {true, false, false, true, true, true, true, true}, //
        new[] {false, true, false, true, true, false, false, true}, //
        new[] {true, true, false, true, false, false, true, true}, 

        new[] {false, false, true, true, false, true, true, false}, //
        new[] {true, false, true, true, true, true, false, false}, //
        new[] {false, true, true, true, true, false, true, false}, //
        new[] {true, true, true, true, false, false, false, false} //
    };

    public readonly static int[] squareVertexCount = {
        0,
        3,
        3,
        4,

        3,
        4,
        6,
        5,

        3,
        6,
        4,
        5,

        4,
        5,
        5,
        4
    };

    public readonly static int[][] squareMinTriangles = {
        new int[] {}, //
        new[] {0, 1, 2}, //
        new[] {1, 0, 2}, //
        new[] {0, 1, 2, 0, 2, 3}, //

        new[] {2, 0, 1}, //
        new[] {0, 2, 3, 0, 3, 1}, //
        new[] {2, 0, 3, 2, 3, 5, 2, 5, 4, 5, 1, 4}, //
        new[] {0, 1, 3, 0, 3, 4, 0, 4, 2}, //

        new[] {1, 0, 2}, //
        new[] {0, 2, 4, 2, 3, 4, 3, 5, 4, 3, 1, 5}, //
        new[] {2, 0, 1, 2, 1, 3}, //
        new[] {0, 1, 3, 1, 4, 3, 1, 2, 4}, //

        new[] {2, 1, 0, 2, 0, 3}, //
        new[] {0, 3, 1, 3, 4, 1, 4, 2, 1}, //
        new[] {3, 0, 2, 3, 2, 4, 2, 1, 4}, //
        new[] {0, 1, 3, 0, 3, 2} //
    };

    // public readonly static int[][] squareMinToMaxTriangles = {
    //     new int[] {},
    //     new int[] {0, 4, 6},
    //     new int[] {1, 4, 5},
    //     new int[] {0, 1, 5, 6},

    //     new int[] {2, 6, 7},
    //     new int[] {0, 2, 4, 7},
    //     new int[] {1, 2, 4, 5, 6, 7},
    //     new int[] {0, 1, 2, 5, 7},
        
    //     new int[] {3, 5, 7},
    //     new int[] {0, 3, 4, 5, 6, 7},
    //     new int[] {1, 3, 4, 7},
    //     new int[] {0, 1, 3, 6, 7},
        
    //     new int[] {2, 3, 5, 6},
    //     new int[] {0, 2, 3, 4, 5},
    //     new int[] {1, 2, 3, 4, 6},
    //     new int[] {0, 1, 2, 3}
    // };

    // public readonly static int[][] squareMaxToMinTriangles = {
    //     new int[] {-1, -1, -1, -1, -1, -1, -1, -1},
    //     new int[] {0, -1, -1, -1, 1, -1, 2, -1},
    //     new int[] {-1, 0, -1, -1, 1, 2, -1, -1},
    //     new int[] {0, 1, -1, -1, -1, 2, 3, -1},
        
    //     new int[] {-1, -1, 0, -1, -1, -1, 1, 2},
    //     new int[] {0, -1, 1, -1, 2, -1, -1, 3},
    //     new int[] {-1, 0, 1, -1, 2, 3, 4, 5},
    //     new int[] {0, 1, 2, -1, -1, 3, -1, 4},
        
    //     new int[] {-1, -1, -1, 0, -1, 1, -1, 2},
    //     new int[] {0, -1, -1, 1, 2, 3, 4, 5},
    //     new int[] {-1, 0, -1, 1, 2, -1, -1, 3},
    //     new int[] {0, 1, -1, 2, -1, -1, 3, 4},
        
    //     new int[] {-1, -1, 0, 1, -1, 2, 3, -1},
    //     new int[] {0, -1, 1, 2, 3, 4, -1, -1},
    //     new int[] {-1, 0, 1, 2, 3, -1, 4, -1},
    //     new int[] {0, 1, 2, 3, -1, -1, -1, -1}
    // };

    public readonly static int[][] squareTriangles = {
        new int[] {}, //
        new[] {0, 4, 6}, //
        new[] {4, 1, 5}, //
        new[] {0, 1, 5, 0, 5, 6}, //

        new[] {7, 2, 6}, //
        new[] {0, 4, 7, 0, 7, 2}, //
        new[] {4, 1, 5, 4, 5, 7, 4, 7, 6, 7, 2, 6}, //
        new[] {0, 1, 5, 0, 5, 7, 0, 7, 2}, //

        new[] {5, 3, 7}, //
        new[] {0, 4, 6, 4, 5, 6, 5, 7, 6, 5, 3, 7}, //
        new[] {4, 1, 3, 4, 3, 7}, //
        new[] {0, 1, 6, 1, 7, 6, 1, 3, 7}, //

        new[] {5, 3, 2, 5, 2, 6}, //
        new[] {0, 4, 2, 4, 5, 2, 5, 3, 2}, //
        new[] {4, 1, 3, 4, 3, 6, 3, 2, 6}, //
        new[] {0, 1, 3, 0, 3, 2} //
    };

    public readonly static int[][] squareEdges = {
        new int[] {},
        new int[] {4, 6},
        new int[] {5, 4},
        new int[] {5, 6},

        new int[] {6, 7},
        new int[] {4, 7},
        new int[] {5, 7, 6, 4},
        new int[] {5, 7},

        new int[] {7, 5}, 
        new int[] {4, 5, 7, 6},
        new int[] {7, 4},
        new int[] {7, 6},

        new int[] {6, 5},
        new int[] {4, 5},
        new int[] {6, 4},
        new int[] {}
    };

    public readonly static int[,][] squareFullEdges = {
        {new int[] {}, new int[] {}, new int[] {}, new int[] {}},
        {new int[] {0, 4}, new int[] {}, new int[] {6, 0}, new int[] {}},
        {new int[] {4, 1}, new int[] {1, 5}, new int[] {}, new int[] {}},
        {new int[] {0, 1}, new int[] {1, 5}, new int[] {6, 0}, new int[] {}},
        
        {new int[] {}, new int[] {}, new int[] {2, 6}, new int[] {7, 2}},
        {new int[] {0, 4}, new int[] {}, new int[] {2, 0}, new int[] {7, 2}},
        {new int[] {4, 1}, new int[] {1, 5}, new int[] {2, 6}, new int[] {7, 2}},
        {new int[] {0, 1}, new int[] {1, 5}, new int[] {2, 0}, new int[] {7, 2}},
        
        {new int[] {}, new int[] {5, 3}, new int[] {}, new int[] {3, 7}},
        {new int[] {0, 4}, new int[] {5, 3}, new int[] {6, 0}, new int[] {3, 7}},
        {new int[] {4, 1}, new int[] {1, 3}, new int[] {}, new int[] {3, 7}},
        {new int[] {0, 1}, new int[] {1, 3}, new int[] {6, 0}, new int[] {3, 7}},
        
        {new int[] {}, new int[] {5, 3}, new int[] {2, 6}, new int[] {3, 2}},
        {new int[] {0, 4}, new int[] {5, 3}, new int[] {2, 0}, new int[] {3, 2}},
        {new int[] {4, 1}, new int[] {1, 3}, new int[] {2, 6}, new int[] {3, 2}},
        {new int[] {0, 1}, new int[] {1, 3}, new int[] {2, 0}, new int[] {3, 2}}
    };

}
