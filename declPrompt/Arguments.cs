﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.ComponentModel.DataAnnotations;

namespace declPrompt
{
    class Arguments
    {
        [Option('s', "statement", HelpText = "The statement to parse.", MutuallyExclusiveSet = "stringSource")]
        public string Statement { get; set; }

        [Option('i', "interactive", HelpText = "Enter interactive mode", MutuallyExclusiveSet = "interactive")]
        public bool IsInteractiveModeRequested { get; set; }

        [Option('f', "file", HelpText = "The file to be loaded", MutuallyExclusiveSet ="fileSource")]
        public string FilePath { get; set; }
    }
}
