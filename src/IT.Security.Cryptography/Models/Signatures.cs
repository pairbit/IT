﻿using System;
using System.Collections.Generic;

namespace IT.Security.Cryptography.Models;

/// <summary>
/// Необязательный список с подробной информацией о результатах проверки 
/// каждой из имеющихся в проверяемых данных подписей. 
/// Список может отсутствовать либо быть пустым, в случае если, например, 
/// на проверку были переданы данные в формате CMS SignedData, 
/// не содержащие ни одного элемента SignerInfo. 
/// Элементы в данном списке будут перечислены в порядке нахождения подписей в проверяемых данных. 
/// В случае если проверяемые данные содержали вложенные подписи, 
/// результаты проверки будут перечислены в порядке от внешнего уровня к вложенным.
/// </summary>
public class Signatures : List<Signature>
{
    /// <summary>
    /// Время проверки по GMT
    /// </summary>
    public DateTime? DateTime { get; set; }

    /// <summary>
    /// Итоговый результат проверки, 
    /// определяется по совокупности результатов проверки всех подписей, 
    /// содержавшихся в переданных на проверку данных
    /// </summary>
    public SignaturesStatus? Status { get; set; }
}