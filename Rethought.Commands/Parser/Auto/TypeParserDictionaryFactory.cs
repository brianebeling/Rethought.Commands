using System;
using System.Collections.Generic;
using Optional;

namespace Rethought.Commands.Parser.Auto
{
    public static class TypeParserDictionaryFactory
    {
        public static Dictionary<Type, ITypeParser<string, object>> CreateDefault()
        {
            return new Dictionary<Type, ITypeParser<string, object>>()
            {
                {typeof(string), new ObjectTypeParserWrapper<string, string>(Func<string, string>.Create(Option.Some))},
                {typeof(uint), new ObjectTypeParserWrapper<string, uint>(Func<string, uint>.Create(s => Option.Some(uint.Parse(s))))},
                {typeof(int),  new ObjectTypeParserWrapper<string, int>(Func<string, int>.Create(s => Option.Some(int.Parse(s))))},
                {typeof(long),  new ObjectTypeParserWrapper<string, long>(Func<string, long>.Create(s => Option.Some(long.Parse(s))))},
                {typeof(ulong),  new ObjectTypeParserWrapper<string, ulong>(Func<string, ulong>.Create(s => Option.Some(ulong.Parse(s))))},
                {typeof(double),  new ObjectTypeParserWrapper<string, double>(Func<string, double>.Create(s => Option.Some(double.Parse(s))))},
                {typeof(bool),  new ObjectTypeParserWrapper<string, bool>(Func<string, bool>.Create(s => Option.Some(bool.Parse(s))))},
                {typeof(byte),  new ObjectTypeParserWrapper<string, byte>(Func<string, byte>.Create(s => Option.Some(byte.Parse(s))))},
                {typeof(sbyte),  new ObjectTypeParserWrapper<string, sbyte>(Func<string, sbyte>.Create(s => Option.Some(sbyte.Parse(s))))},
                {typeof(char),  new ObjectTypeParserWrapper<string, char>(Func<string, char>.Create(s => Option.Some(char.Parse(s))))},
                {typeof(decimal),  new ObjectTypeParserWrapper<string, decimal>(Func<string, decimal>.Create(s => Option.Some(decimal.Parse(s))))},
                {typeof(float),  new ObjectTypeParserWrapper<string, float>(Func<string, float>.Create(s => Option.Some(float.Parse(s))))},
                {typeof(object),  new ObjectTypeParserWrapper<string, object>(Func<string, object>.Create(Option.Some<object>))},
                {typeof(short),  new ObjectTypeParserWrapper<string, short>(Func<string, short>.Create(s => Option.Some(short.Parse(s))))},
                {typeof(ushort),  new ObjectTypeParserWrapper<string, ushort>(Func<string, ushort>.Create(s => Option.Some(ushort.Parse(s))))},       
            };
        }
    }
}