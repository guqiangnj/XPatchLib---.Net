﻿// Copyright © 2013-2018 - GuQiang
// Licensed under the LGPL-3.0 license. See LICENSE file in the project root for full license information.

#if NET_40_UP || NETSTANDARD_2_0_UP

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace XPatchLib
{
    internal class ConverterDynamic : ConverterObject
    {
        internal ConverterDynamic(ITextWriter pWriter, TypeExtend pType) : base(pWriter, pType)
        {
        }

        internal ConverterDynamic(TypeExtend pType) : base(pType)
        {
        }

        protected override bool DivideAction(string pName, object pOriObject, object pRevObject,
            DivideAttachment pAttach = null)
        {
            var result = base.DivideAction(pName, pOriObject, pRevObject, pAttach);

            DynamicObject oriObj = pOriObject as DynamicObject;
            DynamicObject revObj = pRevObject as DynamicObject;

            List<string> names =
                new[] {oriObj, revObj}.GetMembers(Type.FieldsToBeSerialized.Select(x => x.Name)
                    .ToArray());

            foreach (string name in names)
            {
                object oriValue = oriObj.GetMemberValue(name);
                object revValue = revObj.GetMemberValue(name);
                Type memberType = GetType(oriValue, revValue);

                TypeExtend t =
                    TypeExtendContainer.GetTypeExtend(Writer.Setting, memberType, Writer.Setting.IgnoreAttributeType,
                        Type);

                ConverterBase d = new ConverterCore(Writer, t);
                d.Assign(this);

                if (pAttach == null)
                    pAttach = new DivideAttachment();
                if (!result)
                {
                    ParentObject parentObject =
                        new ParentObject(pName, pOriObject, Type, GetType(pOriObject, pRevObject));
                    if (pAttach.ParentQuere.Count <= 0 || !pAttach.ParentQuere.Last().Equals(parentObject))
                    {
                        //将当前节点加入附件中，如果遇到子节点被写入前，会首先根据队列先进先出写入附件中的节点的开始标记
                        //只有当没有写入过的情况下才需要写入父节点
                        pAttach.ParentQuere.Enqueue(new ParentObject(pName, pOriObject, Type, GetType(pOriObject, pRevObject))
                        {
                            Action = pAttach.CurrentAction
                        });
                        pAttach.CurrentAction = Action.Edit;
                    }
                }

                var childResult = d.Divide(name, oriValue, revValue, pAttach);
                if (!result)
                    result = childResult;
            }

            return result;
        }

        private Type GetType(params object[] o)
        {
            foreach (object dynamicObject in o)
            {
                if (dynamicObject == null) continue;
                return dynamicObject.GetType();
            }

            return typeof(object);
        }

        protected override object CombineAction(ITextReader pReader, object pOriObject, string pName)
        {
            //当原始对象为null时，先创建一个实例。
            if (pOriObject == null)
                pOriObject = Type.CreateInstance();

            DynamicObject oriValue = pOriObject as DynamicObject;

            List<string> names =
                new[] {oriValue}.GetMembers(Type.FieldsToBeSerialized.Select(x => x.Name)
                    .ToArray());

            while (!pReader.EOF)
            {
                if (pReader.Name.Equals(pName, StringComparison.OrdinalIgnoreCase))
                    if (pReader.NodeType == NodeType.Element)
                    {
                        pReader.Read();
                        continue;
                    }
                    else if (pReader.NodeType == NodeType.EndElement)
                    {
                        break;
                    }

                Type memberType = null;

                //读取除Action以外的所有Action，将其赋值给属性
                for (int i = 0; i < Attributes.Count; i++)
                {
                    MemberWrapper member;
                    String key = Attributes.Keys[i];
                    if (TryFindMember(key, out member))
                    {
                        Type.SetMemberValue(pOriObject, key, Attributes.Values[i]);
#if DEBUG
                        Debug.WriteLine("{2} SetMemberValue: {0}={1}", key, Attributes.Values[i],
                            Type.TypeFriendlyName);
#endif
                    }
                }

                string[,] curAttrs = pReader.GetAttributes();
                string assembly = string.Empty;
                Action action = Action.Edit;
                for (int i = 0; i < curAttrs.GetLength(0); i++)
                {
                    if (curAttrs[i, 0] == pReader.Setting.AssemblyQualifiedName)
                    {
                        memberType = System.Type.GetType(curAttrs[i, 1]);
                        assembly = curAttrs[i, 1];
                        continue;
                    }

                    if (curAttrs[i, 0] == pReader.Setting.ActionName) ActionHelper.TryParse(curAttrs[i, 1], out action);
                }

                if (string.IsNullOrEmpty(assembly))
                    throw new InvalidOperationException(
                        ResourceHelper.GetResourceString(LocalizationRes.Exp_String_TypeAssemblyQualifiedNameNotFound,
                            pReader.Name));

                if (memberType == null)
                    throw new FileNotFoundException(
                        ResourceHelper.GetResourceString(LocalizationRes.Exp_String_TypeAssemblyQualifiedNameNotFound,
                            assembly));

                string curName = pReader.Name;

                if (pReader.NodeType == NodeType.Element || pReader.NodeType == NodeType.FullElement)
                {
                    if (oriValue != null)
                        if (action != Action.SetNull)
                        {
                            object memberObj = null;
                            if (names.Contains(pReader.Name)) memberObj = oriValue.GetMemberValue(pReader.Name);
                            oriValue.SetMemberValue(pReader.Name, CombineInstanceContainer
                                .GetCombineInstance(
                                    TypeExtendContainer.GetTypeExtend(pReader.Setting, memberType, null, Type))
                                .Combine(pReader, memberObj, pReader.Name));
                        }
                        else
                        {
                            oriValue.SetMemberValue(pReader.Name, null);
                        }

                    while (!(curName == pReader.Name && (pReader.NodeType == NodeType.EndElement ||
                                                         pReader.NodeType == NodeType.FullElement)))
                        pReader.Read();
                }

                //如果不是当前正在读取的节点的结束标记，就一直往下读
                pReader.Read();
            }

            return pOriObject;
        }
    }
}
#endif