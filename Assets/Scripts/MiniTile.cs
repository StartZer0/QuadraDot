using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniTile : MonoBehaviour
{

    public Image MiniTileImage;
    public Color colorForMiniTile;
    private Color colorTemp;

    // Use this for initialization
	void Awake ()
	{
	    MiniTileImage = gameObject.GetComponent<Image>();
	    
	    

	}

    void Update()
    {
        colorTemp = gameObject.GetComponent<Image>().color;
        if (colorTemp == Color.cyan || colorTemp == Color.green || colorTemp == Color.magenta || colorTemp == Color.yellow) {
            colorForMiniTile = colorTemp;
        }
    }

    //void ApplyStyleFromMiniTileHolder(int index)
    //{
    //    MiniTileImage.color = MiniTileStyleHolder.Instance.MiniTileStyles.MiniTileColor;
    //}
}
