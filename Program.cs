﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sshserver
{
    class Program
    {
        /*****/
        static void Main(string[] args)
        {
            Server server = new Server();
            server.lancer();
        }
    }
}
