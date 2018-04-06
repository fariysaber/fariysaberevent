using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class testPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        RenderTexture dd = new RenderTexture(256,256,100);
        GameObject ss = GameObject.Find("Camera (1)");
        ss.GetComponent<Camera>().targetTexture = dd;
        GameObject qq = GameObject.Find("Image (2)");
        qq.GetComponent<RawImage>().texture = dd;
	}
	
	// Update is called once per frame
	void Update () {
       
	}
    void OnGUI()
    {
        if (GUI.Button(new Rect(25, 25, 300, 300), "PlayAnim1"))
        {
            gameObject.GetComponent<UISpriteAnim_play>().PlayAnim(1, false, false, true, 0, 2);
        }
    }
}
