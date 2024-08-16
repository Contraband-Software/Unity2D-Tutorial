using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assembly = System.Reflection.Assembly;


public abstract class Test
{
    public abstract void Insult();
}

public class Wombe : Test
{
    public override void Insult()
    {
        Debug.Log("fuk u");
    }
}

public class Exeo : Test
{
    public override void Insult()
    {
        Debug.Log("mmmm ur gay");
    }
}

public class TestBench : MonoBehaviour
{
    private Dictionary<Type, Test> lookup = new();
    
    void Start()
    {
        print("test");

        var tests = Assembly.GetAssembly(typeof(Test)).GetTypes()
            .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Test)));
        foreach (var type in tests)
        {
            Test v = Activator.CreateInstance(type) as Test;
            lookup.Add(v.GetType(), v);
        }
        
        lookup[typeof(Wombe)].Insult();
        lookup[typeof(Exeo)].Insult();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
