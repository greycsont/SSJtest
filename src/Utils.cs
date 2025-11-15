using System.Diagnostics;


namespace SSJtest;

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