using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class Pattern
{
    [SerializeField]
    private List<Attribute> attributes = new List<Attribute>();

    [Serializable]
    public class Attribute
    {
        public string position;
        public string type;
        public float wait;
    }

    public Pattern(string unparsedText)
    {
        Parse(unparsedText);
    }

    public void Parse(string unparsedText)
    {
        var tempStringArray = unparsedText.
            ToString().
            Split('\n').
            Skip(1);

        tempStringArray.ForEach(s => {
            var temp = s.Split('\t');

            Attribute attribute = new Attribute();
            attribute.position = temp[0];
            attribute.type = temp[1];
            //attribute.wait = ParseUtiltiy.SafeFloatParse(temp[2].Replace($"f", ""));

            attributes.Add(attribute);
        }) ;
    }

    public Attribute[] GetAttributes()
    {
        return attributes.ToArray();
    }
}
