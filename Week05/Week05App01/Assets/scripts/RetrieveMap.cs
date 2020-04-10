using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;
using System;

public class RetrieveMap : MonoBehaviour
{
    [Header("Map Settings")]
    public int zoom = 0;
    public int x = 0;
    public int y = 0;
    [Header("Material Settings")]
    public Material mapMaterial;
    [Tooltip("Set size of the generated mesh")]
    public Vector2Int meshCount = new Vector2Int(16, 16);
    [Tooltip("Set scale of the generated mesh")]
    public Vector2 meshScale = new Vector2(8, 8);
    [Tooltip("Leave blank if you want to generate mesh")]
    public GameObject tileObject;
    [Header("Debug Fields")] // remove or comment out after debuging
    [SerializeField] // Testing to see tex member
    Texture2D tex;
    private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
    {
        // All Certificates are accepted. Not good
        // practice, but outside scope of this
        // example.
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;
        tex = new Texture2D(meshCount.x, meshCount.y);
        StartCoroutine(AddTex());
        RetrieveTile(x, y, zoom);
        
    }
    IEnumerator AddTex()
    {
        // due to bug where material kept clearing reassigning it here. https://youtu.be/HydAsvDXk9o
        // the Deley Seems to sort this out. if no delay then bug still happens. and needs to be at least 0.025f
        yield return new WaitForSeconds(0.05f);
        if (!mapMaterial.GetTexture("_MainTex") && tex != null)
        {
            mapMaterial.mainTexture = tex;
        }
        if (tileObject == null) makeMesh(16, 16);

    }
    private void RetrieveTile( int x, int y, int z)
    {
        string url = "https://s3.amazonaws.com/elevation-tiles-prod/terrarium/" + z + "/" + x + "/" + y + ".png";

        //  "https://a.tile.openstreetmap.org/" 

        WebRequest www = WebRequest.Create(url);
        ((HttpWebRequest)www).UserAgent = "DeakinMapTerrains";
        WebResponse response = www.GetResponse();

        
        ImageConversion.LoadImage(tex, new BinaryReader(response.GetResponseStream()).ReadBytes(1000000));
        // due to bug where material kept clearing moved to AddText Method. https://youtu.be/HydAsvDXk9o
        //mapMaterial.SetTexture("_MainTex",  tex);
    }

    void makeMesh(int width, int height)
    {
        Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];
        Vector2[] UVs = new Vector2[(width + 1) * (height + 1)];
        int[] triangles = new int[6 * width * height];
        int triangleIndex = 0;
        for(int y = 0; y < height; y++ )
        {
            for (int x = 0; x < width; x++)
            {
                float xc = (float)x / (width + 1);
                float yc = (float)y / (height + 1);
                // float zc = Mathf.Sin(10f * xc);
                //Texture2D _tex = (Texture2D)mapMaterial.mainTexture;


                //Color c = _tex.GetPixel((int)xc * _tex.width, (int)yc * _tex.height);
                Color c = tex.GetPixel((int)xc * tex.width, (int)yc * tex.height);

                float zc = (c.r * 256 + c.g + c.b / 256) - 128;

                Debug.Log("At " + x + "; " + y + "; " + triangleIndex);
                vertices[y * (width + 1) + x] = new Vector3(meshScale.x*(xc - 0.5f), meshScale.y * (yc - 0.5f), zc);
                UVs[y * (width + 1) + x] = new Vector2(xc, yc);

                if ((x < width) && (y < height))
                {
                    triangles[triangleIndex++] = (y) * (width + 1) + (x + 1);
                    triangles[triangleIndex++] = (y) * (width + 1) + (x);
                    triangles[triangleIndex++] = (y + 1) * (width + 1) + (x);

                    triangles[triangleIndex++] = (y + 1) * (width + 1) + (x + 1);
                    triangles[triangleIndex++] = (y) * (width + 1) + (x + 1);
                    triangles[triangleIndex++] = (y + 1) * (width + 1) + (x);
                }
            }
        }
        Mesh m = new Mesh();
        
        m.vertices = vertices;
        m.triangles = triangles;
        m.uv = UVs;
        m.RecalculateNormals();
        tileObject = new GameObject("TerrainGen", typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider));
        tileObject.GetComponent<MeshRenderer>().material = mapMaterial;
        tileObject.GetComponent<MeshFilter>().mesh = m;
    }
}