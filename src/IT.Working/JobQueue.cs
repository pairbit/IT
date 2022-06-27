namespace IT.Working;

public enum JobQueue
{
    /// <summary>
    /// По расписанию
    /// </summary>
    Scheduled,

    /// <summary>
    /// Отложен
    /// </summary>
    Delayed,

    /// <summary>
    /// Ожидает выполнения связанной задачи
    /// </summary>
    Awaiting,

    /// <summary>
    /// Добавлена в очередь
    /// </summary>
    Enqueued,

    /// <summary>
    /// В процессе выполнения
    /// </summary>
    Processing,

    /// <summary>
    /// Успешно выполена
    /// </summary>
    Succeeded,

    /// <summary>
    /// Не выполнена
    /// </summary>
    Failed,

    /// <summary>
    /// Удален
    /// </summary>
    Deleted
}