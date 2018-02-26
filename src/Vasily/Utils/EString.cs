using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Versioning;

#if BIT64
    using nuint = System.UInt64;
#else // BIT64
using nuint = System.UInt32;
#endif // BIT64

namespace System
{

    public delegate string AllocateStringDelegate(int length);

    /// <summary>
    /// 扩展string类，提高字符串拼接性能
    /// </summary>
    public class EString
    {
        internal static readonly AllocateStringDelegate FastAllocateString;

        internal static int CharSize;

        private string _value;
        static EString()
        {
            CharSize = sizeof(char);
            MethodInfo fastAllocateString = typeof(string).GetMethod("FastAllocateString", BindingFlags.NonPublic | BindingFlags.Static);
            FastAllocateString = (AllocateStringDelegate)Delegate.CreateDelegate(typeof(AllocateStringDelegate), fastAllocateString);
        }

        public EString(string value)
        {
            this._value = value;
        }

        public string Remove(int start, int length)
        {
            return _value.Remove(start, length);
        }
        public string Replace(string old,string reStr)
        {
            return _value.Replace(old,reStr);
        }
        public string Replace(EString old, string reStr)
        {
            return _value.Replace(old._value, reStr);
        }
        public override string ToString()
        {
            return _value;
        }

        public static implicit operator EString(string value)
        {
            return new EString(value);
        }
        public static EString operator +(EString source, long dest)
        {
            source.ContactString(dest.ToString());
            return source;
        }
        public static EString operator +(EString source, ulong dest)
        {
            source.ContactString(dest.ToString());
            return source;
        }
        public static EString operator +(EString source, short dest)
        {
            source.ContactString(dest.ToString());
            return source;
        }
        public static EString operator +(EString source, ushort dest)
        {
            source.ContactString(dest.ToString());
            return source;
        }
        public static EString operator +(EString source, double dest)
        {
            source.ContactString(dest.ToString());
            return source;
        }
        public static EString operator +(EString source, float dest)
        {
            source.ContactString(dest.ToString());
            return source;
        }
        public static EString operator +(EString source, byte dest)
        {
            source.ContactString(dest.ToString());
            return source;
        }
        public static EString operator +(EString source, sbyte dest)
        {
            source.ContactString(dest.ToString());
            return source;
        }
        public static EString operator +(EString source, int dest)
        {
            source.ContactString(dest.ToString());
            return source;
        }
        public static EString operator +(EString source, uint dest)
        {
            source.ContactString(dest.ToString());
            return source;
        }
        public static EString operator +(EString source, decimal dest)
        {
            source.ContactString(dest.ToString());
            return source;
        }
        public static EString operator +(EString source, DateTime dest)
        {
            source.ContactString(dest.ToString("yyyy-MM-dd HH:mm:ss"));
            return source;
        }
        public static EString operator +(EString source,string dest)
        {
            source.ContactString(dest);
            return source;
        }
        public static EString operator +(EString source, EString dest)
        {
            source.ContactString(dest._value);
            return source;
        }


        [System.Security.SecurityCritical]
        public unsafe static string Contact(params string[] values)
        {
            int length = 0;
            int count = values.Length;
            for (int i = 0; i < count; i += 1)
            {
                length += values[i].Length;
            }
            string joinString = FastAllocateString(length);

            fixed (char* pointerToResult = joinString)
            {
                int step = 0;
                for (int i = 0; i < count; i += 1)
                {
                    fixed (char* pointerToTempString = values[i])
                    {
                        Memmove((byte*)(pointerToResult + step), (byte*)pointerToTempString, (uint)(values[i].Length * CharSize));
                    }
                    step += values[i].Length;
                }
            }

            return joinString;
        }

        [System.Security.SecurityCritical]
        public unsafe string Append(params string[] values)
        {
            int length = 0;
            int count = values.Length;
            for (int i = 0; i < count; i += 1)
            {
                length += values[i].Length;
            }
            string joinString = FastAllocateString(length);

            fixed (char* pointerToResult = joinString)
            {
                int step = 0;
                for (int i = 0; i < count; i += 1)
                {
                    fixed (char* pointerToTempString = values[i])
                    {
                        Memmove((byte*)(pointerToResult + step), (byte*)pointerToTempString, (uint)(values[i].Length * CharSize));
                    }
                    step += values[i].Length;
                }
            }

            return ContactString(joinString);
        }

        [System.Security.SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe string ContactString(string newString)
        {
            int newLength = newString.Length;
            int oldLength = _value.Length;
            string joinString = FastAllocateString(oldLength + newLength);
            fixed (char* pointerToResult = joinString)
            {
                if (oldLength != 0)
                {
                    fixed (char* pointerToOldString = _value)
                    {
                        Memmove((byte*)(pointerToResult), (byte*)pointerToOldString, (uint)(oldLength * CharSize));
                    }
                }
                fixed (char* pointerToNewString = newString)
                {
                    Memmove((byte*)(pointerToResult + oldLength), (byte*)pointerToNewString, (uint)(newLength * CharSize));
                }
            }
            _value = joinString;
            return joinString;
        }

        [System.Security.SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // This method has different signature for x64 and other platforms and is done for performance reasons.
        internal unsafe static void Memmove(byte* dest, byte* src, nuint len)
        {
#if AMD64 || (BIT32 && !ARM)
            const nuint CopyThreshold = 2048;
#elif ARM64
#if PLATFORM_WINDOWS
            // Determined optimal value for Windows.
            // https://github.com/dotnet/coreclr/issues/13843
            const nuint CopyThreshold = UInt64.MaxValue;
#else // PLATFORM_WINDOWS
            // Managed code is currently faster than glibc unoptimized memmove
            // TODO-ARM64-UNIX-OPT revisit when glibc optimized memmove is in Linux distros
            // https://github.com/dotnet/coreclr/issues/13844
            const nuint CopyThreshold = UInt64.MaxValue;
#endif // PLATFORM_WINDOWS
#else
            const nuint CopyThreshold = 512;
#endif // AMD64 || (BIT32 && !ARM)

            // P/Invoke into the native version when the buffers are overlapping.

            if (((nuint)dest - (nuint)src < len) || ((nuint)src - (nuint)dest < len)) goto PInvoke;

            byte* srcEnd = src + len;
            byte* destEnd = dest + len;

            if (len <= 16) goto MCPY02;
            if (len > 64) goto MCPY05;

            MCPY00:
            // Copy bytes which are multiples of 16 and leave the remainder for MCPY01 to handle.
#if HAS_CUSTOM_BLOCKS
            *(Block16*)dest = *(Block16*)src;                   // [0,16]
#elif BIT64
            *(long*)dest = *(long*)src;
            *(long*)(dest + 8) = *(long*)(src + 8);             // [0,16]
#else
            *(int*)dest = *(int*)src;
            *(int*)(dest + 4) = *(int*)(src + 4);
            *(int*)(dest + 8) = *(int*)(src + 8);
            *(int*)(dest + 12) = *(int*)(src + 12);             // [0,16]
#endif
            if (len <= 32) goto MCPY01;
#if HAS_CUSTOM_BLOCKS
            *(Block16*)(dest + 16) = *(Block16*)(src + 16);     // [0,32]
#elif BIT64
            *(long*)(dest + 16) = *(long*)(src + 16);
            *(long*)(dest + 24) = *(long*)(src + 24);           // [0,32]
#else
            *(int*)(dest + 16) = *(int*)(src + 16);
            *(int*)(dest + 20) = *(int*)(src + 20);
            *(int*)(dest + 24) = *(int*)(src + 24);
            *(int*)(dest + 28) = *(int*)(src + 28);             // [0,32]
#endif
            if (len <= 48) goto MCPY01;
#if HAS_CUSTOM_BLOCKS
            *(Block16*)(dest + 32) = *(Block16*)(src + 32);     // [0,48]
#elif BIT64
            *(long*)(dest + 32) = *(long*)(src + 32);
            *(long*)(dest + 40) = *(long*)(src + 40);           // [0,48]
#else
            *(int*)(dest + 32) = *(int*)(src + 32);
            *(int*)(dest + 36) = *(int*)(src + 36);
            *(int*)(dest + 40) = *(int*)(src + 40);
            *(int*)(dest + 44) = *(int*)(src + 44);             // [0,48]
#endif

            MCPY01:
            // Unconditionally copy the last 16 bytes using destEnd and srcEnd and return.
#if HAS_CUSTOM_BLOCKS
            *(Block16*)(destEnd - 16) = *(Block16*)(srcEnd - 16);
#elif BIT64
            *(long*)(destEnd - 16) = *(long*)(srcEnd - 16);
            *(long*)(destEnd - 8) = *(long*)(srcEnd - 8);
#else
            *(int*)(destEnd - 16) = *(int*)(srcEnd - 16);
            *(int*)(destEnd - 12) = *(int*)(srcEnd - 12);
            *(int*)(destEnd - 8) = *(int*)(srcEnd - 8);
            *(int*)(destEnd - 4) = *(int*)(srcEnd - 4);
#endif
            return;

            MCPY02:
            // Copy the first 8 bytes and then unconditionally copy the last 8 bytes and return.
            if ((len & 24) == 0) goto MCPY03;
#if BIT64
            *(long*)dest = *(long*)src;
            *(long*)(destEnd - 8) = *(long*)(srcEnd - 8);
#else
            *(int*)dest = *(int*)src;
            *(int*)(dest + 4) = *(int*)(src + 4);
            *(int*)(destEnd - 8) = *(int*)(srcEnd - 8);
            *(int*)(destEnd - 4) = *(int*)(srcEnd - 4);
#endif
            return;

            MCPY03:
            // Copy the first 4 bytes and then unconditionally copy the last 4 bytes and return.
            if ((len & 4) == 0) goto MCPY04;
            *(int*)dest = *(int*)src;
            *(int*)(destEnd - 4) = *(int*)(srcEnd - 4);
            return;

            MCPY04:
            // Copy the first byte. For pending bytes, do an unconditionally copy of the last 2 bytes and return.
            if (len == 0) return;
            *dest = *src;
            if ((len & 2) == 0) return;
            *(short*)(destEnd - 2) = *(short*)(srcEnd - 2);
            return;

            MCPY05:
            // PInvoke to the native version when the copy length exceeds the threshold.
            if (len > CopyThreshold)
            {
                goto PInvoke;
            }
            // Copy 64-bytes at a time until the remainder is less than 64.
            // If remainder is greater than 16 bytes, then jump to MCPY00. Otherwise, unconditionally copy the last 16 bytes and return.
            nuint n = len >> 6;

            MCPY06:
#if HAS_CUSTOM_BLOCKS
            *(Block64*)dest = *(Block64*)src;
#elif BIT64
            *(long*)dest = *(long*)src;
            *(long*)(dest + 8) = *(long*)(src + 8);
            *(long*)(dest + 16) = *(long*)(src + 16);
            *(long*)(dest + 24) = *(long*)(src + 24);
            *(long*)(dest + 32) = *(long*)(src + 32);
            *(long*)(dest + 40) = *(long*)(src + 40);
            *(long*)(dest + 48) = *(long*)(src + 48);
            *(long*)(dest + 56) = *(long*)(src + 56);
#else
            *(int*)dest = *(int*)src;
            *(int*)(dest + 4) = *(int*)(src + 4);
            *(int*)(dest + 8) = *(int*)(src + 8);
            *(int*)(dest + 12) = *(int*)(src + 12);
            *(int*)(dest + 16) = *(int*)(src + 16);
            *(int*)(dest + 20) = *(int*)(src + 20);
            *(int*)(dest + 24) = *(int*)(src + 24);
            *(int*)(dest + 28) = *(int*)(src + 28);
            *(int*)(dest + 32) = *(int*)(src + 32);
            *(int*)(dest + 36) = *(int*)(src + 36);
            *(int*)(dest + 40) = *(int*)(src + 40);
            *(int*)(dest + 44) = *(int*)(src + 44);
            *(int*)(dest + 48) = *(int*)(src + 48);
            *(int*)(dest + 52) = *(int*)(src + 52);
            *(int*)(dest + 56) = *(int*)(src + 56);
            *(int*)(dest + 60) = *(int*)(src + 60);
#endif
            dest += 64;
            src += 64;
            n--;
            if (n != 0) goto MCPY06;

            len %= 64;
            if (len > 16) goto MCPY00;
#if HAS_CUSTOM_BLOCKS
            *(Block16*)(destEnd - 16) = *(Block16*)(srcEnd - 16);
#elif BIT64
            *(long*)(destEnd - 16) = *(long*)(srcEnd - 16);
            *(long*)(destEnd - 8) = *(long*)(srcEnd - 8);
#else
            *(int*)(destEnd - 16) = *(int*)(srcEnd - 16);
            *(int*)(destEnd - 12) = *(int*)(srcEnd - 12);
            *(int*)(destEnd - 8) = *(int*)(srcEnd - 8);
            *(int*)(destEnd - 4) = *(int*)(srcEnd - 4);
#endif
            return;

            PInvoke:

            if (dest <= src || dest >= (src + len))
            {
                //Non-Overlapping Buffers
                //copy from lower addresses to higher addresses

                while (len > 0)
                {
                    *dest++ = *src++;
                    len -= 1;
                }
            }
            else
            {
                //Overlapping Buffers
                //copy from higher addresses to lower addresses
                dest += len - 1;
                src += len - 1;

                while (len > 0)
                {
                    *dest-- = *src--;
                    len -= 1;
                }
            }
        }
    }
}