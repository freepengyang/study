using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class EncryptionMain
{

    public static string ReplaceValue(string str)
    {
        for (mdicServer.Begin(); mdicServer.Next();)
        {
            str = str.Replace(mdicServer.Key, mdicServer.Value);
        }
        return str;


    }

    public static Map<char, char> mdicServer = new Map<char, char>()
    {
        {'ā','0'},
        {'±','0'},
        {'ě','0'},
        {'ξ','0'},
        {'♂','1'},
        {'τ','1'},
        {'ο','1'},
        {'δ','1'},
        {'φ','2'},
        {'ю','2'},
        {'é','2'},
        {'ī','2'},
        {'ш','3'},
        {'ō','3'},
        {'ē','3'},
        {'п','3'},
        {'б','4'},
        {'ь','4'},
        {'ǒ','4'},
        {'я','4'},
        {'＋','5'},
        {'á','5'},
        {'σ','5'},
        {'α','5'},
        {'ó','6'},
        {'∩','6'},
        {'ǎ','6'},
        {'ε','6'},
        {'í','7'},
        {'ò','7'},
        {'ì','7'},
        {'°','7'},
        {'à','8'},
        {'○','8'},
        {'ψ','8'},
        {'♀','8'},
        {'ζ','9'},
        {'ョ','9'},
        {'è','9'},
        {'ǐ','9'},
        //{'"','9'},
        {'λ','"'},
        {'ǘ','"'},
        {'ǔ','"'},
        {'ι','"'},
        {'ǚ',','},
        {'˙',','},
        {'ù',','},
        {'ü',','},
        {'ǖ','['},
        {'ū','['},
        {'ё','['},
        {'з','['},
        {'ǜ',']'},
        {'ú',']'},
        {'ω',']'},
        {'э',']'},
    };

    #region 配置文件加密
    #region 各种加密解密字典(可用工具自动生成一套新的然后填入)
    public static Dictionary<object, object> VersioneEncryptDic = null;

    public static Dictionary<object, object> VersionDeEncryptDic = null;


    public static Dictionary<object, object> SDKConfigEncryptDic = null;

    public static Dictionary<object, object> SDKConfigDeEncryptDic = null;
    #endregion

    #region 加密解密版本控制
    public static void InitConfigEncryptDic()
    {
        VersioneEncryptDic = new Dictionary<object, object>();
        VersionDeEncryptDic = new Dictionary<object, object>();

        VersionDeEncryptDic.Add('ㄋ', '"');
        VersionDeEncryptDic.Add('δ', '"');
        VersionDeEncryptDic.Add('ひ', '"');
        VersioneEncryptDic.Add('"', "ㄋδひ");
        VersionDeEncryptDic.Add('Β', ',');
        VersionDeEncryptDic.Add('θ', ',');
        VersionDeEncryptDic.Add('ゃ', ',');
        VersioneEncryptDic.Add(',', "Βθゃ");
        VersionDeEncryptDic.Add('ュ', '.');
        VersionDeEncryptDic.Add('Е', '.');
        VersionDeEncryptDic.Add('け', '.');
        VersioneEncryptDic.Add('.', "ュЕけ");
        VersionDeEncryptDic.Add('Α', '/');
        VersionDeEncryptDic.Add('ㄍ', '/');
        VersionDeEncryptDic.Add('て', '/');
        VersioneEncryptDic.Add('/', "Αㄍて");
        VersionDeEncryptDic.Add('С', '0');
        VersionDeEncryptDic.Add('Ι', '0');
        VersionDeEncryptDic.Add('Ψ', '0');
        VersioneEncryptDic.Add('0', "СΙΨ");
        VersionDeEncryptDic.Add('は', '1');
        VersionDeEncryptDic.Add('ス', '1');
        VersionDeEncryptDic.Add('λ', '1');
        VersioneEncryptDic.Add('1', "はスλ");
        VersionDeEncryptDic.Add('ト', '2');
        VersionDeEncryptDic.Add('ㄧ', '2');
        VersionDeEncryptDic.Add('η', '2');
        VersioneEncryptDic.Add('2', "トㄧη");
        VersionDeEncryptDic.Add('せ', '3');
        VersionDeEncryptDic.Add('κ', '3');
        VersionDeEncryptDic.Add('Χ', '3');
        VersioneEncryptDic.Add('3', "せκΧ");
        VersionDeEncryptDic.Add('﹄', '4');
        VersionDeEncryptDic.Add('И', '4');
        VersionDeEncryptDic.Add('ィ', '4');
        VersioneEncryptDic.Add('4', "﹄Иィ");
        VersionDeEncryptDic.Add('く', '5');
        VersionDeEncryptDic.Add('ミ', '5');
        VersionDeEncryptDic.Add('П', '5');
        VersioneEncryptDic.Add('5', "くミП");
        VersionDeEncryptDic.Add('Ζ', '6');
        VersionDeEncryptDic.Add('ι', '6');
        VersionDeEncryptDic.Add('ㄗ', '6');
        VersioneEncryptDic.Add('6', "Ζιㄗ");
        VersionDeEncryptDic.Add('ㄝ', '7');
        VersionDeEncryptDic.Add('か', '7');
        VersionDeEncryptDic.Add('β', '7');
        VersioneEncryptDic.Add('7', "ㄝかβ");
        VersionDeEncryptDic.Add('μ', '8');
        VersionDeEncryptDic.Add('З', '8');
        VersionDeEncryptDic.Add('ゑ', '8');
        VersioneEncryptDic.Add('8', "μЗゑ");
        VersionDeEncryptDic.Add('α', '9');
        VersionDeEncryptDic.Add('Ν', '9');
        VersionDeEncryptDic.Add('Ш', '9');
        VersioneEncryptDic.Add('9', "αΝШ");
        VersionDeEncryptDic.Add('У', ':');
        VersionDeEncryptDic.Add('ね', ':');
        VersionDeEncryptDic.Add('テ', ':');
        VersioneEncryptDic.Add(':', "Уねテ");
        VersionDeEncryptDic.Add('し', 'A');
        VersionDeEncryptDic.Add('А', 'A');
        VersionDeEncryptDic.Add('Я', 'A');
        VersioneEncryptDic.Add('A', "しАЯ");
        VersionDeEncryptDic.Add('Ф', 'B');
        VersionDeEncryptDic.Add('ゐ', 'B');
        VersionDeEncryptDic.Add('サ', 'B');
        VersioneEncryptDic.Add('B', "Фゐサ");
        VersionDeEncryptDic.Add('︿', 'C');
        VersionDeEncryptDic.Add('ソ', 'C');
        VersionDeEncryptDic.Add('き', 'C');
        VersioneEncryptDic.Add('C', "︿ソき");
        VersionDeEncryptDic.Add('ァ', 'D');
        VersionDeEncryptDic.Add('ξ', 'D');
        VersionDeEncryptDic.Add('Ε', 'D');
        VersioneEncryptDic.Add('D', "ァξΕ");
        VersionDeEncryptDic.Add('Υ', 'E');
        VersionDeEncryptDic.Add('ㄔ', 'E');
        VersionDeEncryptDic.Add('Γ', 'E');
        VersioneEncryptDic.Add('E', "ΥㄔΓ");
        VersionDeEncryptDic.Add('ャ', 'F');
        VersionDeEncryptDic.Add('Л', 'F');
        VersionDeEncryptDic.Add('∑', 'F');
        VersioneEncryptDic.Add('F', "ャЛ∑");
        VersionDeEncryptDic.Add('ㄒ', 'G');
        VersionDeEncryptDic.Add('ㄈ', 'G');
        VersionDeEncryptDic.Add('ゎ', 'G');
        VersioneEncryptDic.Add('G', "ㄒㄈゎ");
        VersionDeEncryptDic.Add('ο', 'H');
        VersionDeEncryptDic.Add('フ', 'H');
        VersionDeEncryptDic.Add('ㄤ', 'H');
        VersioneEncryptDic.Add('H', "οフㄤ");
        VersionDeEncryptDic.Add('Μ', 'I');
        VersionDeEncryptDic.Add('γ', 'I');
        VersionDeEncryptDic.Add('カ', 'I');
        VersioneEncryptDic.Add('I', "Μγカ");
        VersionDeEncryptDic.Add('υ', 'J');
        VersionDeEncryptDic.Add('Т', 'J');
        VersionDeEncryptDic.Add('ツ', 'J');
        VersioneEncryptDic.Add('J', "υТツ");
        VersionDeEncryptDic.Add('ョ', 'K');
        VersionDeEncryptDic.Add('﹁', 'K');
        VersionDeEncryptDic.Add('χ', 'K');
        VersioneEncryptDic.Add('K', "ョ﹁χ");
        VersionDeEncryptDic.Add('Б', 'L');
        VersionDeEncryptDic.Add('ρ', 'L');
        VersionDeEncryptDic.Add('Ч', 'L');
        VersioneEncryptDic.Add('L', "БρЧ");
        VersionDeEncryptDic.Add('ぃ', 'M');
        VersionDeEncryptDic.Add('Д', 'M');
        VersionDeEncryptDic.Add('Г', 'M');
        VersioneEncryptDic.Add('M', "ぃДГ");
        VersionDeEncryptDic.Add('σ', 'N');
        VersionDeEncryptDic.Add('ψ', 'N');
        VersionDeEncryptDic.Add('も', 'N');
        VersioneEncryptDic.Add('N', "σψも");
        VersionDeEncryptDic.Add('ㄘ', 'O');
        VersionDeEncryptDic.Add('ケ', 'O');
        VersionDeEncryptDic.Add('Ρ', 'O');
        VersioneEncryptDic.Add('O', "ㄘケΡ");
        VersionDeEncryptDic.Add('ㄛ', 'P');
        VersionDeEncryptDic.Add('︺', 'P');
        VersionDeEncryptDic.Add('ク', 'P');
        VersioneEncryptDic.Add('P', "ㄛ︺ク");
        VersionDeEncryptDic.Add('ッ', 'Q');
        VersionDeEncryptDic.Add('ζ', 'Q');
        VersionDeEncryptDic.Add('Й', 'Q');
        VersioneEncryptDic.Add('Q', "ッζЙ");
        VersionDeEncryptDic.Add('を', 'R');
        VersionDeEncryptDic.Add('ㄏ', 'R');
        VersionDeEncryptDic.Add('ナ', 'R');
        VersioneEncryptDic.Add('R', "をㄏナ");
        VersionDeEncryptDic.Add('ω', 'S');
        VersionDeEncryptDic.Add('ㄨ', 'S');
        VersionDeEncryptDic.Add('ほ', 'S');
        VersioneEncryptDic.Add('S', "ωㄨほ");
        VersionDeEncryptDic.Add('ヘ', 'T');
        VersionDeEncryptDic.Add('В', 'T');
        VersionDeEncryptDic.Add('シ', 'T');
        VersioneEncryptDic.Add('T', "ヘВシ");
        VersionDeEncryptDic.Add('Ω', 'U');
        VersionDeEncryptDic.Add('ヮ', 'U');
        VersionDeEncryptDic.Add('Х', 'U');
        VersioneEncryptDic.Add('U', "ΩヮХ");
        VersionDeEncryptDic.Add('ぅ', 'V');
        VersionDeEncryptDic.Add('の', 'V');
        VersionDeEncryptDic.Add('︶', 'V');
        VersioneEncryptDic.Add('V', "ぅの︶");
        VersionDeEncryptDic.Add('Ж', 'W');
        VersionDeEncryptDic.Add('ハ', 'W');
        VersionDeEncryptDic.Add('ε', 'W');
        VersioneEncryptDic.Add('W', "Жハε");
        VersionDeEncryptDic.Add('Ы', 'X');
        VersionDeEncryptDic.Add('Э', 'X');
        VersionDeEncryptDic.Add('Σ', 'X');
        VersioneEncryptDic.Add('X', "ЫЭΣ");
        VersionDeEncryptDic.Add('ㄣ', 'Y');
        VersionDeEncryptDic.Add('Ξ', 'Y');
        VersionDeEncryptDic.Add('︾', 'Y');
        VersioneEncryptDic.Add('Y', "ㄣΞ︾");
        VersionDeEncryptDic.Add('コ', 'Z');
        VersionDeEncryptDic.Add('ㄡ', 'Z');
        VersionDeEncryptDic.Add('︷', 'Z');
        VersioneEncryptDic.Add('Z', "コㄡ︷");
        VersionDeEncryptDic.Add('Τ', '[');
        VersionDeEncryptDic.Add('︹', '[');
        VersionDeEncryptDic.Add('こ', '[');
        VersioneEncryptDic.Add('[', "Τ︹こ");
        VersionDeEncryptDic.Add('セ', ']');
        VersionDeEncryptDic.Add('Ъ', ']');
        VersionDeEncryptDic.Add('そ', ']');
        VersioneEncryptDic.Add(']', "セЪそ");
        VersionDeEncryptDic.Add('︸', '_');
        VersionDeEncryptDic.Add('М', '_');
        VersionDeEncryptDic.Add('ホ', '_');
        VersioneEncryptDic.Add('_', "︸Мホ");
        VersionDeEncryptDic.Add('ㄌ', 'a');
        VersionDeEncryptDic.Add('ㄕ', 'a');
        VersionDeEncryptDic.Add('め', 'a');
        VersioneEncryptDic.Add('a', "ㄌㄕめ");
        VersionDeEncryptDic.Add('ㄇ', 'b');
        VersionDeEncryptDic.Add('﹂', 'b');
        VersionDeEncryptDic.Add('ㄜ', 'b');
        VersioneEncryptDic.Add('b', "ㄇ﹂ㄜ");
        VersionDeEncryptDic.Add('さ', 'c');
        VersionDeEncryptDic.Add('ち', 'c');
        VersionDeEncryptDic.Add('Ё', 'c');
        VersioneEncryptDic.Add('c', "さちЁ");
        VersionDeEncryptDic.Add('∏', 'd');
        VersionDeEncryptDic.Add('ぁ', 'd');
        VersionDeEncryptDic.Add('ㄩ', 'd');
        VersioneEncryptDic.Add('d', "∏ぁㄩ");
        VersionDeEncryptDic.Add('キ', 'e');
        VersionDeEncryptDic.Add('φ', 'e');
        VersionDeEncryptDic.Add('ㄞ', 'e');
        VersioneEncryptDic.Add('e', "キφㄞ");
        VersionDeEncryptDic.Add('メ', 'f');
        VersionDeEncryptDic.Add('タ', 'f');
        VersionDeEncryptDic.Add('ヲ', 'f');
        VersioneEncryptDic.Add('f', "メタヲ");
        VersionDeEncryptDic.Add('と', 'g');
        VersionDeEncryptDic.Add('ㄑ', 'g');
        VersionDeEncryptDic.Add('Κ', 'g');
        VersioneEncryptDic.Add('g', "とㄑΚ");
        VersionDeEncryptDic.Add('ㄖ', 'h');
        VersionDeEncryptDic.Add('ヰ', 'h');
        VersionDeEncryptDic.Add('Ο', 'h');
        VersioneEncryptDic.Add('h', "ㄖヰΟ");
        VersionDeEncryptDic.Add('︽', 'i');
        VersionDeEncryptDic.Add('へ', 'i');
        VersionDeEncryptDic.Add('К', 'i');
        VersioneEncryptDic.Add('i', "︽へК");
        VersionDeEncryptDic.Add('Φ', 'j');
        VersionDeEncryptDic.Add('О', 'j');
        VersionDeEncryptDic.Add('ㄦ', 'j');
        VersioneEncryptDic.Add('j', "ΦОㄦ");
        VersionDeEncryptDic.Add('Р', 'k');
        VersionDeEncryptDic.Add('ゅ', 'k');
        VersionDeEncryptDic.Add('Н', 'k');
        VersioneEncryptDic.Add('k', "РゅН");
        VersionDeEncryptDic.Add('ゥ', 'l');
        VersionDeEncryptDic.Add('π', 'l');
        VersionDeEncryptDic.Add('ㄙ', 'l');
        VersioneEncryptDic.Add('l', "ゥπㄙ");
        VersionDeEncryptDic.Add('Ь', 'm');
        VersionDeEncryptDic.Add('ヌ', 'm');
        VersionDeEncryptDic.Add('ν', 'm');
        VersioneEncryptDic.Add('m', "Ьヌν");
        VersionDeEncryptDic.Add('ヴ', 'n');
        VersionDeEncryptDic.Add('ム', 'n');
        VersionDeEncryptDic.Add('た', 'n');
        VersioneEncryptDic.Add('n', "ヴムた");
        VersionDeEncryptDic.Add('チ', 'o');
        VersionDeEncryptDic.Add('む', 'o');
        VersionDeEncryptDic.Add('Π', 'o');
        VersioneEncryptDic.Add('o', "チむΠ");
        VersionDeEncryptDic.Add('ふ', 'p');
        VersionDeEncryptDic.Add('ヒ', 'p');
        VersionDeEncryptDic.Add('Δ', 'p');
        VersioneEncryptDic.Add('p', "ふヒΔ");
        VersionDeEncryptDic.Add('ネ', 'q');
        VersionDeEncryptDic.Add('っ', 'q');
        VersionDeEncryptDic.Add('モ', 'q');
        VersioneEncryptDic.Add('q', "ネっモ");
        VersionDeEncryptDic.Add('つ', 'r');
        VersionDeEncryptDic.Add('ニ', 'r');
        VersionDeEncryptDic.Add('ㄎ', 'r');
        VersioneEncryptDic.Add('r', "つニㄎ");
        VersionDeEncryptDic.Add('Ц', 's');
        VersionDeEncryptDic.Add('Щ', 's');
        VersionDeEncryptDic.Add('す', 's');
        VersioneEncryptDic.Add('s', "ЦЩす");
        VersionDeEncryptDic.Add('な', 't');
        VersionDeEncryptDic.Add('﹃', 't');
        VersionDeEncryptDic.Add('ん', 't');
        VersioneEncryptDic.Add('t', "な﹃ん");
        VersionDeEncryptDic.Add('に', 'u');
        VersionDeEncryptDic.Add('︵', 'u');
        VersionDeEncryptDic.Add('Λ', 'u');
        VersioneEncryptDic.Add('u', "に︵Λ");
        VersionDeEncryptDic.Add('ぉ', 'v');
        VersionDeEncryptDic.Add('ヶ', 'v');
        VersionDeEncryptDic.Add('Ю', 'v');
        VersioneEncryptDic.Add('v', "ぉヶЮ");
        VersionDeEncryptDic.Add('ㄉ', 'w');
        VersionDeEncryptDic.Add('∧', 'w');
        VersionDeEncryptDic.Add('ヱ', 'w');
        VersioneEncryptDic.Add('w', "ㄉ∧ヱ");
        VersionDeEncryptDic.Add('ま', 'x');
        VersionDeEncryptDic.Add('τ', 'x');
        VersionDeEncryptDic.Add('Η', 'x');
        VersioneEncryptDic.Add('x', "まτΗ");
        VersionDeEncryptDic.Add('み', 'y');
        VersionDeEncryptDic.Add('ㄆ', 'y');
        VersionDeEncryptDic.Add('ォ', 'y');
        VersioneEncryptDic.Add('y', "みㄆォ");
        VersionDeEncryptDic.Add('ㄢ', 'z');
        VersionDeEncryptDic.Add('ン', 'z');
        VersionDeEncryptDic.Add('ㄚ', 'z');
        VersioneEncryptDic.Add('z', "ㄢンㄚ");
        VersionDeEncryptDic.Add('ぇ', '{');
        VersionDeEncryptDic.Add('﹀', '{');
        VersionDeEncryptDic.Add('Θ', '{');
        VersioneEncryptDic.Add('{', "ぇ﹀Θ");
        VersionDeEncryptDic.Add('ㄠ', '}');
        VersionDeEncryptDic.Add('ㄊ', '}');
        VersionDeEncryptDic.Add('ょ', '}');
        VersioneEncryptDic.Add('}', "ㄠㄊょ");
    }

    public static string EncrypVersionConfig(string data)
    {
        if (VersioneEncryptDic == null) InitConfigEncryptDic();
        return EncrypConfig(data, VersioneEncryptDic);
    }

    public static string DeEncrypVersionConfig(string data)
    {
        if (VersionDeEncryptDic == null) InitConfigEncryptDic();
        return DeEncrypConfig(data, VersionDeEncryptDic);
    }
    #endregion

    #region 加密解密SDK配置
    public static string EncrypSDKConfig(string data)
    {
        return EncrypConfig(data, SDKConfigEncryptDic);
    }

    public static string DeEncrypSDKConfig(string data)
    {
        byte[] byteData = DeEncryptStringToByte(data);
        byteData[1] -= 5;
        byteData[byteData.Length - 1] -= 1;
        return System.Text.Encoding.UTF8.GetString(byteData);
    }

    public static string EncryptByteToString(byte[] bytes) // 0xae00cf => "AE00CF "
    {
        string hexString = string.Empty;

        if (bytes != null)
        {
            StringBuilder strB = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                int num = bytes[i];
                if (num % 2 == 0)//为奇数
                    num = num - 2;
                else
                    num = num + 10;
                strB.Append(((char)num).ToString());
            }

            hexString = strB.ToString();

        }
        return hexString;
    }

    public static byte[] DeEncryptStringToByte(string data) // 0xae00cf => "AE00CF "
    {
        byte[] bytes = null;
        if (!string.IsNullOrEmpty(data))
        {
            bytes = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                int num = data[i];
                if (num % 2 == 0)//为奇数
                    num = num + 2;
                else
                    num = num - 10;
                bytes[i] = (byte)num;
            }

        }
        return bytes;
    }
    #endregion

    private static string EncrypConfig(string data, Dictionary<object, object> dic)
    {
        if (dic == null)
        {
            UnityEngine.Debug.LogError("加密字典为空");
            return data;
        }
        StringBuilder finalData = new StringBuilder();
        System.Random random = new System.Random();
        for (int i = 0; i < data.Length; i++)
        {
            if (dic.ContainsKey(data[i]))
                finalData.Append(dic[data[i]].ToString()[random.Next(3)]);
            else
                finalData.Append(data[i]);
        }
        return finalData.ToString();
    }

    private static string DeEncrypConfig(string data, Dictionary<object, object> dic)
    {
        if (dic == null)
        {
            UnityEngine.Debug.LogError("解密字典为空");
            return data;
        }
        StringBuilder finalData = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            if (dic.ContainsKey(data[i]))
                finalData.Append(dic[data[i]].ToString());
            else
                finalData.Append(data[i]);
        }
        return finalData.ToString();
    }
    #endregion
}
