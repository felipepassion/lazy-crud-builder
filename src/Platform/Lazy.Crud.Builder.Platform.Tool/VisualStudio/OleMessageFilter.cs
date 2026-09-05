using System.Runtime.InteropServices;

namespace Lazy.Crud.Builder.Platform.Tool.VisualStudio;

/// <summary>
/// COM message filter that retries calls rejected by a busy Visual Studio automation server
/// (RPC_E_CALL_REJECTED / RPC_E_SERVERCALL_RETRYLATER). Must be registered on an STA thread.
/// </summary>
[ComVisible(true)]
public sealed class OleMessageFilter : IOleMessageFilter
{
    private const int SERVERCALL_ISHANDLED = 0;
    private const int SERVERCALL_RETRYLATER = 2;
    private const int PENDINGMSG_WAITDEFPROCESS = 2;

    public static void Register()
    {
        if (!OperatingSystem.IsWindows()) return;
        IOleMessageFilter? old;
        CoRegisterMessageFilter(new OleMessageFilter(), out old);
    }

    public static void Revoke()
    {
        if (!OperatingSystem.IsWindows()) return;
        IOleMessageFilter? old;
        CoRegisterMessageFilter(null, out old);
    }

    int IOleMessageFilter.HandleInComingCall(int dwCallType, IntPtr hTaskCaller, int dwTickCount, IntPtr lpInterfaceInfo)
        => SERVERCALL_ISHANDLED;

    int IOleMessageFilter.RetryRejectedCall(IntPtr hTaskCallee, int dwTickCount, int dwRejectType)
    {
        // Retry immediately for up to ~2 minutes of accumulated waiting; VS is single-threaded and
        // legitimately busy while a custom tool runs.
        if (dwRejectType == SERVERCALL_RETRYLATER && dwTickCount < 120_000) return 150;
        return -1;
    }

    int IOleMessageFilter.MessagePending(IntPtr hTaskCallee, int dwTickCount, int dwPendingType)
        => PENDINGMSG_WAITDEFPROCESS;

    [DllImport("ole32.dll")]
    private static extern int CoRegisterMessageFilter(IOleMessageFilter? newFilter, out IOleMessageFilter? oldFilter);
}

[ComImport, Guid("00000016-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IOleMessageFilter
{
    [PreserveSig] int HandleInComingCall(int dwCallType, IntPtr hTaskCaller, int dwTickCount, IntPtr lpInterfaceInfo);
    [PreserveSig] int RetryRejectedCall(IntPtr hTaskCallee, int dwTickCount, int dwRejectType);
    [PreserveSig] int MessagePending(IntPtr hTaskCallee, int dwTickCount, int dwPendingType);
}
