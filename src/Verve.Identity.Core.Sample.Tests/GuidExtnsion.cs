using System;

namespace Verve.Identity.Core.Tests
{
    public static class StaticGuids
    {
        public static Guid One
            => new Guid(0, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 });


        public static Guid Two
            => new Guid(0, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 2 });

        public static Guid OneOne
            => new Guid(0, 0, 0, new byte[] { 0, 1, 0, 0, 0, 0, 0, 1 });

        public static Guid OneTwo
            => new Guid(0, 0, 0, new byte[] { 0, 1, 0, 0, 0, 0, 0, 2 });
    }
}