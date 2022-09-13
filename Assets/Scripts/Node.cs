using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public string name;
    public int currentChild = 0;
  public enum Status
    {
        RUNNING,
        SUCCESS,
        FAILURE
    };

    public Node()
    {

    }
   public  Node(string n)
    {
        name = n;

    }
    public Status status;
    public List<Node> children = new List<Node>();

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }
    public void AddChild(Node node)
    {
        children.Add(node);

    }

}
