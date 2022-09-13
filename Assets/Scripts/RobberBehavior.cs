using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehavior : MonoBehaviour
{
    BehaviourTree tree;
    public GameObject dimond;
    public GameObject frontDoor;
    public GameObject backDoor;
    public GameObject van;
    NavMeshAgent agent;

    Node.Status treeStatus = Node.Status.RUNNING;

    public enum ActionState { IDLE, WORKING};
    ActionState state = ActionState.IDLE;

    [Range(0, 1000)]
    public int money = 800;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        tree = new BehaviourTree();
        Sequence steal = new Sequence("Steal Something");
        Leaf goToDimond = new Leaf("Go to Dimond", GoToDimond);
        Leaf hasGotMoney = new Leaf("HasGotMoney", HasMoney);
        Leaf goToBackDoor = new Leaf("Go to Back Door", GoToBackDoor);
        Leaf goToFrontDoor = new Leaf("Go to Front Door", GoToFrontDoor);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        Selector openDoor = new Selector("OpenDoor");

       
        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);

        steal.AddChild(hasGotMoney);
        steal.AddChild(openDoor);
        steal.AddChild(goToDimond);
        //steal.AddChild(openDoor);
        steal.AddChild(goToVan);

        tree.AddChild(steal);

        // tree.PrintTree();
        


    }

    private void Update()
    {
        //Kepp on running the tree
        if(treeStatus != Node.Status.SUCCESS)
        {
            treeStatus= tree.Process();
        }
       
    }

    public Node.Status HasMoney()
    {
        if (money < 500)
        {
            return Node.Status.SUCCESS;
        }
        else return Node.Status.FAILURE;
    }

    public Node.Status GoToDimond()
    {
        
        Node.Status s = GoToLocation(dimond.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            dimond.transform.parent = this.gameObject.transform; //Attach dimond to robber
            
        }
         return s;

    }
    public Node.Status GoToBackDoor()
    {
        return GoToDoor(backDoor);

    }

    public Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontDoor);

    }

    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            if (!door.GetComponent<DoorLocking>().isDoorLocked)
            {
                door.SetActive(false);
                return Node.Status.SUCCESS;
            }
            return Node.Status.FAILURE;
        }
        else return s;
    }
    public Node.Status GoToVan()
    {

        Node.Status s = GoToLocation(van.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            money += 300;
            dimond.SetActive(false);
           

        }
        return s;

    }
    
    Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }

        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }


   
}
