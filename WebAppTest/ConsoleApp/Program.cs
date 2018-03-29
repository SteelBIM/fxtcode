using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Configuration;

namespace ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            #region delegate sort example
            //Employee[] employees = { 
            //                          new Employee("1",20000),
            //                          new Employee("2",30000),
            //                          new Employee("3",19000),
            //                          new Employee("4",25000),
            //                          new Employee("5",20000),
            //                          new Employee("6",18000),
            //                          };


            //Employee e = new Employee("1",100);
            //Type t = e.GetType();
            //foreach (var item in t.GetProperties())
            //{
            //    Console.WriteLine(item.Name);
            //}

            //BubbleSorter.sort(employees, Employee.CompareSalary);

            //foreach (var item in employees)
            //{
            //    Console.WriteLine(item);
            //}
            #endregion

            #region 多线程
            //const int repetitions = 10000;
            //Task task = new Task(() => {
            //    for (int i = 0; i < repetitions; i++)
            //    {
            //        Console.Write('-');
            //    }
            //});
            //task.Start();

            //for (int c = 0; c < repetitions; c++)
            //{
            //     Console.Write('.');
            //}
            //task.Wait();
            #endregion

            #region 组合模式
            
            //ConcreteCompany root = new ConcreteCompany("北京总公司");
            //root.Add(new HRDepartment("总公司人力资源部"));
            //root.Add(new FinanceDepartment("总公司财务部"));

            //ConcreteCompany comp = new ConcreteCompany("上海华东分公司");
            //comp.Add(new HRDepartment("华东分公司人力资源部"));
            //comp.Add(new FinanceDepartment("华东分公司财务部"));
            //root.Add(comp);

            //ConcreteCompany comp1 = new ConcreteCompany("南京办事处");
            //comp1.Add(new HRDepartment("南京办事处人力资源部"));
            //comp1.Add(new FinanceDepartment("南京办事处财务部"));
            //comp.Add(comp1);

            //ConcreteCompany comp2 = new ConcreteCompany("杭州办事处");
            //comp2.Add(new HRDepartment("杭州办事处人力资源部"));
            //comp2.Add(new FinanceDepartment("杭州办事处财务部"));
            //comp.Add(comp2);


            //Console.WriteLine("\n结构图：");

            //root.Display(1);

            //Console.WriteLine("\n职责：");

            //root.LineOfDuty();
            #endregion

            #region 下载
            //WebClient client = new WebClient();
            ////client.DownloadFile("http://localhost:3158", "main.aspx");
            
            #endregion

            #region 解密
            //string text = "<?xml version='10' encoding='GB2312'?><root><head><TransCode>G00017</TransCode><TransDate>20130703</TransDate><TransTime>154257</TransTime><InstCode>658757719815</InstCode><SeqNo>BD28F62871214F1FA2F6E2F6CBE2710F</SeqNo><BizType>0</BizType></head><body><Detail>65</Detail></body></root>"; //明文  
            //string keys = "HoUsePrOHoUsePrO";//密钥,128位              
            //byte[] encryptBytes = AESEncryption.AESEncrypt(text, keys);
            ////将加密后的密文转换为Base64编码，以便显示，可以查看下结果  
            //Console.WriteLine("明文:" + text);
            //Console.WriteLine("密文:" + Convert.ToBase64String(encryptBytes));
            ////解密  
            //byte[] decryptBytes = AESEncryption.AESDecrypt(encryptBytes, keys);
            ////将解密后的结果转换为字符串,也可以将该步骤封装在解密算法中  
            //string result = Encoding.UTF8.GetString(decryptBytes);
            //Console.WriteLine("解密结果：" + result);
            //Console.Read();   
            #endregion

            Console.WriteLine(System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion());
            int count = 1000000;
            #region 测试
            //Console.Write("ExeConfigurationFileMap访问花费时间：       ");
            //Stopwatch watchmap = Stopwatch.StartNew();

            //ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            //map.ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "App.config"; ;
            //Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            //string methName = config.AppSettings.Settings["show"].Value;
            //watchmap.Stop();
            //Console.WriteLine(watchmap.Elapsed.ToString());

            //Console.Write("dicFileMap访问花费时间：       ");
            //Stopwatch watchLook = Stopwatch.StartNew();


            //string methodlook = Dir.ErrorCodes["look"];
            //watchLook.Stop();
            //Console.WriteLine(watchLook.Elapsed.ToString());

            //Console.Write("dicFileMap访问花费时间：       ");
            //Stopwatch watchdic = Stopwatch.StartNew();


            //string methodName = Dir.ErrorCodes["show"];
            //watchdic.Stop();
            //Console.WriteLine(watchdic.Elapsed.ToString());

            //Console.Write("Two访问花费时间：       ");
            //Stopwatch watchTwo = Stopwatch.StartNew();


            //string methodTwo = config.AppSettings.Settings["look"].Value;
            //watchTwo.Stop();
            //Console.WriteLine(watchdic.Elapsed.ToString());
            #endregion
           


           

            OrderInfo testObj = new OrderInfo();
            PropertyInfo propInfo = typeof(OrderInfo).GetProperty("OrderID");

            Console.Write("直接访问花费时间：       ");
            Stopwatch watch1 = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
                testObj.OrderID = 123;

            watch1.Stop();
            Console.WriteLine(watch1.Elapsed.ToString());


            SetValueDelegate setter2 = DynamicMethodFactory.CreatePropertySetter(propInfo);
            Console.Write("EmitSet花费时间：        ");
            Stopwatch watch2 = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
                setter2(testObj, 123);

            watch2.Stop();
            Console.WriteLine(watch2.Elapsed.ToString());


            Console.Write("纯反射花费时间：　       ");
            Stopwatch watch3 = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
                propInfo.SetValue(testObj, 123, null);

            watch3.Stop();
            Console.WriteLine(watch3.Elapsed.ToString());


            Console.WriteLine("-------------------");
            Console.WriteLine("{0} / {1} = {2}",
                watch3.Elapsed.ToString(),
                watch1.Elapsed.ToString(),
                watch3.Elapsed.TotalMilliseconds / watch1.Elapsed.TotalMilliseconds);

            Console.WriteLine("{0} / {1} = {2}",
                watch3.Elapsed.ToString(),
                watch2.Elapsed.ToString(),
                watch3.Elapsed.TotalMilliseconds / watch2.Elapsed.TotalMilliseconds);

            Console.WriteLine("{0} / {1} = {2}",
                watch2.Elapsed.ToString(),
                watch1.Elapsed.ToString(),
                watch2.Elapsed.TotalMilliseconds / watch1.Elapsed.TotalMilliseconds);
        }
    }

    public class Dir 
    {
        public static readonly Dictionary<string, string> ErrorCodes
                = new Dictionary<string, string>
            {
                { "show", "GetSurveyInfo" },
                { "look", "Error Two" }
            };
    }

    public class OrderInfo 
    {
        public int OrderID { get; set; }
    }

    public delegate void SetValueDelegate(object target, object arg);

    public static class DynamicMethodFactory
    {
        public static SetValueDelegate CreatePropertySetter(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (!property.CanWrite)
                return null;

            MethodInfo setMethod = property.GetSetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertySetter", null,
                new Type[] { typeof(object), typeof(object) },
                property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!setMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);

            EmitCastToReference(il, property.PropertyType);
            if (!setMethod.IsStatic && !property.DeclaringType.IsValueType)
            {
                il.EmitCall(OpCodes.Callvirt, setMethod, null);
            }
            else
                il.EmitCall(OpCodes.Call, setMethod, null);

            il.Emit(OpCodes.Ret);

            return (SetValueDelegate)dm.CreateDelegate(typeof(SetValueDelegate));
        }

        private static void EmitCastToReference(ILGenerator il, Type type)
        {
            if (type.IsValueType)
                il.Emit(OpCodes.Unbox_Any, type);
            else
                il.Emit(OpCodes.Castclass, type);
        }
    }

    #region 观察者
        ////通知者接口
        //interface Subject
        //{
        //    void Notify();
        //    string SubjectState
        //    {
        //        get;
        //        set;
        //    }
        //}

        ////事件处理程序的委托
        //delegate void EventHandler();

        //class Secretary : Subject
        //{
        //    //声明一事件Update，类型为委托EventHandler
        //    public event EventHandler Update;

        //    private string action;

        //    public void Notify()
        //    {
        //        Update();
        //    }
        //    public string SubjectState
        //    {
        //        get { return action; }
        //        set { action = value; }
        //    }
        //}

        //class Boss : Subject
        //{
        //    //声明一事件Update，类型为委托EventHandler
        //    public event EventHandler Update;

        //    private string action;

        //    public void Notify()
        //    {
        //        Update();
        //    }
        //    public string SubjectState
        //    {
        //        get { return action; }
        //        set { action = value; }
        //    }
        //}

        ////看股票的同事
        //class StockObserver
        //{
        //    private string name;
        //    private Subject sub;
        //    public StockObserver(string name, Subject sub)
        //    {
        //        this.name = name;
        //        this.sub = sub;
        //    }

        //    //关闭股票行情
        //    public void CloseStockMarket()
        //    {
        //        Console.WriteLine("{0} {1} 关闭股票行情，继续工作！", sub.SubjectState, name);
        //    }
        //}

        ////看NBA的同事
        //class NBAObserver
        //{
        //    private string name;
        //    private Subject sub;
        //    public NBAObserver(string name, Subject sub)
        //    {
        //        this.name = name;
        //        this.sub = sub;
        //    }

        //    //关闭NBA直播
        //    public void CloseNBADirectSeeding()
        //    {
        //        Console.WriteLine("{0} {1} 关闭NBA直播，继续工作！", sub.SubjectState, name);
        //    }
        //}
    #endregion

    #region 组合模式
    //abstract class Company
    //{
    //    protected string name;

    //    public Company(string name)
    //    {
    //        this.name = name;
    //    }

    //    public abstract void Add(Company c);//增加
    //    public abstract void Remove(Company c);//移除
    //    public abstract void Display(int depth);//显示
    //    public abstract void LineOfDuty();//履行职责
    //}

    //class ConcreteCompany : Company
    //{
    //    private List<Company> children = new List<Company>();

    //    public ConcreteCompany(string name)
    //        : base(name)
    //    { }

    //    public override void Add(Company c)
    //    {
    //        children.Add(c);
    //    }

    //    public override void Remove(Company c)
    //    {
    //        children.Remove(c);
    //    }

    //    public override void Display(int depth)
    //    {
    //        Console.WriteLine(new String('-', depth) + name);

    //        foreach (Company component in children)
    //        {
    //            component.Display(depth + 2);
    //        }
    //    }

    //    //履行职责
    //    public override void LineOfDuty()
    //    {
    //        foreach (Company component in children)
    //        {
    //            component.LineOfDuty();
    //        }
    //    }

    //}

    ////人力资源部
    //class HRDepartment : Company
    //{
    //    public HRDepartment(string name)
    //        : base(name)
    //    { }

    //    public override void Add(Company c)
    //    {
    //    }

    //    public override void Remove(Company c)
    //    {
    //    }

    //    public override void Display(int depth)
    //    {
    //        Console.WriteLine(new String('-', depth) + name);
    //    }


    //    public override void LineOfDuty()
    //    {
    //        Console.WriteLine("{0} 员工招聘培训管理", name);
    //    }
    //}

    ////财务部
    //class FinanceDepartment : Company
    //{
    //    public FinanceDepartment(string name)
    //        : base(name)
    //    { }

    //    public override void Add(Company c)
    //    {
    //    }

    //    public override void Remove(Company c)
    //    {
    //    }

    //    public override void Display(int depth)
    //    {
    //        Console.WriteLine(new String('-', depth) + name);
    //    }

    //    public override void LineOfDuty()
    //    {
    //        Console.WriteLine("{0} 公司财务收支管理", name);
    //    }

    //}
    #endregion

    class AESEncryption
    {

        
        //默认密钥向量   

        /// <summary>  
        /// AES加密算法  
        /// </summary>  
        /// <param name="plainText">明文字符串</param>  
        /// <param name="strKey">密钥</param>  
        /// <returns>返回加密后的密文字节数组</returns>  
        public static byte[] AESEncrypt(string plainText, string strKey)
        {
            //分组加密算法  
            int discarded = 16;
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组      
            //设置密钥及密钥向量  
            des.Key = Encoding.UTF8.GetBytes(strKey);
            des.IV = GetBytes(Convert.ToBase64String(Encoding.Default.GetBytes("HoUsePrOHoUsePrO")),out discarded);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组  
            cs.Close();
            ms.Close();
            return cipherBytes;
        }

        public static byte[] GetBytes(string hexString, out int discarded)
        {
            discarded = 0;
            string newString = "";
            char c;
            // remove all none A-F, 0-9, characters
            for (int i = 0; i < hexString.Length; i++)
            {
                c = hexString[i];
                if (Uri.IsHexDigit(c))
                    newString += c;
                else
                    discarded++;
            }
            // if odd number of characters, discard last character
            if (newString.Length % 2 != 0)
            {
                discarded++;
                newString = newString.Substring(0, newString.Length - 1);
            }

            int byteLength = newString.Length / 2;
            byte[] bytes = new byte[byteLength];
            string hex;
            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                hex = new String(new Char[] { newString[j], newString[j + 1] });
                bytes[i] = HexToByte(hex)[0];
                j = j + 2;
            }
            return bytes;
        }

        private static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="cipherText">密文字节数组</param>  
        /// <param name="strKey">密钥</param>  
        /// <returns>返回解密后的字符串</returns>  
        public static byte[] AESDecrypt(byte[] cipherText, string strKey)
        {
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(strKey);
            int discarded = 16;
            des.IV = GetBytes(Convert.ToBase64String(Encoding.Default.GetBytes("HoUsePrOHoUsePrO")), out discarded);
            byte[] decryptBytes = new byte[cipherText.Length];
            MemoryStream ms = new MemoryStream(cipherText);
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
            cs.Read(decryptBytes, 0, decryptBytes.Length);
            cs.Close();
            ms.Close();
            return decryptBytes;
        }
    }

    #region 中介者模式
    
    #endregion

}