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



    //void ApplyStyleFromHolder(int index) {
    //    //if (index == 4)
    //    //{
    //    //    TileImage.enabled = false;
    //    //}
    //    TileText.text = TileStyleHolder.Instance.TileStyles[index].Number.ToString();
    //    //TileText.color = TileStyleHolder.Instance.TileStyles[index].TextColor;
    //    TileImage.color = TileStyleHolder.Instance.TileStyles[index].TileColor;
       
    //}

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
            //case 128:
            //    ApplyThisColorFromHolder(6);
            //    break;
            //case 256:
            //    ApplyThisColorFromHolder(7);
            //    break;
            //case 512:
            //    ApplyThisColorFromHolder(8);
            //    break;
            //case 1024:
            //    ApplyThisColorFromHolder(8);
            //    break;
            default:
                Debug.LogError("Check the numbers that you passed to ApplyStyle");
                break;
        }

    }

    //void ApplyStyle(int num, int child ) {
        
    //    if (child == 4)
    //    {
    //            switch (num) {
    //                case 2:
    //                    ApplyStyleFromHolder(0);
    //                    break;
    //                case 4:
    //                    ApplyStyleFromHolder(1);
    //                    break;
    //                case 8:
    //                    ApplyStyleFromHolder(2);
    //                    break;
    //                case 16:
    //                    ApplyStyleFromHolder(3);
    //                    break;
    //                case 32:
    //                    ApplyStyleFromHolder(4);
    //                    break;
    //                case 64:
    //                    ApplyStyleFromHolder(5);
    //                    break;
    //                case 128:
    //                    ApplyStyleFromHolder(6);
    //                    break;
    //                case 256:
    //                    ApplyStyleFromHolder(7);
    //                    break;
    //                case 512:
    //                    ApplyStyleFromHolder(8);
    //                    break;
    //                case 1024:
    //                    ApplyStyleFromHolder(9);
    //                    break;
    //                case 2048:
    //                    ApplyStyleFromHolder(10);
    //                    break;
    //                case 4096:
    //                    ApplyStyleFromHolder(11);
    //                    break;
    //                default:
    //                Debug.LogError("Check the numbers that you passed to ApplyStyle");
    //                break;
    //        }
    //    }
        

    //}

    

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
