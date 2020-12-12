using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TyHookCodingAssignment.Clients
{
    /// <summary>
    /// used to hold the authentication values
    /// </summary>
    public class Token
    {
        public string APIKey {get; set;}
        public string APISecret {get; set;}
        public string BearerToken {get; set; }
    }
}