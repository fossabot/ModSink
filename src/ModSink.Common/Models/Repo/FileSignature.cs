﻿using System;
using System.IO;

namespace ModSink.Common.Models.Repo
{
    [Serializable]
    public struct FileSignature : IEquatable<FileSignature>
    {
        public FileSignature(HashValue hash, ulong length)
        {
            Hash = hash;
            Length = length;
        }

        public FileSignature(HashValue hash, long length)
        {
            Hash = hash;
            Length = Convert.ToUInt64(length);
        }

        public HashValue Hash { get; }

        /// <summary>
        ///     The length of the file taken from <see cref="FileInfo" />, in bytes.
        /// </summary>
        public ulong Length { get; }


        public bool Equals(FileSignature other)
        {
            return Hash.Equals(other.Hash) && Length == other.Length;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is FileSignature signature && Equals(signature);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Hash.GetHashCode() * 397) ^ Length.GetHashCode();
            }
        }

        public override string ToString()
        {
            return Hash.ToString();
        }
    }
}