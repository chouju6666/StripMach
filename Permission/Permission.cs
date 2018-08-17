﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RT.Permission
{
    public class Permission
    {
        public bool IsAllowed { get; set; }
        internal List<string> AccessControllList { get; private set; }

        internal Permission(params string[] allowedRoles)
        {
            IsAllowed = false;
            AccessControllList = new List<string>();
            foreach (var role in allowedRoles)
            {
                AccessControllList.Add(role);
            }
        }
    }
}
