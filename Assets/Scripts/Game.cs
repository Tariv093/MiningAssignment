using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Game : MonoBehaviour
{
    private System.Random rand = new System.Random();
    public GameObject prefab;
    [SerializeField]
    public GameObject[] tiles;
    public int rowColNum;
    public int scans, drills, score;
    public GameObject[,] grid;
    public bool gameStart;
    public Button enDrill;
    bool sp, tp;
    public Text text;
    
    [SerializeField]
    bool drilling;

    // Start is called before the first frame update
    void Start()
    {
        gameStart = false;
        grid = new GameObject[rowColNum, rowColNum];
        tiles = new GameObject[(rowColNum * rowColNum)];

        for (int i = 0; i < tiles.Length; i++)
        {
            GameObject temp = Instantiate(prefab, transform);
            tiles[i] = temp;
        }


    }
    void SecondPass()
    {
        Debug.Log("secondpass");
        for (int r = 0; r < rowColNum; r++)
        {
            Debug.Log("checking length");
            for (int c = 0; c < rowColNum; c++)
            {
                //  Debug.Log("checking column");
                // r -2 , c + 2 , r + 2, c - 2 
                // Debug.Log(grid[r, c]);
                if (grid[r, c].GetComponent<TileComponent>().GetRType() == Resource.FULL_RESOURCE)
                {
                    //  Debug.Log("finding resource");
                    for (int r2 = r - 1; r2 < r + 2; r2++)
                    {
                        for (int c2 = c - 1; c2 < c + 2; c2++)
                        {
                            if (r2 >= 0 && c2 >= 0 && r2 < rowColNum && c2 < rowColNum)
                            { // check to see if tile is valid

                                if (grid[r2, c2].GetComponent<TileComponent>().GetRType() != Resource.FULL_RESOURCE)
                                {
                                    grid[r2, c2].GetComponent<TileComponent>().SetRType(Resource.HALF_RESOURCE);
                                    //Debug.Log("allocating new resource");
                                }

                            }
                            else
                                continue;

                        }
                        //Debug.Log("half");
                    }
                }

            }

        }
        tp = true;
    }

    void ThirdPass()
    {
        for (int r = 0; r < rowColNum; r++)
        {
            //Debug.Log("checking length");
            for (int c = 0; c < rowColNum; c++)
            {
                //  Debug.Log("checking column");
                // r -2 , c + 2 , r + 2, c - 2 
                // Debug.Log(grid[r, c]);
                if (grid[r, c].GetComponent<TileComponent>().GetRType() == Resource.FULL_RESOURCE)
                {
                    //Debug.Log("finding resource");
                    for (int r2 = r - 2; r2 < r + 3; r2++)
                    {
                        for (int c2 = c - 2; c2 < c + 3; c2++)
                        {
                            if (r2 >= 0 && c2 >= 0 && r2 < rowColNum && c2 < rowColNum)
                            { // check to see if tile is valid

                                if (grid[r2, c2].GetComponent<TileComponent>().GetRType() != Resource.FULL_RESOURCE &&
                                    grid[r2, c2].GetComponent<TileComponent>().GetRType() != Resource.HALF_RESOURCE)
                                {
                                    grid[r2, c2].GetComponent<TileComponent>().SetRType(Resource.QUARTER_RESOURCE);
                                    // Debug.Log("allocating new resource");
                                }

                            }
                            else
                                continue;

                        }
                        // Debug.Log("Quarters");
                    }
                }

            }

        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameStart == true)
        {
            gameStart = false;
            FirstPass();
            sp = true;
        }
        if (sp == true)
        {
            SecondPass();
            sp = false;
        }
        if (tp == true)
        {
            ThirdPass();
            tp = false;
        }
        text.text = " Scans Left: " + scans + "\n Drills Left: " + drills + "\n Current Score: " + score;
    }

    private void FirstPass()
    {

        for (int r = 0; r < rowColNum; r++)
        {
            for (int c = 0; c < rowColNum; c++)
            {

                grid[r, c] = tiles[(r * rowColNum) + c];
                grid[r, c].GetComponent<TileComponent>().SetGridIndices(r, c);
                grid[r, c].GetComponent<RectTransform>().localPosition = new Vector3((c - 1) * 30 + 5, (r - 1) * 30 + 5, 0);
                grid[r, c].GetComponent<TileComponent>().SetNum(UnityEngine.Random.Range(1, 41));
                if (grid[r, c].GetComponent<TileComponent>().num == 40)
                {
                    grid[r, c].GetComponent<TileComponent>().SetRType(Resource.FULL_RESOURCE);
                }

            }
        }
        //throw new NotImplementedException();
    }

    public void GameStart()
    {
        Reset();
        gameStart = true;
    }

    private void Reset()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetComponent<TileComponent>().SetRType(Resource.NO_RESOURCE);
            tiles[i].GetComponent<TileComponent>().img.enabled = true;
            tiles[i].GetComponent<TileComponent>().SetNum(0);
            scans = 3;
            drills = 3;
            score = 0;
        }
    }

    public void Interaction(TileComponent clickedTile)
    {
        Debug.Log("Interacted");
        if (scans > 0 && drilling == false)
        {
            Debug.Log("scan start");
            for (int r = clickedTile.row - 2; r < clickedTile.row + 3; r++)
            {
                for (int c = clickedTile.col - 2; c < clickedTile.col + 3; c++)
                {
                    if (r >= 0 && c >= 0 && r < rowColNum && c < rowColNum)
                    {
                        grid[r, c].GetComponent<TileComponent>().img.enabled = false;
                    }
                }
            }
            scans--;
            //do scanning 
        }
        
        if (drills > 0 && drilling == true)
        {
            for (int r = clickedTile.row - 2; r < clickedTile.row + 3; r++)
            {
                for (int c = clickedTile.col - 2; c < clickedTile.col + 3; c++)
                {
                    if (r >= 0 && c >= 0 && r < rowColNum && c < rowColNum)
                    {
                        switch (grid[r, c].GetComponent<TileComponent>().GetRType())
                        {
                            case Resource.FULL_RESOURCE:
                                score += 100;
                                break;
                            case Resource.HALF_RESOURCE:
                                score += 50;
                                break;
                            case Resource.QUARTER_RESOURCE:
                                score += 25;
                                break;
                            case Resource.NO_RESOURCE:
                                break;
                        }
                        grid[r, c].GetComponent<TileComponent>().img.enabled = false;
                        grid[r, c].GetComponent<TileComponent>().SetRType(Resource.NO_RESOURCE);
                    }
                }
            }
            //do drilling
            drills--;
        }
    }

   public void EnableDrill()
    {
        drilling = !drilling;
        if (drilling) enDrill.image.color = Color.black;
        else enDrill.image.color = Color.white;
        Debug.Log("button push");
    }
}
