using HarmonyLib;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


namespace SSJtest;

/*[HarmonyPatch(typeof(NewMovement), nameof(NewMovement.TrySSJ))]
public static class TrySSJ_Patch
{
    static void Prefix(NewMovement __instance)
    {
        // 输出简单调用栈到控制台
        Plugin.Logger.LogInfo("NewMovement.TrySSJ called! Call stack:");
        // 获取当前调用栈
        StackTrace trace = new StackTrace();
        string methodNames = "at ";
        foreach (var frame in trace.GetFrames())
        {
            var method = frame.GetMethod();
            methodNames += ($"{method.DeclaringType.FullName}.{method.Name} <- ");
        }
        Plugin.Logger.LogInfo(methodNames);
        Plugin.Logger.LogInfo($"SSJ state: {__instance.framesSinceSlide > 0 && (float)__instance.framesSinceSlide < __instance.ssjMaxFrames && !__instance.boost}");
    }
}*/

[HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Jump))]
public static class Jump_Patch
{
    static void Prefix(NewMovement __instance)
    {
        Plugin.Logger.LogInfo($"Jumping prefix : (sliding: {__instance.sliding}, boost: {__instance.boost})");
        if(!__instance.sliding && __instance.boost && __instance.boostCharge > 100f)
        {
            Plugin.Logger.LogInfo("-100 boostCharge in Jump()");
        }
    }
}

[HarmonyPatch(typeof(NewMovement), nameof(NewMovement.StopSlide))]
public static class StopSlide_Patch
{
    static void Prefix(NewMovement __instance)
    {
        Plugin.Logger.LogInfo($"StopSlide called");
    }
}

[HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Dodge))]
public static class Dodge_Patch
{
    static void Prefix(NewMovement __instance)
    {
        if(!__instance.sliding)
        {
            Plugin.Logger.LogInfo($"set boost to false in Dodge()");
        }
    }
}

[HarmonyPatch(typeof(Revolver), nameof(Revolver.Shoot))]
public static class Revolver_Shoot_Patch
{
    static void Prefix()
    {
        InputSettings.UpdateMode currentMode = InputSystem.settings.updateMode;
        Plugin.Logger.LogInfo("Input System Update Mode: " + currentMode);
    }
}

public static class Utils
{
    public static string GetCallStack()
    {
        StackTrace trace = new StackTrace();
        string methodNames = "at ";
        foreach (var frame in trace.GetFrames())
        {
            var method = frame.GetMethod();
            methodNames += ($"{method.DeclaringType.FullName}.{method.Name} <- ");
        }
        return methodNames;
    }
}