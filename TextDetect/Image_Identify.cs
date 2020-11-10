using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Newtonsoft.Json;

namespace TextDetect
{
    public class Probability
    {
        /// <summary>
        /// 
        /// </summary>
        public double variance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double average { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double min { get; set; }
    }

    public class Words_resultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string words { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Probability probability { get; set; }
    }
    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public Int64 log_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int direction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int words_result_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Words_resultItem> words_result { get; set; }
    }
    public class Image_Identify_Helper
    {
        private Baidu.Aip.Ocr.Ocr client;

        public Image_Identify_Helper()
        {
            // 设置APPID/AK/SK
            var APP_ID = "18543940";
            var API_KEY = "RzHCBY3gRg8QXBugwbRUpULh";
            var SECRET_KEY = "5d5CXEa3UCkoOlExVaCbKrzF7XiI44ds";

            client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间
        }
        public string GetText(string path)
        {
     

            var image = File.ReadAllBytes(path);
            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            // var result = client.GeneralBasic(image);
            // Console.WriteLine(result);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                {"language_type", "CHN_ENG"},
                {"detect_direction", "true"},
                {"detect_language", "false"},
                {"probability", "true"}
            };
            // 带参数调用通用文字识别, 图片参数为本地图片
            var result = client.GeneralBasic(image, options);
            StringBuilder str = new StringBuilder();
            
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                StringReader sr = new StringReader(result.ToString());
                object o = serializer.Deserialize(new JsonTextReader(sr), typeof(Root));

                Root t = o as Root;
              
                foreach (var v in t.words_result)
                {
                    foreach (var x in v.words)
                    {
                        str.Append(x.ToString()) ;
                    }

                }

                TextDetect.Properties.Settings.Default.used_cnt++;
                TextDetect.Properties.Settings.Default.date_record = System.DateTime.Now.ToLongDateString();
                TextDetect.Properties.Settings.Default.Save();

                return str.ToString();

            }
            catch (Exception ex)
            {

                return "";
            }


        }
        private byte[] imageToByte(System.Drawing.Image _image)
        {
            MemoryStream ms = new MemoryStream();
            _image.Save(ms,System.Drawing.Imaging.ImageFormat.Jpeg);
            return  ms.ToArray();
        }
       
        public string GetText(Image path)
        {
    
            var image = imageToByte(path);
            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            // var result = client.GeneralBasic(image);
            // Console.WriteLine(result);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                {"language_type", "CHN_ENG"},
                {"detect_direction", "true"},
                {"detect_language", "false"},
                {"probability", "true"}
            };
            // 带参数调用通用文字识别, 图片参数为本地图片
            var result = client.GeneralBasic(image, options);
            StringBuilder str = new StringBuilder();

            try
            {
                JsonSerializer serializer = new JsonSerializer();
                StringReader sr = new StringReader(result.ToString());
                object o = serializer.Deserialize(new JsonTextReader(sr), typeof(Root));

                Root t = o as Root;

                foreach (var v in t.words_result)
                {
                    foreach (var x in v.words)
                    {
                        str.Append(x.ToString());
                    }

                }

                TextDetect.Properties.Settings.Default.used_cnt++;
                TextDetect.Properties.Settings.Default.date_record = System.DateTime.Now.ToLongDateString();
                TextDetect.Properties.Settings.Default.Save();

                return str.ToString();

            }
            catch (Exception ex)
            {

                return "";
            }


        }
    }
}
