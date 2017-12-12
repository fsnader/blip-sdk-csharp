﻿using System.Collections.Generic;

namespace Take.Blip.Builder.Actions.ProcessHttp
{
    public class ProcessHttpSettings
    {
        public string Method { get; set; }

        public string Url { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string Body { get; set; }

        public string Variable { get; set; }
    }
}