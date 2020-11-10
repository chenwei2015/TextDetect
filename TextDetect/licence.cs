using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextDetect
{
    public class licence_helper
    {
        private const int max_times = 10;
       
        public static bool licenced
        {
            get
            {
                return Properties.Settings.Default.key == kex;
            }
            
        }
        public static bool enable
        {
            get
            {
                if (licenced)
                {
                    return true;
                }
                else
                {
                    return (TextDetect.Properties.Settings.Default.used_cnt < max_times);
                }
               
            }

        }

        public static bool set_code(string code)
        {
            int code_number = System.DateTime.Now.Year * 199 + System.DateTime.Now.Month * 55 + (int)Math.Pow(System.DateTime.Now.Day / 2,3);
            bool x= code ==System.Convert.ToString(code_number, 16).ToUpper();
            return x;

        }

        public static string kex
        {
            get { return Computer.CpuID + Computer.DiskID + Computer.MacAddress + Computer.TotalPhysicalMemory; }
        }
        public static void generate_key()
        {
            Properties.Settings.Default.key = kex;
            Properties.Settings.Default.Save();

        }
    }
}
