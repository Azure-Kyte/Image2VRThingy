using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;



public class imageLoader : MonoBehaviour {

    public Texture2D t;

    public GameObject g;
    public Transform plot;
    public Material n;

    Text textthing;

    public string filename;
    public string filename2;
    public string filename3;

    public int blocksize;

    public GameObject texture;

    bool done = false;

    [System.SerializableAttribute]
    public class Pixel
    {
        public Color32 colour;
        public int count;

    }

    [System.SerializableAttribute]
    public class Materil 
    {
        public int minR;
        public int maxR;
        public int minG;
        public int maxG;
        public int minB;
        public int maxB;
        public Material m;

        
    }

    public List<Pixel> pixels = new List<Pixel>();
    public List<Materil> materials;
    Color32[] colours;
    int position;
    int length;
    public int posColours;
    public int posI;
    bool doneProcessing;
    

    // Use this for initialization
    void Start () {

        textthing = GameObject.Find("TextThing").GetComponent<Text>();

        t = LoadPNG(System.Environment.CurrentDirectory + @"\" + filename3);
        int x = t.width;
        int y = t.height;
        int numPixels = x * y;
        texture.GetComponent<MeshRenderer>().material.mainTexture = t;
        texture.transform.localScale = new Vector3((float)(x * 0.005f), 1f, (float)(y * 0.005f));
        Debug.Log("Width: " + x + " | Height: " + y + " | Total Pixels: " + numPixels);

        colours = t.GetPixels32();
        length = colours.Length;

        StartCoroutine(ProcessRandomColours(20000));
        //ProcessInBulk(true);

	}
	
	// Update is called once per frame
	void Update () {
        

		if (!done && doneProcessing)
        {
            for (int i = position; i < position + 30 && i < pixels.Count; i++)
            {
                Pixel p = pixels[i];
                if (p.count > 2)
                {
                    Material m = new Material(n);
                    m.color = new Color(Mathf.InverseLerp(0, 255, p.colour.r), Mathf.InverseLerp(0, 255, p.colour.g), Mathf.InverseLerp(0, 255, p.colour.b));
                    GameObject f = Instantiate(g, new Vector3(p.colour.r, p.colour.g, p.colour.b), Quaternion.Euler(0, 0, 0), plot);
                    f.transform.localPosition = new Vector3(p.colour.r, p.colour.g, p.colour.b);
                    f.transform.localScale = new Vector3(0.15f * Mathf.Clamp(p.count, 1, 100), 0.15f * Mathf.Clamp(p.count, 1, 100), 0.15f * Mathf.Clamp(p.count, 1, 100));
                    ValidateColours(m, p, f);
                    f.name = "Object_" + i;
                }
            }
            position += 30;
            if (position >= pixels.Count)
                done = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Destroy(plot.gameObject);
            GameObject g = new GameObject("plot");
            plot = g.transform;
            plot.localScale = new Vector3(0.005f, 0.005f, 0.005f);
            t = LoadPNG(System.Environment.CurrentDirectory + @"\" + filename);
            colours = t.GetPixels32();
            length = colours.Length;
            int x = t.width;
            int y = t.height;
            int numPixels = x * y;
            texture.GetComponent<MeshRenderer>().material.mainTexture = t;
            texture.transform.localScale = new Vector3((float)(x * 0.005f), 1f, (float)(y * 0.005f));
            Debug.Log("Width: " + x + " | Height: " + y + " | Total Pixels: " + numPixels);
            doneProcessing = false;
            done = false;
            position = 0;
            posColours = 0;

            pixels.Clear();
            
            StartCoroutine(ProcessRandomColours(20000));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Destroy(plot.gameObject);
            GameObject g = new GameObject("plot");
            plot = g.transform;
            plot.localScale = new Vector3(0.005f, 0.005f, 0.005f);
            t = LoadPNG(System.Environment.CurrentDirectory + @"\" + filename2);
            colours = t.GetPixels32();
            length = colours.Length;
            int x = t.width;
            int y = t.height;
            int numPixels = x * y;
            texture.GetComponent<MeshRenderer>().material.mainTexture = t;
            texture.transform.localScale = new Vector3((float)(x * 0.005f), 1f, (float)(y * 0.005f));
            
            Debug.Log("Width: " + x + " | Height: " + y + " | Total Pixels: " + numPixels);
            doneProcessing = false;
            done = false;
            position = 0;
            posColours = 0;

            pixels.Clear();

            StartCoroutine(ProcessRandomColours(20000));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Destroy(plot.gameObject);
            GameObject g = new GameObject("plot");
            plot = g.transform;
            plot.localScale = new Vector3(0.005f, 0.005f, 0.005f);
            t = LoadPNG(System.Environment.CurrentDirectory + @"\" + filename3);
            colours = t.GetPixels32();
            length = colours.Length;
            int x = t.width;
            int y = t.height;
            int numPixels = x * y;
            texture.GetComponent<MeshRenderer>().material.mainTexture = t;
            texture.transform.localScale = new Vector3((float)(x * 0.005f), 1f, (float)(y * 0.005f));

            Debug.Log("Width: " + x + " | Height: " + y + " | Total Pixels: " + numPixels);
            doneProcessing = false;
            done = false;
            position = 0;
            posColours = 0;

            pixels.Clear();

            StartCoroutine(ProcessRandomColours(20000));
        }
    }

    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    void ProcessColoursNonCoRoutine()
    {
        foreach (Color32 p in t.GetPixels32())
        {
            bool found = false;
            Pixel pixel = null;
            foreach (Pixel pix in pixels)
            {
                if (p.r == pix.colour.r && p.g == pix.colour.g && p.b == pix.colour.b)
                {
                    found = true;
                    pixel = pix;
                    break;
                }
            }

            if (found)
            {
                pixel.count += 1;
            }
            else
            {
                pixel = new Pixel();
                pixel.count = 1;
                pixel.colour = p;
                pixels.Add(pixel);
            }
        }
    }

    IEnumerator ProcessRandomColours(int size)
    {
        while (!doneProcessing)
        {
            for (int i = posColours; i < size - 1 && i < posColours + blocksize; i++)
            {
                //posI = i;
                bool found = false;
                Pixel pixel = null;

                if (i % 100 == 0)
                    pixels.Sort((b, a) => a.count.CompareTo(b.count));

                int randomIndex = Random.Range(0, colours.Length - 1);

                foreach (Pixel pix in pixels)
                {
                    if (colours[randomIndex].r == pix.colour.r && colours[randomIndex].g == pix.colour.g && colours[randomIndex].b == pix.colour.b)
                    {
                        found = true;
                        pixel = pix;
                        break;
                    }
                }

                if (found)
                {
                    pixel.count += 1;
                }
                else
                {
                    pixel = new Pixel();
                    pixel.count = 1;
                    pixel.colour = colours[randomIndex];
                    pixels.Add(pixel);
                }

            }
            string s = (100f / size * posColours).ToString("0.00") + "%";
            textthing.text = "Processing Colours..." + System.Environment.NewLine + s;
            posColours += blocksize;
            if (posColours >= size)
            {
                pixels.Sort((b, a) => a.count.CompareTo(b.count));
                textthing.text = "";
                StopAllCoroutines();
                doneProcessing = true;
            }

            yield return null;
        }
    }

    IEnumerator ProcessColours()
    {
        while (!doneProcessing) {
            for (int i = posColours; i < colours.Length - 1 && i < posColours + blocksize; i++)
            {
                //posI = i;
                bool found = false;
                Pixel pixel = null;

                if (i % 100 == 0 )
                pixels.Sort((b, a) => a.count.CompareTo(b.count));

                foreach (Pixel pix in pixels)
                {
                    if (
                        colours[i].r == pix.colour.r && colours[i].g == pix.colour.g && colours[i].b == pix.colour.b)
                    {
                        found = true;
                        pixel = pix;
                        break;
                    }
                }

                if (found)
                {
                    pixel.count += 1;
                }
                else
                {
                    pixel = new Pixel();
                    pixel.count = 1;
                    pixel.colour = colours[i];
                    pixels.Add(pixel);
                }
                
            }
            string s = (100f / colours.Length * posColours).ToString("0.00") + "%";
            textthing.text = "Processing Colours..." + System.Environment.NewLine + s;
            posColours += blocksize;
            if (posColours >= colours.Length)
            {
                pixels.Sort((b, a) => a.count.CompareTo(b.count));
                textthing.text = "";
                StopAllCoroutines();
                doneProcessing = true;
            }
            
            yield return null;
        }
    }


    void ProcessInBulk(bool allInStart)
    {
       

        Debug.Log(pixels.Count);

        if (allInStart)
        {
            int i = 0;

            foreach (Pixel p in pixels)
            {

                if (p.count > 1)
                {
                    i++;
                    Material m = new Material(n);
                    m.color = new Color(Mathf.InverseLerp(0, 255, p.colour.r), Mathf.InverseLerp(0, 255, p.colour.g), Mathf.InverseLerp(0, 255, p.colour.b));
                    GameObject f = Instantiate(g, new Vector3(p.colour.r, p.colour.g, p.colour.b), Quaternion.Euler(0, 0, 0), plot);
                    f.transform.localScale = new Vector3(0.1f * Mathf.Clamp(p.count, 1, 100), 0.1f * Mathf.Clamp(p.count, 1, 100), 0.1f * Mathf.Clamp(p.count, 1, 100));
                    f.GetComponent<MeshRenderer>().material = m;
                    f.name = "Object_" + i;
                }
            }
            done = true;
        }

        plot.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    void ValidateColours(Material m, Pixel p, GameObject f)
    {
        bool collision = false;
        Material mms = null;
        foreach (Materil mm in materials)
        {
            bool hitR = false;
            bool hitG = false;
            bool hitB = false;

            if (p.colour.r >= mm.minR && p.colour.r < mm.maxR)
            {
                hitR = true;
            }
            if (p.colour.g >= mm.minG && p.colour.g < mm.maxG)
            {
                hitG = true;
            }
            if (p.colour.b >= mm.minB && p.colour.b < mm.maxB)
            {
                hitB = true;
            }

            if (hitR && hitG && hitB)
            {
                collision = true;
                mms = mm.m;
                break;
            }
        }
        if (collision)
        {
            f.GetComponent<MeshRenderer>().material = mms;
        }
        else
            f.GetComponent<MeshRenderer>().material = m;
    }

    void GenerateMaterials()
    {
        for (int i = 0; i < 8; i++)
        {
            Material R = new Material(n);
            Material G = new Material(n);
            Material B = new Material(n);

            R.color = new Color();
        }
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

}


