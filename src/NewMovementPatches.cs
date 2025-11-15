using HarmonyLib;

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