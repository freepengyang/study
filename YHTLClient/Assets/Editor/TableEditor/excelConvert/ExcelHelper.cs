using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

namespace Smart.Editor
{
    class ExcelHelper
    {
		public enum ConvertType
		{
			CT_PROTO = 0,
			CT_TXT,
            CT_CSHARP,
		}
		public enum ConvertMode
		{
			CM_PROTO2 = 1,
			CM_PROTO3 = 2,
			CM_IL_FAST_MODE = 3,
		}
    }
}