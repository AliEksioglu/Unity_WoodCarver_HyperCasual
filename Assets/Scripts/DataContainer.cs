using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers
{
    public const string triggerWood = "triggerwood";
    public const string collectWood = "collectWood";
    public const string wood = "wood";
    public const string obstacle = "obstacle";
    public const string door = "Door";
}

public static class Tags
{
    public static string[] taglar = new string[] { "tree", "Wood", "plank", "Paintable", "polished", "pattern" };
}

public static class AnimName
{
    public static string CharacterRunnig = "isRunning";
    public static string CharacterObstacleHit = "obstacleHit";
    public static string Woodcount = "WoodCount";
}