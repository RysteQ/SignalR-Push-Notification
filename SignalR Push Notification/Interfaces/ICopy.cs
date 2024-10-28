namespace SignalR_Push_Notification.Interfaces;

interface ICopy<T> where T : class
{
    void Clone(T toCopy);
}