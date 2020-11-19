using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;


// When moving the TreeSpawner around the scene, change its position and rotation
public class TransformInfo
{
    public Vector3 position;
    public Quaternion rotation;
}

public class LSystemScript : MonoBehaviour
{
    [SerializeField] public int iterations;
    [SerializeField] public GameObject Branch;
    [SerializeField] public float length;
    [SerializeField] public float angle;

    // Starting point of the tree, will always begin with a rule
    private const string axiom = "X";

    // Grammar: take a string, perform operation based on each character of the string
    // Repeat the sequence for each iteration
    private Stack<TransformInfo> transformStack;
    private Dictionary<char, string> rules;
    private string currentString = string.Empty;

    void Start()
    {
        transformStack = new Stack<TransformInfo>();

        rules = new Dictionary<char, string>
        {
            {'X', "F[+X][-X]FX" },
            {'F', "FF" }
        };

        Spawn();
    }

    /*
     All of the information needed to spawn the tree 
    */
    private void Spawn()
    {
        // Current starting point rule
        currentString = axiom;

        StringBuilder sb = new StringBuilder();
        Debug.Log("Before the 1st iteration sb: " + sb + "\n");

        for(int i = 0; i < iterations; i++)
        {
            foreach (char c in currentString)
            {
                // If the character being read is a key in the dict, add the value from that key to the string builder
                // If it doesn't, add the character itself to the string builder
                // NOTE: Use ternirary operator
                Debug.Log("The current character being read is: " + c + "\n");

                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());

                Debug.Log("(String after iteration #" + i + " is:" + sb + "\n");

                /*
                if (rules.ContainsKey(c))
                {
                    sb.Append(rules[c]);
                    Debug.Log("Character is a key in the rule set and has been added: \n");
                }
                else
                {
                    sb.Append(c.ToString());
                    Debug.Log("Character not recognized in rule set: " + sb + "\n");
                }
                */
            }

            currentString = sb.ToString();
            sb = new StringBuilder();
        }



        // New set of instructions
        foreach (char c in currentString)
        {
            switch (c)
            {
                
                case 'F':       // Draws straight line
                    Vector3 initialPosition = transform.position;       // Initial position is equal to the position of the tree spawner
                    transform.Translate(Vector3.up * length);

                    GameObject treeSegment = Instantiate(Branch);       // Referencing the branch and instatiating it then setting the line renderer values to draw
                    treeSegment.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
                    treeSegment.GetComponent<LineRenderer>().SetPosition(1, transform.position);

                    break;

                case 'X':       // Generates more F's
                    break;

                case '+':       // Rotates tree spawner clockwise
                    transform.Rotate(Vector3.forward * angle);
                    break;

                case '-':       // Rotates tree spawner anti-clockwise
                    transform.Rotate(Vector3.back * angle);
                    break;

                case '[':       // Saves current transform info
                    transformStack.Push(new TransformInfo()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;

                case ']':       // Returns to previously saved transform info 
                    TransformInfo ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;

                default:
                    throw new InvalidOperationException("Invalid L-Tree operation");
            }
        }
    }

}
