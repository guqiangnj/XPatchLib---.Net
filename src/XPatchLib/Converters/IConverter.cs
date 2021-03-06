﻿// Copyright © 2013-2018 - GuQiang
// Licensed under the LGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace XPatchLib
{
    internal interface IConverter:ICombine, IDivide
    {
        /// <summary>
        /// 获取当前对象类型的无参数构造函数实例。
        /// </summary>
        /// <returns></returns>
        object CreateInstance();
    }
}