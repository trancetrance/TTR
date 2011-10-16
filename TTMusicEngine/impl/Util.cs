// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTMusicEngine
{
    /**
     * utility functions collection
     */
    internal static class Util
    {
        public static bool Logging = false;

        private static TextWriter logTw = null;        

        /**
         * log msg to file, if Util.Logging = true
         */
        internal static void Log(string s)
        {
            if (!Util.Logging) return;
            if(logTw==null)
                logTw = new StreamWriter("log_ttme.txt");
            logTw.Write(s);
            logTw.Flush();
        }

        /**
         * apply a simple Hash function to two values
         */
        internal static uint HashValues(uint h1, uint h2)
        {
	        // taken and modified from http://en.wikipedia.org/wiki/Jenkins_hash_function
            uint hash = h1;
            hash += (hash << 10);
            hash ^= (hash >> 6);

	        hash += h2;
            hash += (hash << 10);
            hash ^= (hash >> 6);

            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);
            return hash;
        }

        /**
         * FMOD error checking util function
        * @return true if error found
        */
        internal static bool ERRCHECK(FMOD.RESULT result)
        {
            if (result != FMOD.RESULT.OK)
            {
                Util.Log("FMOD ERR: " + result + " - " + FMOD.Error.String(result) + "\n" );
                return true;
            }
            return false;
        }

    }
}