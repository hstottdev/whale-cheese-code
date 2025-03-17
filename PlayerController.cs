using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Hi, this script is from my game Whale Cheese.

    //In this game, you can only move left, right, up, and down.
    //The script refers to an "action queueing system" which queues 
    //actions and then plays them in a modular fashion. 

    public static PlayerController inst;
    [Header("Position")]
    Vector3 targetPosition;
    [SerializeField] float moveAmount;
    [SerializeField] float updatePositionSpeed;
    [SerializeField] LayerMask wallLayers;

    [Header("Rotation")]
    Quaternion targetRotation;
    [SerializeField] float updateRotationSpeed;
    Dictionary<Vector3, float> canMoveInDirection = new Dictionary<Vector3, float>();

    Dictionary<string, Vector3> directionFromAction = new Dictionary<string, Vector3>();

    Dictionary<string, Quaternion> angleFromAction =  new Dictionary<string, Quaternion>();
    bool inWall;
    [SerializeField] GameObject debugSphere;
    [SerializeField] bool showDebugSphere;

    // Start is called before the first frame update
    void Start()
    {
        inst = this;
        targetPosition = transform.position;
        FillDictionaries();
    }

    void FillDictionaries()
    {
        string[] actions = {"up","down","left","right"};
        Vector3[] directions = {Vector3.up,Vector3.down,Vector3.left,Vector3.right};
        for(int i = 0; i < actions.Length; i++)
        {
            directionFromAction.Add(actions[i],directions[i]);
            canMoveInDirection.Add(directions[i],moveAmount);

            float yAngle = directions[i].x == 0? 180: -directions[i].x*90; 
            Quaternion newAngle = Quaternion.Euler(directions[i].y*90,yAngle,0);
            angleFromAction.Add(actions[i],newAngle);
        }
    }
    // Update is called once per frame
    void Update()
    {
        WallCheck();
        Inputs();
    }

    void WallCheck()
    {
        bool upHit = Physics.Raycast(targetPosition, Vector3.up, moveAmount, wallLayers);
        bool downHit = Physics.Raycast(targetPosition, Vector3.down, moveAmount, wallLayers);
        bool rightHit = Physics.Raycast(targetPosition, Vector3.right, moveAmount, wallLayers);
        bool leftHit = Physics.Raycast(targetPosition, Vector3.left, moveAmount, wallLayers);

        Debug.DrawRay(targetPosition, -transform.right * moveAmount, Color.red);

        canMoveInDirection[Vector3.up] = upHit     ? 0: moveAmount;
        canMoveInDirection[Vector3.left] = leftHit   ? 0: moveAmount;
        canMoveInDirection[Vector3.down] = downHit   ? 0: moveAmount;
        canMoveInDirection[Vector3.right] = rightHit  ? 0: moveAmount;

        //Debug.Log("left hit:" + leftHit);
    }

    void Inputs()
    {
        // if (Input.GetKeyDown("w"))
        // {
        //     ActionManager.QueueAction("up");
        // }
        // if (Input.GetKeyDown("a"))
        // {
        //     ActionManager.QueueAction("left");
        // }
        // if (Input.GetKeyDown ("s"))
        // {
        //     ActionManager.QueueAction("down");
        // }
        // if (Input.GetKeyDown("d"))
        // {
        //     ActionManager.QueueAction("right");
        // }
    }

    bool Move(Vector3 direction, Quaternion newAngle)
    {
        targetPosition += direction * canMoveInDirection[direction];
        targetRotation = newAngle;
        DebugSphere();

        return canMoveInDirection[direction] != 0;
    }

    bool Move(string action)
    {
        Vector3 direction = directionFromAction[action];
        Quaternion newAngle = angleFromAction[action];
        bool moved = Move(direction, newAngle);
        AudioManager.PlaySound("jump pixel",Random.Range(1.8f,2.2f),2);
        ActionManager.SetActionPlayed(action,moved);

        return moved;
    }

    void DebugSphere()
    {
        if (showDebugSphere) debugSphere.transform.position = targetPosition;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * updatePositionSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation, Time.deltaTime * updateRotationSpeed);

        ActionCheck(); 
    }

    void ActionCheck()
    {
        if (ActionManager.AwatingAction("up"))
        {
            Move("up");
            //return is used for each action to ensure multiple actions can't happen in the same frame
            return;
        }
        if (ActionManager.AwatingAction("left"))
        {
            Move("left");         
            //return is used for each action to ensure multiple actions can't happen in the same frame
            return;
        }
        if (ActionManager.AwatingAction("down"))
        {
            Move("down");    
            //return is used for each action to ensure multiple actions can't happen in the same frame
            return;
        }
        if (ActionManager.AwatingAction("right"))
        {
            Move("right");       
            //return is used for each action to ensure multiple actions can't happen in the same frame
            return;
        }

        if(ActionManager.AwatingAction("undo"))
        {
            UndoLastMove();
        }

    }
    void UndoLastMove()
    {
        //this does not actually undo the movement,
        //just the admin of refunding the available moves and updating ui etc 

        //did you actually move, or was it into a wall or something
        bool moved = ActionManager.DidJustMove();
        Action previousAction = ActionManager.Undo();

        //if there are no movements to undo then we are done, just skip the rest
        if(previousAction == null) return;

        string previousActionID = previousAction.actionID;
        
        //un move
        if(directionFromAction.ContainsKey(previousActionID))
        {
            Vector3 undoDirection = -directionFromAction[previousActionID];
            Debug.Log($"unmoving previous action: {previousActionID} in direction: {undoDirection}");
            if(moved) Move(undoDirection,targetRotation);

            Quaternion oldAngle = angleFromAction["right"];
            Action evenEarlierAction = ActionManager.GetPreviousAction();
            if(evenEarlierAction != null)
            {
                oldAngle = angleFromAction[evenEarlierAction.actionID]; 
            }
            targetRotation = oldAngle;
        }

        AudioManager.PlaySound("click1",Random.Range(0.4f,0.6f),1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "food")
        {
            Destroy(other.gameObject);
            LevelManager.inst.ShowWinScreen();
        }

        if(other.tag == "portal")
        {
            Transform otherPortal = other.transform;
            //find another portal
            foreach(GameObject g in GameObject.FindGameObjectsWithTag("portal"))
            {
                if(g.transform != other.transform)
                {
                    otherPortal = g.transform;
                }
            }
            targetPosition = new Vector3(otherPortal.transform.position.x,otherPortal.transform.position.y-1,targetPosition.z);
        }
    }
}
