using System;
using System.Linq;
using System.Reflection;
using UnhollowerRuntimeLib.XrefScans;
using MelonLoader;

public static class XrefScannerUtils
{


    public static bool XRefScanForGlobal(this MethodBase methodBase, string searchTerm, bool ignoreCase = true)
    {
        if (!string.IsNullOrEmpty(searchTerm))
            return XrefScanner.XrefScan(methodBase).Any(
                xref => xref.Type == XrefType.Global && xref.ReadAsObject()?.ToString().IndexOf(
                            searchTerm,
                            ignoreCase
                                ? StringComparison.OrdinalIgnoreCase
                                : StringComparison.Ordinal) >= 0);
        MelonLogger.LogError($"XRefScanForGlobal \"{methodBase}\" has an empty searchTerm. Returning false");
        return false;
    }

    public static bool XRefScanForMethod(this MethodBase methodBase, string methodName = null, string parentType = null, bool ignoreCase = true)
    {
        if (!string.IsNullOrEmpty(methodName)
            || !string.IsNullOrEmpty(parentType))
            return XrefScanner.XrefScan(methodBase).Any(
                xref =>
                    {
                        if (xref.Type != XrefType.Method) return false;

                        var found = false;
                        MethodBase resolved = xref.TryResolve();
                        if (resolved == null) return false;

                        if (!string.IsNullOrEmpty(methodName))
                            found = !string.IsNullOrEmpty(resolved.Name) && resolved.Name.IndexOf(
                                        methodName,
                                        ignoreCase
                                            ? StringComparison.OrdinalIgnoreCase
                                            : StringComparison.Ordinal) >= 0;

                        if (!string.IsNullOrEmpty(parentType))
                            found = !string.IsNullOrEmpty(resolved.ReflectedType?.Name) && resolved.ReflectedType.Name.IndexOf(
                                        parentType,
                                        ignoreCase
                                            ? StringComparison
                                                .OrdinalIgnoreCase
                                            : StringComparison.Ordinal)
                                    >= 0;

                        return found;
                    });
        MelonLogger.LogError($"XRefScanForMethod \"{methodBase}\" has all null/empty parameters. Returning false");
        return false;
    }

    public static int XRefScanMethodCount(this MethodBase methodBase, string methodName = null, string parentType = null, bool ignoreCase = true)
    {
        if (!string.IsNullOrEmpty(methodName)
            || !string.IsNullOrEmpty(parentType))
            return XrefScanner.XrefScan(methodBase).Count(
                xref =>
                    {
                        if (xref.Type != XrefType.Method) return false;

                        var found = false;
                        MethodBase resolved = xref.TryResolve();
                        if (resolved == null) return false;

                        if (!string.IsNullOrEmpty(methodName))
                            found = !string.IsNullOrEmpty(resolved.Name) && resolved.Name.IndexOf(
                                        methodName,
                                        ignoreCase
                                            ? StringComparison.OrdinalIgnoreCase
                                            : StringComparison.Ordinal) >= 0;

                        if (!string.IsNullOrEmpty(parentType))
                            found = !string.IsNullOrEmpty(resolved.ReflectedType?.Name) && resolved.ReflectedType.Name.IndexOf(
                                        parentType,
                                        ignoreCase
                                            ? StringComparison
                                                .OrdinalIgnoreCase
                                            : StringComparison.Ordinal)
                                    >= 0;

                        return found;
                    });
        MelonLogger.LogError($"XRefScanMethodCount \"{methodBase}\" has all null/empty parameters. Returning -1");
        return -1;
    }
    public static bool checkXref(this MethodBase m, string match)
    {
        try
        {
            return XrefScanner.XrefScan(m).Any(
                instance => instance.Type == XrefType.Global && instance.ReadAsObject() != null && instance.ReadAsObject().ToString()
                               .Equals(match, StringComparison.OrdinalIgnoreCase));
        }
        catch { }

        return false;
    }

    public static T Cast<T>(this object o)
    {
        return (T)o;
    }
}
