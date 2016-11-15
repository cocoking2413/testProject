using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandLine;
using CommandLine.Text;
using Newtonsoft.Json;

namespace getUserInfo {
    class Program {
        static void Main(string [ ] args) {
            Program p = new Program();
            if ( !SetConsoleCtrlHandler(p.HandlerRoutineMethod, true) ) {
                Console.WriteLine("无法注册系统事件!\n");
            }
            var options = new Options();
            if ( Parser.Default.ParseArguments(args, options) ) {
                if ( options.UserInfo == 1 ) {
                    getIPMac((ip1, ip2, mac) => {
                        //输出
                        string outPutStr = "公网IP:{0},\n IP:{1},\n MAC地址:{2}\n";
                        Console.WriteLine(string.Format(outPutStr, ip1, ip2, mac));
                    }, error => { Console.Write(error); });
                } else if ( options.UserInfo == 2) {
                    Console.WriteLine(JsonConvert.SerializeObject(new { pcName = Environment.MachineName, platForm = Environment.OSVersion.Platform, version = Environment.OSVersion.VersionString, cpu = Environment.ProcessorCount, is64bit = Environment.Is64BitOperatingSystem, HasNetWork = SystemInformation.Network, monitorCount = SystemInformation.MonitorCount, px = SystemInformation.PrimaryMonitorMaximizedWindowSize.Width + "*" + SystemInformation.PrimaryMonitorMaximizedWindowSize.Height, mainPx = SystemInformation.PrimaryMonitorSize.Width + "*" + SystemInformation.PrimaryMonitorSize.Height, mouseButtons = SystemInformation.MouseButtons, dirLimit = Environment.SystemDirectory, pageSize = Environment.SystemPageSize }, Formatting.Indented));
                }
                else if ( options.UserInfo == 3 ) {
                    Console.WriteLine(JsonConvert.SerializeObject(new { CurrentDirectory = Environment.CurrentDirectory, LogicalDrives = Environment.GetLogicalDrives(), version = Environment.Version, UserName = Environment.UserName, FolderDeskTopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop), CommandLineArgs = Environment.GetCommandLineArgs(), EnvironmentVariables = Environment.GetEnvironmentVariables(), DomainName = Environment.UserDomainName, startTime = Environment.TickCount / 1000, workSize = Environment.WorkingSet }, Formatting.Indented));
                }
                if ( options.Version ) {
                    Console.WriteLine("getUserInfo 1.0 by coco king");
                }
                while ( true ) {
                    Console.Write("$ > ");
                    var argstr = System.Text.RegularExpressions.Regex.Replace(Console.ReadLine(),@"\s+"," ").Split(' ');
                    try {
                        if ( argstr.Length < 1 ) continue;
                        switch ( argstr [0] ) {
                            case "i":
                            case @"\i":
                            case "-i":
                                if ( argstr.Length == 1 || argstr [1] == "1" ) {
                                    getIPMac((ip1, ip2, mac) => {
                                        //输出
                                        string outPutStr = "公网ip:{0},\n IP:{1},\n MAC地址:{2}\n";
                                        Console.WriteLine(string.Format(outPutStr, ip1, ip2, mac));
                                    }, error => { Console.WriteLine(error); });
                                }
                                else if ( argstr [1] == "2" ) {
                                    Console.WriteLine(JsonConvert.SerializeObject(new { pcName = Environment.MachineName, platForm = Environment.OSVersion.Platform, version = Environment.OSVersion.VersionString, cpu = Environment.ProcessorCount, is64bit = Environment.Is64BitOperatingSystem, HasNetWork = SystemInformation.Network, monitorCount = SystemInformation.MonitorCount, px = SystemInformation.PrimaryMonitorMaximizedWindowSize.Width + "*" + SystemInformation.PrimaryMonitorMaximizedWindowSize.Height, mainPx = SystemInformation.PrimaryMonitorSize.Width + "*" + SystemInformation.PrimaryMonitorSize.Height, mouseButtons = SystemInformation.MouseButtons, dirLimit = Environment.SystemDirectory, pageSize = Environment.SystemPageSize }, Formatting.Indented));
                                }
                                else if ( argstr [1] == "3" ) {
                                    Console.WriteLine(JsonConvert.SerializeObject(new { CurrentDirectory = Environment.CurrentDirectory, LogicalDrives = Environment.GetLogicalDrives(), version = Environment.Version, UserName = Environment.UserName, FolderDeskTopPath = Environment.GetFolderPath( Environment.SpecialFolder.Desktop), CommandLineArgs = Environment.GetCommandLineArgs(), EnvironmentVariables = Environment.GetEnvironmentVariables(), DomainName = Environment.UserDomainName, startTime =Environment.TickCount/1000, workSize = Environment.WorkingSet}, Formatting.Indented));
                                }
                                break;
                            case "q":
                            case @"\q":
                            case "-q":
                            case "exit":
                            case "init":
                                if ( argstr.Length == 1 )
                                    GenerateConsoleCtrlEvent(0, 0);
                                else
                                    GenerateConsoleCtrlEvent(int.Parse(argstr [1]), 0);
                                break;
                          
                            case "v":
                            case @"\v":
                            case "-v":
                            case "version":
                                Console.WriteLine("getUserInfo 1.0 by coco king");
                                break;
                            default:
                                break;
                        }
                    }
                    catch ( Exception ex ) {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
        const int Colse = 0;
        public bool HandlerRoutineMethod(int type) {
            switch ( type ) {
                case Colse:
                    return false;
                default:
                    break;
            }
            return true;
        }

        public delegate bool HandlerRoutine(int type);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine HandlerRoutine, bool add);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool GenerateConsoleCtrlEvent(int code, int value);
        private static void getIPMac(Action<string, string, string> callBack, Action<string> error = null) {
            try {
                string ip = "";
                string mac = "";
                ManagementClass mc;
                string hostInfo = Dns.GetHostName();
                //IP地址
                System.Net.IPAddress [ ] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                if ( addressList.Length > 0 )
                    for ( int i = 0; i < addressList.Length; i++ ) {
                        ip += "\n\t"+ addressList [i].ToString();
                    }
                //mac地址
                mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach ( ManagementObject mo in moc ) {
                    if ( mo ["IPEnabled"].ToString() == "True" ) {
                        mac = mo ["MacAddress"].ToString();
                    }
                }
                callBack(GetIP(), ip, mac);
            }
            catch ( Exception ex ) {
                error(ex.Message);
            }
        }

        private static string GetIP() {
            string tempip = "";
            try {
                WebRequest wr = WebRequest.Create("http://ip.chinaz.com/getip.aspx");//"http://www.ip138.com/");

                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.UTF8);
                string all = sr.ReadToEnd(); //读取网站的数据
                tempip = all;
                //int start = all.IndexOf("您的IP地址是：[") + 9;
                //int end = all.IndexOf("]", start);

               // tempip = all.Substring(start, end - start);
                sr.Close();
                s.Close();
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
            }
            return tempip;
        }
    }

    class Options {
        [Option('i', "info", DefaultValue = 0, HelpText = "用户信息")]
        public int UserInfo { get; set; }

        [Option('q', "exit", DefaultValue = 0, HelpText = "退出")]
        public int Exit { get; set; }

        [Option('v', "version", DefaultValue = true, HelpText = "版本信息")]
        public bool Version { get; set; }

        [HelpOption]
        public string GetUsage() {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
