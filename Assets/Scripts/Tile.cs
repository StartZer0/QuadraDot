using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
    public int intRow;
    public int intColumn;
    public bool mergeThisTile = false;
    public int miniTilesCount = 0;
    public bool colorIsChanged = false;
    public Color tileColor;
    private MiniTile colorMini;
    
    // List<Transform> MiniTiles = new List<Transform>();
    public List<GameObject> MiniTiles = new List<GameObject>();

    void Start()
    {
        colorMini = GameObject.FindObjectOfType<MiniTile>();
    }

    Color colordForTile1;
    

    public static Color TheMiniTileColor(Tile[] MiniTiles)
    {
        Color colordForTile1 = Color.clear;

        for (int j = 0; j < 4; j++) {
            if (MiniTiles[j].GetComponent<Image>().enabled) {
                colordForTile1 = MiniTiles[j].GetComponent<Image>().color;
                break;
            }
        }
        return colordForTile1;

    }
    
  

    public int Number {
        get { return number; }
        set {
            number = value;
            if (number == 0) {
                SetEmpty();
            }
            else {
                ApplyThisColor(number);
                SetVisible();
            }
        }
    }  

    private int temp = 4;
    private int number;
    private Text TileText;
    private Image TileImage;
    private Animator anim;
    
    public static int ChildCountActive(GameObject t) {
        int k = 0;
        for (int i = 0; i < 4; i++) {

            if (t.transform.GetChild(0).GetChild(i).GetComponentInChildren<Image>().enabled)
            {
                k++; 
                Debug.Log("K is " + k);
            }
        }

        return k;
    }


    void Awake()
    {
        anim = GetComponent<Animator>();
        TileText = GetComponentInChildren<Text>();
        TileImage = transform.Find("NumberedCell").GetComponent<Image>(); //In hierarchy
    }

    public void PlayMergeAnimation()
    {
        anim.SetTrigger("Merge");
    }
    public void PlayAppearAnimation()
    {
        anim.SetTrigger("Appear");
    }

    void ApplyThisColorFromHolder(int index)
    {
        TileImage.color = TileStyleHolder.Instance.TileStyles[index].TileColor;
    }

    void ApplyThisColor(int num)
    {
        switch (num)
        {
            case 2:
                ApplyThisColorFromHolder(0);
                break;
            case 3:
                ApplyThisColorFromHolder(1);
                break;
            case 4:
                ApplyThisColorFromHolder(2);
                break;
            case 5:
                ApplyThisColorFromHolder(3);
                break;
            case 6:
                ApplyThisColorFromHolder(4);
                break;
            case 7:
                ApplyThisColorFromHolder(5);
                break;
            default:
                Debug.LogError("Check the numbers that you passed to ApplyStyle");
                break;
        }
    }   

    private void SetVisible() {
        TileImage.enabled = true;
        TileText.enabled = false;
    }

    private void SetEmpty() {
        TileImage.enabled = false;
        foreach (var mini in this.MiniTiles) {
            mini.GetComponent<Image>().enabled = false;
        }
        TileText.enabled = false;
    }

}
