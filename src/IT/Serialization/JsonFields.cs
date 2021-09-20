namespace IT.Serialization
{
    public enum JsonFields
    {
        /// <summary>
        /// Все поля
        /// </summary>
        /// <remarks>
        /// По умолчанию
        /// </remarks>
        All,
        
        /// <summary>
        /// Все поля, а данные обрезать
        /// </summary>
        /// <remarks>
        /// Для отладочной информации
        /// </remarks>
        MetaDataShorten,

        /// <summary>
        /// Только поля с данными
        /// </summary>
        /// <remarks>
        /// Для сохранения на диск объемные данные
        /// </remarks>
        Data,

        /// <summary>
        /// Только метаданные
        /// </summary>
        /// <remarks>
        /// Для сохранения в кеш или базу данных метаинформацию
        /// </remarks>
        Meta,

        /// <summary>
        /// Только поля для записи
        /// </summary>
        /// <remarks>
        /// Для хранения конфигов или только минимально-необходимой ифнормации.
        /// Когда вычисляемые поля не нужны.
        /// </remarks>
        Writable
    }
}