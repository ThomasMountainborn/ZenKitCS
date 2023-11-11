using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ZenKit
{
	public enum DaedalusDataType
	{
		Void = 0,
		Float = 1,
		Int = 2,
		String = 3,
		Class = 4,
		Function = 5,
		Prototype = 6,
		Instance = 7
	}

	public enum DaedalusOpcode
	{
		Add = 0,
		Sub = 1,
		Mul = 2,
		Div = 3,
		Mod = 4,
		Or = 5,
		Andb = 6,
		Lt = 7,
		Gt = 8,
		Movi = 9,
		Orr = 11,
		And = 12,
		Lsl = 13,
		Lsr = 14,
		Lte = 15,
		Eq = 16,
		Neq = 17,
		Gte = 18,
		AddMovi = 19,
		SubMovi = 20,
		MulMovi = 21,
		DivMovi = 22,
		Plus = 30,
		Negate = 31,
		Not = 32,
		Cmpl = 33,
		Nop = 45,
		Rsr = 60,
		Bl = 61,
		Be = 62,
		Pushi = 64,
		Pushv = 65,
		Pushvi = 67,
		Movs = 70,
		Movss = 71,
		Movvf = 72,
		Movf = 73,
		Movvi = 74,
		B = 75,
		Bz = 76,
		Gmovi = 80,
		Pushvv = 245
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct DaedalusInstruction
	{
		[FieldOffset(0)] public DaedalusOpcode Opcode;

		[FieldOffset(4)] public byte size;

		[FieldOffset(8)] private uint opAddress;

		[FieldOffset(8)] private uint opSymbol;

		[FieldOffset(8)] private int opImmediateInt;

		[FieldOffset(8)] private float opImmediateFloat;

		[FieldOffset(12)] public byte opIndex;
	}

	public class DaedalusSymbol
	{
		public DaedalusSymbol(UIntPtr handle)
		{
			Handle = handle;
		}

		public UIntPtr Handle { get; }

		public bool IsConst => Native.ZkDaedalusSymbol_getIsConst(Handle);
		public bool IsMember => Native.ZkDaedalusSymbol_getIsMember(Handle);
		public bool IsExternal => Native.ZkDaedalusSymbol_getIsExternal(Handle);
		public bool IsMerged => Native.ZkDaedalusSymbol_getIsMerged(Handle);
		public bool IsGenerated => Native.ZkDaedalusSymbol_getIsGenerated(Handle);
		public bool HasReturn => Native.ZkDaedalusSymbol_getHasReturn(Handle);
		public string Name => Native.ZkDaedalusSymbol_getName(Handle).MarshalAsString() ?? string.Empty;
		public int Address => Native.ZkDaedalusSymbol_getAddress(Handle);
		public int Parent => Native.ZkDaedalusSymbol_getParent(Handle);
		public int Size => Native.ZkDaedalusSymbol_getSize(Handle);
		public DaedalusDataType Type => Native.ZkDaedalusSymbol_getType(Handle);
		public uint Index => Native.ZkDaedalusSymbol_getIndex(Handle);
		public DaedalusDataType ReturnType => Native.ZkDaedalusSymbol_getReturnType(Handle);

		public string GetString(ushort index, DaedalusInstance? context = null)
		{
			return Native.ZkDaedalusSymbol_getString(Handle, index, context?.Handle ?? UIntPtr.Zero)
				       .MarshalAsString() ??
			       string.Empty;
		}

		public float GetFloat(ushort index, DaedalusInstance? context = null)
		{
			return Native.ZkDaedalusSymbol_getFloat(Handle, index, context?.Handle ?? UIntPtr.Zero);
		}

		public int GetInt(ushort index, DaedalusInstance? context = null)
		{
			return Native.ZkDaedalusSymbol_getInt(Handle, index, context?.Handle ?? UIntPtr.Zero);
		}

		public void SetString(string value, ushort index, DaedalusInstance? context = null)
		{
			Native.ZkDaedalusSymbol_setString(Handle, value, index, context?.Handle ?? UIntPtr.Zero);
		}

		public void SetFloat(float value, ushort index, DaedalusInstance? context = null)
		{
			Native.ZkDaedalusSymbol_setFloat(Handle, value, index, context?.Handle ?? UIntPtr.Zero);
		}

		public void SetInt(int value, ushort index, DaedalusInstance? context = null)
		{
			Native.ZkDaedalusSymbol_setInt(Handle, value, index, context?.Handle ?? UIntPtr.Zero);
		}
	}

	public class DaedalusScript
	{
		internal readonly UIntPtr Handle;

		public DaedalusScript(string path)
		{
			Handle = Native.ZkDaedalusScript_loadPath(path);
			if (Handle == UIntPtr.Zero) throw new Exception("Failed to load DaedalusScript");
		}

		public DaedalusScript(Read r)
		{
			Handle = Native.ZkDaedalusScript_load(r.Handle);
			if (Handle == UIntPtr.Zero) throw new Exception("Failed to load DaedalusScript");
		}

		public DaedalusScript(Vfs vfs, string name)
		{
			Handle = Native.ZkDaedalusScript_loadVfs(vfs.Handle, name);
			if (Handle == UIntPtr.Zero) throw new Exception("Failed to load DaedalusScript");
		}

		internal DaedalusScript(UIntPtr handle)
		{
			Handle = handle;
		}

		public uint SymbolCount => Native.ZkDaedalusScript_getSymbolCount(Handle);

		public List<DaedalusSymbol> Symbols
		{
			get
			{
				var symbols = new List<DaedalusSymbol>();

				Native.ZkDaedalusScript_enumerateSymbols(Handle, (_, symbol) =>
				{
					symbols.Add(new DaedalusSymbol(symbol));
					return false;
				}, UIntPtr.Zero);

				return symbols;
			}
		}

		protected virtual void Delete()
		{
			Native.ZkDaedalusScript_del(Handle);
		}

		~DaedalusScript()
		{
			Delete();
		}

		public List<DaedalusSymbol> GetInstanceSymbols(string className)
		{
			var symbols = new List<DaedalusSymbol>();

			Native.ZkDaedalusScript_enumerateInstanceSymbols(Handle, className, (_, symbol) =>
			{
				symbols.Add(new DaedalusSymbol(symbol));
				return false;
			}, UIntPtr.Zero);

			return symbols;
		}

		public DaedalusInstruction GetInstruction(ulong address)
		{
			return Native.ZkDaedalusScript_getInstruction(Handle, address);
		}

		public DaedalusSymbol? GetSymbolByIndex(uint index)
		{
			var sym = Native.ZkDaedalusScript_getSymbolByIndex(Handle, index);
			return sym == UIntPtr.Zero ? null : new DaedalusSymbol(sym);
		}

		public DaedalusSymbol? GetSymbolByAddress(ulong address)
		{
			var sym = Native.ZkDaedalusScript_getSymbolByAddress(Handle, address);
			return sym == UIntPtr.Zero ? null : new DaedalusSymbol(sym);
		}

		public DaedalusSymbol? GetSymbolByName(string name)
		{
			var sym = Native.ZkDaedalusScript_getSymbolByName(Handle, name);
			return sym == UIntPtr.Zero ? null : new DaedalusSymbol(sym);
		}
	}
}