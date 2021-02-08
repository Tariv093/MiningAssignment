using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Resource
{
    FULL_RESOURCE,
    HALF_RESOURCE,
    QUARTER_RESOURCE,
    NO_RESOURCE
}

public class TileComponent : MonoBehaviour
{
    public int row, col;
    public Image img;
    private Button button;
    private System.Random rand = new System.Random();
    private Game gamescript;
    public int num;
    public Resource rType;
   

    // Start is called before the first frame update
    void Start()
    {
        rType = Resource.NO_RESOURCE;
        gamescript = transform.parent.GetComponent<Game>();
        

        button = GetComponentInChildren<Button>(); 
       



    }

    public void OnClicked()
    {
        Debug.Log("Scanned");
       // img.enabled = false;
        gamescript.Interaction(this);
     //   img.color = Color.clear;
    }
    public void SetGridIndices(int r, int c)
    {
        row = r;
        col = c;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if(rType == Resource.FULL_RESOURCE)
        {
            //SetRType(Resource.FULL_RESOURCE);
            button.image.color = Color.red;
            
        }
        if(rType == Resource.HALF_RESOURCE)
        {
            button.image.color = Color.yellow;
        }
        if(rType == Resource.QUARTER_RESOURCE)
        {
            button.image.color = Color.green;

        }
        if(rType == Resource.NO_RESOURCE)
        {
            button.image.color = Color.white;
        }
    }
    public void SetNum(int n)
    {
        num = n; 
        if (n == 20)Debug.Log("full");
    }
    public void SetRType(Resource newrtype)
    {
        rType = newrtype;
    }
    public Resource GetRType()
    {
        return rType;
    }
}
