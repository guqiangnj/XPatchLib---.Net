﻿// Copyright © 2013-2017 - GuQiang
// Licensed under the LGPL-3.0 license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;

namespace XPatchLib
{
    /// <summary>
    ///     XML类型写入器的默认设置。
    /// </summary>
    /// <seealso cref="XPatchLib.ISerializeSetting" />
    public class XmlSerializeSetting : SerializeSetting
    {
        private string _actionName = "Action";

        private DateTimeSerializationMode _mode = DateTimeSerializationMode.RoundtripKind;

        private bool _serializeDefalutValue;

        /// <summary>
        ///     在更改属性值时发生。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     获取或设置在字符串与 <see cref="DateTime" /> 之间转换时，如何处理时间值。
        /// </summary>
        /// <value>默认为 <see cref="DateTimeSerializationMode.RoundtripKind" />。</value>
        public DateTimeSerializationMode Mode
        {
            get { return _mode; }
            set
            {
                if (_mode != value)
                {
                    _mode = value;
                    OnPropertyChanged("Mode");
                }
            }
        }

        /// <summary>
        ///     获取或设置是否序列化默认值。
        /// </summary>
        /// <value>默认为 <c>false</c>。</value>
        /// <example>
        ///     <include file='docs/docs.xml' path='Comments/examples/example[@class="XmlSerializeSetting" and @property="SerializeDefalutValue"]/*' />
        /// </example>
        public bool SerializeDefalutValue
        {
            get { return _serializeDefalutValue; }
            set
            {
                if (_serializeDefalutValue != value)
                {
                    _serializeDefalutValue = value;
                    OnPropertyChanged("SerializeDefalutValue");
                }
            }
        }

        /// <summary>
        ///     获取或设置序列化/反序列化时，文本中标记 '<b>动作</b>' 的文本。
        /// </summary>
        /// <value>
        ///     默认值是 <c>Action</c> 。
        /// </value>
        /// <exception cref="ArgumentNullException">当设置值是传入 <b>null</b> 时。</exception>
        /// <exception cref="ArgumentException">当设置值为空时。</exception>
        /// <example>
        ///     <include file='docs/docs.xml' path='Comments/examples/example[@class="XmlSerializeSetting" and @property="ActionName"]/*' />
        /// </example>
        public string ActionName
        {
            get { return _actionName; }
            set
            {
                Guard.ArgumentNotNullOrEmpty(value, "value");
                if (_actionName != value)
                {
                    _actionName = value;
                    OnPropertyChanged("ActionName");
                }
            }
        }

        /// <summary>
        /// 播发属性变更后事件。
        /// </summary>
        /// <param name="propertyName">被变更的属性名称。</param>
        protected virtual void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}