using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class snakeMovement : MonoBehaviour {
	//sets the default movement of the snake to the left
	Vector2 direction = Vector2.left;
	//will be used to reset the snake between lives
	Vector2 startingPosition;
	//used to house all the extensions to the snake
	List<GameObject> tailSections = new List<GameObject>();
	// the border walls
	public Transform borderTop;
	public Transform borderBottom;
	public Transform borderLeft;
	public Transform borderRight;
	//used when creating body of the snake
	public GameObject tailPrefab;
	//get the Lives Textbox
	public Text Lives;
	//get the Score Textbox
	public Text Score;
	//used to determine if the snake should grow or not
	bool ateFood = false;
	//the speed that the snake moves at
	float speed = 0.1f;
	void Start () {
		//grabs the starting position, which is used when the game resets
		startingPosition = transform.position;
		//constantly calls move with the predetermined speed and distance
		InvokeRepeating ("Move", 0.1f, speed);
	}
	
	// Update is called once per frame
	void Update () {
		//if the right arrow is pressed, the direction the snake moves in will be set to right
		if (Input.GetKey(KeyCode.RightArrow))
			direction = Vector2.right;
		//if the right arrow is pressed, the direction the snake moves in will be set to down
		else if (Input.GetKey(KeyCode.DownArrow))
			direction = -Vector2.up;   
		//if the right arrow is pressed, the direction the snake moves in will be set to left
		else if (Input.GetKey(KeyCode.LeftArrow))
			direction = -Vector2.right; 
		//if the right arrow is pressed, the direction the snake moves in will be set to up
		else if (Input.GetKey(KeyCode.UpArrow))
			direction = Vector2.up;
		
	}
	void Move(){
		//gets the current position of the snake, which the new (or last) tail element will go
		Vector2 tailPosition = transform.position;
		//if food was eaten
		if (ateFood) {
			//create a tailPart GameObject at the current head position
			GameObject tailPart = GameObject.Instantiate (tailPrefab, tailPosition, Quaternion.identity) as GameObject;
			//attach the new tailPart to the List of tailSections
			tailSections.Insert (0, tailPart);
			//reset the ateFood flag
			ateFood = false;
		}
		//if there are tailParts
		 if (tailSections.Count > 0) {
			//make the last tailPart move to the current head's position
			tailSections.Last().transform.position = tailPosition;
			tailSections.Insert(0, tailSections.Last());
			//remove the last tailPart, as it has just moved to the front
			tailSections.RemoveAt(tailSections.Count-1);
		}
		//move in the desired location set by direction
		transform.Translate (direction);
	}
	void OnTriggerEnter2D(Collider2D coll){
		//integer to hold the current score
		int currentScore = 0;
		//integer to hold the current lives
		int lives = 0;
		//holds coordinates for random food placement
		int xPos = 0;
		int yPos = 0;
		//if the head of the snake touched food
		if (coll.name.StartsWith ("foodPrefab")) {
			//set the ateFood flag
			ateFood = true;
			//get the integer score value from the text box
			currentScore = int.Parse (Score.text);
			//increment the score by 1
			currentScore += 1;
			//set the Score text to the new score
			Score.text = currentScore.ToString ();
			//get random x and y positions inside the boundaries of the map, the +2/-2 is to avoid food appearing at the edge of the map
			xPos = (int)Random.Range (borderLeft.position.x+2, borderRight.position.x-2);
			yPos = (int)Random.Range (borderTop.position.y-2, borderBottom.position.y+2);
			//move the food prefab to the new coordinates
			GameObject.Find ("foodPrefab(Clone)").transform.position = new Vector2 (xPos, yPos);
		}
		//if the head of the snake touches a border or its own tail
		if (coll.name.StartsWith ("border") || coll.name.StartsWith("tail")) {
			//get the lives value from the text bot
			lives = int.Parse (Lives.text);
			//decrement the lives by 1
			lives -= 1;
			//if the player has run out of lives
			if (lives <= 0) {
				//reset the application
				Application.LoadLevel (0);
			} 
			//set the textbox to the new number of lives
			Lives.text = lives.ToString ();
			//go through each tail object and destroy each one (the player always starts back out with a length of 1
			foreach (GameObject tail in tailSections) {
				Destroy (tail);
			}
			//clear the list
			tailSections.Clear ();
			//set the player's starting position to the position from the start of the game.
			this.transform.position = startingPosition;


		}

	}
}
