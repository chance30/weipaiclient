using UnityEngine;
using System.Collections;

public class UI_Rotation : MonoBehaviour {

    public float spped;
    Transform m_myTra;
	// Use this for initialization
	void Start () {
        m_myTra = transform;

    }
	
	// Update is called once per frame
	void Update () {
        m_myTra.Rotate(Vector3.forward, spped * Time.deltaTime);

    }
}
