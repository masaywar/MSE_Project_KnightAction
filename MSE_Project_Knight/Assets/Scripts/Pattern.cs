using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class Pattern
{
    public string path;

    public List<Item> itemList = new List<Item>();

    public class Item
    {
        public string position;
        public string type;
        public float wait;
    }

    public Pattern(string path)
    {
        this.path = path;
        Parse();
    }

    public void Parse()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);

        var tempStringArray = textAsset.
            ToString().
            Split('\n').
            Skip(1);

        tempStringArray.ForEach(s => {
            var temp = s.Split('\t');

            Item item = new Item();
            item.position = temp[0];
            item.type = temp[1];
            item.wait = ParseUtiltiy.SafeFloatParse(temp[2].Replace($"f", ""));

            itemList.Add(item);
        }) ;
    }
}
