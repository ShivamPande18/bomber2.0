using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdScroller : MonoBehaviour {
    [ SerializeField ]
    private GameObject          ItemPrefab;
    [ HideInInspector ]
	public  List< GameObject >	ItemList    = new List<GameObject> ();
	public  Transform			PageRootT;

    // Start is called before the first frame update
    void Start () {

    }

    public void CreateAdSpot (int nItems, Action callBack) {
        for (int i = 0; i < nItems; i++) {
            Transform t = Instantiate (ItemPrefab).transform;
            t.parent = PageRootT;
            t.localPosition = Vector3.zero;
            t.localScale = Vector3.one;
            ItemList.Add( t.gameObject );
        }

        if( callBack != null ) callBack.Invoke();
    }
}