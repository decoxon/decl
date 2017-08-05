using System;
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
        [Option('s', "statement", HelpText ="The statement to parse.")]
        public string Statement { get; set; }

        [Option('i', "interactive", HelpText ="Enter interactive mode", MutuallyExclusiveSet ="interactive")]
        public bool IsInteractiveModeRequested { get; set; }
    }
}
