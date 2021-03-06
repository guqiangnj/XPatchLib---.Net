﻿// Copyright © 2013-2017 - GuQiang
// Licensed under the LGPL-3.0 license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace XPatchLib
{
    /// <summary>
    ///     常量定义。
    /// </summary>
    internal class ConstValue
    {
#if (NET || NETSTANDARD_2_0_UP)
        /// <summary>
        ///     <see cref="System.Drawing.Color" />类型的格式化字符串。
        /// </summary>
        internal static String COLOR_FORMAT
        {
            get { return "#{0:X}"; }
        }

        /// <summary>
        ///     <see cref="System.Drawing.Color" />类型输出的开始字符串。
        /// </summary>
        internal static String COLOR_STARTCHAR
        {
            get { return "#"; }
        }
#endif

        /// <summary>
        ///     生成或读取XML时，<see cref="KeyValuePair{TKey,TValue}" />类型的Key值的字符串。
        /// </summary>
        internal static String KEY
        {
            get { return "Key"; }
        }

        /// <summary>
        ///     集合类型 增加 操作方法名
        /// </summary>
        internal static String OPERATOR_TRY_ADD
        {
            get { return "TryAdd"; }
        }

        /// <summary>
        ///     集合类型 增加 操作方法名
        /// </summary>
        internal static String OPERATOR_ADD
        {
            get { return "Add"; }
        }

        /// <summary>
        ///     集合类型 增加 操作方法名
        /// </summary>
        internal static String OPERATOR_ENQUEUE
        {
            get { return "Enqueue"; }
        }

        /// <summary>
        ///     集合类型 增加 操作方法名
        /// </summary>
        internal static String OPERATOR_PUSH
        {
            get { return "Push"; }
        }

        /// <summary>
        ///     集合类型 获取值 操作方法名
        /// </summary>
        internal static String OPERATOR_GET
        {
            get { return "get_Item"; }
        }

        /// <summary>
        ///     集合类型 删除 操作方法名
        /// </summary>
        internal static String OPERATOR_REMOVE
        {
            get { return "Remove"; }
        }

        /// <summary>
        ///     集合类型 删除 操作方法名
        /// </summary>
        internal static String OPERATOR_TRY_REMOVE
        {
            get { return "TryRemove"; }
        }

        /// <summary>
        ///     集合类型 更改值 操作方法名
        /// </summary>
        internal static String OPERATOR_TRY_UPDATE
        {
            get { return "TryUpdate"; }
        }

        /// <summary>
        ///     集合类型 更改值 操作方法名
        /// </summary>
        internal static String OPERATOR_SET
        {
            get { return "set_Item"; }
        }

        /// <summary>
        ///     下划线。
        /// </summary>
        internal static String UNDERLINE
        {
            get { return "_"; }
        }

        /// <summary>
        ///     生成或读取XML时，<see cref="KeyValuePair{TKey,TValue}" />类型的Value值的字符串。
        /// </summary>
        internal static String VALUE
        {
            get { return "Value"; }
        }
    }
}