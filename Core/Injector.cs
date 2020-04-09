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
            var Process = Interop.OpenProcess(Interop.ProcessAccessFlags.All, false, Target.Id);
            if (Process == null)
                return Result.ObtainingProcessHandleFailed;
            var Allocation = Interop.VirtualAllocEx(Process, IntPtr.Zero, (IntPtr)Location.Length,
                Interop.AllocationType.Reserve | Interop.AllocationType.Commit,
                Interop.MemoryProtection.ExecuteReadWrite);
            if (Allocation == null)
                return Result.DLLAllocationFailed;
            var Bytes = Encoding.ASCII.GetBytes(Location);
            var Write = Interop.WriteProcessMemory(Process, Allocation, Bytes, Bytes.Length, out _);
            if (Write == false)
                return Result.DLLWritingFailed;
            var Kernel = Interop.GetModuleHandle("kernel32.dll");
            var Loader = Interop.GetProcAddress(Kernel, "LoadLibraryA");
            if (Loader == null)
                return Result.ObtainingLoaderAddressFailed;
            var Thread = Interop.CreateRemoteThread(Process, IntPtr.Zero, 0, Loader, Allocation, 0, IntPtr.Zero);
            if (Thread == null)
                return Result.ObtainingThreadHandleFailed;
            var Release = Interop.VirtualFreeEx(Process, Allocation, 0, Interop.AllocationType.Release);
            if (Release == false)
                return Result.UnableReleaseMemory;
            Interop.CloseHandle(Thread);
            Interop.CloseHandle(Process);
            return Result.InjectionSuccessful;
        }

    }

}