using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FBPkgGen;
using UnityEngine;

namespace FBPkgGen
{
    public class SHPackage
    {
        /// <summary>
        /// 秘钥
        /// </summary>
        const string keyWord = "flybird";

        private static readonly string RESOURCEFILEDIR =
            Path.GetDirectoryName(Application.dataPath + "/../../Normal/Resource/");

        // key => <offset, data_size>
        readonly Dictionary<string, KeyValuePair<long, int>> offsets_ =
            new Dictionary<string, KeyValuePair<long, int>>();

        Stream stream_;

        public Dictionary<string, KeyValuePair<long, int>> GetOffsets()
        {
            return offsets_;
        }

        public void Load(Stream input_stream)
        {
            stream_ = input_stream;
            offsets_.Clear();
            DecodeOffsets(input_stream);
        }

        public byte[] FindFile(string key)
        {
            if (!offsets_.ContainsKey(key)) return null;

            KeyValuePair<long, int> p = offsets_[key];

            BinaryReader br = new BinaryReader(stream_);

            br.BaseStream.Seek(p.Key, SeekOrigin.Begin);

            byte[] buffer = new byte[p.Value];

            br.Read(buffer, 0, buffer.Length);

            return buffer;
        }

        private Dictionary<UInt64, int> DecodeOffsets(Stream stream)
        {
            Dictionary<UInt64, int> r = new Dictionary<UInt64, int>();

            BinaryReader br = new BinaryReader(stream);

            byte[] key_len = FBPkgGen.Tools.GetBytes(keyWord);

            br.BaseStream.Seek(key_len.Length, SeekOrigin.Begin);

            int num = br.ReadInt32();

            int[] offsets = new int[num];

            for (int i = 0; i < num; i++)
                offsets[i] = br.ReadInt32();

            string offsetStr = "";


            for (int i = 0; i < num; i++)
            {
                br.BaseStream.Seek(offsets[i], SeekOrigin.Begin);
                int f_bytes_len = br.ReadInt16();
                byte[] f_bytes = br.ReadBytes(f_bytes_len);
                string key = FBPkgGen.Tools.GetString(f_bytes);
                int data_size = br.ReadInt32();
                long pos = br.BaseStream.Position;
                offsets_.Add(key, new KeyValuePair<long, int>(pos, data_size));

                offsetStr += key + "#" + pos + "#" + data_size + "#" + f_bytes_len + "\r\n";
            }

            string output_path = Path.Combine(RESOURCEFILEDIR, "gen");
            File.WriteAllText(Path.Combine(output_path, "resList.txt"), offsetStr);


            return r;
        }

        public MemoryStream Save(string res_path, List<string> dst_keys)
        {
            byte[] key_bytes = FBPkgGen.Tools.GetBytes(keyWord);
            byte[] head = new byte[key_bytes.Length];
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(head);
            int num = dst_keys.Count;
            bw.Write(num);

            long start_pos = bw.BaseStream.Position;

            for (int i = 0; i < num; ++i)
            {
                bw.Write(0);
            }

            long[] offsets = new long[num];

            for (int i = 0; i < dst_keys.Count; ++i)
            {
                var filename = Path.Combine(res_path, dst_keys[i]);

                offsets[i] = bw.BaseStream.Position;

                byte[] f_bytes = FBPkgGen.Tools.GetBytes(dst_keys[i]);

                ushort f_bytes_len = (ushort) f_bytes.Length; //最大长度(65,535)

                bw.Write(f_bytes_len);
                bw.Write(f_bytes);
                byte[] data = File.ReadAllBytes(filename);
                bw.Write(data.Length);
                bw.Write(data);
            }

            bw.BaseStream.Seek(start_pos, SeekOrigin.Begin);

            for (int i = 0; i < num; ++i)
            {
                bw.Write((int) offsets[i]);
            }

            return ms;
        }
    }
}