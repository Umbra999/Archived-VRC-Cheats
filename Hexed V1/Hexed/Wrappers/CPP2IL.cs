﻿using BestHTTP.JSON;
using ExitGames.Client.Photon;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Collections;
using UnityEngine;

namespace Hexed.Wrappers
{
    internal class CPP2IL
    {
        public class BinarySerializer
        {
            private static byte[] IL2CPPObjectToByteArray(Il2CppSystem.Object obj)
            {
                if (obj == null) return null;
                var bf = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                var ms = new Il2CppSystem.IO.MemoryStream();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }

            private static byte[] ManagedObjectToByteArray(object obj)
            {
                if (obj == null) return null;
                var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                var ms = new MemoryStream();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }

            private static object ManagedObjectFromArray(byte[] data)
            {
                if (data == null) return default;
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new();
                using MemoryStream ms = new(data);
                object obj = bf.Deserialize(ms);
                return obj;
            }

            private static Il2CppSystem.Object IL2CPPObjectFromArray(byte[] data)
            {
                if (data == null) return default;
                var bf = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                var ms = new Il2CppSystem.IO.MemoryStream(data);
                object obj = bf.Deserialize(ms);
                return (Il2CppSystem.Object)obj;
            }

            public static object Serialize(Il2CppSystem.Object obj)
            {
                return ManagedObjectFromArray(IL2CPPObjectToByteArray(obj));
            }

            public static Il2CppSystem.Object Serialize(object obj)
            {
                return IL2CPPObjectFromArray(ManagedObjectToByteArray(obj));
            }
        }

        public class TypeSerializer
        {
            public static Il2CppSystem.Object ManagedToIL(object Data)
            {
                Type Type = Data.GetType();

                if (Type.IsArray)
                {

                }
                else
                {
                    if (Type == typeof(string)) return new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(Data.ToString()));
                    if (Type == typeof(bool)) return new Il2CppSystem.Boolean() { m_value = (bool)Data }.BoxIl2CppObject();
                    if (Type == typeof(byte)) return new Il2CppSystem.Byte() { m_value = (byte)Data }.BoxIl2CppObject();
                    if (Type == typeof(sbyte)) return new Il2CppSystem.SByte() { m_value = (sbyte)Data }.BoxIl2CppObject();
                    if (Type == typeof(char)) return new Il2CppSystem.Char() { m_value = (char)Data }.BoxIl2CppObject();
                    if (Type == typeof(double)) return new Il2CppSystem.Double() { m_value = (double)Data }.BoxIl2CppObject();
                    if (Type == typeof(float)) return new Il2CppSystem.Single() { m_value = (float)Data }.BoxIl2CppObject();
                    if (Type == typeof(int)) return new Il2CppSystem.Int32() { m_value = (int)Data }.BoxIl2CppObject();
                    if (Type == typeof(uint)) return new Il2CppSystem.UInt32() { m_value = (uint)Data }.BoxIl2CppObject();
                    if (Type == typeof(nint)) return new Il2CppSystem.IntPtr() { m_value = (nint)Data }.BoxIl2CppObject();
                    if (Type == typeof(nuint)) return new Il2CppSystem.UIntPtr() { _pointer = (IntPtr)Data }.BoxIl2CppObject();
                    if (Type == typeof(long)) return new Il2CppSystem.Int64() { m_value = (long)Data }.BoxIl2CppObject();
                    if (Type == typeof(ulong)) return new Il2CppSystem.UInt64() { m_value = (ulong)Data }.BoxIl2CppObject();
                    if (Type == typeof(short)) return new Il2CppSystem.Int16() { m_value = (short)Data }.BoxIl2CppObject();
                    if (Type == typeof(ushort)) return new Il2CppSystem.UInt16() { m_value = (ushort)Data }.BoxIl2CppObject();

                    if (Type == typeof(Vector2)) return ((Vector2)Data).BoxIl2CppObject();
                    if (Type == typeof(Vector3)) return ((Vector3)Data).BoxIl2CppObject();
                    if (Type == typeof(Vector4)) return ((Vector4)Data).BoxIl2CppObject();
                    if (Type == typeof(Quaternion)) return ((Quaternion)Data).BoxIl2CppObject();
                    if (Type == typeof(Color)) return ((Color)Data).BoxIl2CppObject();
                    if (Type == typeof(Color32)) return ((Color32)Data).BoxIl2CppObject();
                }

                //Logger.LogError($"ManagedToIL2CPP: Not Implemented type: {Type.FullName} in {Type.Assembly.FullName} [{Type.MetadataToken}]");
                return BinarySerializer.Serialize(Data);
            }

            public static object ILToManaged(Il2CppSystem.Object Data)
            {
                if (Data == null) return null;

                Il2CppSystem.Type Type = Data.GetIl2CppType();

                if (Type.IsArray)
                {
                    if (Type.GetElementType().IsArray)
                    {
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<string>()) return (string[][])Il2CppArrayBase<string[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<bool>()) return (bool[][])Il2CppArrayBase<bool[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<byte>()) return (byte[][])Il2CppArrayBase<byte[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<sbyte>()) return (sbyte[][])Il2CppArrayBase<sbyte[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<char>()) return (char[][])Il2CppArrayBase<char[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<double>()) return (double[][])Il2CppArrayBase<double[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<float>()) return (float[][])Il2CppArrayBase<float[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<int>()) return (int[][])Il2CppArrayBase<int[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<uint>()) return (uint[][])Il2CppArrayBase<uint[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<nint>()) return (nint[][])Il2CppArrayBase<nint[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<nuint>()) return (nuint[][])Il2CppArrayBase<nuint[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<long>()) return (long[][])Il2CppArrayBase<long[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<ulong>()) return (ulong[][])Il2CppArrayBase<ulong[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<short>()) return (short[][])Il2CppArrayBase<short[]>.WrapNativeGenericArrayPointer(Data.Pointer);
                        //if (Type.GetElementType().GetElementType() == Il2CppType.Of<ushort>()) return (ushort[][])Il2CppArrayBase<ushort[]>.WrapNativeGenericArrayPointer(Data.Pointer);

                        if (Type.Name == "Object[][]") return ReadObjectArrayArray(Data);
                    }
                    else
                    {
                        if (Type.GetElementType() == Il2CppType.Of<string>()) return (string[])Il2CppArrayBase<string>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<bool>()) return (bool[])Il2CppArrayBase<bool>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<byte>()) return (byte[])Il2CppArrayBase<byte>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<sbyte>()) return (sbyte[])Il2CppArrayBase<sbyte>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<char>()) return (char[])Il2CppArrayBase<char>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<double>()) return (double[])Il2CppArrayBase<double>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<float>()) return (float[])Il2CppArrayBase<float>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<int>()) return (int[])Il2CppArrayBase<int>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<uint>()) return (uint[])Il2CppArrayBase<uint>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<nint>()) return (nint[])Il2CppArrayBase<nint>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<nuint>()) return (nuint[])Il2CppArrayBase<nuint>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<long>()) return (long[])Il2CppArrayBase<long>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<ulong>()) return (ulong[])Il2CppArrayBase<ulong>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<short>()) return (short[])Il2CppArrayBase<short>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<ushort>()) return (ushort[])Il2CppArrayBase<ushort>.WrapNativeGenericArrayPointer(Data.Pointer);

                        if (Type.GetElementType() == Il2CppType.Of<Vector2>()) return (Vector2[])Il2CppArrayBase<Vector2>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<Vector3>()) return (Vector3[])Il2CppArrayBase<Vector3>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<Vector4>()) return (Vector4[])Il2CppArrayBase<Vector4>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<Quaternion>()) return (Quaternion[])Il2CppArrayBase<Quaternion>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<Color>()) return (Color[])Il2CppArrayBase<Color>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<Color32>()) return (Color32[])Il2CppArrayBase<Color32>.WrapNativeGenericArrayPointer(Data.Pointer);

                        if (Type.GetElementType() == Il2CppType.Of<GameObject>()) return (GameObject[])Il2CppArrayBase<GameObject>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<UnityEngine.Object>()) return (UnityEngine.Object[])Il2CppArrayBase<UnityEngine.Object>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<Transform>()) return (Transform[])Il2CppArrayBase<Transform>.WrapNativeGenericArrayPointer(Data.Pointer);
                        if (Type.GetElementType() == Il2CppType.Of<VRCSDK2.VRC_SyncVideoPlayer.VideoEntry>()) return (VRCSDK2.VRC_SyncVideoPlayer.VideoEntry[])Il2CppArrayBase<VRCSDK2.VRC_SyncVideoPlayer.VideoEntry>.WrapNativeGenericArrayPointer(Data.Pointer);

                        if (Type.Name == "Object[]") return ReadObjectArray(Data);
                    }
                }

                else if (Type.Name == "Dictionary`2")
                {
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Object>>()) return ReadDictionary<string>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<bool, Il2CppSystem.Object>>()) return ReadDictionary<bool>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, Il2CppSystem.Object>>()) return ReadDictionary<byte>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<sbyte, Il2CppSystem.Object>>()) return ReadDictionary<sbyte>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<char, Il2CppSystem.Object>>()) return ReadDictionary<char>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<double, Il2CppSystem.Object>>()) return ReadDictionary<double>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<float, Il2CppSystem.Object>>()) return ReadDictionary<float>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, Il2CppSystem.Object>>()) return ReadDictionary<int>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<uint, Il2CppSystem.Object>>()) return ReadDictionary<uint>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<nint, Il2CppSystem.Object>>()) return ReadDictionary<nint>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<nuint, Il2CppSystem.Object>>()) return ReadDictionary<nuint>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<long, Il2CppSystem.Object>>()) return ReadDictionary<long>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<ulong, Il2CppSystem.Object>>()) return ReadDictionary<ulong>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<short, Il2CppSystem.Object>>()) return ReadDictionary<short>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<ushort, Il2CppSystem.Object>>()) return ReadDictionary<ushort>(Data);

                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, string>>()) return ReadDictionary<byte, string>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, bool>>()) return ReadDictionary<byte, bool>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, byte>>()) return ReadDictionary<byte, byte>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, sbyte>>()) return ReadDictionary<byte, sbyte>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, char>>()) return ReadDictionary<byte, char>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, double>>()) return ReadDictionary<byte, double>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, float>>()) return ReadDictionary<byte, float>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, int>>()) return ReadDictionary<byte, int>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, uint>>()) return ReadDictionary<byte, uint>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, nint>>()) return ReadDictionary<byte, nint>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, nuint>>()) return ReadDictionary<byte, nuint>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, long>>()) return ReadDictionary<byte, long>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, ulong>>()) return ReadDictionary<byte, ulong>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, short>>()) return ReadDictionary<byte, short>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<byte, ushort>>()) return ReadDictionary<byte, ushort>(Data);

                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, string>>()) return ReadDictionary<string, string>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, bool>>()) return ReadDictionary<string, bool>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, byte>>()) return ReadDictionary<string, byte>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, sbyte>>()) return ReadDictionary<string, sbyte>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, char>>()) return ReadDictionary<string, char>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, double>>()) return ReadDictionary<string, double>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, float>>()) return ReadDictionary<string, float>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, int>>()) return ReadDictionary<string, int>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, uint>>()) return ReadDictionary<string, uint>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, nint>>()) return ReadDictionary<string, nint>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, nuint>>()) return ReadDictionary<string, nuint>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, long>>()) return ReadDictionary<string, long>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, ulong>>()) return ReadDictionary<string, ulong>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, short>>()) return ReadDictionary<string, short>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<string, ushort>>()) return ReadDictionary<string, ushort>(Data);

                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, string>>()) return ReadDictionary<int, string>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, bool>>()) return ReadDictionary<int, bool>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, byte>>()) return ReadDictionary<int, byte>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, sbyte>>()) return ReadDictionary<int, sbyte>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, char>>()) return ReadDictionary<int, char>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, double>>()) return ReadDictionary<int, double>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, float>>()) return ReadDictionary<int, float>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, int>>()) return ReadDictionary<int, int>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, uint>>()) return ReadDictionary<int, uint>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, nint>>()) return ReadDictionary<int, nint>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, nuint>>()) return ReadDictionary<int, nuint>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, long>>()) return ReadDictionary<int, long>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, ulong>>()) return ReadDictionary<int, ulong>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, short>>()) return ReadDictionary<int, short>(Data);
                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Generic.Dictionary<int, ushort>>()) return ReadDictionary<int, ushort>(Data);
                }
                else
                {
                    if (Type == Il2CppType.Of<string>()) return Data.ToString();
                    if (Type == Il2CppType.Of<bool>()) return Data.Unbox<bool>();
                    if (Type == Il2CppType.Of<byte>()) return Data.Unbox<byte>();
                    if (Type == Il2CppType.Of<sbyte>()) return Data.Unbox<sbyte>();
                    if (Type == Il2CppType.Of<char>()) return Data.Unbox<char>();
                    if (Type == Il2CppType.Of<double>()) return Data.Unbox<double>();
                    if (Type == Il2CppType.Of<float>()) return Data.Unbox<float>();
                    if (Type == Il2CppType.Of<int>()) return Data.Unbox<int>();
                    if (Type == Il2CppType.Of<uint>()) return Data.Unbox<uint>();
                    if (Type == Il2CppType.Of<nint>()) return Data.Unbox<nint>();
                    if (Type == Il2CppType.Of<nuint>()) return Data.Unbox<nuint>();
                    if (Type == Il2CppType.Of<long>()) return Data.Unbox<long>();
                    if (Type == Il2CppType.Of<ulong>()) return Data.Unbox<ulong>();
                    if (Type == Il2CppType.Of<short>()) return Data.Unbox<short>();
                    if (Type == Il2CppType.Of<ushort>()) return Data.Unbox<ushort>();

                    if (Type == Il2CppType.Of<Vector2>()) return Data.Unbox<Vector2>();
                    if (Type == Il2CppType.Of<Vector3>()) return Data.Unbox<Vector3>();
                    if (Type == Il2CppType.Of<Vector4>()) return Data.Unbox<Vector4>();
                    if (Type == Il2CppType.Of<Quaternion>()) return Data.Unbox<Quaternion>();
                    if (Type == Il2CppType.Of<Color>()) return Data.Unbox<Color>();
                    if (Type == Il2CppType.Of<Color32>()) return Data.Unbox<Color32>();
                    if (Type == Il2CppType.Of<EnumPublicSealedvaLeCu3vUnique>()) return Data.Unbox<EnumPublicSealedvaLeCu3vUnique>();
                    //if (Type == Il2CppType.Of<EnumPublicSealedvaPoShSt4vUnique>()) return Data.Unbox<EnumPublicSealedvaPoShSt4vUnique>();
                    //if (Type == Il2CppType.Of<ObjectPublicByVoByVoByByByByByByUnique.EnumNPublicSealedvaAvFaUsStAlPrBiTaBiUnique>()) return Data.Unbox<ObjectPublicByVoByVoByByByByByByUnique.EnumNPublicSealedvaAvFaUsStAlPrBiTaBiUnique>();
                    if (Type == Il2CppType.Of<EnumPublicSealedvaNoImApPl5vUnique>()) return Data.Unbox<EnumPublicSealedvaNoImApPl5vUnique>();
                    if (Type == Il2CppType.Of<EnumPublicSealedvaCrUpDePoReCr7vUnique>()) return Data.Unbox<EnumPublicSealedvaCrUpDePoReCr7vUnique>();
                    if (Type == Il2CppType.Of<EnumPublicSealedvaCrDeUpUn5vUnique>()) return Data.Unbox<EnumPublicSealedvaCrDeUpUn5vUnique>();
                    if (Type == Il2CppType.Of<EnumPublicSealedvaAlFrGr4vUnique>()) return Data.Unbox<EnumPublicSealedvaAlFrGr4vUnique>();

                    if (Type == Il2CppType.Of<GameObject>()) return Data.Cast<GameObject>();
                    if (Type == Il2CppType.Of<UnityEngine.Object>()) return Data.Cast<UnityEngine.Object>();
                    if (Type == Il2CppType.Of<Transform>()) return Data.Cast<Transform>();
                    if (Type == Il2CppType.Of<VRCSDK2.VRC_SyncVideoPlayer.VideoEntry>()) return Data.Cast<VRCSDK2.VRC_SyncVideoPlayer.VideoEntry>();
                    if (Type == Il2CppType.Of<Json.Token>()) return Data.Cast<Json.Token>();
                    if (Type == Il2CppType.Of<VRC.SDK.Internal.Whiteboard.VRC_Presentation_Utils.InkStroke>()) return Data.Cast<VRC.SDK.Internal.Whiteboard.VRC_Presentation_Utils.InkStroke>();

                    if (Type == Il2CppType.Of<Il2CppSystem.Collections.Hashtable>()) return ReadHashtable(Data);
                    if (Type == Il2CppType.Of<ParameterDictionary>()) return ReadParamDictionary(Data);
                }

                try
                {
                    return BinarySerializer.Serialize(Data);
                }
                catch 
                {
                    Logger.LogError($"Il2CPPToManaged: Not Implemented type: {Type.FullName} in {Type.Assembly.FullName} [{Type.MetadataToken} | {Type.Pointer}]");
                    foreach (var x in Type.GetFields())
                    {
                        Logger.LogError(x.Name);
                    }
                    foreach (var x in Type.GetMethods())
                    {
                        Logger.LogError(x.Name);
                    }

                    return null;
                }
            }

            private static Dictionary<T, object> ReadDictionary<T>(Il2CppSystem.Object Data)
            {
                var Table = new Dictionary<T, object>();
                var ILTable = Data.Cast<Il2CppSystem.Collections.Generic.Dictionary<T, Il2CppSystem.Object>>();
                foreach (var Pair in ILTable)
                {
                    if (Pair.Key == null) continue;
                    T Key = Pair.Key;
                    object Value = ILToManaged(Pair.Value);
                    Table.Add(Key, Value);
                }
                return Table;
            }

            private static Dictionary<T, object> ReadDictionary<T, V>(Il2CppSystem.Object Data)
            {
                var Table = new Dictionary<T, object>();
                var ILTable = Data.Cast<Il2CppSystem.Collections.Generic.Dictionary<T, V>>();
                foreach (var Pair in ILTable)
                {
                    if (Pair.Key == null) continue;
                    T Key = Pair.Key;
                    V Value = Pair.Value;
                    Table.Add(Key, Value);
                }
                return Table;
            }

            private static Hashtable ReadHashtable(Il2CppSystem.Object Data)
            {
                var Table = new Hashtable();
                var ILTalbe = Data.Cast<Il2CppSystem.Collections.Hashtable>();

                foreach (var pair in ILTalbe._buckets)
                {
                    if (pair.key == null) continue;
                    var Value = pair.val == null ? null : ILToManaged(pair.val);
                    Table.Add(ILToManaged(pair.key), Value);
                }
                return Table;
            }

            private static Dictionary<byte, object> ReadParamDictionary(Il2CppSystem.Object Data)
            {
                var Table = new Dictionary<byte, object>();
                var ILTalbe = Data.Cast<ParameterDictionary>();

                foreach (var pair in ILTalbe.paramDict)
                {
                    var Value = pair.Value == null ? null : ILToManaged(pair.value);
                    Table.Add(pair.Key, Value);
                }

                return Table;
            }

            private static object[] ReadObjectArray(Il2CppSystem.Object Data) 
            {
                var ILArray = Data.Cast<Il2CppSystem.Array>();
                var len = ILArray.Length;
                var managedArray = new object[len];
                for (int i = 0; i < len; i++)
                {
                    var obj = ILToManaged(ILArray.GetValue(i));
                    managedArray[i] = obj;
                }
                return managedArray;
            }

            private static object[][] ReadObjectArrayArray(Il2CppSystem.Object Data)
            {
                var ILArray = Data.Cast<Il2CppSystem.Array>();
                var len = ILArray.Length;
                var managedArray = new object[len][];
                for (int i = 0; i < len; i++)
                {
                    managedArray[i] = ReadObjectArray(ILArray.GetValue(i));
                }
                return managedArray;
            }
        }
    }
}
