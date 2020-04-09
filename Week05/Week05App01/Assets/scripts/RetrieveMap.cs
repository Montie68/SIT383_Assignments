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
        RetrieveTile( x, y, zoom);
    }

    private void RetrieveTile( int x, int y, int z)
    {
        string url = "https://s3.amazonaws.com/elevation-tiles-prod/terrarium/" + 
            z + "/" + x + "/" + y + ".png";
        WebRequest www = WebRequest.Create(url);
        WebResponse response = www.GetResponse();

        Texture2D tex = new Texture2D(10,10);
        ImageConversion.LoadImage(tex, new BinaryReader(response.GetResponseStream()).ReadBytes(1000000));

        mapMaterial.mainTexture = tex;
    }
}
