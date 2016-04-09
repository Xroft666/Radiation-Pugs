using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class DepthOrder : MonoBehaviour {

	private SpriteRenderer cachedSpriteRenderer;
	public float Size = 0.4f; 

	// Use this for initialization
	void Start () {
		cachedSpriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		cachedSpriteRenderer.sortingOrder = -Mathf.RoundToInt((transform.position.y - Size/2) * 100);
	}
}
