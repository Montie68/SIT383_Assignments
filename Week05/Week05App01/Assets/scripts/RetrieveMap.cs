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
    public static RetrieveMap instance;
    [Header("Map Settings")]
    [Range(0,15)]
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
    [Header("Map Coordinates")] // remove or comment out after debuging
    public float latitude = 0;
    public float longitude  = 0;

    // Testing to see tex member
    Texture2D heightTexure;
    [SerializeField] // Testing to see tex member
    Texture2D mainTex;
    const float scaleExponent = 0.731f;
    const float scaleMultiplier = 0.0025f;
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
        if (instance !=null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;
        RetrieveTile(x, y, zoom);
    }
    public void SetZoom(int value)
    {
        zoom = value;
        Destroy(tileObject.gameObject);
        RetrieveTile(x, y, zoom);
    }
    public void RetrieveTile( int x, int y, int z)
    {
        getTileCoordinates(longitude, latitude, zoom, out x, out y);
        mainTex = new Texture2D(meshCount.x, meshCount.y);

        string url = "https://s3.amazonaws.com/elevation-tiles-prod/terrarium/" + z + "/" + x + "/" + y + ".png";

        heightTexure = GetImage(url);
        url = "https://a.tile.openstreetmap.org/" + z + "/" + x + "/" + y + ".png";
        mainTex = GetImage(url);
        mapMaterial.mainTexture = mainTex;
        makeMesh(meshCount.x, meshCount.y);
    }

    private Texture2D GetImage(string url)
    {
        WebRequest www = WebRequest.Create(url);
        ((HttpWebRequest)www).UserAgent = "DeakinMapTerrains";
        WebResponse response = www.GetResponse();

        Texture2D tex = new Texture2D(10, 10);
        ImageConversion.LoadImage(tex, new BinaryReader(response.GetResponseStream()).ReadBytes(1000000));
        return tex;
    }

    void makeMesh(int width, int height)
    {
        Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];
        Vector2[] UVs = new Vector2[(width + 1) * (height + 1)];
        int[] triangles = new int[6 * width * height];
        int triangleIndex = 0;
        float maxdistance = 0;
        for(int y = 0; y < height + 1; y++ )
        {
            for (int x = 0; x < width + 1; x++)
            {
                float xc = (float)x / (width + 1);
                float yc = (float)y / (height + 1);
                // float zc = 0.0f;
                
                Color c = heightTexure.GetPixel((int)(xc * heightTexure.width), (int)(yc * heightTexure.height));
               float scaleTerainByZoom = -scaleMultiplier * (Mathf.Pow(2 , zoom * scaleExponent)); // x*2^(zoom*n) where n = 0.731
               float zc = scaleTerainByZoom * ((c.r * 256 + c.g + c.b / 256) - 128);
               if (zc < maxdistance)
                {
                    maxdistance = zc;
                }
                if (zc > 0) zc = 0;
               // Debug.Log("At " + x + "; " + y + "; " + triangleIndex);

                vertices[y * (width + 1) + x] = 
                    new Vector3(meshScale.x * (xc-0.5f), meshScale.y * (yc - 0.5f), zc);

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
        tileObject.transform.SetPositionAndRotation(new Vector3(0,0, -1 * maxdistance), Quaternion.identity);
    }
    private void getTileCoordinates(float _longitude, float _latitude, int zoom, out int x, out int y)
    {
        x = (int)(Mathf.Floor((_longitude + 180.0f) / 360.0f * Mathf.Pow(2.0f, zoom)));
        y = (int)(Mathf.Floor((1.0f - Mathf.Log(Mathf.Tan(_latitude * Mathf.PI / 180.0f) + 1.0f / Mathf.Cos(_latitude * Mathf.PI / 180.0f)) / Mathf.PI) / 2.0f * Mathf.Pow(2.0f, zoom)));
    }
}
