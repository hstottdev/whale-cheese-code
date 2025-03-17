using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ActionManager : MonoBehaviour
{
    public int actionsRemaining;
    [SerializeField] TextMeshProUGUI actionRemainText;
    public static ActionManager inst;
    public List<Action> actionHistory = new List<Action>();
    public List<bool> moveHistory = new List<bool>();
    
    public static List<Action> playActions = new List<Action>();

    [Header("Action/Moves History")]
    [SerializeField] GameObject actionPrefab;
    [SerializeField] Transform actionSlotParent;
    [HideInInspector] public List<ActionSlotUI> actionSlots = new List<ActionSlotUI>();

    [SerializeField] List<Action> allActions;
    public Dictionary<string, Action> allActionsDict = new Dictionary<string, Action>();
    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    } 

    void Start()
    {
        FillActionDict();
        SpawnActionSlots();
        UpdateActionText();
    }

    void FillActionDict()
    {
        foreach(Action action  in allActions)
        {
            allActionsDict.Add(action.actionID, action);
        }
    }

    public void UpdateActionText()
    {
        actionRemainText.text = $"Remaining: {actionsRemaining}";
    } 


    public static Action GetActionFromId(string id)
    {
        return inst.allActionsDict[id];
    }

    void SpawnActionSlots()
    {
        //destroy all exisiting children first
        for(int c = 0; c < actionSlotParent.childCount; c++)
        {
            Destroy(actionSlotParent.GetChild(c).gameObject);
        }
        //spawn new ones
        for(int i = 0; i < actionsRemaining; i++)
        {
            GameObject spawnedOb = Instantiate(actionPrefab, actionSlotParent);
            actionSlots.Add(spawnedOb.GetComponent<ActionSlotUI>());
        }
    }

    public static void QueueAction(Action action)
    {
        if(inst.actionsRemaining > 0)
        {
            playActions.Add(action);
            inst.actionsRemaining -= GetActionCost(action);
            inst.UpdateActionText();
        }
    }

    public static void QueueAction(string actionId)
    {
        Action actionHistory = GetActionFromId(actionId);
        QueueAction(actionHistory);
    }

    public static int GetActionCost(Action action)
    {
        if(action.actionID == "undo")
        {
            Debug.Log($"{action.actionID} is costing 0");
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public static bool AwatingAction(string actionId)
    {
        Action action = GetActionFromId(actionId);
        return playActions.Contains(action);
    }

    public static void SetActionPlayed(string actionId, bool moved)
    {
        Action action = GetActionFromId(actionId);

        inst.actionHistory.Add(action);
        inst.moveHistory.Add(moved);
        playActions.Remove(action);

        foreach(ActionSlotUI actionSlot in inst.actionSlots)
        {
            //await an empty action slot
            if(actionSlot.currentAction == null)
            {
                actionSlot.UpdateCurrentAction(action);
                break;
            }
        }
    }

    public static Action Undo()
    {
        //remove undo from action queue
        playActions.Remove(GetActionFromId("undo"));

        if(inst.actionHistory.Count > 0)
        {
            //get previous action and index for the most recent ui slot
            Action previousAction = GetPreviousAction();
            int actionHistoryIndex = inst.actionHistory.Count-1;
            //remove from action move history
            inst.actionHistory.RemoveAt(actionHistoryIndex);
            inst.moveHistory.RemoveAt(actionHistoryIndex);
            //refund 1 action
            inst.actionsRemaining += 1;
            inst.UpdateActionText();

            //update action UI
            ActionSlotUI previousActionSlot = inst.actionSlots[actionHistoryIndex];
            previousActionSlot.UpdateCurrentAction(null);

            return previousAction;
        }
        return null;
    }

    public static Action GetPreviousAction()
    {
        if(inst.actionHistory.Count == 0) return null;
        return inst.actionHistory.Last();
    }

    public static bool DidJustMove()
    {
        if(inst.moveHistory.Count == 0) return false;
        return inst.moveHistory.Last();
    }
}
