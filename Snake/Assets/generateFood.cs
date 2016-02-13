using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class generateFood : MonoBehaviour {
	//the walls
	public Transform borderLeft;
	public Transform borderRight;
	public Transform borderTop;
	public Transform borderBottom;
	//the snake head object
	public Transform snakeHead;
	//the food prefab object
	public GameObject foodPrefab;
	//used to get random x and y coordinates
	int xPos;
	int yPos;

	void Start () {
		//spawns the initial piece of food on the map.
		Spawn ();
	}

	// Update is called once per frame
	void Update () {
			
	}
	void Spawn(){
		
		//gets a random x coordinate within the boundaries of the top and bottom wall, with a 2 coordinate buffer
		//so that food doesn't appear exactly at the wall
		xPos = (int)Random.Range (borderLeft.position.x+2, borderRight.position.x-2);
		//gets a random y coordinate within the boundaries of the left and right wall, with a 2 coordinate buffer
		//so that food doesn't appear exactly at the wall
		yPos = (int)Random.Range (borderTop.position.y-2, borderBottom.position.y+2);
		//creates a food object at the random coordinates assigned above
		Instantiate (foodPrefab, new Vector2 (xPos, yPos), Quaternion.identity);

	}
}
