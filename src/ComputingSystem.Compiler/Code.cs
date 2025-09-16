﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingSystem.Compiler
{
    internal static class Code
    {
        public static string Dest(string mnemonic)
        {
            return mnemonic switch
            {
                "M" => "001",
                "D" => "010",
                "MD" => "011",
                "A" => "100",
                "AM" => "101",
                "AD" => "110",
                "AMD" => "111",
                _ => "000", // null
            };
        }

        public static string Comp(string mnemonic)
        {
            return mnemonic switch
            {
                "0" => "0101010",
                "1" => "0111111",
                "-1" => "0111010",
                "D" => "0001100",
                "A" => "0110000",
                "!D" => "0001101",
                "!A" => "0110001",
                "-D" => "0001111",
                "-A" => "0110011",
                "D+1" => "0011111",
                "A+1" => "0110111",
                "D-1" => "0001110",
                "A-1" => "0110010",
                "D+A" => "0000010",
                "D-A" => "0010011",
                "A-D" => "0000111",
                "D&A" => "0000000",
                "D|A" => "0010101",
                "M" => "1110000",
                "!M" => "1110001",
                "-M" => "1110011",
                "M+1" => "1110111",
                "M-1" => "1110010",
                "D+M" => "1000010",
                "D-M" => "1010011",
                "M-D" => "1000111",
                "D&M" => "1000000",
                "D|M" => "1010101",
                _ => throw new ArgumentException($"Invalid comp mnemonic: {mnemonic}"),
            };
        }

        public static string Jump(string mnemonic)
        {
            return mnemonic switch
            {
                "JGT" => "001",
                "JEQ" => "010",
                "JGE" => "011",
                "JLT" => "100",
                "JNE" => "101",
                "JLE" => "110",
                "JMP" => "111",
                _ => "000", // null
            };
        }
    }
}
