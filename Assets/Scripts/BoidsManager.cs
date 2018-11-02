using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Movement;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoidsManager : MonoBehaviour
{
    public const float X_WORLD_SIZE = 65.0f;
    public const float Z_WORLD_SIZE = 32.5f;
    public const float AVOID_MARGIN = 5.0f;
    public const float MAX_SPEED = 50.0f;
    public const float MAX_ACCELERATION = 40.0f;
    public const float DRAG = 0.1f;

    private List<DynamicCharacter> Characters { get; set; }
    private Camera CameraComponent { get; set; }

    // CLICK EVENT
    public delegate void ClickAction(Vector3 clickPosition);
    public static event ClickAction OnMouseClicked;

    // Use this for initialization
    void Start () 
	{
        var redObj = GameObject.Find ("Red");

        var camera = GameObject.Find("Main Camera");
        this.CameraComponent = camera.GetComponent<Camera>();

        var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

	    this.Characters = this.CloneCharacters(redObj, 100, obstacles);
	       
	    foreach (var character in this.Characters)
	    {
	        this.InitializeCharacter(character);
	    }
	}

    private void InitializeCharacter(DynamicCharacter character)
    {

        var blended = new BlendedMovement
        {
            Character = character.KinematicData,
            MovementDebugColor = Color.black
        };


        var priority = new PriorityMovement
        {
            Character = character.KinematicData
        };

        // VELOCITY MATCHING
        var flockVelocityMatching = new FlockVelocityMatching()
        {
            Character = character.KinematicData,
            Flock = this.getCharactersKinematicData(this.Characters),
            MaxAcceleration = MAX_ACCELERATION,
            MovementDebugColor = Color.green
        };

        // SEPARATION
        var flockSeparation = new FlockSeparation()
        {
            Character = character.KinematicData,
            Flock = this.getCharactersKinematicData(this.Characters),
            MaxAcceleration = MAX_ACCELERATION,
            MovementDebugColor = Color.grey
        };

        // COHESION
        var flockCohesion = new FlockCohesion()
        {
            Character = character.KinematicData,
            Flock = this.getCharactersKinematicData(this.Characters),
            MaxAcceleration = MAX_ACCELERATION,
            MovementDebugColor = Color.cyan
        };

        // OBSTACLE AVOIDANCE
        var obstacleAvoidMovement = new DynamicObstacleAvoid()
        {
            MaxAcceleration = MAX_ACCELERATION / 2,
            Character = character.KinematicData,
            MovementDebugColor = Color.magenta
        };

        // FLEE
        var flockFlee = new FlockFlee()
        {
            Character = character.KinematicData,
            Flock = this.getCharactersKinematicData(this.Characters),
            Target = new KinematicData(),
            MaxAcceleration = MAX_ACCELERATION * 3.0f,
            MovementDebugColor = Color.green
        };

        //TODO: ADD THE 3 MOVEMENTS
        blended.Movements.Add(new MovementWithWeight(flockSeparation, 1.5f));
        blended.Movements.Add(new MovementWithWeight(flockCohesion, 1.7f));
        blended.Movements.Add(new MovementWithWeight(flockVelocityMatching, 2.0f));
        blended.Movements.Add(new MovementWithWeight(flockFlee, 2.0f));

        priority.Movements.Add(obstacleAvoidMovement);
        priority.Movements.Add(blended);


        character.Movement = priority;
    }

    private List<DynamicCharacter> CloneCharacters(GameObject objectToClone, int numberOfCharacters, GameObject[] obstacles)
    {
        var characters = new List<DynamicCharacter>();
        for (int i = 0; i < numberOfCharacters; i++)
        {
            var clone = GameObject.Instantiate(objectToClone);
            clone.transform.position = this.GenerateRandomClearPosition(obstacles);
            var character = new DynamicCharacter(clone)
            {
                MaxSpeed = MAX_SPEED,
                Drag = DRAG
            };
            character.KinematicData.orientation = Random.Range(0, 360);
            characters.Add(character);
        }

        return characters;
    }

    private Vector3 GenerateRandomClearPosition(GameObject[] obstacles)
    {
        Vector3 position = new Vector3();
        var ok = false;
        while (!ok)
        {
            ok = true;

            position = new Vector3(Random.Range(-X_WORLD_SIZE,X_WORLD_SIZE), 0, Random.Range(-Z_WORLD_SIZE,Z_WORLD_SIZE));

            foreach (var obstacle in obstacles)
            {
                var distance = (position - obstacle.transform.position).magnitude;

                //assuming obstacle is a sphere just to simplify the point selection
                if (distance < obstacle.transform.localScale.x + AVOID_MARGIN)
                {
                    ok = false;
                    break;
                }
            }
        }

        return position;
    }

	void Update()
	{
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = this.CameraComponent.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, this.CameraComponent.transform.position.y));
            clickPosition.y = 0;
            if (OnMouseClicked != null)
                OnMouseClicked(clickPosition);
   
        }
        foreach (var character in this.Characters)
	    {
	        this.UpdateMovingGameObject(character);
	    }

        
	}

    private void UpdateMovingGameObject(DynamicCharacter movingCharacter)
    {
        if (movingCharacter.Movement != null)
        {
            movingCharacter.Update();
            movingCharacter.KinematicData.ApplyWorldLimit(X_WORLD_SIZE,Z_WORLD_SIZE);
            movingCharacter.GameObject.transform.position = movingCharacter.Movement.Character.position;
        }
    }

    private List<KinematicData> getCharactersKinematicData (List<DynamicCharacter> characters) {
        var kinematicData = new List<KinematicData>();
        foreach (var character in characters)
            kinematicData.Add(character.KinematicData);
        return kinematicData;
    }
    


    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = Time.deltaTime * 1000.0f;
        float fps = 1.0f / Time.deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
