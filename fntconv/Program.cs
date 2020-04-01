using System;
using  System.Xml;
using System.Collections.Generic;
using System.IO;
namespace fntconv
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("fntconv.exe [input fnt] [output fnt name] image name]");
                Console.WriteLine("exp:fontconv.exe aaa.fnt ingame.fnt ingame.png]");
                Console.WriteLine("By:ICE AGE");
                return;
            }
            var xmlFnt = args[0];
            var bfntName = args[1];
            var imageName = args[2];
            
            XmlDocument xml=new XmlDocument();
            xml.Load(xmlFnt);

            XmlNode fontNode=xml.SelectSingleNode("font");
            var info=fontNode.SelectSingleNode("info");
//            var common = fontNode.SelectSingleNode("common");
            var chars = fontNode.SelectSingleNode("chars");
            Console.WriteLine();
            string head = "bfnt";
            short fontSize = Convert.ToInt16(info.Attributes["size"].Value);
            Int32 unknown = 32;
//            string imageName = "ingame.png";
            short nameLen = (short)imageName.Length;//Convert.ToInt16(common.Attributes["lineHeight"].Value);
            short count = Convert.ToInt16(chars.Attributes["count"].Value);

            FileStream file = File.Create(bfntName);
            BinaryWriter br=new BinaryWriter(file);
            
            br.Write(System.Text.Encoding.Default.GetBytes(head));
            br.Write(fontSize);
            br.Write(unknown);
            br.Write(nameLen);
            br.Write(System.Text.Encoding.Default.GetBytes(imageName));
            br.Write(count);
            
            foreach (var c in chars.ChildNodes)
            {
                XmlElement xe = (XmlElement) c;
                int id=Convert.ToInt32(xe.Attributes["id"].Value);
                float x=Convert.ToSingle(xe.Attributes["x"].Value);
                float y=Convert.ToSingle(xe.Attributes["y"].Value);
                float w=Convert.ToSingle(xe.Attributes["width"].Value);
                float h=Convert.ToSingle(xe.Attributes["height"].Value);
                float xo=Convert.ToSingle(xe.Attributes["xoffset"].Value);
                float yo=Convert.ToSingle(xe.Attributes["yoffset"].Value);
                int xad=Convert.ToInt32(xe.Attributes["xadvance"].Value);
                br.Write(id);
                br.Write(x);
                br.Write(y);
                br.Write(w);
                br.Write(h);
                br.Write(xo);
                br.Write(yo);
                br.Write(xad);
            }

            int zero = 0;
            br.Write(zero);
            br.Flush();
//            br.Close();
            file.Flush();
            file.Close();
        }
    }
}