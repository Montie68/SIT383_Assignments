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
    public int zoom = 0;
    public int x = 0;
    public int y = 0;

    public Material mapMaterial;
    public GameObject tileObject;
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
        makeMesh(16,16);
        RetrieveTile( x, y, zoom);
    }

    private void RetrieveTile( int x, int y, int z)
    {
        string url = "https://s3.amazonaws.com/elevation-tiles-prod/terrarium/" + z + "/" + x + "/" + y + ".png";

        //  "https://a.tile.openstreetmap.org/" 

        WebRequest www = WebRequest.Create(url);
        ((HttpWebRequest)www).UserAgent = "DeakinMapTerrains";
        WebResponse response = www.GetResponse();

        Texture2D tex = new Texture2D(10,10);
        ImageConversion.LoadImage(tex, new BinaryReader(response.GetResponseStream()).ReadBytes(1000000));

        mapMaterial.mainTexture = tex;
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
                float zc = 0.0f;

                Debug.Log("At " + x + "; " + y + "; " + triangleIndex);
                vertices[y * (width + 1) + x] = new Vector3(xc, yc, zc);
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
        tileObject.transform.localScale = new Vector3(width / 2, height / 2, 1);
        tileObject.GetComponent<MeshRenderer>().material = mapMaterial;
        tileObject.GetComponent<MeshFilter>().mesh = m;
    }
}
