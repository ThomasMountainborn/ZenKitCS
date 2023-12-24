using System;
using System.Collections.Generic;
using ZenKit.Daedalus;

namespace ZenKit
{
	public class DaedalusVm : DaedalusScript
	{
		public delegate void ExternalDefaultFunction(DaedalusVm vm, DaedalusSymbol sym);

		public delegate TR ExternalFunc<out TR>();

		public delegate TR ExternalFunc<out TR, in TP0>(TP0 p0);

		public delegate TR ExternalFunc<out TR, in TP0, in TP1>(TP0 p0, TP1 p1);

		public delegate TR ExternalFunc<out TR, in TP0, in TP1, in TP2>(TP0 p0, TP1 p1, TP2 p2);

		public delegate TR ExternalFunc<out TR, in TP0, in TP1, in TP2, in TP3>(TP0 p0, TP1 p1, TP2 p2, TP3 p3);

		public delegate TR ExternalFunc<out TR, in TP0, in TP1, in TP2, in TP3, in TP4>(TP0 p0, TP1 p1, TP2 p2, TP3 p3,
			TP4 p4);

		public delegate void ExternalFuncV();

		public delegate void ExternalFuncV<in TP0>(TP0 p0);

		public delegate void ExternalFuncV<in TP0, in TP1>(TP0 p0, TP1 p1);

		public delegate void ExternalFuncV<in TP0, in TP1, in TP2>(TP0 p0, TP1 p1, TP2 p2);

		public delegate void ExternalFuncV<in TP0, in TP1, in TP2, in TP3>(TP0 p0, TP1 p1, TP2 p2, TP3 p3);

		public delegate void ExternalFuncV<in TP0, in TP1, in TP2, in TP3, in TP4>(TP0 p0, TP1 p1, TP2 p2, TP3 p3,
			TP4 p4);

		public DaedalusVm(string path) : base(Native.ZkDaedalusVm_loadPath(path))
		{
			if (Handle == UIntPtr.Zero) throw new Exception("Failed to load DaedalusVm");
		}

		public DaedalusVm(Read r) : base(Native.ZkDaedalusVm_load(r.Handle))
		{
			if (Handle == UIntPtr.Zero) throw new Exception("Failed to load DaedalusVm");
		}

		public DaedalusVm(Vfs vfs, string name) : base(Native.ZkDaedalusVm_loadVfs(vfs.Handle, name))
		{
			if (Handle == UIntPtr.Zero) throw new Exception("Failed to load DaedalusVm");
		}

		public DaedalusInstance? GlobalSelf
		{
			get => DaedalusInstance.FromNative(Native.ZkDaedalusVm_getGlobalSelf(Handle));
			set => Native.ZkDaedalusVm_setGlobalSelf(Handle, value?.Handle ?? UIntPtr.Zero);
		}

		public DaedalusInstance? GlobalOther
		{
			get => DaedalusInstance.FromNative(Native.ZkDaedalusVm_getGlobalOther(Handle));
			set => Native.ZkDaedalusVm_setGlobalOther(Handle, value?.Handle ?? UIntPtr.Zero);
		}

		public DaedalusInstance? GlobalVictim
		{
			get => DaedalusInstance.FromNative(Native.ZkDaedalusVm_getGlobalVictim(Handle));
			set => Native.ZkDaedalusVm_setGlobalVictim(Handle, value?.Handle ?? UIntPtr.Zero);
		}

		public DaedalusInstance? GlobalHero
		{
			get => DaedalusInstance.FromNative(Native.ZkDaedalusVm_getGlobalHero(Handle));
			set => Native.ZkDaedalusVm_setGlobalHero(Handle, value?.Handle ?? UIntPtr.Zero);
		}

		public DaedalusInstance? GlobalItem
		{
			get => DaedalusInstance.FromNative(Native.ZkDaedalusVm_getGlobalItem(Handle));
			set => Native.ZkDaedalusVm_setGlobalItem(Handle, value?.Handle ?? UIntPtr.Zero);
		}

		protected override void Delete()
		{
			Native.ZkDaedalusVm_del(Handle);
		}

		public void PrintStackTrace()
		{
			Native.ZkDaedalusVm_printStackTrace(Handle);
		}

		public T InitInstance<T>(string symbolName)
		{
			var sym = GetSymbolByName(symbolName);
			if (sym == null) throw new Exception("Symbol not found");
			return InitInstance<T>(sym);
		}

		public T InitInstance<T>(DaedalusSymbol symbol)
		{
			DaedalusInstanceType type;

			if (typeof(T) == typeof(GuildValuesInstance)) type = DaedalusInstanceType.GuildValues;
			else if (typeof(T) == typeof(NpcInstance)) type = DaedalusInstanceType.Npc;
			else if (typeof(T) == typeof(MissionInstance)) type = DaedalusInstanceType.Mission;
			else if (typeof(T) == typeof(ItemInstance)) type = DaedalusInstanceType.Item;
			else if (typeof(T) == typeof(FocusInstance)) type = DaedalusInstanceType.Focus;
			else if (typeof(T) == typeof(InfoInstance)) type = DaedalusInstanceType.Info;
			else if (typeof(T) == typeof(ItemReactInstance)) type = DaedalusInstanceType.ItemReact;
			else if (typeof(T) == typeof(SpellInstance)) type = DaedalusInstanceType.Spell;
			else if (typeof(T) == typeof(MenuInstance)) type = DaedalusInstanceType.Menu;
			else if (typeof(T) == typeof(MenuItemInstance)) type = DaedalusInstanceType.MenuItem;
			else if (typeof(T) == typeof(CameraInstance)) type = DaedalusInstanceType.Camera;
			else if (typeof(T) == typeof(MusicSystemInstance)) type = DaedalusInstanceType.MusicSystem;
			else if (typeof(T) == typeof(MusicThemeInstance)) type = DaedalusInstanceType.MusicTheme;
			else if (typeof(T) == typeof(MusicJingleInstance)) type = DaedalusInstanceType.MusicJingle;
			else if (typeof(T) == typeof(ParticleEffectInstance)) type = DaedalusInstanceType.ParticleEffect;
			else if (typeof(T) == typeof(EffectBaseInstance)) type = DaedalusInstanceType.EffectBase;
			else if (typeof(T) == typeof(ParticleEffectEmitKeyInstance))
				type = DaedalusInstanceType.ParticleEffectEmitKey;
			else if (typeof(T) == typeof(FightAiInstance)) type = DaedalusInstanceType.FightAi;
			else if (typeof(T) == typeof(SoundEffectInstance)) type = DaedalusInstanceType.SoundEffect;
			else if (typeof(T) == typeof(SoundSystemInstance)) type = DaedalusInstanceType.SoundSystem;
			else if (typeof(T) == typeof(InvalidOperationException)) type = DaedalusInstanceType.Svm;
			else throw new NotSupportedException("Must be DaedalusInstance");

			var ptr = DaedalusInstance.FromNative(Native.ZkDaedalusVm_initInstance(Handle, symbol.Handle, type)) ??
			          throw new InvalidOperationException();
			return (T)(object)ptr;
		}

		public void Call(string name)
		{
			var sym = GetSymbolByName(name);
			if (!(sym is { Type: DaedalusDataType.Function })) throw new Exception("Symbol not found");
			Native.ZkDaedalusVm_callFunction(Handle, sym.Handle);
		}

		public TR Call<TR>(string name)
		{
			var sym = GetSymbolByName(name);
			if (!(sym is { Type: DaedalusDataType.Function })) throw new Exception("Symbol not found");
			Native.ZkDaedalusVm_callFunction(Handle, sym.Handle);

			if (!sym.HasReturn) throw new InvalidOperationException("The function does not return anything!");
			return Pop<TR>();
		}

		public TR Call<TR, TP0>(string name, TP0 p0)
		{
			Push(p0);
			return Call<TR>(name);
		}

		public TR Call<TR, TP0, TP1>(string name, TP0 p0, TP1 p1)
		{
			Push(p0);
			Push(p1);
			return Call<TR>(name);
		}

		public TR Call<TR, TP0, TP1, TP2>(string name, TP0 p0, TP1 p1, TP2 p2)
		{
			Push(p0);
			Push(p1);
			Push(p2);
			return Call<TR>(name);
		}

		public TR Call<TR, TP0, TP1, TP2, TP3>(string name, TP0 p0, TP1 p1, TP2 p2, TP3 p3)
		{
			Push(p0);
			Push(p1);
			Push(p2);
			Push(p3);
			return Call<TR>(name);
		}

		public void Call<TP0>(string name, TP0 p0)
		{
			Push(p0);
			Call(name);
		}

		public void Call<TP0, TP1>(string name, TP0 p0, TP1 p1)
		{
			Push(p0);
			Push(p1);
			Call(name);
		}

		public void Call<TP0, TP1, TP2>(string name, TP0 p0, TP1 p1, TP2 p2)
		{
			Push(p0);
			Push(p1);
			Push(p2);
			Call(name);
		}

		public void Call<TP0, TP1, TP2, TP3>(string name, TP0 p0, TP1 p1, TP2 p2, TP3 p3)
		{
			Push(p0);
			Push(p1);
			Push(p2);
			Push(p3);
			Call(name);
		}

		private Native.Callbacks.ZkDaedalusVmExternalDefaultCallback? _externalDefaultCallback = null;

		public void RegisterExternalDefault(ExternalDefaultFunction cb)
		{
			_externalDefaultCallback = (_0, _1, sym) => cb(this, new DaedalusSymbol(sym));
			Native.ZkDaedalusVm_registerExternalDefault(Handle, _externalDefaultCallback, UIntPtr.Zero);
		}

		public void RegisterExternal<TR>(string name, ExternalFunc<TR> cb)
		{
			RegisterExternalUnsafe(name, () => Push(cb()));
		}

		public void RegisterExternal<TR, TP0>(string name, ExternalFunc<TR, TP0> cb)
		{
			RegisterExternalUnsafe(name, () =>
			{
				var p0 = Pop<TP0>();
				Push(cb(p0));
			});
		}

		public void RegisterExternal<TR, TP0, TP1>(string name, ExternalFunc<TR, TP0, TP1> cb)
		{
			RegisterExternalUnsafe(name, () =>
			{
				var p1 = Pop<TP1>();
				var p0 = Pop<TP0>();
				Push(cb(p0, p1));
			});
		}

		public void RegisterExternal<TR, TP0, TP1, TP2>(string name, ExternalFunc<TR, TP0, TP1, TP2> cb)
		{
			RegisterExternalUnsafe(name, () =>
			{
				var p2 = Pop<TP2>();
				var p1 = Pop<TP1>();
				var p0 = Pop<TP0>();
				Push(cb(p0, p1, p2));
			});
		}

		public void RegisterExternal<TR, TP0, TP1, TP2, TP3>(string name, ExternalFunc<TR, TP0, TP1, TP2, TP3> cb)
		{
			RegisterExternalUnsafe(name, () =>
			{
				var p3 = Pop<TP3>();
				var p2 = Pop<TP2>();
				var p1 = Pop<TP1>();
				var p0 = Pop<TP0>();
				Push(cb(p0, p1, p2, p3));
			});
		}

		public void RegisterExternal<TR, TP0, TP1, TP2, TP3, TP4>(string name,
			ExternalFunc<TR, TP0, TP1, TP2, TP3, TP4> cb)
		{
			RegisterExternalUnsafe(name, () =>
			{
				var p4 = Pop<TP4>();
				var p3 = Pop<TP3>();
				var p2 = Pop<TP2>();
				var p1 = Pop<TP1>();
				var p0 = Pop<TP0>();
				Push(cb(p0, p1, p2, p3, p4));
			});
		}


		public void RegisterExternal(string name, ExternalFuncV cb)
		{
			RegisterExternalUnsafe(name, () => cb());
		}

		public void RegisterExternal<TP0>(string name, ExternalFuncV<TP0> cb)
		{
			RegisterExternalUnsafe(name, () =>
			{
				var p0 = Pop<TP0>();
				cb(p0);
			});
		}

		public void RegisterExternal<TP0, TP1>(string name, ExternalFuncV<TP0, TP1> cb)
		{
			RegisterExternalUnsafe(name, () =>
			{
				var p1 = Pop<TP1>();
				var p0 = Pop<TP0>();
				cb(p0, p1);
			});
		}

		public void RegisterExternal<TP0, TP1, TP2>(string name, ExternalFuncV<TP0, TP1, TP2> cb)
		{
			RegisterExternalUnsafe(name, () =>
			{
				var p2 = Pop<TP2>();
				var p1 = Pop<TP1>();
				var p0 = Pop<TP0>();
				cb(p0, p1, p2);
			});
		}

		public void RegisterExternal<TP0, TP1, TP2, TP3>(string name, ExternalFuncV<TP0, TP1, TP2, TP3> cb)
		{
			RegisterExternalUnsafe(name, () =>
			{
				var p3 = Pop<TP3>();
				var p2 = Pop<TP2>();
				var p1 = Pop<TP1>();
				var p0 = Pop<TP0>();
				cb(p0, p1, p2, p3);
			});
		}

		public void RegisterExternal<TP0, TP1, TP2, TP3, TP4>(string name, ExternalFuncV<TP0, TP1, TP2, TP3, TP4> cb)
		{
			RegisterExternalUnsafe(name, () =>
			{
				var p4 = Pop<TP4>();
				var p3 = Pop<TP3>();
				var p2 = Pop<TP2>();
				var p1 = Pop<TP1>();
				var p0 = Pop<TP0>();
				cb(p0, p1, p2, p3, p4);
			});
		}

		private void Push<T>(T value)
		{
			switch (value)
			{
				case string v:
					Native.ZkDaedalusVm_pushString(Handle, v);
					break;
				case uint v:
					Native.ZkDaedalusVm_pushInt(Handle, (int)v);
					break;
				case int v:
					Native.ZkDaedalusVm_pushInt(Handle, v);
					break;
				case bool v:
					Native.ZkDaedalusVm_pushInt(Handle, v ? 1 : 0);
					break;
				case float v:
					Native.ZkDaedalusVm_pushFloat(Handle, v);
					break;
				case DaedalusInstance v:
					Native.ZkDaedalusVm_pushInstance(Handle, v.Handle);
					break;
				default:
					throw new InvalidOperationException("Unsupported type: " + value?.GetType());
			}
		}

		private T Pop<T>()
		{
			if (typeof(T) == typeof(string))
				return (T)(object)(Native.ZkDaedalusVm_popString(Handle).MarshalAsString() ?? string.Empty);
			if (typeof(T) == typeof(int) || typeof(T) == typeof(bool))
				return (T)(object)Native.ZkDaedalusVm_popInt(Handle);
			if (typeof(T) == typeof(float)) return (T)(object)Native.ZkDaedalusVm_popFloat(Handle);
			if (typeof(T).IsSubclassOf(typeof(DaedalusInstance)))
				return (T)(object)DaedalusInstance.FromNative(Native.ZkDaedalusVm_popInstance(Handle));
			if (typeof(T) == typeof(void)) return (T)new object();

			throw new InvalidOperationException("Unsupported type: " + typeof(T));
		}

		private void RegisterExternalUnsafe(string name, ExternalFunc cb)
		{
			var sym = GetSymbolByName(name);
			if (sym == null) throw new Exception("Symbol not found");
			RegisterExternalUnsafe(sym, cb);
		}

		private List<Native.Callbacks.ZkDaedalusVmExternalCallback> _externalCallbacks =
			new List<Native.Callbacks.ZkDaedalusVmExternalCallback>();

		private void RegisterExternalUnsafe(DaedalusSymbol sym, ExternalFunc cb)
		{
			Native.Callbacks.ZkDaedalusVmExternalCallback handle = (_0, _1) => cb();
			_externalCallbacks.Add(handle);

			Native.ZkDaedalusVm_registerExternal(Handle, sym.Handle, handle, UIntPtr.Zero);
		}

		private delegate void ExternalFunc();
	}
}