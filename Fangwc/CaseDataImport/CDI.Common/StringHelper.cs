using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CDI.Common
{
    /// <summary>
    /// 字符串相关操作类
    /// </summary>
    public class StringHelper
    {
        #region 截取字符串

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="length">最大长度</param>
        /// <returns>截取后的字符串</returns>
        public static string GetCut(string sourceString, int length)
        {
            return GetCut(sourceString, length, "");
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="length">最大长度</param>
        /// <param name="replaceStr">替换被截取掉的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetCut(string sourceString, int length, string replaceStr)
        {
            if (!string.IsNullOrEmpty(sourceString) && sourceString.Length > length)
            {
                return sourceString.Substring(0, length) + replaceStr;
            }
            return sourceString;
        }

        #endregion

        #region 检查某集合中是否包含某字符串

        /// <summary>
        /// 检查某集合中是否包含某字符串(区分大小写)
        /// </summary>
        /// <param name="findString">要查询的字符串</param>
        /// <param name="allStr">被检查的集合</param>
        /// <returns>bool</returns>
        public static bool Contains(string findString, List<string> allStr)
        {
            return allStr.Contains(findString);
        }

        #endregion

        /// <summary>
        /// 获得登录Api中的Code,即对时间加密
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string GetLoginCodeMd5(string pwd)
        {
            string strmd5 = pwd + WcfLoginMd5Key;
            strmd5 = GetMd5(strmd5);
            return strmd5;
        }
        /// <summary>
        /// 进行MD5效验
        /// </summary>
        /// <param name="strmd5"></param>
        /// <returns></returns>
        public static string GetMd5(string strmd5)
        {
            return GetMd5(strmd5, string.Empty);
        }
        /// <summary>
        /// 进行MD5效验
        /// </summary>
        /// <param name="strmd5"></param>
        /// <param name="key">加密key</param>
        /// <returns></returns>
        public static string GetMd5(string strmd5, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                strmd5 += key;
            }
            byte[] md5Bytes = ASCIIEncoding.Default.GetBytes(strmd5);
            byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            //FileStream fs= new FileStream(filepath,FileMode.Open,FileAccess.Read);
            encodedBytes = md5.ComputeHash(md5Bytes);
            string nn = BitConverter.ToString(encodedBytes);
            nn = Regex.Replace(nn, "-", "");//因为转化完的都是34-2d这样的，所以替换掉- 
            nn = nn.ToLower();//根据需要转化成小写
            //fs.Close();
            return nn;
        }
        /// <summary>
        /// 获得加密后的密码
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static string GetPassWordMd5(string pwd)
        {
            string strmd5 = pwd + WcfPassWordMd5Key;
            strmd5 = GetMd5(strmd5);
            return strmd5;
        }
        /// <summary>
        /// WCF登录接口加密的Key
        /// </summary>
        public const string WcfLoginMd5Key = "fxtlogin*$^0314";

        /// <summary>
        /// 用户密码加密的Key
        /// </summary>
        public const string WcfPassWordMd5Key = "fxtproduct*&2014";

        #region 根据中文得到拼音
        /// <summary>
        /// 得到中文的简拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetPYString(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                //if ((int)c >= 33 && (int)c <= 126)
                //{
                //    tempStr += c.ToString();
                //}
                //else
                //{
                //    tempStr += GetPYChar(c.ToString());
                //}
                try
                {
                    ChineseChar chineseChar = new ChineseChar(c);
                    string t = chineseChar.Pinyins[0].ToString();
                    tempStr += t.Substring(0, 1);
                }
                catch
                {
                    tempStr += c.ToString();
                }

            }
            return tempStr;
        }

        public static string GetAllPYString(string str)
        {
            string r = string.Empty;
            foreach (char c in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(c);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, 1) + t.Substring(1, t.Length - 2).ToLower();
                }
                catch
                {
                    r += c.ToString();
                }
            }
            return r;
        }

        public static string GetPYChar(string str)
        {
            if (str == " ") return " ";
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(str);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));

            if (i < 0xB0A1) return "";
            if (i < 0xB0C5) return "a";
            if (i < 0xB2C1) return "b";
            if (i < 0xB4EE) return "c";
            if (i < 0xB6EA) return "d";
            if (i < 0xB7A2) return "e";
            if (i < 0xB8C1) return "f";
            if (i < 0xB9FE) return "g";
            if (i < 0xBBF7) return "h";
            if (i < 0xBFA6) return "j";
            if (i < 0xC0AC) return "k";
            if (i < 0xC2E8) return "l";
            if (i < 0xC4C3) return "m";
            if (i < 0xC5B6) return "n";
            if (i < 0xC5BE) return "o";
            if (i < 0xC6DA) return "p";
            if (i < 0xC8BB) return "q";
            if (i < 0xC8F6) return "r";
            if (i < 0xCBFA) return "s";
            if (i < 0xCDDA) return "t";
            if (i < 0xCEF4) return "w";
            if (i < 0xD1B9) return "x";
            if (i < 0xD4D1) return "y";
            if (i < 0xD7FA) return "z";
            return "";
        }

        private static int[] pyvalue = new int[]{-20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,-20032,-20026,
    -20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,-19756,-19751,-19746,-19741,-19739,-19728,
    -19725,-19715,-19540,-19531,-19525,-19515,-19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,
    -19261,-19249,-19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,-19003,-18996,
    -18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,-18731,-18722,-18710,-18697,-18696,-18526,
    -18518,-18501,-18490,-18478,-18463,-18448,-18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183,
    -18181,-18012,-17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,-17733,-17730,
    -17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,-17468,-17454,-17433,-17427,-17417,-17202,
    -17185,-16983,-16970,-16942,-16915,-16733,-16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,
    -16452,-16448,-16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,-16212,-16205,
    -16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,-15933,-15920,-15915,-15903,-15889,-15878,
    -15707,-15701,-15681,-15667,-15661,-15659,-15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,
    -15408,-15394,-15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,-15149,-15144,
    -15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,-14941,-14937,-14933,-14930,-14929,-14928,
    -14926,-14922,-14921,-14914,-14908,-14902,-14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,
    -14663,-14654,-14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,-14170,-14159,
    -14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,-14109,-14099,-14097,-14094,-14092,-14090,
    -14087,-14083,-13917,-13914,-13910,-13907,-13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,
    -13611,-13601,-13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,-13340,-13329,
    -13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,-13068,-13063,-13060,-12888,-12875,-12871,
    -12860,-12858,-12852,-12849,-12838,-12831,-12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,
    -12320,-12300,-12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,-11781,-11604,
    -11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,-11055,-11052,-11045,-11041,-11038,-11024,
    -11020,-11019,-11018,-11014,-10838,-10832,-10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,
    -10329,-10328,-10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254};
        private static string[] pystr = new string[]{"a","ai","an","ang","ao","ba","bai","ban","bang","bao","bei","ben","beng","bi","bian","biao",
   "bie","bin","bing","bo","bu","ca","cai","can","cang","cao","ce","ceng","cha","chai","chan","chang","chao","che","chen",
   "cheng","chi","chong","chou","chu","chuai","chuan","chuang","chui","chun","chuo","ci","cong","cou","cu","cuan","cui",
   "cun","cuo","da","dai","dan","dang","dao","de","deng","di","dian","diao","die","ding","diu","dong","dou","du","duan",
   "dui","dun","duo","e","en","er","fa","fan","fang","fei","fen","feng","fo","fou","fu","ga","gai","gan","gang","gao",
   "ge","gei","gen","geng","gong","gou","gu","gua","guai","guan","guang","gui","gun","guo","ha","hai","han","hang",
   "hao","he","hei","hen","heng","hong","hou","hu","hua","huai","huan","huang","hui","hun","huo","ji","jia","jian",
   "jiang","jiao","jie","jin","jing","jiong","jiu","ju","juan","jue","jun","ka","kai","kan","kang","kao","ke","ken",
   "keng","kong","kou","ku","kua","kuai","kuan","kuang","kui","kun","kuo","la","lai","lan","lang","lao","le","lei",
   "leng","li","lia","lian","liang","liao","lie","lin","ling","liu","long","lou","lu","lv","luan","lue","lun","luo",
   "ma","mai","man","mang","mao","me","mei","men","meng","mi","mian","miao","mie","min","ming","miu","mo","mou","mu",
   "na","nai","nan","nang","nao","ne","nei","nen","neng","ni","nian","niang","niao","nie","nin","ning","niu","nong",
   "nu","nv","nuan","nue","nuo","o","ou","pa","pai","pan","pang","pao","pei","pen","peng","pi","pian","piao","pie",
   "pin","ping","po","pu","qi","qia","qian","qiang","qiao","qie","qin","qing","qiong","qiu","qu","quan","que","qun",
   "ran","rang","rao","re","ren","reng","ri","rong","rou","ru","ruan","rui","run","ruo","sa","sai","san","sang",
   "sao","se","sen","seng","sha","shai","shan","shang","shao","she","shen","sheng","shi","shou","shu","shua",
   "shuai","shuan","shuang","shui","shun","shuo","si","song","sou","su","suan","sui","sun","suo","ta","tai",
   "tan","tang","tao","te","teng","ti","tian","tiao","tie","ting","tong","tou","tu","tuan","tui","tun","tuo",
   "wa","wai","wan","wang","wei","wen","weng","wo","wu","xi","xia","xian","xiang","xiao","xie","xin","xing",
   "xiong","xiu","xu","xuan","xue","xun","ya","yan","yang","yao","ye","yi","yin","ying","yo","yong","you",
   "yu","yuan","yue","yun","za","zai","zan","zang","zao","ze","zei","zen","zeng","zha","zhai","zhan","zhang",
   "zhao","zhe","zhen","zheng","zhi","zhong","zhou","zhu","zhua","zhuai","zhuan","zhuang","zhui","zhun","zhuo",
   "zi","zong","zou","zu","zuan","zui","zun","zuo"};

        /// <summary>
        ///输入中文得到全拼音
        /// </summary>
        /// <param name="chrstr"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string ConvertPinYin(string chrstr, int len)
        {
            if (chrstr == "-")
            {
                return "admoffnone";
            }
            else
            {
                byte[] array = new byte[2];
                string returnstr = "";
                int chrasc = 0;
                int i1 = 0;
                int i2 = 0;
                char[] nowchar = chrstr.ToCharArray();
                int pyLen = 0;
                if (chrstr.Length > len)
                {
                    pyLen = len;
                }
                else
                {
                    pyLen = chrstr.Length;
                }
                for (int j = 0; j < pyLen; j++)
                {
                    array = System.Text.Encoding.Default.GetBytes(nowchar[j].ToString());
                    if (array.Length == 1)
                    {
                        returnstr += nowchar[j];
                    }
                    else
                    {
                        i1 = (short)(array[0]);
                        i2 = (short)(array[1]);
                        chrasc = i1 * 256 + i2 - 65536;
                        if (chrasc > 0 && chrasc < 160)
                        {
                            returnstr += nowchar[j];
                        }
                        else
                        {
                            for (int i = (pyvalue.Length - 1); i >= 0; i--)
                            {
                                if (pyvalue[i] <= chrasc)
                                {
                                    returnstr += pystr[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                return returnstr;
            }
        }
        #endregion



        private static int _rep = 0;

        /// <summary>
        /// 生成随机数字字符串
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        /// <returns>生成的数字字符串</returns>
        public static string GenerateCheckCodeNum(int codeCount)
        {
            var str = string.Empty;
            var num2 = DateTime.Now.Ticks + _rep;
            _rep++;
            var random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> _rep)));
            for (int i = 0; i < codeCount; i++)
            {
                var num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10))));
            }
            return str;
        }

        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        /// <returns>生成的字母字符串</returns>
        public static string GenerateCheckCode(int codeCount)
        {
            var str = string.Empty;
            var num2 = DateTime.Now.Ticks + _rep;
            _rep++;
            var random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> _rep)));
            for (var i = 0; i < codeCount; i++)
            {
                char ch;
                var num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch;
            }
            return str;
        }
    }
}
