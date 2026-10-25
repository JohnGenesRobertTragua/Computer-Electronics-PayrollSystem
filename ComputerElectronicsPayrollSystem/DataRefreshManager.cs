using System;

namespace ComputerElectronicsPayrollSystem
{
    public enum DataChangeType
    {
        Employee,
        Attendance,
        Payroll
    }

    public sealed class DataChangedEventArgs : EventArgs
    {
        public DataChangeType ChangeType { get; }

        public object? Source { get; }

        public DataChangedEventArgs(
            DataChangeType changeType,
            object? source)
        {
            ChangeType = changeType;
            Source = source;
        }
    }

    public static class DataRefreshManager
    {
        public static event EventHandler<DataChangedEventArgs>? DataChanged;

        public static void Notify(
            DataChangeType changeType,
            object? source = null)
        {
            DataChanged?.Invoke(
                null,
                new DataChangedEventArgs(
                    changeType,
                    source));
        }
    }
}
