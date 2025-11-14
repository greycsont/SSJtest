using HarmonyLib;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


namespace SSJtest;


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

[HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Dodge))]
public static class Dodge_Patch
{
    static void Prefix(NewMovement __instance)
    {
        if(!__instance.sliding)
        {
            Plugin.Logger.LogInfo($"set boost to false in Dodge()");
            Plugin.Logger.LogInfo("Call Stack: " + Utils.GetCallStack());
        }
    }
}

[HarmonyPatch(typeof(Revolver), nameof(Revolver.Shoot))]
public static class Revolver_Shoot_Patch
{
    static void Prefix()
    {
        Plugin.Logger.LogInfo("Input System Update Mode: " + InputSystem.settings.updateMode);
        Plugin.Logger.LogInfo("Current Boost value: " + NewMovement.instance.boost);
        Plugin.Logger.LogInfo("fixedDeltaTime: " + Time.fixedDeltaTime);
        Plugin.Logger.LogInfo("----");
    }
}

[HarmonyPatch(typeof(Nailgun), nameof(Nailgun.Shoot))]
public static class Nailgun_Shoot_Patch
{
    static void Prefix()
    {
        NewMovement.instance.boostLeft = 10f;
        Plugin.Logger.LogInfo("Toogled boostLeft to: " + NewMovement.instance.boostLeft);
    }
}

[HarmonyPatch(typeof(Railcannon), nameof(Railcannon.Shoot))]
public static class Railcannon_Shoot_Patch
{
    static void Prefix()
    {
        Settings.replaceUpdate = true;
        Plugin.Logger.LogInfo("Toggled replaceFixedUpdate to: " + Settings.replaceFixedUpdate);
    }
}

[HarmonyPatch(typeof(RocketLauncher), nameof(RocketLauncher.Shoot))]
public static class RocketLauncher_Shoot_Patch
{
    static void Prefix()
    {
        NewMovement.instance.boost = !NewMovement.instance.boost;
        Plugin.Logger.LogInfo("Toggled Boost value to: " + NewMovement.instance.boost);
    }
}


[HarmonyPatch(typeof(NewMovement), nameof(NewMovement.FixedUpdate))]
public static class NewMovement_FixedUpdate_Patch
{
    static void Postfix(NewMovement __instance)
    {
        if (Settings.replaceUpdate)
        {
            SimulateInput.ReleaseC();
            Settings.replaceUpdateCount = 1;
            Settings.replaceUpdate = false;
        }
    }
}

[HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Update))]
public static class NewMovement_Update_Patch
{
    static void Postfix(NewMovement __instance)
    {
        if (Settings.replaceUpdateCount > 0)
        {
            SimulateInput.PressSpace();
            Settings.replaceUpdateCount--;
        }
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
            methodNames += method.DeclaringType.FullName + "." + method.Name + " <- ";
        }
        return methodNames;
    }
}