using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrangeSprites : MonoBehaviour {

    List<SpriteRenderer> allSprites;

	void Start ()
    {
        allSprites = new List<SpriteRenderer>();
        foreach (SpriteRenderer sr in GameObject.FindObjectsOfType<SpriteRenderer>())
        {
            if (!sr.CompareTag("Floor"))
            {
                allSprites.Add(sr);
            }
            else if (sr.CompareTag("Floor"))
            {
                sr.sortingOrder = -10000;
            }
        }
	}

	void Update () 
    {
        foreach (SpriteRenderer sr in allSprites)
        {
            sr.sortingOrder = Mathf.RoundToInt(-sr.transform.parent.position.y * 100);
        }
	}
}
