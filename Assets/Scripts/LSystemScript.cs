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
    private int iterations;
    //[HideInInspector]
    private float length;
    private float angle;
    // Starting point of the tree, will always begin with a rule
    private string axiom;

    public GameObject branch;
    public GameObject leaf;
    public GameObject treeParent;

    private List<GameObject> branches;



    private Vector3 initialTransform;
    private Quaternion initialRotation;

    // Grammar: take a string, perform operation based on each character of the string
    // Repeat the sequence for each iteration
    private Stack<TransformInfo> transformStack;
    private Dictionary<char, string> rules;
    private string currentString = string.Empty;

    private int lsystemNum = 0;

    void Start()
    {
        initialTransform = transform.position;
        initialRotation = transform.rotation;

        transformStack = new Stack<TransformInfo>();

        branches = new List<GameObject>();

        Spawn();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.iterations++;
            Spawn();
        }
        else if (Input.GetKeyDown(KeyCode.E) && iterations != 0)
        {
            this.iterations--;
            DeleteElements();
            ResetTransform();
            Spawn();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.angle++;
            DeleteElements();
            ResetTransform();
            Spawn();

        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.angle--;
            DeleteElements();
            ResetTransform();
            Spawn();

        }
        else if (Input.GetKey(KeyCode.W))
        {
            this.length++;
            DeleteElements();
            ResetTransform();
            Spawn();

        }
        else if (Input.GetKey(KeyCode.S) && length != 0)
        {
            this.length--;
            DeleteElements();
            ResetTransform();
            Spawn();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && lsystemNum <= 7)
        {
            lsystemNum++;
            DeleteElements();
            ChangeLsystemtree();
            Spawn();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && lsystemNum >= 0)
        {
            lsystemNum--;
            DeleteElements();
            ChangeLsystemtree();
            Spawn();
        }



        UIMenu.iterations = iterations.ToString();
        UIMenu.angleText = angle;
        UIMenu.lengthText = length;
        UIMenu.axiom = axiom;
        UIMenu.lsystemNumText = lsystemNum;
        UIMenu.rule2Text = "";
        UIMenu.rule1Text = "";
        foreach (KeyValuePair<char, string> rule in rules)
        {
            if (rule.Key == 'X')
            {
                UIMenu.rule1Text = rule.Value;
            }
            else if (rule.Key == 'F')
            {
                UIMenu.rule2Text = rule.Value;
            }
        }
    }

    /*
     All of the information needed to spawn the tree 
    */
    private void Spawn()
    {

        if (rules != null)
        {
            // Current starting point rule
            currentString = axiom;

            StringBuilder sb = new StringBuilder();
            // Debug.Log("Before the 1st iteration sb: " + sb + "\n");

            for (int i = 0; i < iterations; i++)
            {
                foreach (char c in currentString)
                {
                    // If the character being read is a key in the dict, add the value from that key to the string builder
                    // If it doesn't, add the character itself to the string builder
                    // NOTE: Use ternirary operator
                    // Debug.Log("The current character being read is: " + c + "\n");

                    sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());

                }

                currentString = sb.ToString();
                sb = new StringBuilder();
            }
        }




        // New set of instructions
        foreach (char c in currentString)
        {
            switch (c)
            {

                case 'F':       // Draws straight line
                    Vector3 initialPosition = transform.position;       // Initial position is equal to the position of the tree spawner
                    transform.Translate(Vector3.up * length);

                    GameObject treeSegment = Instantiate(branch);       // Referencing the branch and instatiating it then setting the line renderer values to draw
                    treeSegment.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
                    treeSegment.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                    branches.Add(treeSegment);
                    break;

                case 'X':       // Generates more F's
                    break;

                case '+':       // Rotates tree spawner clockwise
                    transform.Rotate(Vector3.back * angle);
                    break;

                case '-':       // Rotates tree spawner anti-clockwise
                    transform.Rotate(Vector3.forward * angle);
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

    private void DeleteElements()
    {
        foreach (GameObject b in branches)
        {
            Destroy(b);
        }
    }



    private void ChangeLsystemtree()
    {
        ResetTransform();
        switch (lsystemNum)
        {
            case 1:
                TreeOne();
                break;
            case 2:
                TreeTwo();
                break;
            case 3:
                TreeThree();
                break;
            case 4:
                TreeFour();
                break;
            case 5:
                TreeFive();
                break;
            case 6:
                TreeSix();
                break;
            case 7:
                TreeSeven();
                break;
        }
    }

    private void ResetTransform()
    {
        transform.position = initialTransform;
        transform.rotation = initialRotation;
    }

    private void TreeOne()
    {
        axiom = "X";
        angle = 20f;
        iterations = 7;
        length = 1;

        rules = new Dictionary<char, string>
        {
            {'X', "F[+X]F[-X]+X" },
            {'F', "FF" }
        };

    }

    private void TreeTwo()
    {
        axiom = "X";
        angle = 25.7f;
        iterations = 7;
        length = 1;

        rules = new Dictionary<char, string>
        {
            {'X', "F[+X][-X]FX" },
            {'F', "FF" }
        };

    }

    private void TreeThree()
    {
        axiom = "X";
        angle = 22.5f;
        iterations = 5;
        length = 3;

        rules = new Dictionary<char, string>
        {
            {'X', "F-[[X]+X]+F[+FX]-X" },
            {'F', "FF" }
        };

    }

    private void TreeFour()
    {
        axiom = "F";
        angle = 22.5f;
        iterations = 4;
        length = 4; 

        rules = new Dictionary<char, string>
        {
            {'F', "FF-[-F+F+F]+[+F-F-F]" }
        };

    }

    private void TreeFive()
    {
        axiom = "F";
        angle = 20f;
        iterations = 5;
        length = 4;

        rules = new Dictionary<char, string>
        {
            {'F', "F[+F]F[-F][F]" }
        };

    }

    private void TreeSix()
    {
        axiom = "F";
        angle = 22.5f;
        iterations = 5;
        length = 1;

        rules = new Dictionary<char, string>
        {
            {'F', "F[+F]F[-F]F" }
        };

    }

    private void TreeSeven()
    {
        axiom = "X";
        angle = 20f;
        iterations = 7;
        length = 1;

        rules = new Dictionary<char, string>
        {
            {'X', "F[+X]F[-X]+X" },
            {'F', "FF" }
        };

    }
}
