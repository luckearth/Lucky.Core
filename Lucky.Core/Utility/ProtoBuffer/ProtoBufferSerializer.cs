﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IO;
using ProtoBuf;

namespace Lucky.Core.Utility.ProtoBuffer
{
    public class ProtoBufferSerializer: IProtoBufferSerializer
    {
        private const int GZIP_BUFFER_SIZE = 64 * 1024;

        /// <summary>
        ///     Saves item to file
        /// </summary>
        /// <param name="item">Item to be saved</param>
        /// <param name="filePath">Destination filepath</param>
        /// <param name="overWriteExistingFile"></param>
        /// <param name="gzipCompress">Use gzip compression</param>
        /// <returns>Saved filepath</returns>
        public string SaveToFile(
                                 [NotNull] object item,
                                 [NotNull] string filePath,
                                 FileMode fileMode = FileMode.Create,
                                 bool gzipCompress = false)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            if (!filePath.EndsWith(".bin"))
                throw new ArgumentException("filePath must end with .bin");

            var byteArray = ToByteArray(item, gzipCompress);

            File.WriteAllBytes(filePath, byteArray);

            return filePath;
        }

        /// <summary>
        ///     Saves item to file
        /// </summary>
        /// <param name="item">Item to be saved</param>
        /// <param name="filePath">Destination filepath</param>
        /// <param name="overWriteExistingFile"></param>
        /// <param name="gzipCompress">Use gzip compression</param>
        /// <returns>Saved filepath</returns>
        public async Task<string> SaveToFileAsync(
                                                  [NotNull] object item,
                                                  [NotNull] string filePath,
                                                  FileMode fileMode = FileMode.Create,
                                                  bool gzipCompress = false)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            if (!filePath.EndsWith(".bin"))
                throw new ArgumentException("filePath must end with .bin");

            var byteArray = ToByteArray(item, gzipCompress);

            using (var fs = new FileStream(filePath, fileMode))
            {
                await fs.WriteAsync(byteArray, 0, byteArray.Length);
            }

            return filePath;
        }

        /// <summary>
        ///     Transforms item to protobuf string
        /// </summary>
        /// <param name="item">Item to be serialized</param>
        /// <param name="gzipCompress">Use gzip compression</param>
        /// <returns>String serialization of the item</returns>
        public byte[] ToByteArray(
                                  [NotNull] object item,
                                  bool gzipCompress = false)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var manager = new RecyclableMemoryStreamManager();
            using (var ms = manager.GetStream())
            {
                if (gzipCompress)
                {
                    using (var gzip = new GZipStream(ms, CompressionMode.Compress, true))
                    using (var bs = new BufferedStream(gzip, GZIP_BUFFER_SIZE))
                    {
                        Serializer.Serialize(bs, item);
                    } //flush gzip                                     
                }
                else
                {
                    Serializer.Serialize(ms, item);
                }

                return ms.ToArray();
            }
        }
    }
}
