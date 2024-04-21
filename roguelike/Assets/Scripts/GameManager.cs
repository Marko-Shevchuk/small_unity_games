using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Roguelike
{
	using System.Collections.Generic;		
	using UnityEngine.UI;					
	
	public class GameManager : MonoBehaviour
	{
		public float levelStartDelay = 2f;						
		public float turnDelay = 0.1f;							//todo fix
		public int playerFoodPoints = 100;						
		public static GameManager instance = null;				
		[HideInInspector] public bool playersTurn = true;		
		
		
		private Text levelText;									
		private GameObject levelImage;							
		private BoardManager boardScript;						
		private int level = 1;									
		private List<Enemy> enemies;							
		private bool enemiesMoving;								
		private bool doingSetup = true;							
		
		
		void Awake()
		{
            if (instance == null)
                instance = this;

            else if (instance != this)
                Destroy(gameObject);	

			DontDestroyOnLoad(gameObject);
			
			enemies = new List<Enemy>();
			
			boardScript = GetComponent<BoardManager>();

			InitGame();
		}

        //this is called only ONCE, and the parameter tells it to be called only after the scene was loaded

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //each time a scene is loaded.
        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            instance.level++;
            instance.InitGame();
        }

		void InitGame()
		{
			//prevent player from moving while title card is up
			doingSetup = true;
			levelImage = GameObject.Find("LevelImage");
			levelText = GameObject.Find("LevelText").GetComponent<Text>();
			levelText.text = "Room " + level;

			levelImage.SetActive(true);
			
			Invoke("HideLevelImage", levelStartDelay);
			
			//prepare for next level.
			enemies.Clear();

			boardScript.SetupScene(level);
			
		}
		
		
		void HideLevelImage()
		{
			levelImage.SetActive(false);
			doingSetup = false;
		}
		
		void Update()
		{
			if(enemiesMoving || doingSetup) //todo fix
				return;
			
			StartCoroutine (MoveEnemies ());
		}
		
		public void AddEnemyToList(Enemy script)
		{
			enemies.Add(script);
		}
		
		public void GameOver()
		{
			levelText.text = "After " + level + " rooms, you died.";
			levelImage.SetActive(true);
			enabled = false;
		}
		

		IEnumerator MoveEnemies()
		{
			enemiesMoving = true;

			yield return new WaitForSeconds(turnDelay);

			if (enemies.Count == 0) 
			{
				yield return new WaitForSeconds(turnDelay);
			}

			for (int i = 0; i < enemies.Count; i++)
			{
				enemies[i].MoveEnemy ();
				
				//Wait for Enemy's moveTime before moving next Enemy?
				yield return new WaitForSeconds(enemies[i].moveTime);
			}
			playersTurn = true;
			enemiesMoving = false;
		}
	}
}

