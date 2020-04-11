using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace WxInjector.Core
{

    [SuppressMessage("Design", "CA1063")]
    public class Injector : IDisposable
    {

        public enum Result
        {
            SystemProcessDisallowed,
            ObtainingProcessHandleFailed,
            DLLAllocationFailed,
            DLLWritingFailed,
            ObtainingLoaderAddressFailed,
            ObtainingThreadHandleFailed,
            UnableReleaseMemory,
            InjectionSuccessful
        }

        private readonly Process Target;

        public Injector(int Target)
        {
            this.Target = Process.GetProcessById(Target);
        }

        [SuppressMessage("Design", "CA1063")]
        [SuppressMessage("Usage", "CA1816")]
        public void Dispose()
        {
            Target.Dispose();
        }

        [SuppressMessage("Design", "CA1062")]
        public Result Inject(string Location)
        {
            if (Target.Id == 0 || Target.Id == 4)
                return Result.SystemProcessDisallowed;
            var Process = Native.OpenProcess(Native.ProcessAccessFlags.All, false, Target.Id);
            if (Process == null)
                return Result.ObtainingProcessHandleFailed;
            var Allocation = Native.VirtualAllocEx(Process, IntPtr.Zero, (IntPtr)Location.Length,
                Native.AllocationType.Reserve | Native.AllocationType.Commit,
                Native.MemoryProtection.ExecuteReadWrite);
            if (Allocation == null)
                return Result.DLLAllocationFailed;
            var Bytes = Encoding.ASCII.GetBytes(Location);
            var Write = Native.WriteProcessMemory(Process, Allocation, Bytes, Bytes.Length, out _);
            if (Write == false)
                return Result.DLLWritingFailed;
            var Kernel = Native.GetModuleHandle("kernel32.dll");
            var Loader = Native.GetProcAddress(Kernel, "LoadLibraryA");
            if (Loader == null)
                return Result.ObtainingLoaderAddressFailed;
            var Thread = Native.CreateRemoteThread(Process, IntPtr.Zero, 0, Loader, Allocation, 0, IntPtr.Zero);
            if (Thread == null)
                return Result.ObtainingThreadHandleFailed;
            var Release = Native.VirtualFreeEx(Process, Allocation, 0, Native.AllocationType.Release);
            if (Release == false)
                return Result.UnableReleaseMemory;
            Native.CloseHandle(Thread);
            Native.CloseHandle(Process);
            return Result.InjectionSuccessful;
        }

    }

}