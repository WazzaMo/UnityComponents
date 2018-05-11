/*
 * (c) Copyright 2018 Lokel Digital Pty Ltd
 * CNN for Unity
 */

using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Common {

    public static class FileNameUtil {
        static readonly char[]
            INVALID_PATH_CHARS = Path.GetInvalidPathChars(),
            INVALID_FILE_CHARS = Path.GetInvalidFileNameChars();

        public static bool IsValidFileName(string candidate) {
            bool ok = true;
            INVALID_FILE_CHARS.ForEach(ch => ok = ok && candidate.IndexOf(ch) < 0);
            return ok;
        }

        public static bool IsValidPathName(string candidate) {
            return candidate != null && candidate.Length > 0 &&
                ! candidate.ToCharArray()
                    .Where(x => INVALID_PATH_CHARS.Contains(x))
                    .Any();
        }

    }

}
