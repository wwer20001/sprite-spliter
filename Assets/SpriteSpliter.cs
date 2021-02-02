using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSpliter : MonoBehaviour
{
    [SerializeField]
    private InputField fileNameInputfield = null;

    [SerializeField]
    private InputField extensionInputfield = null;

    [SerializeField]
    private InputField widthInputfield = null;

    [SerializeField]
    private InputField heightInputfield = null;

    [SerializeField]
    private InputField xSpacingInputfield = null;

    [SerializeField]
    private InputField ySpacingInputfield = null;

    [SerializeField]
    private InputField columnInputfield = null;

    [SerializeField]
    private InputField rawInputfield = null;

    public Texture2D texture2D;
    public Texture2D newTexture;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton()
    {
        string filePath = string.Format("{0}/{1}.{2}", Application.streamingAssetsPath, fileNameInputfield.text, extensionInputfield.text);
        if (File.Exists(filePath))
        {
            var stream = File.OpenRead(filePath);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            stream.Close();

            int width = int.Parse(widthInputfield.text);
            int height = int.Parse(heightInputfield.text);
            int xSpace = int.Parse(xSpacingInputfield.text);
            int ySpace = int.Parse(ySpacingInputfield.text);
            int column = int.Parse(columnInputfield.text);
            int raw = int.Parse(rawInputfield.text);
            int xRes = (width * column) + (xSpace * (column - 1));
            int yRes = (height * raw) + (ySpace * (raw - 1));

            texture2D = new Texture2D(xRes, yRes, TextureFormat.RGBA32, false, false);
            texture2D.LoadImage(data);
            texture2D.Apply();


            for (int y = 0; y < raw; y++)
             {
                for (int x = 0; x < column; x++)
                {
                    newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false, false);
                    for (int yp = 0; yp < height; yp++)
                    {
                        for (int xp = 0; xp < width; xp++)
                        {
                            newTexture.SetPixel(xp, yp, 
                                texture2D.GetPixel(
                                    xp + (width * x) + (xSpace * x), 
                                    yp + (height * y) + (ySpace * y)));
                        }
                    }
                    newTexture.Apply();
                    byte[] newPng = newTexture.EncodeToPNG();
                    var file = File.Create(string.Format("{0}/{1}_{2}.{3}", Application.streamingAssetsPath, fileNameInputfield.text, x + (column * y), extensionInputfield.text));
                    file.Write(newPng, 0, newPng.Length);
                    file.Close();
                }
            }
        }
        else
        {
            Debug.LogError($"Not exist {filePath} file!");
        }

    }

    
}
