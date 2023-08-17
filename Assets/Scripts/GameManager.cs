using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public enum GameState
{
	Playing,
	Gameover,
	WaitingForMoveToEnd
}

public class GameManager : MonoBehaviour
{
	// Sound
	public AudioClip scoreClip;
	public AudioSource scoreSound;

	//new after added delays 
	public GameState State;
	[Range(0, 2f)] public float delay;
	private bool moveMade;
	private bool[] lineMoveComplete = new bool[4] {true, true, true, true};
	//new after added delays 

	public Text GameOverText;
	public Text GameOverScoreText;
	public GameObject GameOverPanel;

	//Timer Bars
	private RadialBarProgressBlue barBlue;
	private RadialBarProgressRed barRed;
	private RadialBarProgressGreen barGreen;
	private RadialBarProgressYellow barYellow;
	//Timer Bars

	//using UnityEditor.IMGUI.Controls;


	private Tile[,] AllTiles = new Tile[4, 4]; //
	private List<Tile> EmptyTiles = new List<Tile>();
	private List<Tile[]> columns = new List<Tile[]>();
	private List<Tile[]> rows = new List<Tile[]>();
	private List<Transform> MiniTiles = new List<Transform>();
	private Tile tileScript;

	private Vector3 blueTimer;
	void Awake()
	{
		Time.timeScale = 1f;
		Initialize();
	}

	void Initialize()
	{
		barBlue = GameObject.FindObjectOfType<RadialBarProgressBlue>();
		barRed = GameObject.FindObjectOfType<RadialBarProgressRed>();
		barGreen = GameObject.FindObjectOfType<RadialBarProgressGreen>();
		barYellow = GameObject.FindObjectOfType<RadialBarProgressYellow>();

		scoreSound.clip = scoreClip;

	}

	void Update()
	{

		StopTime();
	}

	// Use this for initialization
	void Start()
	{
		tileScript = GameObject.FindObjectOfType<Tile>();
		Tile[] AllTilesOneDim = GameObject.FindObjectsOfType<Tile>();
		foreach (var t in AllTilesOneDim) {
			t.Number = 0;
			// Set to deactive all childs in Tile
			foreach (MiniTile child in t.GetComponentsInChildren<MiniTile>())
			{
				child.gameObject.GetComponent<Image>().enabled = false; 
			}

			AllTiles[t.intRow, t.intColumn] = t;
			EmptyTiles.Add(t);
		}
		columns.Add(new Tile[] { AllTiles[0, 0], AllTiles[1, 0], AllTiles[2, 0], AllTiles[3, 0] }); //
		columns.Add(new Tile[] { AllTiles[0, 1], AllTiles[1, 1], AllTiles[2, 1], AllTiles[3, 1] }); //
		columns.Add(new Tile[] { AllTiles[0, 2], AllTiles[1, 2], AllTiles[2, 2], AllTiles[3, 2] });  //
		columns.Add(new Tile[] { AllTiles[0, 3], AllTiles[1, 3], AllTiles[2, 3], AllTiles[3, 3] }); //


		rows.Add(new Tile[] { AllTiles[0, 0], AllTiles[0, 1], AllTiles[0, 2], AllTiles[0, 3] });  //
		rows.Add(new Tile[] { AllTiles[1, 0], AllTiles[1, 1], AllTiles[1, 2], AllTiles[1, 3] });  //
		rows.Add(new Tile[] { AllTiles[2, 0], AllTiles[2, 1], AllTiles[2, 2], AllTiles[2, 3] }); //
		rows.Add(new Tile[] { AllTiles[3, 0], AllTiles[3, 1], AllTiles[3, 2], AllTiles[3, 3] });   //


		Generate();
		Generate();



	}

	private void GameOver()
	{
		Time.timeScale = 0f;
		GameOverScoreText.text = ScoreTracker.Instance.Score.ToString();
		GameOverPanel.SetActive(true);

	}

	public void NewGameButtonHandler() {
		Time.timeScale = 1f;
		Application.LoadLevel(Application.loadedLevel);
	}

	public void StopTime()
	{
		if (barBlue.fillAmountValue <= 0.01f || barRed.fillAmountValue <= 0.01f|| barGreen.fillAmountValue <= 0.01f || barYellow.fillAmountValue <= 0.01f )
		{
			GameOver();
		}
	}

	bool CanMove()
	{
		if (EmptyTiles.Count > 0)
		{
			Debug.Log("True is triggered");
			Debug.Log("fillAmountValue = 0");
			return true;

		}

		else
		{   
			//check columns
			for (int i = 0; i < columns.Count; i++)
			{
				for (int j = 0; j < rows.Count -1; j++)
				{
					Color colordForTile1 = Color.clear;

					for (int z = 0; z < 4; z++) {
						if (AllTiles[j, i].MiniTiles[z].GetComponent<Image>().enabled) {
							colordForTile1 = AllTiles[j, i].MiniTiles[z].GetComponent<Image>().color;
							Debug.Log("Color is: " + colordForTile1);
							break;
						}
					}
					Color colordForTile2 = Color.clear;

					for (int z = 0; z < 4; z++) {
						if (AllTiles[j + 1, i].MiniTiles[z].GetComponent<Image>().enabled) {
							colordForTile2 = AllTiles[j + 1, i].MiniTiles[z].GetComponent<Image>().color;
							Debug.Log("Color is: " + colordForTile2);
							break;
						}
					}



					AllTiles[j, i].miniTilesCount = 0;
					AllTiles[j, i].miniTilesCount = Tile.ChildCountActive(AllTiles[j, i].gameObject);

					AllTiles[j + 1, i].miniTilesCount = 0;
					AllTiles[j + 1, i].miniTilesCount = Tile.ChildCountActive(AllTiles[j + 1, i].gameObject);


					if (colordForTile1 == colordForTile2 && AllTiles[j, i].miniTilesCount + AllTiles[j + 1, i].miniTilesCount <=4)
					{
						return true;
					}
				}
			}

			//check rows
			for (int i = 0; i < rows.Count; i++)
			{
				for (int j = 0; j < columns.Count -1; j++)
				{
					Color colordForTile1 = Color.clear;

					for (int z = 0; z < 4; z++) {
						if (AllTiles[i, j].MiniTiles[z].GetComponent<Image>().enabled) {
							colordForTile1 = AllTiles[i, j].MiniTiles[z].GetComponent<Image>().color;
							Debug.Log("Color is: " + colordForTile1);
							break;
						}
					}
					Color colordForTile2 = Color.clear;

					for (int z = 0; z < 4; z++) {
						if (AllTiles[i, j + 1].MiniTiles[z].GetComponent<Image>().enabled) {
							colordForTile2 = AllTiles[i, j + 1].MiniTiles[z].GetComponent<Image>().color;
							Debug.Log("Color is: " + colordForTile2);
							break;
						}
					}

					//bool sameColor = colordForTile1 == colordForTile2;

					AllTiles[i, j].miniTilesCount = 0;
					AllTiles[i, j].miniTilesCount = Tile.ChildCountActive(AllTiles[i, j].gameObject);

					AllTiles[i, j + 1].miniTilesCount = 0;
					AllTiles[i, j + 1].miniTilesCount = Tile.ChildCountActive(AllTiles[i, j + 1].gameObject);


					if (colordForTile1 == colordForTile2 && AllTiles[i, j].miniTilesCount + AllTiles[i, j + 1].miniTilesCount <=4)
					{
						return true;
					}
				}
			}

		}
		return false;
	}

	void Generate() {
		if (EmptyTiles.Count > 0) {


			int indexForNewNumber = Random.Range(0, EmptyTiles.Count);

			EmptyTiles[indexForNewNumber].Number = 2; 

			int randomindex = Random.Range(0, 4);



			EmptyTiles[indexForNewNumber].MiniTiles[randomindex].GetComponent<Image>().enabled = true;
			switch (randomindex)
			{
			case 0:
				EmptyTiles[indexForNewNumber].MiniTiles[randomindex].GetComponent<Image>().color = Color.cyan;
				break;
			case 1:
				EmptyTiles[indexForNewNumber].MiniTiles[randomindex].GetComponent<Image>().color = Color.green;
				break;
			case 2:
				EmptyTiles[indexForNewNumber].MiniTiles[randomindex].GetComponent<Image>().color = Color.magenta;
				break;
			case 3:
				EmptyTiles[indexForNewNumber].MiniTiles[randomindex].GetComponent<Image>().color = Color.yellow;
				break;
			default:
				Debug.LogError("Check the numbers that you passed to ranomindex for color");
				break;
			}
			//Debug.Log("This is minitilesCount: " + EmptyTiles[indexForNewNumber].miniTilesCount);

			EmptyTiles[indexForNewNumber].PlayAppearAnimation();
			EmptyTiles.RemoveAt(indexForNewNumber);
			//Debug.Log(EmptyTiles.Count);
		}
	}


	private void ResetMergedFlags() {  
		foreach (Tile t in AllTiles) {
			t.mergeThisTile = false;
			t.miniTilesCount = 0;
		}
	}

	private void UpdateEmptyTiles() {
		EmptyTiles.Clear();
		foreach (Tile t in AllTiles) {
			if (t.Number == 0) {
				EmptyTiles.Add(t);
			}
		}
	}



	public void Move(MoveDirection md) {
		Debug.Log(md.ToString() + " move");
		moveMade = false;
		ResetMergedFlags();
		if (delay>0)
		{
			StartCoroutine(MoveCoroutine(md));
		}
		else
		{
			for (int i = 0; i < rows.Count; i++) {
				switch (md) {
				case MoveDirection.Down:
					while (MakeOneMoveUpIndex(columns[i])) { moveMade = true; }
					break;
				case MoveDirection.Up:
					while (MakeOneMoveDownIndex(columns[i])) { moveMade = true; }
					break;
				case MoveDirection.Left:
					while (MakeOneMoveDownIndex(rows[i])) { moveMade = true; }
					break;
				case MoveDirection.Right:
					while (MakeOneMoveUpIndex(rows[i])) { moveMade = true; }
					break;

				}
			}
			if (moveMade) {
				UpdateEmptyTiles();
				Generate();

				if (!CanMove()) {

					GameOver();
				}

			}
		}


	}

	IEnumerator MoveCoroutine(MoveDirection md)
	{
		State = GameState.WaitingForMoveToEnd;
		// start moving each line with delays depending on MoveDirection md
		switch (md)
		{
		case MoveDirection.Down:
			for (int i = 0; i < columns.Count; i++)
			{
				StartCoroutine(MoveOneLineUpIndexCoroutine(columns[i], i));

			}
			break;
		case MoveDirection.Left:
			for (int i = 0; i < rows.Count; i++)
			{
				StartCoroutine(MoveOneLineDownIndexCoroutine(rows[i], i));

			}
			break;
		case MoveDirection.Right:
			for (int i = 0; i < rows.Count; i++)
			{
				StartCoroutine(MoveOneLineUpIndexCoroutine(rows[i], i));

			}
			break;
		case MoveDirection.Up:
			for (int i = 0; i < columns.Count; i++)
			{
				StartCoroutine(MoveOneLineDownIndexCoroutine(columns[i], i));

			}
			break;
		}
		// wait until the move is over in all lines
		while (!(lineMoveComplete[0] && lineMoveComplete[1] && lineMoveComplete[2] && lineMoveComplete[3] ))
		{
			yield return null;
		}
		if (moveMade)
		{
			UpdateEmptyTiles();
			Generate();

			if (!CanMove())
			{
				GameOver();
			}

		}
		State = GameState.Playing;
		StopAllCoroutines();

	}

	IEnumerator MoveOneLineUpIndexCoroutine(Tile[] line, int index)
	{
		lineMoveComplete[index] = false;
		while (MakeOneMoveUpIndex(line))
		{
			moveMade = true;
			yield return new WaitForSeconds(delay);
		}
		lineMoveComplete[index] = true;
	}
	IEnumerator MoveOneLineDownIndexCoroutine(Tile[] line, int index)
	{
		lineMoveComplete[index] = false;
		while (MakeOneMoveDownIndex(line))
		{
			moveMade = true;
			yield return new WaitForSeconds(delay);
		}
		lineMoveComplete[index] = true;
	}


	bool MakeOneMoveDownIndex(Tile[] LineOfTiles) {

		for (int i = 0; i < LineOfTiles.Length - 1; i++) {
			//Move Block
			if (LineOfTiles[i].Number == 0 && LineOfTiles[i + 1].Number != 0) {
				LineOfTiles[i].Number = LineOfTiles[i + 1].Number;
				LineOfTiles[i].MiniTiles[0].GetComponent<Image>().enabled = LineOfTiles[i + 1].MiniTiles[0].GetComponent<Image>().enabled;
				LineOfTiles[i].MiniTiles[0].GetComponent<Image>().color = LineOfTiles[i + 1].MiniTiles[0].GetComponent<Image>().color;
				LineOfTiles[i].MiniTiles[1].GetComponent<Image>().enabled = LineOfTiles[i + 1].MiniTiles[1].GetComponent<Image>().enabled;
				LineOfTiles[i].MiniTiles[1].GetComponent<Image>().color = LineOfTiles[i + 1].MiniTiles[1].GetComponent<Image>().color;
				LineOfTiles[i].MiniTiles[2].GetComponent<Image>().enabled = LineOfTiles[i + 1].MiniTiles[2].GetComponent<Image>().enabled;
				LineOfTiles[i].MiniTiles[2].GetComponent<Image>().color = LineOfTiles[i + 1].MiniTiles[2].GetComponent<Image>().color;
				LineOfTiles[i].MiniTiles[3].GetComponent<Image>().enabled = LineOfTiles[i + 1].MiniTiles[3].GetComponent<Image>().enabled;
				LineOfTiles[i].MiniTiles[3].GetComponent<Image>().color = LineOfTiles[i + 1].MiniTiles[3].GetComponent<Image>().color;
				LineOfTiles[i + 1].Number = 0;
				foreach (var mini in LineOfTiles[i + 1].MiniTiles)
				{
					mini.GetComponent<Image>().enabled = false;
				}


				//Debug.Log("The childs of Line " + LineOfTiles[i].transform.GetChild(0).GetChild(0).name);
				return true;
			}



			//Merge Block
			if (LineOfTiles[i].Number != 0 && LineOfTiles[i].Number == LineOfTiles[i + 1].Number && LineOfTiles[i].mergeThisTile == false && LineOfTiles[i + 1].mergeThisTile == false) {


				LineOfTiles[i].miniTilesCount = 0;
				LineOfTiles[i].miniTilesCount = Tile.ChildCountActive(LineOfTiles[i].gameObject);

				Debug.Log(" LineOfTiles[i].miniTilesCount = " + LineOfTiles[i].miniTilesCount);

				Color colordForTile1= Color.clear;

				for (int j = 0; j < 4; j++) {
					if (LineOfTiles[i].MiniTiles[j].GetComponent<Image>().enabled) {
						colordForTile1 = LineOfTiles[i].MiniTiles[j].GetComponent<Image>().color;
						break;
					}
				}

				LineOfTiles[i + 1].miniTilesCount = 0;
				LineOfTiles[i + 1].miniTilesCount = Tile.ChildCountActive(LineOfTiles[i + 1].gameObject);
				Debug.Log(" LineOfTiles[i].miniTilesCount = " + LineOfTiles[i + 1].miniTilesCount);

				Color colordForTile2 = Color.clear;

				for (int j = 0; j < 4; j++) {
					if (LineOfTiles[i + 1].MiniTiles[j].GetComponent<Image>().enabled) {
						colordForTile2 = LineOfTiles[i + 1].MiniTiles[j].GetComponent<Image>().color;
						break;
					}
				}

				bool sameColor = colordForTile1 == colordForTile2;            

				if (LineOfTiles[i].miniTilesCount + LineOfTiles[i + 1].miniTilesCount < 4 && LineOfTiles[i].miniTilesCount + LineOfTiles[i + 1].miniTilesCount != 0 && LineOfTiles[i].mergeThisTile == false && LineOfTiles[i + 1].mergeThisTile == false && sameColor)

				{
					int enableCounts = LineOfTiles[i].miniTilesCount + LineOfTiles[i + 1].miniTilesCount;
					Debug.Log("int enableCounts = " + enableCounts);
					int g = 0;
					Color tempColor = colordForTile2;
					Debug.Log(string.Format("<tempColor=#{0:X2}{1:X2}{2:X2}></color>", (byte)(tempColor.r * 255f), (byte)(tempColor.g * 255f), (byte)(tempColor.b * 255f)));

					foreach (var mini in LineOfTiles[i].MiniTiles) {
						mini.GetComponent<Image>().enabled = false;
					}

					for (int j = 0; j <= enableCounts - 1; j++)
					{
						LineOfTiles[i].MiniTiles[j].GetComponent<Image>().enabled = true;
						LineOfTiles[i].MiniTiles[j].GetComponent<Image>().color = colordForTile2;

						g++;
					}
					Debug.Log("int g = " + g);
					Debug.Log("LineOfTiles[i] = " + i);
					LineOfTiles[i].mergeThisTile = true;
					LineOfTiles[i + 1].Number = 0;
					foreach (var mini in LineOfTiles[i + 1].MiniTiles) {
						mini.GetComponent<Image>().enabled = false;
					}
					LineOfTiles[i].PlayMergeAnimation();
					ScoreTracker.Instance.Score += 3;

					if (tempColor == Color.cyan)
					{
						barBlue.AmountD += 5f;
						//barBlue.numberText = 5f;
						CombatTextManager.Instance.CreateText(barBlue.transform.position, "+5", Color.cyan);
						//Debug.Log("cyan");
					}
					else if (tempColor == Color.magenta)
					{
						barRed.AmountD += 5f;
						CombatTextManager.Instance.CreateText(barRed.transform.position, "+5", Color.magenta);
						//Debug.Log("magenta");
					}
					else if (tempColor == Color.green)
					{
						barGreen.AmountD += 5f;
						CombatTextManager.Instance.CreateText(barGreen.transform.position, "+5", Color.green);
						//Debug.Log("green");
					}
					else if (tempColor == Color.yellow)
					{
						barYellow.AmountD += 5f;
						CombatTextManager.Instance.CreateText(barYellow.transform.position, "+5", Color.yellow);
						//Debug.Log("yellow");
					}


					return true;

				}
				else if (LineOfTiles[i].miniTilesCount + LineOfTiles[i + 1].miniTilesCount == 4 && LineOfTiles[i].mergeThisTile == false && LineOfTiles[i + 1].mergeThisTile == false && sameColor) {
					Color tempColor = colordForTile2;
					Debug.Log(string.Format("<tempColor=#{0:X2}{1:X2}{2:X2}></color>", (byte)(tempColor.r * 255f), (byte)(tempColor.g * 255f), (byte)(tempColor.b * 255f)));

					LineOfTiles[i + 1].Number = 0;

					foreach (var mini in LineOfTiles[i + 1].MiniTiles) {
						mini.GetComponent<Image>().enabled = false;
					}
					LineOfTiles[i].PlayMergeAnimation();
					LineOfTiles[i].Number = 0;
					foreach (var mini in LineOfTiles[i].MiniTiles) {
						mini.GetComponent<Image>().enabled = false;
					}

					ScoreTracker.Instance.Score += 10;
					if ( tempColor== Color.cyan) {
						barBlue.AmountD += 10f;
						CombatTextManager.Instance.CreateText(barBlue.transform.position, "+10", Color.cyan);
						//AudioSource.PlayClipAtPoint(scoreClip, barBlue.transform.position);
						scoreSound.Play();
					}
					else if (tempColor == Color.magenta) {
						barRed.AmountD += 10f;
						CombatTextManager.Instance.CreateText(barRed.transform.position, "+10", Color.magenta);
						//AudioSource.PlayClipAtPoint(scoreClip, barRed.transform.position);
						scoreSound.Play();
					}
					else if (tempColor == Color.green) {
						barGreen.AmountD += 10f;
						CombatTextManager.Instance.CreateText(barGreen.transform.position, "+10", Color.green);
						//AudioSource.PlayClipAtPoint(scoreClip, barGreen.transform.position);
						scoreSound.Play();
					}
					else if (tempColor == Color.yellow) {
						barYellow.AmountD += 10f;
						CombatTextManager.Instance.CreateText(barYellow.transform.position, "+10", Color.yellow);
						//AudioSource.PlayClipAtPoint(scoreClip, barYellow.transform.position);
						scoreSound.Play();
					}
					return true;

				}

			}


		}
		return false;

	}



	bool MakeOneMoveUpIndex(Tile[] LineOfTiles) {
		for (int i = LineOfTiles.Length - 1; i > 0; i--) {
			if (LineOfTiles[i].Number == 0 && LineOfTiles[i - 1].Number != 0) {
				LineOfTiles[i].Number = LineOfTiles[i - 1].Number;
				LineOfTiles[i].MiniTiles[0].GetComponent<Image>().enabled = LineOfTiles[i - 1].MiniTiles[0].GetComponent<Image>().enabled;
				LineOfTiles[i].MiniTiles[0].GetComponent<Image>().color = LineOfTiles[i - 1].MiniTiles[0].GetComponent<Image>().color;
				LineOfTiles[i].MiniTiles[1].GetComponent<Image>().enabled = LineOfTiles[i - 1].MiniTiles[1].GetComponent<Image>().enabled;
				LineOfTiles[i].MiniTiles[1].GetComponent<Image>().color = LineOfTiles[i - 1].MiniTiles[1].GetComponent<Image>().color;
				LineOfTiles[i].MiniTiles[2].GetComponent<Image>().enabled = LineOfTiles[i - 1].MiniTiles[2].GetComponent<Image>().enabled;
				LineOfTiles[i].MiniTiles[2].GetComponent<Image>().color = LineOfTiles[i - 1].MiniTiles[2].GetComponent<Image>().color;
				LineOfTiles[i].MiniTiles[3].GetComponent<Image>().enabled = LineOfTiles[i - 1].MiniTiles[3].GetComponent<Image>().enabled;
				LineOfTiles[i].MiniTiles[3].GetComponent<Image>().color = LineOfTiles[i - 1].MiniTiles[3].GetComponent<Image>().color;
				LineOfTiles[i - 1].Number = 0;
				foreach (var mini in LineOfTiles[i - 1].MiniTiles) {
					mini.GetComponent<Image>().enabled = false;
				}
				//Debug.Log("The childs of Line in up " + LineOfTiles[i].transform.GetChild(0).GetChild(0).name);
				return true;
			}


			if (LineOfTiles[i].Number != 0 && LineOfTiles[i].Number == LineOfTiles[i - 1].Number && LineOfTiles[i].mergeThisTile == false && LineOfTiles[i - 1].mergeThisTile == false)
			{
				LineOfTiles[i].miniTilesCount = 0;
				LineOfTiles[i].miniTilesCount = Tile.ChildCountActive(LineOfTiles[i].gameObject);
				Debug.Log(" LineOfTiles[i].miniTilesCount = " + LineOfTiles[i].miniTilesCount);

				Color colordForTile1 = Color.clear;

				for (int j = 0; j < 4; j++) {
					if (LineOfTiles[i].MiniTiles[j].GetComponent<Image>().enabled) {
						colordForTile1 = LineOfTiles[i].MiniTiles[j].GetComponent<Image>().color;
						break;
					}

				}

				LineOfTiles[i - 1].miniTilesCount = 0;
				LineOfTiles[i - 1].miniTilesCount = Tile.ChildCountActive(LineOfTiles[i - 1].gameObject);
				Debug.Log(" LineOfTiles[i -1].miniTilesCount = " + LineOfTiles[i - 1].miniTilesCount);

				Color colordForTile2 = Color.clear;

				for (int j = 0; j < 4; j++)
				{
					if (LineOfTiles[i - 1].MiniTiles[j].GetComponent<Image>().enabled)
					{
						colordForTile2 = LineOfTiles[i - 1].MiniTiles[j].GetComponent<Image>().color;
						break;
					}
				}

				bool sameColor = colordForTile1 == colordForTile2;

				if (LineOfTiles[i].miniTilesCount + LineOfTiles[i - 1].miniTilesCount < 4 && LineOfTiles[i].miniTilesCount + LineOfTiles[i - 1].miniTilesCount != 0 && LineOfTiles[i].mergeThisTile == false && LineOfTiles[i - 1].mergeThisTile == false && sameColor) {
					int enableCounts = LineOfTiles[i].miniTilesCount + LineOfTiles[i - 1].miniTilesCount;
					Debug.Log("int enableCounts = " + enableCounts);
					Color tempColor = colordForTile2;
					Debug.Log(string.Format("<tempColor=#{0:X2}{1:X2}{2:X2}></color>", (byte)(tempColor.r * 255f), (byte)(tempColor.g * 255f), (byte)(tempColor.b * 255f)));

					foreach (var mini in LineOfTiles[i].MiniTiles) {
						mini.GetComponent<Image>().enabled = false;
					}

					for (int j = 0; j <= enableCounts - 1; j++) {
						LineOfTiles[i].MiniTiles[j].GetComponent<Image>().enabled = true;
						LineOfTiles[i].MiniTiles[j].GetComponent<Image>().color = colordForTile2;
					}

					LineOfTiles[i].mergeThisTile = true;
					LineOfTiles[i - 1].Number = 0;
					foreach (var mini in LineOfTiles[i - 1].MiniTiles) {
						mini.GetComponent<Image>().enabled = false;
					}
					LineOfTiles[i].PlayMergeAnimation();
					ScoreTracker.Instance.Score += 3;
					if (tempColor == Color.cyan) {
						barBlue.AmountD += 5f;
						CombatTextManager.Instance.CreateText(barBlue.transform.position, "+5", Color.cyan);
						//Debug.Log("cyan");
					}
					else if (tempColor == Color.magenta) {
						barRed.AmountD += 5f;
						CombatTextManager.Instance.CreateText(barRed.transform.position, "+5", Color.magenta);
						//Debug.Log("magenta");
					}
					else if (tempColor == Color.green) {
						barGreen.AmountD += 5f;
						CombatTextManager.Instance.CreateText(barGreen.transform.position, "+5", Color.green);
						//Debug.Log("green");
					}
					else if (tempColor == Color.yellow) {
						barYellow.AmountD += 5f;
						CombatTextManager.Instance.CreateText(barYellow.transform.position, "+5", Color.yellow);
						//Debug.Log("yellow");
					}
					return true;

				}
				else if (LineOfTiles[i].miniTilesCount + LineOfTiles[i - 1].miniTilesCount == 4 && LineOfTiles[i].mergeThisTile == false && LineOfTiles[i - 1].mergeThisTile == false && sameColor) {
					Color tempColor = colordForTile2;
					//Debug.Log(string.Format("<tempColor=#{0:X2}{1:X2}{2:X2}></color>", (byte)(tempColor.r * 255f), (byte)(tempColor.g * 255f), (byte)(tempColor.b * 255f)));

					LineOfTiles[i - 1].Number = 0;

					foreach (var mini in LineOfTiles[i - 1].MiniTiles) {
						mini.GetComponent<Image>().enabled = false;
					}
					LineOfTiles[i].PlayMergeAnimation();
					LineOfTiles[i].Number = 0;
					foreach (var mini in LineOfTiles[i].MiniTiles) {
						mini.GetComponent<Image>().enabled = false;
					}
					ScoreTracker.Instance.Score += 10;
					if (tempColor == Color.cyan) {
						barBlue.AmountD += 10f;
						CombatTextManager.Instance.CreateText(barBlue.transform.position, "+10", Color.cyan);
						//AudioSource.PlayClipAtPoint(scoreClip, barBlue.transform.position);
						scoreSound.Play();
					}
					else if (tempColor == Color.magenta) {
						barRed.AmountD += 10f;
						CombatTextManager.Instance.CreateText(barRed.transform.position, "+10", Color.magenta);
						//AudioSource.PlayClipAtPoint(scoreClip, barRed.transform.position);
						scoreSound.Play();
					}
					else if (tempColor == Color.green) {
						barGreen.AmountD += 10f;
						CombatTextManager.Instance.CreateText(barGreen.transform.position, "+10", Color.green);
						//AudioSource.PlayClipAtPoint(scoreClip, barGreen.transform.position);
						scoreSound.Play();

					}
					else if (tempColor == Color.yellow) {
						barYellow.AmountD += 10f;
						CombatTextManager.Instance.CreateText(barYellow.transform.position, "+10", Color.yellow);
						//AudioSource.PlayClipAtPoint(scoreClip, barYellow.transform.position);
						scoreSound.Play();

					}
					return true;

				}

			}

		}
		return false;
	}
}
