using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using TABLE;

namespace Smart.Editor
{
    public static class ExcelCreater
    {
		[MenuItem("Tools/传奇/间接产生的表/BossDrop")]
		public static void CreateBossDropExcel()
		{
            new BossConverter().Run();
        }

        public static void SetStringValue(this ISheet sheet,int row,int coloum,string value)
        {
            if (null == sheet)
                return;
            var irow = sheet.GetRow(row);
            if (null == irow)
                irow = sheet.CreateRow(row);
            var icol = irow.GetCell(coloum);
            if (null == icol)
                icol = irow.CreateCell(coloum);
            icol.SetCellType(CellType.String);
            icol.SetCellValue(value);
        }

        public static void SetIntValue(this ISheet sheet, int row, int coloum, int value)
        {
            if (null == sheet)
                return;
            var irow = sheet.GetRow(row);
            if (null == irow)
                irow = sheet.CreateRow(row);
            var icol = irow.GetCell(coloum);
            if (null == icol)
                icol = irow.CreateCell(coloum);
            icol.SetCellType(CellType.Numeric);
            icol.SetCellValue(value);
        }

        public static void WriteHead(this ISheet sheet,int coloum,string key,VarOptionType varType = VarOptionType.VarTypeSint32,OptionType optionType = OptionType.OptionRequired,bool required = true)
		{
			if (null == sheet)
				return;

			for(int i = 0; i < 9;++i)
            {
                var irow = sheet.GetRow(i);
                if (null == irow)
                    irow = sheet.CreateRow(i);

                var icol = irow.GetCell(coloum);
                if (null == icol)
                    icol = irow.CreateCell(coloum);

				if(i == 0)
				{
                    icol.SetCellType(CellType.String);
                    icol.SetCellValue(optionType == OptionType.OptionRepeated ? "repeated" : "required");
					continue;
                }

                if (i == 1)
                {
                    icol.SetCellType(CellType.String);
                    icol.SetCellValue(varType == VarOptionType.VarTypeSint32 ? "sint32" : "string");
                    continue;
                }

                if (i == 2 || i == 4 || i == 8)
                {
                    icol.SetCellType(CellType.String);
                    icol.SetCellValue(key);
                    continue;
                }

				if(i == 3)
				{
                    icol.SetCellType(CellType.Numeric);
                    icol.SetCellValue(required ? 1 : 0);
                    continue;
                }

                icol.SetCellType(CellType.String);
                icol.SetCellValue(string.Empty);
            }
		}

        public static string GetTableExcelPath(string sheetName)
        {
            return System.IO.Path.GetFullPath($"{Application.dataPath}/../../table/workbook/{sheetName}.xls");
        }

        public static string GetTableBytesPath(string sheetName)
		{
			return System.IO.Path.GetFullPath($"{Application.dataPath}/../../Normal/zt_android/Table/{sheetName}.bytes");
		}

		static T LoadTable<T>() where T : ILFastMode,new()
		{
			var type = typeof(T);
			var name = type.Name;
			if(!name.EndsWith("ARRAY"))
			{
				FNDebug.LogError($"T must end with ARRAY");
				return null;
			}
			var sheetName = name.Substring(0, name.Length - 5);
			var filePath = GetTableBytesPath(sheetName);
			if(!System.IO.File.Exists(filePath))
			{
                FNDebug.LogError($"file not found for {filePath}");
                return null;
            }

			var intance = new T();
			intance.Decode(System.IO.File.ReadAllBytes(filePath));
			return intance;
		}

		public static void Load<A>(System.Action<A> onSucceed) where A : ILFastMode,new()
		{
			var a = LoadTable<A>();
			if(null != a)
			{
				onSucceed?.Invoke(a);
			}
		}

        public static void Load<A,B>(System.Action<A,B> onSucceed) where A : ILFastMode, new() where B : ILFastMode, new()
		{
            var a = LoadTable<A>();
            if (null == a)
            {
				return;
            }
            var b = LoadTable<B>();
            if (null == b)
            {
                return;
            }
			onSucceed?.Invoke(a, b);
		}

        public static void Load<A,B,C>(System.Action<A,B,C> onSucceed) where A : ILFastMode, new() where B : ILFastMode, new() where C : ILFastMode, new()
		{
            var a = LoadTable<A>();
            if (null == a)
            {
                return;
            }
            var b = LoadTable<B>();
            if (null == b)
            {
                return;
            }
            var c = LoadTable<C>();
            if (null == c)
            {
                return;
            }
            onSucceed?.Invoke(a, b,c);
        }
        public static bool Load<A, B, C,D>(System.Action<A, B, C,D> onSucceed) where A : ILFastMode, new() where B : ILFastMode, new() where C : ILFastMode, new()
            where D : ILFastMode, new()
        {
            var a = LoadTable<A>();
            if (null == a)
            {
                return false;
            }
            var b = LoadTable<B>();
            if (null == b)
            {
                return false;
            }
            var c = LoadTable<C>();
            if (null == c)
            {
                return false;
            }
            var d = LoadTable<D>();
            if (null == d)
            {
                return false;
            }
            onSucceed?.Invoke(a, b, c,d);
            return true;
        }
        public static bool Load<A, B, C, D,E>(System.Action<A, B, C, D,E> onSucceed) 
            where A : ILFastMode, new() 
            where B : ILFastMode, new() 
            where C : ILFastMode, new()
            where D : ILFastMode, new()
            where E : ILFastMode, new()
        {
            var a = LoadTable<A>();
            if (null == a)
            {
                return false;
            }
            var b = LoadTable<B>();
            if (null == b)
            {
                return false;
            }
            var c = LoadTable<C>();
            if (null == c)
            {
                return false;
            }
            var d = LoadTable<D>();
            if (null == d)
            {
                return false;
            }
            var e = LoadTable<E>();
            if (null == e)
            {
                return false;
            }
            onSucceed?.Invoke(a, b, c, d,e);
            return true;
        }
    }
}